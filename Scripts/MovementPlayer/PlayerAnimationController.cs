using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animation))]
public class PlayerAnimationController : MonoBehaviour
{
    Animation animationMain;
   
   [SerializeField]public  AnimationClip clipRun;
   [SerializeField]public AnimationClip clipIdle;
   [HideInInspector]public string PLAYER_RUN;
   [HideInInspector]public string PLAYER_IDDLE;
   string currentState = "";
  
   void Awake()
   {
       animationMain = GetComponent<Animation>();
   }
   void Start()
   {
        PLAYER_RUN = AddClip(clipRun);
        PLAYER_IDDLE = AddClip(clipIdle);
        
        ChangeAnimationState(PLAYER_RUN);
   }

   void Update()
   {
       if (Input.GetKeyDown(KeyCode.Space)){
           animationMain.Play(PLAYER_IDDLE);
       }
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
        Debug.Log("Switched to this new state " + state);
        animationMain.Play(state);
        currentState = state;
   }
}
