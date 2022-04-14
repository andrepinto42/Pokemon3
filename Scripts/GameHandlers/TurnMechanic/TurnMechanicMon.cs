using UnityEngine;
using System.Threading.Tasks;

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

    public delegate Task onAttackAnimationOver();
    public event onAttackAnimationOver eventOnAttackAnimationOver;
    //Function called from the animation attack
    async void  AttackOver(string type)
   {
        //Get the last skill used by the player and triggers its effect
        /*
            Maybe the parent doesnt have a MonManager ?
            Normally just displays some text 
            In case of damage this is here the damage effects particles are activated
        */
        Skill skill = GetComponentInParent<MonManager>().lastSkillUsed;
            await skill.ApplyAnimationTrigger();

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