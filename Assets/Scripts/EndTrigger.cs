using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTrigger : MonoBehaviour
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
            // child 0 is win panel
            gameCanvas.transform.GetChild(0).gameObject.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
