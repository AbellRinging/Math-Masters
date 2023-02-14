using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : Parent_PlayerScript
{

    Animator animator;
    int isWalkingHash;

    protected override void Custom_Start()
    {
        animator = GetComponent<Animator>();
        // isWalkingHash = Animator.StringToHash("isWalkingFWD");
    }

    // public void Animate()
    // {
    //     bool isWalking = animator.GetBool(isWalkingHash);
    //     bool forwardPressed = Input.GetKey("w");

    //     //if player presses w Key
    //     if (!isWalking && forwardPressed)
    //     {
    //         // then set the isWalking boolean to true
    //         animator.SetBool(isWalkingHash, true);
    //     }

    //     //if player is not pressing w Key
    //     if (isWalking && !forwardPressed)
    //     {
    //         // then set the isWalking boolean to false
    //         animator.SetBool(isWalkingHash, false);
    //     }
    // }
    public void ToggleAnimation(string AnimationName, bool boolean){
        animator.SetBool(AnimationName, boolean);
    }
}
