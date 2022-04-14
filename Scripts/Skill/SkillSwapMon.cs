using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Swap", menuName = "Mon/Skill/Swap", order = 1)]
public class SkillSwapMon : Skill{
    [HideInInspector] public string monExiting;
    [HideInInspector] public string monEntering;
   //TODO
   public override void HandleAnimation(HandleAnimations handleAnimation)
    {

       return;
    }

    public override bool HandleSkill(Skill skill, MonManager ally, MonManager enemy){
        return false;
    }
    public override async Task ApplyAnimationTrigger()
    {
        await TextDialogManager.Singleton.PushText("C'mon get out "+monExiting+" and let's get into battle "+monEntering);
        monExiting = null;
        monEntering = null;
    }
}
