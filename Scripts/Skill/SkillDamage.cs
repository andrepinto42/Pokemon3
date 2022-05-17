using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;


[CreateAssetMenu(fileName = "Mon", menuName = "Mon/Skill/Damage", order = 1)]
public class SkillDamage : Skill{
    public float damage;

    public BuffStatus[] buffStatus;
    
    //Initialize the type of damage by being the most basic one
    public TypeOfDamage typeOfDamage = TypeOfDamage.PIERCE;


    /*
        LOGIC HANDLER
    */
  
    static Dictionary<float,string> mapEffective = new Dictionary<float,string>(){
        {0f,"It looks like the mon is immune..."},
        {0.25f,"It's barelly scratches..."},
        {0.5f,"It's not very effective."},
        {1f,"It's nice"},
        {2f,"It's sooo effective !"},
        {4f,"It's incredibly overwhelming !!!"}
    };
    static float damageStored;
    static BuffStatus[] statusStored;
    static MonManager enemyStored;
    static MonManager allyStored;
    static float advantageStored;
    static AudioClip soundEffectStored; 
    
    public override void HandleAnimation(HandleAnimations handleAnimation)
    {
        handleAnimation.ChangeAnimationState(handleAnimation.MON_ATTACK);
    }
    public override bool HandleSkill(Skill skill, MonManager ally, MonManager enemy){
        SkillDamage skillDamage = (SkillDamage) skill;
        
        float damage =MonCalculateDamage.CalculateDamage(skillDamage,ally.MonMain,enemy.MonMain,ally,enemy);

        float advantage = MonTypes.GetAdvantage(skillDamage.type,enemy.MonMain.GetTypeMon());

        HelperMonNear.StartGettingNearEnemy(ally,enemy);
        
        float healthAfterDamage = enemy.GetCurrentHealthAfterDamage(damage);

        statusStored = skillDamage.buffStatus;
        advantageStored = advantage;
        damageStored = damage;
        enemyStored = enemy;
        allyStored = ally;
        soundEffectStored = skill.soundEffect;
        
        //Return if the mon will be killed after taking damage
        return (healthAfterDamage<=0);
    }


    //Called when the attack animation is over
    public override async Task ApplyAnimationTrigger()
    {
        
        //Play the audio before the frame Event is activated
        BattleAudioManager.Singleton.PlayAudio(soundEffectStored);

        //TODO find better audio effects for 4x 0.25x and 0x
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

        //Mon goes back to its original place

        GameVisualEffectsHandler.Singleton.StopEmittingDamageParticle();
        
        await HelperMonNear.GoAwayFromEnemy();
        
        //If there is status effects trigger then after dealing damage
        if (statusStored != null)
            for (int i = 0; i < statusStored.Length; i++)
            {
                string messageBuffSucess = SkillHandlerStatusBuff.HandleBuff(allyStored,enemyStored,statusStored[i]);
                await TextDialogManager.Singleton.PushText(messageBuffSucess,500);
            }
        
        //Reset the static variables
        statusStored = null;
        soundEffectStored = null;
        enemyStored = null;
        allyStored = null;
        advantageStored = 0f;
        damageStored = 0f;

    }
    const int TIME_TO_LOWER_HEALTH_BAR = 1000;
    private static async Task LowerHealthBar(MonManager enemy,float damageTaken)
    {
        var manager = enemy.GetMonMeshManager();
        float currentHP = enemy.currentHealth;
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