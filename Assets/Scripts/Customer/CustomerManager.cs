using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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
	}


	void Update()
	{
//		if (Input.GetKeyUp(KeyCode.A))
//		{
//			ClearOrderPanel();
//			CustomerOrderFood();
//		}
	
	}


	private void ClearOrderPanel()
	{
		if (customerPanel.transform.childCount <= 0)
			return;
		
		for (int i = 0; i < customerPanel.transform.childCount; i++)
		{
			Destroy(customerPanel.transform.GetChild(i).gameObject);
		}
	}
	
	//1 - 9
	//1 - 3 = nnn
	
	
	private void RandomFoodAmount()
	{
		customerOrders = new List<FoodOrder>();

		for (int i = 0; i < 3; i++)
		{
			GameObject spawnOrderPicture = Instantiate(orderImagePrefab);
			spawnOrderPicture.transform.parent = customerPanel.transform;
			spawnOrderPicture.GetComponent<FoodOrder>().SetOrder(i);
			customerOrders[i] = spawnOrderPicture.GetComponent<FoodOrder>();
		}
		
	}
	
	public void RecieveOrder(int id)
	{

		if (customerOrders.Count > 0)
		{
				
			foreach (var item in customerOrders)
			{
				if (item.GetOrderId() == id)
				{
					customerOrders.Remove(item);
					PlayEatingAnimation();
					break;
				}
				
			}
		}
		else
		{
			Payment();
		}
	
	}

	private void OrderingFood()
	{
		if (customerOrders.Count > 0)
			foreach (var item in customerOrders)
			{
				item.SetOrder(GameSceneManager.GetInstance().RandomFoodOrderByOne());
			}
	}

	private void PlayEatingAnimation()
	{
		
	}

	private void Payment() //Need RecievingOrder
	{
		
	}
}
