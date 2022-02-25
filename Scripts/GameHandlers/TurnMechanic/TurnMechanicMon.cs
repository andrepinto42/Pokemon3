using System;
using System.Threading;
using UnityEditor;
using UnityEngine;

public class TurnMechanicMon : MonoBehaviour
{
    public static int turnStage = 0;
    public delegate void OnTurnOver();
    public static event OnTurnOver onTurnOver;

    HandleAnimations handleAnimations;
    public delegate void OnAttackOver();
    public event OnAttackOver onAttackOver;

    void Awake()
    {
        handleAnimations= GetComponent<HandleAnimations>();
    }

    void Start()
    {
        AddResetToIdleEventToAnimation(handleAnimations.clipAttack);
        AddResetToIdleEventToAnimation(handleAnimations.clipBoost);
        AddResetToIdleEventToAnimation(handleAnimations.clipGettingHit);
    }

    private void AddResetToIdleEventToAnimation(AnimationClip clip)
    {
        AnimationEvent myEvent = new AnimationEvent();
        myEvent.functionName = "ResetToIdle";
        myEvent.time = clip.length;
        myEvent.objectReferenceParameter = this.gameObject;
        clip.AddEvent(myEvent);
    }

    //Function called from the animation attack
    async void  AttackOver(string type)
   {
        if (type == "damage")
            await SkillHandlerDamage.DealDamageAnimationTrigger();
        if (type == "boost")
            await SkillHandlerBuff.DealBuffAnimationTrigger();
        IncrementTurnStage();
   }

    //Function called when the animation stops
   void ResetToIdle()
   {
       handleAnimations.ChangeAnimationState(handleAnimations.MON_IDDLE);
       if (onAttackOver != null)
            onAttackOver();
   }

   public static void IncrementTurnStage()
   {
       turnStage++;
       if (turnStage == 2)
       {
            turnStage =0;
            //Signal that the turn has ended
            if (onTurnOver != null)
            {
                onTurnOver();
            }
       }
   }
}