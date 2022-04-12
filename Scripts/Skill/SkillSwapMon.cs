using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Swap", menuName = "Mon/Skill/Swap", order = 1)]
public class SkillSwapMon : Skill{
   //TODO
   public override void HandleAnimation(HandleAnimations handleAnimation)
    {
        throw new System.NotImplementedException();
    }

    public override bool HandleSkill(Skill skill, MonManager ally, MonManager enemy){
        return false;
    }
}
