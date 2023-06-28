using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CMCamerasController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera playerCam;
    [SerializeField] CinemachineVirtualCamera bulletCam;

    bool isPlayerCam = true;

    public void SwitchCamera()
    {
        if (isPlayerCam)
        {
            playerCam.Priority = 1;
            bulletCam.Priority = 2;
        }
        else
        {
            playerCam.Priority = 2;
            bulletCam.Priority = 1;
        }
        isPlayerCam = !isPlayerCam;
    }

    public void SetupBulletCam(Transform target)
    {
        bulletCam.Follow = target;
        bulletCam.LookAt = target;
    }
}
