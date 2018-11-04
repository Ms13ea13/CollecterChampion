using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Linq;
using UnityEngine.Experimental.UIElements;
using Image = UnityEngine.UI.Image;

public class CustomerManager : MonoBehaviour
{
	[SerializeField]
	private List<FoodOrder> customerOrders;

	[SerializeField]
	private int orderLength;

	[SerializeField]
	private Image showStatus;

	[SerializeField]
	private GameObject orderImagePrefab;

	[SerializeField]
	private Sprite[] statusImages;

	[SerializeField]
	private GameObject customerPanel;
    
	[SerializeField]
	private bool correctOrder;
	
	void Start()
	{
		OrderingFood();
	}
	
	private void RandomFoodAmount()
	{
		customerOrders = new List<FoodOrder>();
		int foodAmount = Random.Range(1, 4);
		
		for (int i = 0; i < foodAmount; i++)
		{
			GameObject spawnOrderPicture = Instantiate(orderImagePrefab);
			spawnOrderPicture.transform.parent = customerPanel.transform;
			customerOrders.Add(spawnOrderPicture.GetComponent<FoodOrder>());
		}
	}
	
	public bool RecieveOrder(FoodItem foodRecieve)
	{
		if (customerOrders.Count > 0 && foodRecieve)
		{
			foreach (var item in customerOrders)
			{
				if (item.GetOrderId() == foodRecieve.GetFoodItemId() && foodRecieve.GetCookState())
				{
					customerOrders.Remove(item);
					DelayPayment(item.GetOrderPrice());
					Destroy(item.gameObject);
					return true;
				}
			}
			return false;
		}
		else
		{
			return false;
		}
	}

	private void OrderingFood()
	{
		RandomFoodAmount();
		if (customerOrders.Count > 0)
		{
			foreach (var item in customerOrders)
			{
				item.SetOrder(GameSceneManager.GetInstance().RandomFoodOrderByOne());
			}
		}	
	}

    private void Payment(int moneyAmount) 
	{
		//Play coin vfx here
		GameSceneManager.GetInstance().CustomerPayMoneyToStore(moneyAmount);
	}
	
	void DelayPayment(int moneyAmount)
	{
		var seq = LeanTween.sequence();
		seq.append(3f);
		seq.append(() =>
		{
			Payment(moneyAmount);
			if (customerOrders.Count == 0)
			OrderingFood();
		});

	}
}
