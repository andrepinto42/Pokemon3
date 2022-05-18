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
    
    public float rotationY = 0f;
    public float currentHealth = 100f;
    public float currentStamina = 100f;
    public float attackBuff = 1f;
    public float defenseBuff = 1f;
    public float speedBuff =1f;
    public float currentStance=100f;

    [HideInInspector]public Skill lastSkillUsed = null;
    HandleAnimations handleAnimations;
    MonMeshManager monMeshManager;
    

    public void SwapMon(MonGame mon)
    {
        KillGameObject();
        InitializeMeshMon(mon);
    }
    public void InitializeMeshMon(MonGame currentMon)
    {
        MonMain = currentMon;
     
        //Dont do this
        //monMeshManager = currentMon.GetMonMeshManager();   
        var monGameObject = Instantiate(currentMon.GetMonMeshManager().gameObject,this.transform.position,
        Quaternion.Euler(0,rotationY,0),this.transform);

       SetupConfigMon(monGameObject,currentMon);
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

        //Update the current Stats of the Mon
        this.currentHealth = monGame.currentHealth;
        this.currentStamina = monGame.maxStamina;
        this.currentStance = monGame.maxStamina;

        //Update the HUD of the Mon
        string nameMon =MonMain.GetNameMon();
        monMeshManager.textNameMon.SetText(nameMon);
        
        monMeshManager.levelText.SetText(MonMain.level.ToString());
         
        monMeshManager.UpdateHealth(currentHealth,MonMain.maxHealth);
        monMeshManager.UpdateStamina(currentStamina,MonMain.maxStamina);
    }
  
    
    public void TakeDamage(float damage)
    {
        currentHealth = GetCurrentHealthAfterDamage(damage);
        monMeshManager.StartDamagePopUpText(damage);
        handleAnimations.ChangeAnimationState(handleAnimations.MON_GET_HIT);
    }
    public float GetCurrentHealthAfterDamage(float damage)
    {
        return currentHealth - damage;
    } 
    public void KillGameObject()
    {
        Destroy(monMeshManager.gameObject);
    }
    internal bool HandleStaminaManagement(Skill skill)
    {
        if (currentStamina < skill.stamina)
            return false;
        
        currentStamina -= skill.stamina;
        monMeshManager.UpdateStamina(currentStamina,MonMain.maxStamina);
        return true;
    }
    public float ApplyStanceModifier(float buff)
    {
        //If doenst have enought Stance then just apply the normal debuff
        if (currentStance <= 2f)
            return buff;

        float newbuff = CalculateNewBuff(buff);
        //Halve the current Stance if the debuff is < 1;
        // -Log(0,75f,2f) = 0.4150 
        // Log(1,5f,2f) = 0.58496250072116

        currentStance /= (Mathf.Abs(Mathf.Log(Mathf.Abs(1f - buff), 2f)) + 1f);

        // Debug.Log("Current Stance " + MonMain.currentStance + " and the new debuf" + newbuff + " old:" + buff);
        return newbuff;
    }

    public float CalculateNewBuff(float buff)
    {
        float newbuff;
        if (buff < 1f)
        {
            //with a buff of 0.75 it transform it into 0.875 if the stance is 50
            float buffMitigation = (1f - buff) * currentStance * 0.01f;
            newbuff = buff + buffMitigation;
        }
        else
        {
            newbuff = buff * (1f + currentStance * 0.01f);
        }

        return newbuff;
    }
    public delegate void DelegateStatusChange(string value);

    public  DelegateStatusChange delegateDamageChange;
    public void ChangeBaseDamage(float buff)
    {   
        attackBuff *= ApplyStanceModifier(buff);
        delegateDamageChange?.Invoke(attackBuff.ToString());
    }

    public  DelegateStatusChange delegateSpeedChange;
    public void ChangeBaseSpeed(float speedbuff)
    {
        speedBuff *= ApplyStanceModifier(speedbuff);
        delegateSpeedChange?.Invoke(speedBuff.ToString());
    }

    public  DelegateStatusChange delegateDefenseChange;
    public void ChangeBaseDefense(float defensebuff)
    {
        defenseBuff *= ApplyStanceModifier(defensebuff);
        delegateDefenseChange?.Invoke(defenseBuff.ToString());

    }

    public float GetCurrentSpeed()
    {
        return MonMain.SpeedCurrent * speedBuff;
    }
    public MonMeshManager GetMonMeshManager()
    {
        return monMeshManager;
    }
}
