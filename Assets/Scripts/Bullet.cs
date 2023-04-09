using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] GameObject scorePopup;
    [SerializeField] Transform bulletParent;
    [SerializeField] private float throwDuration = 0.5f;

    Rigidbody rigidBody;

    Transform currentTarget;
    FollowBullet followBullet;

    private void OnEnable()
    {
        if (followBullet == null)
        {
            followBullet = Camera.main.GetComponent<FollowBullet>();
        }
        followBullet.SetBullet(transform);
        followBullet.enabled = true;
    }
    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        MoveWithVelocity((currentTarget.position - transform.position) / throwDuration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target"))
        {
            PerformHitActions(other);
        }
    }

    private void PerformHitActions(Collider target)
    {
        target.GetComponent<MeshRenderer>().material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1);
        target.gameObject.GetComponent<BoxCollider>().enabled = false;
        scorePopup.SetActive(true);
        scorePopup.transform.position = Camera.main.WorldToScreenPoint(target.transform.position);

        transform.parent = bulletParent;
        gameObject.SetActive(false);
    }

    private void MoveWithVelocity(Vector3 velocity)
    {
        rigidBody.velocity = velocity;
    }
   
    public void SetTarget(Transform target)
    {
        currentTarget = target;
    }

    private void OnDisable()
    {
        ResetBullet();
        if (followBullet != null)
        {
            followBullet.StartCameraGetBackCoroutine();
        }
        this.enabled = false;
    }

    // Reset bullet to use it again
    private void ResetBullet()
    {
        transform.localPosition = Vector3.zero;
        SetTarget(null);
        rigidBody.velocity = Vector3.zero;
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
