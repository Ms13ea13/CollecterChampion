using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreManager : MonoBehaviour
{
    public AudioClip score_sound;
    [SerializeField]
	private Text scoreText;
	
	[SerializeField]
	private int score;

	public void AddScoreNumber(int plusScore)
	{
		score += plusScore;
		SetScoreContent(score.ToString());

	}

	private void SetScoreContent(string message)
	{
		scoreText.text = message;
        AudioSource audio = GetComponent<AudioSource>();//
        audio.PlayOneShot(score_sound);//
    }
}
