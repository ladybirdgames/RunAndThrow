using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
using Cinemachine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Transform bulletParent;
    [SerializeField] Thrower thrower;

    [SerializeField] CMCamerasController cmCamerasController;

    Transform currentTarget;

    public event Action<Vector3> OnHit;

    private void OnEnable()
    {
        cmCamerasController.SetupBulletCam(transform);
        cmCamerasController.SwitchCamera();
    }
    private void OnDisable()
    {
        cmCamerasController.SetupBulletCam(null);
        this.enabled = false;
    }

    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, currentTarget.position, 2 * Time.deltaTime);
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
        ChangeTargetMaterial(target);

        target.gameObject.GetComponent<BoxCollider>().enabled = false;
        OnHit?.Invoke(target.transform.position);

        ResetBullet();
        gameObject.SetActive(false);
    }
    public void SetTarget(Transform target)
    {
        currentTarget = target;
    }

    // Reset bullet to use it again
    private void ResetBullet()
    {
        transform.parent = bulletParent;
        transform.localPosition = Vector3.zero;
        SetTarget(null);
        transform.GetChild(0).gameObject.SetActive(false);
    }
    public void GetBackToPlayerCam()
    {
        cmCamerasController.SwitchCamera();

        thrower.CheckForRunPhase();
    }
    private void ChangeTargetMaterial(Collider target)
    {
        MaterialPropertyBlock props = new MaterialPropertyBlock();
        props.SetColor("_BaseColor", new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1));
        target.GetComponent<MeshRenderer>().SetPropertyBlock(props);
    }
}
