using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour
{
    static private Text _UI_TEXT;
    static private int _SCORE = 1000;
    private const int LEADERBOARD_SIZE = 8;
    private const int DEFAULT_SCORE = 1000;
    private const string LEGACY_KEY = "HighScore";
    private const string SCORE_KEY_PREFIX = "HighScore_";
    static private bool _initialized = false;
    static private int[] _scores = new int[LEADERBOARD_SIZE];
    private int _lastDisplayedHighScore = int.MinValue;
    private Text txtCOM;        // txtCom is a reference to this GO's Text component

    void Awake()
    {
        _UI_TEXT = GetComponent<Text>();
        EnsureInitialized();
        RefreshHighScoreText(Mathf.Max(_SCORE, ScoreCounter.SCORE));
    }

    void Update()
    {
        int runtimeHighScore = Mathf.Max(_SCORE, ScoreCounter.SCORE);
        if (runtimeHighScore == _lastDisplayedHighScore) return;

        RefreshHighScoreText(runtimeHighScore);
    }

    static public int SCORE
    {
        get { return _SCORE; }
        set
        {
            _SCORE = value;
            PlayerPrefs.SetInt(LEGACY_KEY, value);
            RefreshHighScoreTextStatic(_SCORE);
        }
    }

    static private void EnsureInitialized()
    {
        if (_initialized) return;

        bool hasAllKeys = true;
        for (int i = 0; i < LEADERBOARD_SIZE; i++)
        {
            if (!PlayerPrefs.HasKey(SCORE_KEY_PREFIX + i))
            {
                hasAllKeys = false;
                break;
            }
        }

        if (!hasAllKeys)
        {
            int seedScore = DEFAULT_SCORE;
            if (PlayerPrefs.HasKey(LEGACY_KEY))
            {
                seedScore = Mathf.Max(DEFAULT_SCORE, PlayerPrefs.GetInt(LEGACY_KEY));
            }

            _scores[0] = seedScore;
            for (int i = 1; i < LEADERBOARD_SIZE; i++)
            {
                _scores[i] = DEFAULT_SCORE;
            }
            SortScoresDesc();
            SaveScores();
        }
        else
        {
            for (int i = 0; i < LEADERBOARD_SIZE; i++)
            {
                _scores[i] = PlayerPrefs.GetInt(SCORE_KEY_PREFIX + i, DEFAULT_SCORE);
            }
            SortScoresDesc();
            SaveScores();
        }

        _SCORE = _scores[0];
        _initialized = true;
    }

    static private void SaveScores()
    {
        for (int i = 0; i < LEADERBOARD_SIZE; i++)
        {
            PlayerPrefs.SetInt(SCORE_KEY_PREFIX + i, _scores[i]);
        }
        PlayerPrefs.SetInt(LEGACY_KEY, _scores[0]);
    }

    static private void SortScoresDesc()
    {
        System.Array.Sort(_scores);
        System.Array.Reverse(_scores);
    }

    private void RefreshHighScoreText(int valueToDisplay)
    {
        _lastDisplayedHighScore = valueToDisplay;
        RefreshHighScoreTextStatic(valueToDisplay);
    }

    static private void RefreshHighScoreTextStatic(int valueToDisplay)
    {
        if (_UI_TEXT != null)
        {
            _UI_TEXT.text = "High Score: " + valueToDisplay.ToString("#,0");
        }
    }

    static public bool TryAddToLeaderboard(int scoreToTry)
    {
        EnsureInitialized();

        if (scoreToTry <= _scores[LEADERBOARD_SIZE - 1])
        {
            return false;
        }

        _scores[LEADERBOARD_SIZE - 1] = scoreToTry;
        SortScoresDesc();
        _SCORE = _scores[0];
        SaveScores();
        RefreshHighScoreTextStatic(_SCORE);
        return true;
    }

    static public string GetLeaderboardText()
    {
        EnsureInitialized();

        string[] lines = new string[LEADERBOARD_SIZE];
        for (int i = 0; i < LEADERBOARD_SIZE; i++)
        {
            lines[i] = _scores[i].ToString("#,0");
        }
        return string.Join("\n", lines);
    }

    static public void TRY_SET_HIGH_SCORE(int scoreToTry)
    {
        TryAddToLeaderboard(scoreToTry);
    }

    [Tooltip("Check this box to reset the high score in PlayerPrefs")]
    public bool resetHighScoreNow = false;

    void OnDrawGizmos()
    {
        if (resetHighScoreNow)
        {
            for (int i = 0; i < LEADERBOARD_SIZE; i++)
            {
                _scores[i] = DEFAULT_SCORE;
            }

            _SCORE = DEFAULT_SCORE;
            SaveScores();
            RefreshHighScoreTextStatic(_SCORE);
            _initialized = true;

            resetHighScoreNow = false;
            Debug.LogWarning("PlayerPrefs high score list reset to 1,000 entries.");
        }
    }
    
}
