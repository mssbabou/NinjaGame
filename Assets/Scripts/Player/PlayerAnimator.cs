using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    Animator animator;
    public float speed;
    private int SpeedHash;

    void Start()
    {
        animator = GetComponent<Animator>();

        SpeedHash = Animator.StringToHash("Speed");
    }

    void Update()
    {
        animator.SetFloat(SpeedHash, speed);
    }

    public void DoubleJumpAnim()
    {
        // Player DoubleJump
    }

    public void JumpAnim()
    {
        
    }
}
