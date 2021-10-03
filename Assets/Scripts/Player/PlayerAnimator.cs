using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    Animator animator;
    public bool isWalking = false;
    public bool isRunning = false;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void DoubleJumpAnim()
    {
        animator.Play("Player Flip");
    }

    public void JumpAnim()
    {
        
    }
}
