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
    public bool isGrounded;
    
    private int SpeedHash;
    private int FallingSpeedHash;
    private int isGroundedHash;

    void Start()
    {
        animator = GetComponent<Animator>();

        SpeedHash = Animator.StringToHash("Speed");
        FallingSpeedHash = Animator.StringToHash("FallingSpeed");
        isGroundedHash = Animator.StringToHash("IsGrounded");
    }

    void Update()
    {
        fallingSpeed = Mathf.Clamp(fallingSpeed, float.NegativeInfinity, 0);
        
        animator.SetFloat(SpeedHash, speed);
        animator.SetFloat(FallingSpeedHash, fallingSpeed);
        animator.SetBool(isGroundedHash, isGrounded);
    }

    public void DoubleJumpAnim()
    {
        animator.Play("Armature_001|Idle");
    }

    public void JumpAnim()
    {
        animator.Play("Armature_001|Idle");
    }

    public void KatanaHitAnim()
    {
        
    }

    public void ShurikenThrowAnim()
    {
        
    }
}
