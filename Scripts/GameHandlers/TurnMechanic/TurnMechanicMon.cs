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
        Debug.Log("Going to use "+skill.nameSkill);
        
        await skill.ApplyAnimationTrigger();

        print("Current Stage + "+ turnStage);
        IncrementTurnStage();
   }

    //Function called when the animation stops
   void ResetToIdle()
   {
       handleAnimations.ChangeAnimationState(handleAnimations.MON_IDDLE);
    //Not a good ideia that when a animations stops it should decide that the turn has ended
    //    if (onAttackOver != null)
    //         onAttackOver();
   }

   public void IncrementTurnStage()
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
       else
       {
           //Signal that the first Mon has finished attacking
            if (onAttackOver != null) 
                onAttackOver();
       }
   }

   public void ResetTurnStage()
   {
       turnStage = 0;
   }
}