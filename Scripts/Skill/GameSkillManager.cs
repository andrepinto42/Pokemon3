using System;
using System.Collections.Generic;
using UnityEngine;

public class GameSkillManager
{    
    internal static bool HandleSkill(Skill skill, MonManager ally, MonManager enemy,HandleAnimations allyAnimation)
    { 
        //Store the value in the MonManagerForNow
        ally.lastSkillUsed = skill;

        bool killed = skill.HandleSkill(skill,ally,enemy);

        if (allyAnimation == null)
        Debug.Log("here is null");
        skill.HandleAnimation(allyAnimation);

        return killed;
    }

}