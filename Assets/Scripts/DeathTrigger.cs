using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    GameObject gameCanvas;

    private void Start()
    {
        gameCanvas = GameObject.FindGameObjectWithTag("GameCanvas");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // child 1 is lose panel
            gameCanvas.transform.GetChild(1).gameObject.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
