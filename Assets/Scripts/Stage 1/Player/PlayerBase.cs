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

	protected CharacterController _playercontrol;
	protected Rigidbody _rigibody;
	protected Vector3 _velocity;

	public Vector3 _drag;
	public float _gravity;
	public float _speed;

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
