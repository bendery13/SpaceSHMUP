using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesDisplay : MonoBehaviour
{
    [Header("Dynamic")]
    public int lives = 0;
    private Text uiText;

    void Start()
    {
        uiText = GetComponent<Text>();
    }

    void Update()
    {
        lives = Main.LIVES;
        uiText.text = "Lives: \t\t\tx" + lives.ToString();
    }
}
