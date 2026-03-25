using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public Text pointsText;
    public Text highScoreListText;

    public void SetUp(int score)
    {
        HighScore.TRY_SET_HIGH_SCORE(score);

        gameObject.SetActive(true);
        pointsText.text = "Score: " + score.ToString("#,0");

        if (highScoreListText == null)
        {
            GameObject highScoreListGO = GameObject.Find("HighScoreList");
            if (highScoreListGO != null)
            {
                highScoreListText = highScoreListGO.GetComponent<Text>();
            }
        }

        if (highScoreListText != null)
        {
            highScoreListText.text = HighScore.GetLeaderboardText();
        }

    }
    public void RestartButton()
    {
        SceneManager.LoadScene("__Scene_0");
    }
    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
