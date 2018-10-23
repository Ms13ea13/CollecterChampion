using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodItem : MonoBehaviour
{
	[SerializeField]
	private int foodID;
	
	[SerializeField]
	private string FoodName;

	public int GetFoodItemId()
	{
		return foodID;
	}

	public void SetFoodItemId(int id)
	{
		foodID = id;
	}

	public string GetFoodItemName()
	{
		return FoodName;
	}

	public void SetFoodItemName(string name)
	{
		FoodName = name;
	}


}
