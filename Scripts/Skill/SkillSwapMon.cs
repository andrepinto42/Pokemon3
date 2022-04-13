using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    public override async Task ApplyAnimationTrigger()
    {
        await TextDialogManager.Singleton.PushText("You Changed the pokemon from this to that :)");
    }
}
