using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreManager : MonoBehaviour
{
    public AudioClip score_sound;

    [SerializeField] private Text scoreText;

    [SerializeField] private Text totalScoreText;

    [SerializeField] private int score;

    [SerializeField] private StarManager plusStar;

    [SerializeField] private int minScore;
    [SerializeField] private int middleScore;
    [SerializeField] private int maxScore;

    private AudioSource ScoreMAudioSource;

    void Start()
    {
        ScoreMAudioSource = GetComponent<AudioSource>();
    }

	public void AddScoreNumber(int plusScore)
	{
		score += plusScore;
		SetScoreContent(score.ToString());
        
        CheckScore(score);
    }

    public void RemoveScoreNumber(int minusScore)
    {
        score -= minusScore;
        SetScoreContent(score.ToString());

        CheckScore(score);
    }

    private void SetScoreContent(string message)
	{
		scoreText.text = message;
        totalScoreText.text = message;

        ScoreMAudioSource.PlayOneShot(score_sound);//
    }

    public void CheckScore(int totalScore)
    {
        if (totalScore >= minScore && totalScore < middleScore)
        {
            plusStar.ResetStars();
            plusStar.StarCollect(0);
        }
        else if (totalScore >= middleScore && totalScore < maxScore)
        {
            plusStar.ResetStars();
            plusStar.StarCollect(1);
        }
        else if (totalScore >= maxScore)
        {
            plusStar.ResetStars();
            plusStar.StarCollect(2);
        }
    }
}
