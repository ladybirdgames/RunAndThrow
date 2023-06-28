using System;
using System.Collections.Generic;
using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    [SerializeField] GameObject gameCanvas;

 
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // child 0 is win panel
            gameCanvas.transform.GetChild(0).gameObject.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
