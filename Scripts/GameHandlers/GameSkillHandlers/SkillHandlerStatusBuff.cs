using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class SkillHandlerStatusBuff  
{
    public static string HandleBuff(MonManager ally,MonManager enemy,BuffStatus buffStatus)
    {
        //A little un-optimized but code looks better this way
        MonManager changingMon = (buffStatus.ally) ? ally : enemy;

        //Trigger the particle system
        if (buffStatus.ally) 
            GameVisualEffectsHandler.Singleton.AllyStartSpinning(buffStatus,changingMon);
        else
            GameVisualEffectsHandler.Singleton.EnemyStartSpinning(buffStatus,changingMon);


        switch(buffStatus.effect)
        {
            case SkillBuff.Stat.ATTACK:
                changingMon.ChangeBaseDamage(buffStatus.increase);
                return MessageToDisplay(changingMon,"attack",buffStatus.increase);
    
            case SkillBuff.Stat.DEFENSE:
                changingMon.ChangeBaseDefense(buffStatus.increase);
                return MessageToDisplay(changingMon,"defense",buffStatus.increase);
    
            
            case SkillBuff.Stat.SPEED:
                changingMon.ChangeBaseSpeed(buffStatus.increase);
                return MessageToDisplay(changingMon,"speed",buffStatus.increase);

            default:
                return "Status not found to display";
        }
    }

    private static string MessageToDisplay(MonManager monManager,string status,float increase)
    {
        string increaseText = increase > 1f ? "risen" : "decreased";
        string s =  monManager.MonMain.GetNameMon() + " " + status + " has " + increaseText + " !";
        return s;
    }
}