using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using System.Linq;

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
	private Sprite[] satusImpages;

	[SerializeField]
	private GameObject customerPanel;

	
	[SerializeField]
	private bool correctOrder;
	
	void Start()
	{
		RandomFoodAmount();
		OrderingFood();
	}


	void Update()
	{
		if (Input.GetKeyUp(KeyCode.I))
		{
			ClearAllOrderPanel();
			RandomFoodAmount();
			OrderingFood();
		}
		CheckOrderAmount();
	}


	private void ClearAllOrderPanel()
	{
		if (customerPanel.transform.childCount <= 0)
			return;
		
		for (int i = 0; i < customerPanel.transform.childCount; i++)
		{
			Destroy(customerPanel.transform.GetChild(i).gameObject);
		}
	}
	
	private void RandomFoodAmount()
	{
		customerOrders = new List<FoodOrder>();
		int foodAmoun = Random.Range(1, 4);
		
		for (int i = 0; i < foodAmoun; i++)
		{
			GameObject spawnOrderPicture = Instantiate(orderImagePrefab);
			spawnOrderPicture.transform.parent = customerPanel.transform;
			customerOrders.Add(spawnOrderPicture.GetComponent<FoodOrder>());
		}
		
	}
	
	public bool RecieveOrder(int id)
	{
		if (customerOrders.Count > 0)
		{
			foreach (var item in customerOrders)
			{
				if (item.GetOrderId() == id)
				{
					customerOrders.Remove(item);
					Payment(item.GetOrderPrice());	
					Destroy(item.gameObject);
					PlayEatingAnimation();
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

	private void CheckOrderAmount()
	{
		if (customerOrders.Count == 0)
		{
			PlayerWalkOutAnimation();
		}
	}
	
	

	private void OrderingFood()
	{
		if (customerOrders.Count > 0)
		{
			foreach (var item in customerOrders)
			{
				item.SetOrder(GameSceneManager.GetInstance().RandomFoodOrderByOne());
			}
		}
			
	}

	private void PlayerWalkOutAnimation()
	{
		//Play walk out animation here
		Destroy(gameObject);

	}

	private void PlayEatingAnimation()
	{
		//Play Eat animation here
		
		//When Eat animation done
		EmptyPlate();
	}

	private void EmptyPlate()
	{
		
	}

	private void Payment(int moneyAmount) 
	{
		//Play coin vfx here
		GameSceneManager.GetInstance().CustomerPayMoneyToStore(moneyAmount);
	}
}
