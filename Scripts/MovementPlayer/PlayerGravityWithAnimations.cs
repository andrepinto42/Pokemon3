using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerGravityWithAnimations : PlayerGravity
{
    [Header("Extra Parameters")]
    public PlayerAnimationController animController;
   public override void Update(){
       base.Update();

        if (!isGrounded)
        {
            //Player is airbourne
            animController.ChangeAnimationState(PlayerAnimationController.PLAYER_FALL_LOOP);
        }
        else
        {
            //Player is not airbourne
            animController.StopFalling();
        }
   }
   protected override  void AddJumpForce()
   {
       animController.StartJumping();
       base.AddJumpForce();
   }
}