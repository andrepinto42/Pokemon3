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
    internal static Skill FindBestSkill(MonGame botMon, MonGame userMon, Skill[] allSkills,DIFFICULTY d)
    {
        switch (d)
        {
            case DIFFICULTY.RANDOM:
                return HandleRandomSkill(botMon,userMon,allSkills);    
            case DIFFICULTY.AWARE:
                return HandleAwareSkill(botMon,userMon,allSkills);    
            case DIFFICULTY.GOD:
                return HandleGodSkill(botMon,userMon,allSkills);    
            default:
                return null;
        }
    }

    private static Skill HandleGodSkill(MonGame botMon, MonGame userMon, Skill[] allSkills)
    {
        throw new NotImplementedException();
    }

    //Finds the move that deals the most damage
    private static Skill HandleAwareSkill(MonGame botMon, MonGame userMon, Skill[] allSkills)
    {
        List<Skill> validSkills = GetValidSkills(botMon, allSkills);
        int maxDamage = 0;
        Skill bestSkill = null;
        for (int i = 0; i < validSkills.Count; i++)
        {
            Skill s = validSkills[i];
            
            if (! (s is SkillDamage) )
                continue;
            
            int damage = MonCalculateDamage.CalculateDamage( (SkillDamage) s,botMon,userMon);
            
            if (damage > maxDamage)
            {
                bestSkill = s;
                maxDamage= damage;
            }

        }
        //TODO
        int numberTurns = (int) (userMon.currentHealth / (maxDamage* botMon.attackBuff) ) ;

        for (int i = 0; i < validSkills.Count; i++)
        {
            var s = validSkills[i];

            if (! (s is SkillBuff ))
                continue;
            
            SkillBuff buff = (SkillBuff) s;

            if (buff.buffStatus.effect == SkillBuff.Stat.ATTACK && buff.buffStatus.increase > 1f)
            {
                int numberNewTurns = 1 + (int) (userMon.currentHealth / (maxDamage* buff.buffStatus.increase* botMon.attackBuff ));
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

    private static Skill HandleRandomSkill(MonGame botMon, MonGame userMon, Skill[] allSkills)
    {
        List<Skill> validSkills = GetValidSkills(botMon, allSkills);
        System.Random r = new System.Random();
        int randomPosition = (int)( r.Next(validSkills.Count -1) );
        
        return validSkills[randomPosition];
    }

    //Currently if the pokemon must have at least one valid move
    private static List<Skill> GetValidSkills(MonGame botMon, Skill[] allSkills)
    {
        List<Skill> validSkills = new List<Skill>();
        for (int i = 0; i < allSkills.Length; i++)
        {
            var s = allSkills[i];
            if (s == null)
                continue;

            //If the mon doesnt have enough stamina to perform a skill then discard it
            if (s.stamina > botMon.currentStamina)
            {
                continue;
            }
            validSkills.Add(s);
        }

        return validSkills;
    }
}