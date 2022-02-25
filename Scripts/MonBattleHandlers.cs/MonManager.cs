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

    public void ChangeBaseDamage(float buff)
    {
        MonMain.attackBuff *= buff;
    }

    public void ChangeBaseSpeed(float speedbuff)
    {
        MonMain.speedBuff *= speedbuff;
    }
    public void ChangeBaseDefense(float defenseBuff){
        MonMain.defenseBuff *= defenseBuff;
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
