using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public Transform groundCheck;
    public LayerMask groundMask;
    
    [Space(10)]
    public float MoveSpeed = 6f;
    public float SprintSpeed = 12f;
    public float jumpForce = 10f;
    public float LookSensitivity = 0.001f;
    public int jumpAmount = 2;
    
    [Space(10)]
    public float turnSmoothTime = 0.1f;
    public float gravityMultiplier = 1f;
    public float groundDistance = 0.4f;

    CharacterController controller;
    PlayerAnimator PA;
    PlayerCameraController PCC;
    float gravity = -9.81f;
    float turnSmoothVelocity;
    bool isGrounded = true;
    Vector3 direction;
    Vector3 velocity;
    Vector2 look;
    int jumpsLeft;
    float speed;
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
        PA = GetComponent<PlayerAnimator>();
        PCC = GetComponent<PlayerCameraController>();
        
        gravity *= gravityMultiplier;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Change speed with sprint
        speed = MoveSpeed;
        if (Input.GetButton("Sprint"))
        {
            speed = SprintSpeed;
        }
        

        // Check if player is grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            jumpsLeft = jumpAmount;
            velocity.y = -2f;
        }
        

        // Move with old Input system
        direction.x = Input.GetAxisRaw("Horizontal");
        direction.z = Input.GetAxisRaw("Vertical");
        direction = direction.normalized;
        
        if (direction.magnitude >= 0.1f)
        {
            // Rotate with camera
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + PCC.cam.transform.eulerAngles.y;
            float anglef = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, anglef, 0f);
                                
            // Apply Rotation
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        // Jump when "Jump" and "isGrounded" with jumpForce
        if (jumpsLeft >= 1 && Input.GetButtonDown("Jump"))
        {
            // Double jump animation
            if (jumpsLeft < jumpAmount)
            {
                PA.DoubleJumpAnim();
            }
            else
            {
                PA.JumpAnim();
            }
            
            velocity.y = jumpForce;
            jumpsLeft--;
            isGrounded = false;
        }
        
        // Fall based on gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        if (velocity.y <= 5)
        {
            //Play falling animation
        }
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundDistance);   
    }
}
