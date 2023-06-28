using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrower : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] float rotateDuration = 0.3f;
    [SerializeField] GameObject bullet;

    Rigidbody rigidBody;
    Animator animator;

    bool isRotating;

    List<Transform> potentialTargets = new List<Transform>();
    Transform currentTarget;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        player.OnThrowPhase += RotateToThrow;
    }
    private void OnDisable()
    {
        player.OnThrowPhase -= RotateToThrow;
    }
    public void StartThrowPhase(List<Transform> newTargets)
    {
        player.IsRunning = false;
        potentialTargets.AddRange(newTargets);

        bullet.SetActive(true);

        animator.SetBool("isIdle", true);
        animator.SetBool("isRunning", false);
        rigidBody.velocity = Vector3.zero;
    }
    public void RotateToThrow()
    {
        currentTarget = potentialTargets[Random.Range(0, potentialTargets.Count)];
        potentialTargets.Remove(currentTarget);
        Vector3 throwDir = currentTarget.transform.position - transform.position;
        throwDir.y = 0;
        Quaternion targetRotaion = Quaternion.LookRotation(throwDir);
        StartCoroutine(Rotate(rotateDuration, targetRotaion));
        animator.SetBool("isThrowing", true);
        player.CanThrow = false;
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
        player.IsRunning = true;
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
        player.CanThrow = true;
    }

}
