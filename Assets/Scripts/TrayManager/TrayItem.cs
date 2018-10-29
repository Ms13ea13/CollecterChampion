using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrayItem : MonoBehaviour
{
    [SerializeField]
    private List<FoodOrder> foodInTrayOrders;
    [SerializeField]
    private GameObject foodInTrayImagePrefab;
    [SerializeField]
    private GameObject trayPanel;

    [SerializeField]
	private List<GameObject> itemInTray;

    [SerializeField]
	private int currentIndex;
	private bool onHold;
	private Vector3 temp;

	void Start()
	{
		currentIndex = 0;
		itemInTray = new List<GameObject>();
	}
	
	public void AddFoodToTray(GameObject food)
	{
		if (itemInTray.Count < 3)
		{
			itemInTray.Add(food);
			food.transform.parent = transform;
			food.GetComponent<Collider>().enabled = false;
			food.transform.localPosition = StackFoodVisually(currentIndex, food.transform);
			food.GetComponent<FoodItem>().SetBannedId(currentIndex);
		}
	}

	public void DeliverFoodViaTray(CustomerManager customer)
	{
		foreach (var item in itemInTray)
		{
			if (customer.RecieveOrder(item.gameObject.GetComponent<FoodItem>().GetFoodItemId()))
			{
				itemInTray.Remove(item);
				Destroy(item.gameObject);
				currentIndex -= 1;
				break;
			}
		}
	}
	
	public void RemoveAllFoodFromTray()
	{
		itemInTray.Clear();
		currentIndex = 0;
	}
	
	public void RemoveFoorFromTray()
	{
		currentIndex -= 1;
	}

	private Vector3 StackFoodVisually(int index, Transform targetTransform)
	{
		 temp = targetTransform.localPosition;
		switch (index)
		{
			case 0:
			{
				temp.y = 4f;
				break;
			}
			case 1:
			{
				temp.y = 10f;
				break;
			}
			case 2:
			{
				temp.y = 16f;
				break;
			}
			default:
				break;
			
		}

		temp.x = 0f;
		temp.z = 0f;
		currentIndex += 1;
		return temp;
	}

	public void SetOnHold(bool hold)
	{
		onHold = hold;
	}

	public bool GetOnHold()
	{
		return onHold;
	}

    public void FoodInTrayAmount()
    {
        GameObject spawnOrderPicture = Instantiate(foodInTrayImagePrefab);
        spawnOrderPicture.transform.parent = trayPanel.transform;
        foodInTrayOrders.Add(spawnOrderPicture.GetComponent<FoodOrder>());
    }
}
