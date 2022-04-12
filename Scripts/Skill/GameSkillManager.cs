using System;
using System.Collections.Generic;
using UnityEngine;

public class GameSkillManager
{    
    internal static bool HandleSkill(Skill skill, MonManager ally, MonManager enemy,HandleAnimations allyAnimation)
    { 
        bool killed = skill.HandleSkill(skill,ally,enemy);

        skill.HandleAnimation(allyAnimation);

        return killed;
    }

}