using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
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
	}
}
