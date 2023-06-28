using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5;
    [SerializeField] float jumpForce = 6;

    [Header("Ground Check Settings")]
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] Vector3 groundCheckOffset;
    [SerializeField] LayerMask groundLayer;

    public event Action OnThrowPhase;

    bool isGrounded;

    Rigidbody rigidBody;
    Animator animator;

    Vector3 currentDirection;
    Vector3 firstMousePos;

    bool hasSwipeRegistered = false;

    bool canThrow = true;
    public bool CanThrow { set { canThrow = value; } }

    bool isRunning = true;
    public bool IsRunning { set { isRunning = value; } }

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }
    void FixedUpdate()
    {
        if (isRunning)
        {
            rigidBody.velocity = new Vector3(moveSpeed * Time.deltaTime, rigidBody.velocity.y, 0);
        }
    }
    private void Update()
    {
        GroundCheck();

        if (Input.GetMouseButtonDown(0))
        {
            firstMousePos = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            currentDirection = Input.mousePosition - firstMousePos;

            if (currentDirection.y > 100 && !hasSwipeRegistered)
            {
                if (isRunning && isGrounded)
                {
                    Jump();
                }
                else if (!isRunning && canThrow)
                {
                    OnThrowPhase?.Invoke();
                }
                hasSwipeRegistered = true;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            hasSwipeRegistered = false;
        }
    }

    private void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius, groundLayer);
        animator.SetBool("isGrounded", isGrounded);
        if (isGrounded)
        {
            animator.SetBool("isJumping", false);
        }
        animator.SetBool("isFalling", !isGrounded);
    }
    private void Jump()
    {
        rigidBody.AddForce(new Vector3(0, jumpForce, 0));
        animator.SetBool("isJumping", true);
        animator.SetBool("isGrounded", false);
    }
}
