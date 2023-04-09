using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5;
    [SerializeField] float jumpForce = 6;
    [SerializeField] float rotateDuration = 0.3f;

    [Header("Ground Check Settings")]
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] Vector3 groundCheckOffset;
    [SerializeField] LayerMask groundLayer;

    [SerializeField] GameObject bullet;

    GamePhase gamePhase;

    bool isGrounded;

    Rigidbody rigidBody;
    Animator animator;

    Vector3 currentDirection;
    Vector3 firstMousePos;

    bool hasSwipeRegistered = false;
    bool isRotating;
    bool canThrow = true;

    List<Transform> potentialTargets = new List<Transform>();
    Transform currentTarget;
   
    void Start()
    {
        gamePhase = GamePhase.RunPhase;

        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        //  Time.timeScale = 0.3f;
    }
    void FixedUpdate()
    {
        if (gamePhase == GamePhase.RunPhase)
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
                if (gamePhase == GamePhase.RunPhase && isGrounded)
                {
                    Jump();
                }
                else if (gamePhase == GamePhase.ThrowPhase && !isRotating && canThrow)
                {
                    RotateToThrow();
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
    public void StartThrowPhase(List<Transform> newTargets)
    {
        gamePhase = GamePhase.ThrowPhase;
        potentialTargets.AddRange(newTargets);

        bullet.SetActive(true);

        animator.SetBool("isIdle", true);
        animator.SetBool("isRunning", false);
        rigidBody.velocity = Vector3.zero;
    }
    private void RotateToThrow()
    {
        currentTarget = potentialTargets[Random.Range(0, potentialTargets.Count)];
        potentialTargets.Remove(currentTarget);
        Vector3 throwDir = currentTarget.transform.position - transform.position;
        throwDir.y = 0;
        Quaternion targetRotaion = Quaternion.LookRotation(throwDir);
        StartCoroutine(Rotate(rotateDuration, targetRotaion));
        animator.SetBool("isThrowing", true);
        canThrow = false;
    }

    // animator call
    public void Throw()
    {
        bullet.transform.SetParent(null);
        bullet.transform.GetChild(0).gameObject.SetActive(true);
        bullet.GetComponent<Bullet>().SetTarget(currentTarget);
        bullet.GetComponent<Bullet>().enabled = true;
        animator.SetBool("isThrowing", false);
    }
    private IEnumerator Rotate(float duration, Quaternion rotation)
    {
        if (isRotating)
        {
            yield break;
        }

        isRotating = true;
        Quaternion currentRot = transform.rotation;

        float counter = 0;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(currentRot, rotation, counter / duration);
            yield return null;
        }
        isRotating = false;
    }
    private void ActivateBullet()
    {
        bullet.SetActive(true);
    }
    private void RotateToContinueRunning()
    {
        gamePhase = GamePhase.RunPhase;
        StartCoroutine(Rotate(rotateDuration, Quaternion.Euler(0, 90, 0)));
        animator.SetBool("isRunning", true);
        animator.SetBool("isIdle", false);
    }
    public void CheckForRunPhase()
    {
        if (potentialTargets.Count <= 0)
        {
            RotateToContinueRunning();
        }
        else
        {
            ActivateBullet();
        }
        canThrow = true;
    }
    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius);
    //}
}
