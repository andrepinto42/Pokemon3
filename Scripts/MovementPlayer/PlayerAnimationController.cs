using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationController : MonoBehaviour
{   
    public PlayerMovement playerMovement;
   public AnimatorOverrideController overriderController;
   [HideInInspector]public static int PLAYER_RUN =Animator.StringToHash("Running");
   [HideInInspector]public static int PLAYER_IDDLE=Animator.StringToHash("Iddle");
   
   int currentState = PLAYER_IDDLE;
   Animator anim;
  
   void Awake()
   {
        anim = GetComponent<Animator>();
        
        if (playerMovement == null)
            playerMovement = GetComponentInParent<PlayerMovement>();

        playerMovement.onRunEvent += SetRunState;
        playerMovement.onStopEvent += SetIddleState;

   }
    private void OnDisable() {
        playerMovement.onRunEvent -= SetRunState;
        playerMovement.onStopEvent -= SetIddleState;
   }
   void Start()
   {
       anim.runtimeAnimatorController = overriderController; 
   }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space))
            SetRunState();
    }
    private void SetRunState()
    {
        ChangeAnimationState(PLAYER_RUN);
    }

    private void SetIddleState()
    {
        ChangeAnimationState(PLAYER_IDDLE);
    }
   public void ChangeAnimationState(int state)
   {
        if (currentState.Equals(state))
            return;
        Debug.Log("Switched to this new state " + state);
        anim.Play(state);
        currentState = state;
   }
}
