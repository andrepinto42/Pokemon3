using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MonManager : MonoBehaviour
{
    [SerializeField]public MonGame MonMain;
    public MonTypesCombat buffAttackType = new MonTypesCombat(1f,1f,1f,1f);
    public MonTypesCombat buffDefenseType = new MonTypesCombat(1f,1f,1f,1f);
    
    public float speedBuff =1f;

    [HideInInspector]public Skill lastSkillUsed = null;
    HandleAnimations handleAnimations;
    MonMeshManager monMeshManager;
    

    public async Task SwapMon(MonGame mon,ParticleSystem particles)
    {
        //During some frames the pokemon will decrease in size
        var objectAllyMon = monMeshManager.transform;
        var startScale = objectAllyMon.localScale.x; 
        for (float j = startScale; j > 0f; j-=0.01f)
        {
            objectAllyMon.localScale = new Vector3(j,j,j);
            await Task.Delay(20);
        }

        KillGameObject();
        await InitializeMeshMon(mon,particles);
    }
    //Need to add to all previous references the await callback and be carefull with the particle system
    public async Task InitializeMeshMon(MonGame currentMon,ParticleSystem particles)
    {
        MonMain = currentMon;
     
        //Start the vfx of the particle System
        particles.gameObject.SetActive(true);
        particles.gameObject.transform.position = this.gameObject.transform.position;

        //Dont do this
        //monMeshManager = currentMon.GetMonMeshManager();   
        var monGameObject = Instantiate(currentMon.GetMonMeshManager().gameObject,this.transform.position,
        Quaternion.identity,this.transform);

        // Use local rotation to not interfere with the rotation of the parent
        monGameObject.transform.localRotation = Quaternion.Euler(0,0,0);
        // monGameObject.transform.rotation = Quaternion.Euler(0,0,0);
        
        //Lets consider that x,z,y have all the same scale and store it
        float maxScale =monGameObject.transform.localScale.x;

        monGameObject.transform.localScale = Vector3.zero;
        
        //Set the HUD Aspects of the mon
        SetupConfigMon(monGameObject,currentMon);
        
        for (float i = 0; i < maxScale; i+=0.01f)
        {
            monGameObject.transform.localScale = new Vector3(i,i,i);
            await Task.Delay(20);
        }

        particles.gameObject.SetActive(false);

    }
    public void LoadNewMon(MonGame monGame,GameObject monGameObject)
    {
        MonMain = monGame;
        this.transform.position = monGameObject.transform.position;
        
        //Need to set the parent to the MonManager otherwise the code wont work
        monGameObject.transform.SetParent(this.transform);
       


        monGameObject.transform.localPosition = Vector3.zero;
        
        SetupConfigMon(monGameObject,monGame);
    }
    private void SetupConfigMon(GameObject monGameObject,MonGame monGame)
    {
        
        //Store the value of Handle animations and MonMeshManager in other scripts
        this.handleAnimations = monGameObject.GetComponent<HandleAnimations>();
        this.monMeshManager = monGameObject.GetComponent<MonMeshManager>();

        //Update the HUD of the Mon
        string nameMon =MonMain.GetNameMon();
        monMeshManager.textNameMon.SetText(nameMon);
        monMeshManager.levelText.SetText(MonMain.level.ToString());        
        monMeshManager.UpdateHealth(MonMain.currentHealth,MonMain.maxHealth);
        monMeshManager.UpdateStamina(MonMain.currentStamina,MonMain.maxStamina);

        //Reset the buff of attack and defense
        for (int i = 0; i < buffAttackType.arrayAtributtes.Length; i++)
        {
            buffAttackType.arrayAtributtes[i] = 1f;
            buffDefenseType.arrayAtributtes[i] = 1f;
        }

       
    }
  
    
    public void TakeDamage(float damage)
    {
        MonMain.currentHealth = GetCurrentHealthAfterDamage(damage);
        monMeshManager.StartDamagePopUpText(damage);
        handleAnimations.ChangeAnimationState(handleAnimations.MON_GET_HIT);
    }
    public float GetCurrentHealthAfterDamage(float damage)
    {
        return MonMain.currentHealth - damage;
    } 
    public void KillGameObject()
    {
        Destroy(monMeshManager.gameObject);
    }
    internal bool HandleStaminaManagement(Skill skill)
    {
        if (MonMain.currentStamina < skill.stamina)
            return false;
        
        MonMain.currentStamina -= skill.stamina;
        monMeshManager.UpdateStamina(MonMain.currentStamina,MonMain.maxStamina);
        return true;
    }
    public float ApplyStanceModifier(float buff)
    {
        //If doenst have enought Stance then just apply the normal debuff
        if (MonMain.currentStance <= 2f)
            return buff;

        float newbuff = CalculateNewBuff(buff);
        //Halve the current Stance if the debuff is < 1;
        // -Log(0,75f,2f) = 0.4150 
        // Log(1,5f,2f) = 0.58496250072116

        MonMain.currentStance /= (Mathf.Abs(Mathf.Log(Mathf.Abs(1f - buff), 2f)) + 1f);

        // Debug.Log("Current Stance " + MonMain.currentStance + " and the new debuf" + newbuff + " old:" + buff);
        return newbuff;
    }

    public float CalculateNewBuff(float buff)
    {
        float newbuff;
        if (buff < 1f)
        {
            //with a buff of 0.75 it transform it into 0.875 if the stance is 50
            float buffMitigation = (1f - buff) * MonMain.currentStance * 0.01f;
            newbuff = buff + buffMitigation;
        }
        else
        {
            newbuff = buff * (1f + MonMain.currentStance * 0.01f);
        }

        return newbuff;
    }

    public void ChangeBaseDamage(float buff,TypeOfDamage typeOfDamage)
    {   
        float newAttack  = ApplyStanceModifier(buff);
        
        buffAttackType.arrayAtributtes[(int) typeOfDamage] *= newAttack;
        
    }
    public void ChangeBaseDefense(float defensebuff,TypeOfDamage typeOfDamage)
    {
        float newDefense = ApplyStanceModifier(defensebuff);
        
        buffDefenseType.arrayAtributtes[(int) typeOfDamage] *= newDefense;

    }


    public void ChangeBaseSpeed(float speedbuff)
    {
        float newSpeedBuff = ApplyStanceModifier(speedbuff);
        MonMain.currentSpeed *= newSpeedBuff;
    }

/*
------------------------------
        GETTERS
------------------------------
*/

    public float GetCurrentSpeed()
    {
        return MonMain.currentSpeed;
    }
    public MonMeshManager GetMonMeshManager()
    {
        return monMeshManager;
    }

    public float GetcurrentHealth()
    {
        return MonMain.currentHealth;
    } 
    public float GetcurrentStamina()
    {
        return MonMain.currentStamina;
    } 
    public float GetcurrentStance()
    {
        return MonMain.currentStance;
    }
}
