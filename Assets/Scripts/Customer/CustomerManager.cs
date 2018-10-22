using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;

public class CustomerManager : MonoBehaviour
{

	[SerializeField]
	private FoodOrder[] customerOrders;


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
		orderLength = Random.Range(1, 4);
		customerOrders = new FoodOrder[orderLength];

		for (int i = 0; i < customerOrders.Length; i++)
		{
			GameObject spawnOrderPicture = Instantiate(orderImagePrefab);
			spawnOrderPicture.transform.parent = customerPanel.transform;
			customerOrders[i].SetOrder(i+1);
		}
		
	}
	
	public bool RecieveOrder(int id) //Need Player inventory
	{
		return false;
	}

	private void OrderingFood()
	{
		
	}

	private void Payment() //Need RecievingOrder
	{
		if (correctOrder)
		{
			//animation & delay payment here
		}
	}
}

[Serializable]
public struct FoodOrder
{
	[SerializeField]
	private int orderId;
	[SerializeField]
	private string orderName;

	public void SetOrder(int id)
	{
		orderId = id;
		orderName = GameSceneManager.GetInstance().GetStringSomething(id);

	}

	
	
	public string GetFoodName()
	{
		return orderName;
	}
	public int GetOrderId()
	{
		return orderId;
	}
}
