using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowTrigger : MonoBehaviour
{
    [SerializeField] List<Transform> targets;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Thrower>().StartThrowPhase(targets);
        }
    }
}
