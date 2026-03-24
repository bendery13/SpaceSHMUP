using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour
{
    static private Text _UI_TEXT;
    static private int _SCORE = 1000;
    private Text txtCOM;        // txtCom is a reference to this GO's Text component

    void Awake()
    {
        _UI_TEXT = GetComponent<Text>();

        // If the PlayerPrefs has a HighScore , then use it.
        if (PlayerPrefs.HasKey("HighScore"))
        {
            SCORE = PlayerPrefs.GetInt("HighScore");
        }
        // Assign the high score to HighScore
        PlayerPrefs.SetInt("HighScore", SCORE);
    }

    static public int SCORE
    {
        get { return _SCORE; }
        set
        {
            _SCORE = value;
            PlayerPrefs.SetInt("HighScore", value);
            if (_UI_TEXT != null)
            {
                _UI_TEXT.text = "High Score: " + _SCORE.ToString("#,0");
            }
        }
    }
    static public void TRY_SET_HIGH_SCORE(int scoreToTry)
    {
        if (scoreToTry <= SCORE) return;
        
        SCORE = scoreToTry;
    }

    [Tooltip("Check this box to reset the high score in PlayerPrefs")]
    public bool resetHighScoreNow = false;

    void OnDrawGizmos()
    {
        if (resetHighScoreNow)
        {
            PlayerPrefs.SetInt("HighScore", 1000);
            resetHighScoreNow = false;
            Debug.LogWarning("PlayerPrefs HighScore reset to 1,000");
        }
    }
    
}
