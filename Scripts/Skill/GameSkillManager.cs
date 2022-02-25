using System;
using System.Collections.Generic;
using UnityEngine;

public class GameSkillManager
{   
    static Dictionary<String,ISkillHandler> mapSkillHandlers = new Dictionary<string, ISkillHandler>(){

        {"SkillDamage", new SkillHandlerDamage()},

        {"SkillBuff", new SkillHandlerBuff()},
        
        {"SkillArena", new SkillHandlerArena()}
    
    }; 
    internal static bool HandleSkill(Skill skill, MonManager ally, MonManager enemy,HandleAnimations allyAnimation)
    { 
        ISkillHandler skillHandler = mapSkillHandlers[skill.GetType().Name];

        if (skillHandler == null){
            Debug.LogError("NAO EXISTE ESTA SKILL AINDA " + skill.GetType().Name);
            return false;
        }

        bool killed = skillHandler.HandleSkill(skill, ally,enemy);
        skillHandler.HandleAnimation(allyAnimation);

        return killed;
    }

}