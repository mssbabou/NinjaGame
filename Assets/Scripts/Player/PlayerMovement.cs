using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform groundCheck;
    public LayerMask groundMask;
    
    [Space(10)]
    public float MoveSpeed = 6f;
    public float SprintSpeed = 12f;
    public float MoveAcceleration = 0.1f;
    public float jumpForce = 10f;
    public int jumpAmount = 2;

    [Space(10)] 
    public float slideFriction;
    public float turnSmoothTime = 0.1f;
    public float gravityMultiplier = 1f;
    public float groundDistance = 0.4f;

    CharacterController controller;
    PlayerAnimator PA;
    ThirdPersonCameraController PCC;
    float gravity = -9.81f;
    float newGravity;
    float turnSmoothVelocity;
    float moveSmoothVelocity;
    float targetAngle;
    float anglef;
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
        PCC = GetComponent<ThirdPersonCameraController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        newGravity = gravity * gravityMultiplier;

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
        
        // Change speed with sprint
        if (Input.GetButton("Sprint") && direction.magnitude >= 0.1f)
        {
            speed = Mathf.SmoothDamp(speed, SprintSpeed, ref moveSmoothVelocity, MoveAcceleration);
        }
        else if(direction.magnitude >= 0.1f)
        {
            speed = Mathf.SmoothDamp(speed, MoveSpeed, ref moveSmoothVelocity, MoveAcceleration);
        }
        else
        {
            speed = Mathf.SmoothDamp(speed, 0f, ref moveSmoothVelocity, MoveAcceleration);
        }

        speed = speed < 0.002f ? 0f : speed;
        
        
        if (direction.magnitude >= 0.1f)
        {
            // Rotate with camera
            targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + PCC.cam.transform.eulerAngles.y;
            anglef = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, anglef, 0f);
            
        }
        
        // Apply Rotation
        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        controller.Move(moveDir.normalized * speed * Time.deltaTime);
        PA.speed = speed;
        
        // Jump when "Jump" and "isGrounded" with jumpForce
        if (jumpsLeft >= 1 && UnityEngine.Input.GetButtonDown("Jump"))
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
        velocity.y += newGravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        PA.isGrounded = isGrounded;
        PA.fallingSpeed = velocity.y;
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundDistance);   
    }
}
