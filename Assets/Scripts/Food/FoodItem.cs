using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodItem : MonoBehaviour
{
	[SerializeField]
	private int foodID;
	
	[SerializeField]
	private string FoodName;

	[SerializeField]
	private int trayBandedId;

	public void SetUpFoodItem(int id)
	{
		foodID = id;
		FoodName = GameSceneManager.GetInstance().GetFoodNameById(foodID);
	}

	public int GetFoodItemId()
	{
		return foodID;
	}

	public string GetFoodItemName()
	{
		return FoodName;
	}

	public int GetBanedID()
	{
		return trayBandedId;
	}

	public void SetBannedId(int id)
	{
		trayBandedId = id;
	}
}
