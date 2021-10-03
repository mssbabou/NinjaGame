using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(CharacterController))]
public class PlayerMovementNew : MonoBehaviour
{
    public Transform cam;
    public Transform groundCheck;
    public Transform FollowTarget;
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
    PlayerAnimator pa;
    float gravity = -9.81f;
    float turnSmoothVelocity;
    bool isGrounded = true;
    float isSprinting;
    float isJumping;
    Vector3 direction;
    Vector3 velocity;
    Vector2 look;
    int jumpsLeft;
    float speed;
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
        pa = GetComponent<PlayerAnimator>();
        
        gravity *= gravityMultiplier;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnLook(InputValue value)
    {
        look = value.Get<Vector2>();
    }
    
    public void OnMove(InputValue value)
    {
        direction.x = value.Get<Vector2>().x;
        direction.z = value.Get<Vector2>().y;
    }

    public void OnSprint(InputValue value)
    {
        isSprinting = value.Get<float>();
    }

    public void OnJump(InputValue value)
    {
        isJumping = value.Get<float>();
    }
    
    void Update()
    {
        FollowTarget.transform.rotation *= Quaternion.AngleAxis(look.x * LookSensitivity, Vector3.up);
        FollowTarget.transform.rotation *= Quaternion.AngleAxis(look.y * LookSensitivity, Vector3.right);
        
        Vector3 angles = FollowTarget.transform.localEulerAngles;
        angles.z = 0;

        float angle = FollowTarget.transform.localEulerAngles.x;
        
        if (angle > 180 && angle < 340)
        {
            angles.x = 340;
        }
        else if(angle < 180 && angle > 40)
        {
            angles.x = 40;
        }
        FollowTarget.transform.localEulerAngles = angles;
        
        // Change speed with sprint
        speed = MoveSpeed;
        if (isSprinting == 1)
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

        // Move with new Input system
        direction = direction.normalized;
        
        if (direction.magnitude >= 0.1f)
        {
            // Rotate with camera
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            
            // Apply Rotation
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        // Jump when "Jump" and "isGrounded" with jumpForce
        if (jumpsLeft >= 1 && isJumping == 1)
        {
            // Double jump animation
            if (jumpsLeft < jumpAmount)
            {
                pa.DoubleJumpAnim();
            }
            else
            {
                pa.JumpAnim();
            }
            
            velocity.y = jumpForce;
            jumpsLeft--;
            isGrounded = false;
            isJumping = 0;
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
