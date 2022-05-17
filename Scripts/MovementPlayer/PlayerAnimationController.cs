using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationController : MonoBehaviour
{   
    
    public PlayerGravity playerGravity;
    public PlayerMovement playerMovement;
    public AnimatorOverrideController overriderController;
    [HideInInspector]public static int PLAYER_RUN =Animator.StringToHash("Running");
    [HideInInspector]public static int PLAYER_IDDLE=Animator.StringToHash("Iddle");
    [HideInInspector]public static int PLAYER_JUMP=Animator.StringToHash("JumpUp_Start");
    [HideInInspector]public static int PLAYER_FALL_LOOP=Animator.StringToHash("JumpUp_Loop");
   

    int currentState = PLAYER_IDDLE;
    Animator anim;
    void Awake()
    {
        anim = GetComponent<Animator>();
        
        if (playerMovement == null)
            playerMovement = GetComponentInParent<PlayerMovement>();

        if (playerGravity == null)
            playerGravity = GetComponentInParent<PlayerGravity>();

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
        if (currentState == state)
             return;

        // If the player is currently jumping no other change can affect this behaviour
        if (currentState == PLAYER_JUMP)
            return;

        anim.CrossFade(state,0.1f);
        //Interpolate Instantaneous
        // anim.Play(state);
        currentState = state;
    }

    public void StopFalling()
    {
        if (currentState != PLAYER_JUMP)
        {
            //If the player is not jumping then there is no need to stop it jumping
            return;
        }
        anim.CrossFade(PLAYER_IDDLE,0.05f);
        currentState = PLAYER_IDDLE;
    }

    public void StartJumping()
    {
        anim.Play(PLAYER_JUMP);
        currentState = PLAYER_JUMP;
    }
}
