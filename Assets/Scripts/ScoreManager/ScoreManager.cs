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

	private void SetScoreContent(string message)
	{
		scoreText.text = message;
       
        ScoreMAudioSource.PlayOneShot(score_sound);//
    }

    public void CheckScore(int totalScore)
    {
        if (totalScore >= 60 && totalScore < 80)
        {
            plusStar.ResetStars();
            plusStar.StarCollect(0);
        }
        else if (totalScore >= 80 && totalScore < 100)
        {
            plusStar.ResetStars();
            plusStar.StarCollect(1);
        }
        else if (totalScore >= 100)
        {
            plusStar.ResetStars();
            plusStar.StarCollect(2);
        }
    }
}
