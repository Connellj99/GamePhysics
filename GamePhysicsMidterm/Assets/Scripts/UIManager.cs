using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    GameObject scoreUI;
    GameObject livesUI;
    public float lives = 4.0f;
    public float score = 0;
    private void Start()
    {
        scoreUI = GameObject.Find("ScoreUI");
        livesUI = GameObject.Find("LivesUI");
    }
    void Awake()
    {
        score = 0;

    }

    // Update is called once per frame
    void Update()
    {
        GetScore();
        GetLives();

    }

    private void GetScore()
    {
        
        scoreUI.GetComponent<Text>().text = score.ToString();
    }

    private void GetLives()
    {
        livesUI.GetComponent<Text>().text = "Lives: " + lives.ToString();
    }

    public void IncreaseScoreBig()
    {
        score += 100;
    }
    public void IncreaseScoreSmall()
    {
        score += 50;
    }

    public void DecreseLife()
    {
        lives -= 0.25f;
    }

}
