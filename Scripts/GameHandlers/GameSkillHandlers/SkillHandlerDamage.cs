using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using UnityEngine;
public class SkillHandlerDamage : ISkillHandler 
{
    static Dictionary<float,string> mapEffective = new Dictionary<float,string>(){
        {0.5f,"It's not very effective..."},
        {1f,"It's ok"},
        {2f,"It's sooo effective !"}
    };
    static float damageStored;
    static MonManager enemyStored;
    static float advantageStored;
    static AudioClip soundEffectStored; 
    public bool HandleSkill(Skill skill, MonManager ally, MonManager enemy){
      
        SkillDamage skillDamage = (SkillDamage) skill;
        
        float damage =MonCalculateDamage.CalculateDamage(skillDamage,ally.MonMain,enemy.MonMain);

        float advantage = MonTypes.GetAdvantage(skillDamage.type,enemy.MonMain.type);

        
        float healthAfterDamage = enemy.GetCurrentHealthAfterDamage(damage);
       
        advantageStored = advantage;
        damageStored = damage;
        enemyStored = enemy;
        soundEffectStored = skill.soundEffect;
        
        //Return if the mon will be killed after taking damage
        return (healthAfterDamage<=0);
    }

    public void HandleAnimation(HandleAnimations handleAnimation)
    {
        handleAnimation.ChangeAnimationState(handleAnimation.MON_ATTACK);
    }

    //Called when the attack animation is over
    public static async Task DealDamageAnimationTrigger()
    {
        
        //Play the audio before the frame Event is activated
        BattleAudioManager.Singleton.PlayAudio(soundEffectStored);

        //Play audio effect
        if (advantageStored > 1)
        BattleAudioManager.Singleton.PlayAudio(BattleAudioManager.Singleton.superEffectiveHit);
        if (advantageStored < 1)
        BattleAudioManager.Singleton.PlayAudio(BattleAudioManager.Singleton.lowEffectiveHit);


        //Play the particles effects 
        GameVisualEffectsHandler.Singleton.StartEmittingDamageParticle(enemyStored,damageStored);

        Task[] arr = new Task[2];
        arr[0] = LowerHealthBar(enemyStored,damageStored);
        arr[1] = TextDialogManager.Singleton.PushText(mapEffective[advantageStored],500); 
        
        enemyStored.TakeDamage(damageStored);
        
        await Task.WhenAll(arr);
        GameVisualEffectsHandler.Singleton.StopEmittingDamageParticle();
        
        //Reset the static variables
        soundEffectStored = null;
        enemyStored = null;
        advantageStored = 0f;
        damageStored = 0f;

    }
    const int TIME_TO_LOWER_HEALTH_BAR = 1000;
    private static async Task LowerHealthBar(MonManager enemy,float damageTaken)
    {
        var manager = enemy.GetMonMeshManager();
        float currentHP = enemy.MonMain.currentHealth;
        float maxHP = enemy.MonMain.maxHealth;
        if (damageTaken> currentHP)
        {
            //Mon is going to die, so to overflow the bar with excess damage
            damageTaken = currentHP;
        }
        //The bigger the gap between the damageTaken and its HP the faster the animation will load
        float speedUp = damageTaken/currentHP + 1f;
        //Cerca de 1s para a barra baixar
        int ticks = (int) (20 / speedUp);
        
        int delay = Math.Min(  (int) (TIME_TO_LOWER_HEALTH_BAR / speedUp), TIME_TO_LOWER_HEALTH_BAR / ticks);
        for (int i = 0; i <= ticks; i++)
        {
            manager.UpdateHealth(currentHP- damageTaken*(i/(float)ticks),maxHP);
            await Task.Delay(delay);        
        }

    }
}