using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreManager : MonoBehaviour
{
    public AudioClip score_sound;

    [SerializeField] private Text scoreText;
	
	[SerializeField] private int score;

    [SerializeField] private int totalScore;

    [SerializeField] private StarManager plusStar;

    private AudioSource ScoreMAudioSource;

	public void AddScoreNumber(int plusScore)
	{
		score += plusScore;
		SetScoreContent(score.ToString());
	    ScoreMAudioSource = GetComponent<AudioSource>();
        CheckScore(score);
    }

	private void SetScoreContent(string message)
	{
		scoreText.text = message;
       
        ScoreMAudioSource.PlayOneShot(score_sound);//
    }

    public void CheckScore(int totalScore)
    {
        if (totalScore >= 100 && totalScore < 120)
        {
            plusStar.ResetStars();
            plusStar.StarCollect(0);
        }
        else if (totalScore >= 120 && totalScore < 150)
        {
            plusStar.ResetStars();
            plusStar.StarCollect(1);
        }
        else if (totalScore >= 150)
        {
            plusStar.ResetStars();
            plusStar.StarCollect(2);
        }
    }
}
