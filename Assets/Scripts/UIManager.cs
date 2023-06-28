using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void OnNextLevelPressed()
    {
        PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level") + 1);
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
    public void OnRestartLevelPressed()
    {
        SceneManager.LoadScene(0);
        ScorePopupController.totalScore = 0;
        Time.timeScale = 1;
    }
}
