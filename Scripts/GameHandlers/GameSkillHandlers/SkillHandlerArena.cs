using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillHandlerArena : ISkillHandler 
{
    public void HandleAnimation(HandleAnimations handleAnimation)
    {
        throw new System.NotImplementedException();
    }

    public bool HandleSkill(Skill skill, MonManager ally, MonManager enemy){
        return false;
    }
}