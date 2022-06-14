using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

[RequireComponent(typeof(Animation))]
public class HandleAnimations : MonoBehaviour
{
   Animation animationMain;
    public Transform HeadTransform;
    public float timeToDealDamageAnimation = 0.1f;
    public float timeToDealScreamAnimation = 0.1f;
   
   [SerializeField]public  AnimationClip clipAttack;
   [SerializeField]public AnimationClip clipIdle;
   [SerializeField]public AnimationClip clipGettingHit;
   [SerializeField]public AnimationClip clipBoost;
   [HideInInspector]public string MON_ATTACK;
   [HideInInspector]public string MON_IDDLE;
   [HideInInspector]public string MON_GET_HIT;
   [HideInInspector]public string MON_BOOST;
   string currentState = "";
  
   void Awake()
   {
        animationMain = GetComponent<Animation>();

        // Add to attack animation the event to trigger damage
        AnimationEvent evt = new AnimationEvent();
        evt.functionName = "AttackOver";
        evt.time = timeToDealDamageAnimation;
        evt.stringParameter = "damage";
        evt.objectReferenceParameter = GetComponent<TurnMechanicMon>();
        clipAttack.AddEvent(evt);

        //The attack animation has to be different from the boost animation
        AnimationEvent evt2 = new AnimationEvent();
        evt2.functionName = "AttackOver";
        evt2.time = timeToDealScreamAnimation;
        evt2.stringParameter = "boost";
        evt2.objectReferenceParameter = GetComponent<TurnMechanicMon>();
        clipBoost.AddEvent(evt2);

   }
   void Start()
   {
        MON_ATTACK = AddClip(clipAttack);
        MON_IDDLE = AddClip(clipIdle);
        MON_BOOST = AddClip(clipBoost);
        MON_GET_HIT = AddClip(clipGettingHit);
        
        ChangeAnimationState(MON_IDDLE);
   }
   private string AddClip(AnimationClip clip)
   {
       animationMain.AddClip(clip,clip.name);
       return clip.name;
   }

   public void ChangeAnimationState(string state)
   {
        if (currentState.Equals(state))
            return;
        
        animationMain.Play(state);
        currentState = state;
   }

    //TODO
//    void LateUpdate()
//    {
//        if (HeadTransform)
//             HeadTransform.RotateAround(this.transform.position,Vector3.up,1f);
//    }

}
