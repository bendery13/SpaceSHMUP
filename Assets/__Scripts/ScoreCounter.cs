using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{
    private static ScoreCounter S;
    private static int scoreSession = 0;

    [Header("Dynamic")]
    public int score = 0;
    private Text uiText;

    void Awake()
    {
        S = this;
        score = scoreSession;
    }

    // Start is called before the first frame update
    void Start()
    {
     uiText = GetComponent<Text>();   
    }

    // Update is called once per frame
    void Update()
    {
        score = scoreSession;
        uiText.text = score.ToString("#,0");
    }

    public static void AddScore(int amount)
    {
        scoreSession += amount;

        if (S != null)
        {
            S.score = scoreSession;
        }
    }

    public static int SCORE
    {
        get
        {
            return scoreSession;
        }
    }

    public static void ResetScore()
    {
        scoreSession = 0;

        if (S != null)
        {
            S.score = 0;
        }
    }
}
