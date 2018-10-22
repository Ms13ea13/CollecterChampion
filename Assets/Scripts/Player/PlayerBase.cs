using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
	[SerializeField]
	private int id;

	[SerializeField]
	private int score;

	[SerializeField]
	private String playerName;

	protected CharacterController playercontrol;
	protected Rigidbody rigibody;
	[SerializeField]
	protected Vector3 velocity;

	public Vector3 drag;
	public float gravity;
	public float speed;

	public  int GetScore()
	{
		return score;
	}

	public  void SetScore(int plusScore)
	{
		score += plusScore;
	}

	public int GetIdPlayer()
	{
		return id;
	}

	public string GetPlayerName()
	{
		return playerName;
	}
}
