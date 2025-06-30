using System;
using TMPro;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private float timeBetweenScoreIncrease = 1.85f;

    private int currentScore;

    private int highScore;  

    private bool isPlaying = false;

    private DateTime nextScoreIncrease;

    private void OnEnable()
    {
        Player.OnBirdStateChange += Player_OnBirdStateChange;
    }

    private void OnDisable()
    {
        Player.OnBirdStateChange -= Player_OnBirdStateChange;
    }

    private void Awake()
    {
        highScore = PlayerPrefs.GetInt("Highscore");
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetInt("Highscore", highScore);
    }

    private void Update()
    {
        if(isPlaying)
        {
            if (DateTime.Now >= nextScoreIncrease)
            {
                currentScore++;
                nextScoreIncrease = DateTime.Now + TimeSpan.FromSeconds(timeBetweenScoreIncrease);

                AudioManager.Instance.PlayPointsGainedClip();
            }
        }

        UpdateScoreText(currentScore);
    }

    private void Player_OnBirdStateChange(BirdState newState)
    {
        isPlaying = newState == BirdState.Flying;

        if (newState == BirdState.Idle)
            currentScore = highScore;

        if(newState == BirdState.Dead)
        {
            if (currentScore > highScore)
                highScore = currentScore;
        }

        if (newState == BirdState.Flying)
        {
            nextScoreIncrease = DateTime.Now + TimeSpan.FromSeconds(timeBetweenScoreIncrease);
            currentScore = 0;
        }
    }

    private void UpdateScoreText(int score)
    {
        var scoreMessage = "";
        foreach (var character in currentScore.ToString())
        {
            scoreMessage += $"<sprite name={'"'}{character}{'"'}>";
        }
        scoreText.text = scoreMessage;
    }
}
