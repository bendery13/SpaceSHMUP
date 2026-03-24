using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{
    private static ScoreCounter S;

    [Header("Dynamic")]
    public int score = 0;
    private Text uiText;

    void Awake()
    {
        S = this;
    }

    // Start is called before the first frame update
    void Start()
    {
     uiText = GetComponent<Text>();   
    }

    // Update is called once per frame
    void Update()
    {
        uiText.text = score.ToString("#,0");
    }

    public static void AddScore(int amount)
    {
        if (S == null)
        {
            Debug.LogWarning("ScoreCounter.AddScore called but no ScoreCounter instance exists in the scene.");
            return;
        }

        S.score += amount;
    }

    public static int SCORE
    {
        get
        {
            if (S == null) return 0;
            return S.score;
        }
    }
}
