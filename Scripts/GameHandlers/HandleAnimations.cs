using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

[RequireComponent(typeof(Animation))]
public class HandleAnimations : MonoBehaviour
{
   Animation animationMain;
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

}
