using TMPro;
using UnityEngine;

public class ScorePopupController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI totalScoreText;
    [SerializeField] float moveSpeed = 500;
    [SerializeField] int scorePerShoot = 100;

    public static int totalScore = 0;

  
    void LateUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, totalScoreText.transform.position, moveSpeed * Time.deltaTime);

        if(Vector3.Distance(transform.position, totalScoreText.transform.position) < 50)
        {
            IncreaseScore();
            gameObject.SetActive(false);
        }
    }

    private void IncreaseScore()
    {
        totalScore += scorePerShoot;
        totalScoreText.text = totalScore.ToString();
    }
}
