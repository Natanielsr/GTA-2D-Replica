using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : Weapon
{
    private Animator animator;

    public void SetAnimator(Animator animator)
    {
        this.animator = animator;
    }

    protected override void _interact()
    {
        if (animator == null)
            Debug.LogError("Select the animator using Set animator before call interact");
        animator.SetTrigger("Punching");
    }
}
