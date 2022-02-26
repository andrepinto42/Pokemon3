using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class SkillHandlerBuff : ISkillHandler 
{
    public void HandleAnimation(HandleAnimations handleAnimation)
    {
        handleAnimation.ChangeAnimationState(handleAnimation.MON_BOOST);
    }
    public static string messageBuffSucess;
    public bool HandleSkill(Skill skill, MonManager ally, MonManager enemy){
        //Play the audio when the skill is loaded
        BattleAudioManager.Singleton.PlayAudio(skill.soundEffect);

        SkillBuff buffskill = (SkillBuff) skill;
        BuffStatus buffStatus = buffskill.buffStatus;
    
        messageBuffSucess = SkillHandlerStatusBuff.HandleBuff(ally,enemy,buffStatus);
        return false;
    }

    internal async static Task DealBuffAnimationTrigger()
    {
        await TextDialogManager.Singleton.PushText(messageBuffSucess,500);
        messageBuffSucess = null;
    }    
    
}