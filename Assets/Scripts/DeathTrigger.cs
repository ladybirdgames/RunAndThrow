using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    [SerializeField] GameObject gameCanvas;

   
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Death"))
        {
            // child 1 is lose panel
            gameCanvas.transform.GetChild(1).gameObject.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
