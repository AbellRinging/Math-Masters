using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : Parent_PlayerScript
{

    private Animator animator;

    protected override void Custom_Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ToggleAnimation(string AnimationName, bool boolean)
    {
        animator.SetBool(AnimationName, boolean);
    }
}
