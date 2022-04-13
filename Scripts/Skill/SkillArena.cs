using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Mon", menuName = "Mon/Skill/Arena", order = 1)]
public class SkillArena : Skill{
    public int turns;
    public float increase;
    public override void HandleAnimation(HandleAnimations handleAnimation)
    {
        throw new System.NotImplementedException();
    }

    public override bool HandleSkill(Skill skill, MonManager ally, MonManager enemy){
        return false;
    }

      public async override Task ApplyAnimationTrigger()
    {
        await TextDialogManager.Singleton.PushText("Look at that Arena stuff",500);
    }   
  
}