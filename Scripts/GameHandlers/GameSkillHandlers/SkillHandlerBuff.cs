using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class SkillHandlerBuff : ISkillHandler 
{
    public void HandleAnimation(HandleAnimations handleAnimation)
    {
        handleAnimation.ChangeAnimationState(handleAnimation.MON_BOOST);
    }
    public static string messageBuffSucess;

    public bool HandleSkill(Skill skill, MonManager ally, MonManager enemy){
        var mapBuff = new Dictionary<SkillBuff.Stat, Delegate>(){
            {SkillBuff.Stat.ATTACK,new Func<float,MonManager,string>(HandleAttackBuff)},

            {SkillBuff.Stat.DEFENSE,new Func<float,MonManager,string>(HandleDefenseBuff)},
            
            {SkillBuff.Stat.SPEED,new Func<float,MonManager,string>(HandleSpeedBuff)},
        };

        //Play the audio when the skill is loaded
        BattleAudioManager.Singleton.PlayAudio(skill.soundEffect);

        SkillBuff buffskill = (SkillBuff) skill;

        MonManager changingMon;

        //Trigger the particle system
        if (buffskill.ally) 
        {
            changingMon = ally;
            GameVisualEffectsHandler.Singleton.AllyStartSpinning(buffskill,ally);
        }
        else
        {
            changingMon = enemy;
            GameVisualEffectsHandler.Singleton.EnemyStartSpinning(buffskill,enemy);
        }                
      
        
        messageBuffSucess = (string) mapBuff[buffskill.effect].DynamicInvoke(buffskill.increase,changingMon);
        return false;
    }

    internal async static Task DealBuffAnimationTrigger()
    {
        await TextDialogManager.Singleton.PushText(messageBuffSucess,500);
        messageBuffSucess = null;
    }

    private string HandleSpeedBuff(float increase,MonManager changingMon)
    {
        changingMon.ChangeBaseSpeed(increase);
        return MessageToDisplay(changingMon,"speed",increase);
    }

    private string HandleDefenseBuff(float increase,MonManager changingMon)
    {
        changingMon.ChangeBaseDefense(increase);
        Debug.Log("Decreased defense to " + changingMon.MonMain.defenseBuff);
        return MessageToDisplay(changingMon,"defense",increase);
    }

    private string HandleAttackBuff(float increase,MonManager changingMon)
    {
        changingMon.ChangeBaseDamage(increase);
        return MessageToDisplay(changingMon,"attack",increase);
    }

   

    private string MessageToDisplay(MonManager monManager,string status,float increase)
    {
        string increaseText = increase > 1f ? "risen" : "decreased";
        string s =  monManager.MonMain.GetNameMon() + " " + status + " has " + increaseText + " !";
        return s;
    }
    
}