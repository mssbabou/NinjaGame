using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    Animator animator;
    public float speed;
    public float fallingSpeed;
    private int SpeedHash;
    private int FallingSpeedHash;

    void Start()
    {
        animator = GetComponent<Animator>();

        SpeedHash = Animator.StringToHash("Speed");
        FallingSpeedHash = Animator.StringToHash("FallingSpeed");
    }

    void Update()
    {
        animator.SetFloat(SpeedHash, speed);
        animator.SetFloat(FallingSpeedHash, fallingSpeed);
    }

    public void DoubleJumpAnim()
    {
        // Player DoubleJump
    }

    public void JumpAnim()
    {
        
    }
}
