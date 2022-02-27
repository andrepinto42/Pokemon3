using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MonManager : MonoBehaviour
{
    [SerializeField]public MonGame MonMain;
    public float rotationY = 0f;
    HandleAnimations handleAnimations;
    MonMeshManager monMeshManager;
    void Start()
    {
        TrainerHandler.RestartStatsMon(MonMain);

        InitializeMeshMon();
    }

    public void InitializeMeshMon()
    {
        //Dont do this
        //monMeshManager = MonMain.GetMonMeshManager();

        var monGameObject = Instantiate(MonMain.GetMonMeshManager().gameObject,this.transform.position,
        Quaternion.Euler(0,rotationY,0),this.transform);

        //Store the value of Handle animations and MonMeshManager in other scripts
        this.handleAnimations = monGameObject.GetComponent<HandleAnimations>();
        this.monMeshManager = monGameObject.GetComponent<MonMeshManager>();

        //Update the HUD of the Mon
        string nameMon =MonMain.GetNameMon();
        monMeshManager.textNameMon.SetText(nameMon);
        
        monMeshManager.levelText.SetText(MonMain.level.ToString());
         
        monMeshManager.UpdateHealth(MonMain.currentHealth,MonMain.maxHealth);
        monMeshManager.UpdateStamina(MonMain.currentStamina,MonMain.maxStamina);
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
        
        float newbuff;
        if (buff< 1f)
        {
            //with a buff of 0.75 it transform it into 0.875 if the stance is 50
            float buffMitigation = (1f-buff) * MonMain.currentStance * 0.01f;
            newbuff = buff + buffMitigation;
        }
        else
        {
            newbuff = buff * (1f+ MonMain.currentStance * 0.01f);
        }
        //Halve the current Stance if the debuff is < 1;
        // -Log(0,75f,2f) = 0.4150 
        // Log(1,5f,2f) = 0.58496250072116

        MonMain.currentStance /= ( Mathf.Abs( Mathf.Log( Mathf.Abs(1f-buff) ,2f) ) + 1f);

        Debug.Log("Current Stance " +MonMain.currentStance + " and the new debuf" + newbuff + " old:" + buff);
        return newbuff;
    }
    public void ChangeBaseDamage(float buff)
    {   
        MonMain.attackBuff *= ApplyStanceModifier(buff);
    }

    public void ChangeBaseSpeed(float speedbuff)
    {
        MonMain.speedBuff *= ApplyStanceModifier(speedbuff);
    }
    public void ChangeBaseDefense(float defenseBuff)
    {
        MonMain.defenseBuff *= ApplyStanceModifier(defenseBuff);
    }

    public float GetCurrentSpeed()
    {
        return MonMain.SpeedCurrent * MonMain.speedBuff;
    }
    public MonMeshManager GetMonMeshManager()
    {
        return monMeshManager;
    }
}
