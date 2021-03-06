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
    //Set to default a buff dedicated for piercing
    public TypeOfDamage typeOfDamage = TypeOfDamage.PIERCE;
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
    [HideInInspector]public  SkillHandlerStatusBuff.DataHandlerStatusBuff dataStatusBuff;
    public override bool HandleSkill(Skill skill, MonManager ally, MonManager enemy){
        //Play the audio when the skill is loaded
        BattleAudioManager.Singleton.PlayAudio(skill.soundEffect);

        //Store the value in the MonManagerForNow

        SkillBuff buffskill = (SkillBuff) skill;
        BuffStatus buffStatus = buffskill.buffStatus;

        //Store it Temporarly for later when the attack is over the message will be displayed to the player
        dataStatusBuff = SkillHandlerStatusBuff.HandleBuff(ally,enemy,buffStatus);
        
        return false;
    }

    public async override Task ApplyAnimationTrigger()
    {
        await SkillHandlerStatusBuff.PushMessage(dataStatusBuff);

        //Clear the variable
        dataStatusBuff = null;
    }    

}