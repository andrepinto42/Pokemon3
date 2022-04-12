using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

[Serializable]
public class BuffStatus{
    public float increase = 1f;
    public bool ally;
   
    public SkillBuff.Stat effect;
}

[CreateAssetMenu(fileName = "Mon", menuName = "Mon/Skill/Buff", order = 1)]
public class SkillBuff : Skill{
    //TODO change this to an array so it can suport more than 1 buff
    public BuffStatus buffStatus;

    public enum Stat
    {
        ATTACK,
        DEFENSE,
        SPEED
    }


    public override void HandleAnimation(HandleAnimations handleAnimation)
    {
        handleAnimation.ChangeAnimationState(handleAnimation.MON_BOOST);
    }
    public static string messageBuffSucess;
    public override bool HandleSkill(Skill skill, MonManager ally, MonManager enemy){
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