using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBullet : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] private Vector3 offset;
    [SerializeField] float cameraFollowSpeed = 5f;
    [SerializeField] float getBackSpeed = 2f;


    Transform currentBullet;

    bool shouldFollowPlayer;

    void LateUpdate()
    {
        if (shouldFollowPlayer)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, getBackSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.localPosition, Vector3.zero) < 1)
            {
                player.CheckForRunPhase();

                shouldFollowPlayer = false;
                this.enabled = false;
            }
        }
        if (!currentBullet.gameObject.activeSelf)
        {
            return;
        }
        Vector3 newPosition = currentBullet.position + offset;
        transform.position = Vector3.Lerp(transform.position, newPosition, cameraFollowSpeed * Time.deltaTime);
    }

    IEnumerator GetCameraBackToPlayer()
    {
        yield return new WaitForSeconds(2f);
        shouldFollowPlayer = true;
    }
    public void StartCameraGetBackCoroutine()
    {
        StartCoroutine(GetCameraBackToPlayer());
    }

    public void SetBullet(Transform bullet)
    {
        currentBullet = bullet;
    }
   
}
