using TMPro;
using UnityEngine;

public class ScorePopupController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI totalScoreText;
    [SerializeField] GameObject scorePopup;
    [SerializeField] float moveSpeed = 500;
    [SerializeField] int scorePerShoot = 100;
    [SerializeField] Bullet bullet;

    public static int totalScore = 0;

    Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        bullet.OnHit += ActivateScorePopup;
    }
    private void OnDisable()
    {
        bullet.OnHit -= ActivateScorePopup;
    }
    void ActivateScorePopup(Vector3 position)
    {
        scorePopup.SetActive(true);
        scorePopup.transform.position = mainCamera.WorldToScreenPoint(position);
    }
    void LateUpdate()
    {
        if (scorePopup.activeSelf)
        {
            scorePopup.transform.position = Vector3.MoveTowards(scorePopup.transform.position, totalScoreText.transform.position, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(scorePopup.transform.position, totalScoreText.transform.position) < 50)
            {
                IncreaseScore();
                scorePopup.SetActive(false);
                bullet.GetBackToPlayerCam();
            }
        }
    }

    public void IncreaseScore()
    {
        totalScore += scorePerShoot;
        totalScoreText.text = totalScore.ToString();
    }
}
