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
    }

    public void ToggleAnimation(string AnimationName, bool boolean){
        animator.SetBool(AnimationName, boolean);
    }
}
