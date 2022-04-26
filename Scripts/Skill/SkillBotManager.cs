using System;
using System.Collections.Generic;
using UnityEngine;
public class SkillBotManager
{
    public enum DIFFICULTY
    {
        RANDOM,
        AWARE,
        GOD
    }
    internal static Skill FindBestSkill(MonManager botMonManager, MonManager userMonManager, Skill[] allSkills,DIFFICULTY d)
    {
      
        switch (d)
        {
            case DIFFICULTY.RANDOM:
                return HandleRandomSkill(botMonManager,userMonManager,allSkills);    
            case DIFFICULTY.AWARE:
                return HandleAwareSkill(botMonManager,userMonManager,allSkills);    
            case DIFFICULTY.GOD:
                return HandleGodSkill(botMonManager,userMonManager,allSkills);    
            default:
                return null;
        }
    }

    private static Skill HandleGodSkill(MonManager botMonManager, MonManager userMonManager, Skill[] allSkills)
    {
        throw new NotImplementedException();
    }

    //Finds the move that deals the most damage
    private static Skill HandleAwareSkill(MonManager botMonManager, MonManager userMonManager, Skill[] allSkills)
    {
        MonGame botMon = botMonManager.MonMain;
        MonGame userMon = userMonManager.MonMain;

        List<Skill> validSkills = GetValidSkills(botMonManager, allSkills);
        int maxDamage = 0;
        Skill bestSkill = null;
        for (int i = 0; i < validSkills.Count; i++)
        {
            Skill s = validSkills[i];
            
            if (! (s is SkillDamage) )
                continue;
            
            int damage = MonCalculateDamage.CalculateDamage(
                (SkillDamage) s,botMon,userMon,botMonManager,userMonManager);
            
            if (damage > maxDamage)
            {
                bestSkill = s;
                maxDamage= damage;
            }

        }
        //TODO
        int numberTurns = (int) (userMon.currentHealth / (maxDamage* botMonManager.attackBuff) ) ;

        for (int i = 0; i < validSkills.Count; i++)
        {
            var s = validSkills[i];

            if (! (s is SkillBuff ))
                continue;
            
            SkillBuff buff = (SkillBuff) s;

            if (buff.buffStatus.effect == SkillBuff.Stat.ATTACK && buff.buffStatus.increase > 1f)
            {
                int numberNewTurns = 1 + (int) (userMon.currentHealth / (maxDamage* buff.buffStatus.increase* botMonManager.attackBuff ));
                // Debug.Log("with buffing "+numberNewTurns + " | without " + numberTurns);
                //Found that its worth to use Attack buff to diminish the number of turns needed
                if (numberNewTurns < numberTurns)
                {
                    return s;
                }
            }
        }
        
        return bestSkill;
    }

    private static Skill HandleRandomSkill(MonManager botMonManager, MonManager userMonManager, Skill[] allSkills)
    {
        MonGame botMon = botMonManager.MonMain;
        MonGame userMon = userMonManager.MonMain;

        List<Skill> validSkills = GetValidSkills(botMonManager, allSkills);
        System.Random r = new System.Random();
        int randomPosition = (int)( r.Next(validSkills.Count -1) );
        
        return validSkills[randomPosition];
    }

    //Currently if the pokemon must have at least one valid move
    private static List<Skill> GetValidSkills(MonManager botMonManager, Skill[] allSkills)
    {
        List<Skill> validSkills = new List<Skill>();
        for (int i = 0; i < allSkills.Length; i++)
        {
            var s = allSkills[i];
            if (s == null)
                continue;

            //If the mon doesnt have enough stamina to perform a skill then discard it
            if (s.stamina > botMonManager.currentStamina)
            {
                continue;
            }
            validSkills.Add(s);
        }

        return validSkills;
    }
}