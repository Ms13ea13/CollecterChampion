using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.WSA.Input;

public class PlayerRayCast : MonoBehaviour
{
	[SerializeField]
	private GameObject itemInHold;
	
	[SerializeField]
	private FoodItem currentFoodInFront;

	[SerializeField]
	private CustomerManager currentCustomerInFront;
	
	[SerializeField]
	private BinManager currentBinInFront;

	[SerializeField]
	private TrayItem currentTrayInFront;

    [SerializeField]
    private FoodStockManager[] foodStockManager;

    [SerializeField]
	private CharacterController charContr;
	
	[SerializeField]
	private float playerSightLength = 10f;

	[SerializeField]
	private float distanceToObstacle;
	
	[SerializeField]
	private bool holding;

    private int foodInHoldId;
	private RaycastHit hit;
	private Vector3 p1;
	private Vector3 p2;

	void Start()
	{
		holding = false;
		distanceToObstacle = 0;
	}

	public void ShootRayCast()
	{
		p1 = transform.position + charContr.center + Vector3.up * -charContr.height * 0.5F;
		p2 = p1 + Vector3.up * charContr.height;
		
		if (holding)
		{
			DropOBj(ref itemInHold);
			GetCustomerInFront();
			GetBinInFront();
		}
		else
		{
			GetTrayHolderInFront();
		}
		
		GetFoodInFront();

		if (Input.GetKeyUp(KeyCode.Space))
		{
            PickUpObj();
		}
	}

	private void GetFoodInFront()
	{
		// Cast character controller shape 10 meters forward to see if it is about to hit anything.
		if ((Physics.CapsuleCast(p1, p2, charContr.radius, transform.forward, out hit, 10) && hit.transform.tag == "Food") )
		{
			distanceToObstacle = hit.distance;
			currentFoodInFront = hit.transform.gameObject.GetComponent<FoodItem>();
		}
		else
		{
			currentFoodInFront = null;
		}
	}
	
	private void GetCustomerInFront()
	{
		// Cast character controller shape 10 meters forward to see if it is about to hit anything.
		if ((Physics.CapsuleCast(p1, p2, charContr.radius, transform.forward, out hit, 10) && hit.transform.tag == "Customer") )
		{
			distanceToObstacle = hit.distance;
			currentCustomerInFront = hit.transform.gameObject.GetComponent<CustomerManager>();
		}
		else
		{
			currentCustomerInFront = null;
		}
		
	}
	
	private void GetBinInFront()
	{
		// Cast character controller shape 10 meters forward to see if it is about to hit anything.
		if ((Physics.CapsuleCast(p1, p2, charContr.radius, transform.forward, out hit, 10) && hit.transform.tag == "Bin") )
		{
			distanceToObstacle = hit.distance;
			currentBinInFront = hit.transform.gameObject.GetComponent<BinManager>();
		}
		else
		{
			currentBinInFront = null;
		}
		
	}
	
	private void GetTrayHolderInFront()
	{
		// Cast character controller shape 10 meters forward to see if it is about to hit anything.
		if ((Physics.CapsuleCast(p1, p2, charContr.radius, transform.forward, out hit, 10) && hit.transform.tag == "Tray") )
		{
			distanceToObstacle = hit.distance;
			currentTrayInFront = hit.transform.gameObject.GetComponent<TrayItem>();
		}
		else
			currentTrayInFront = null;
		
	}

	private void PickUpObj()
	{
		if (holding )
		{
			if (currentTrayInFront)
			{
				if (currentFoodInFront)
                {
                    if (currentTrayInFront.currentIndex < 3)
                    {
                        currentTrayInFront.GetComponent<TrayItem>().AddFoodToTray(currentFoodInFront.gameObject);
                        foodStockManager[currentFoodInFront.GetFoodItemId()].RemoveFoodNumber(1);
                    }
                }
			}
		}
		else
		{
			if (currentTrayInFront)
				TakeObjIntoHold(currentTrayInFront.gameObject);
				
			/*if (currentFoodInFront)
				TakeObjIntoHold(currentFoodInFront.gameObject);*/
		}
	}

	private void DropOBj(ref GameObject target)
	{
		if (target && holding)
		{
			if (Input.GetKeyDown(KeyCode.B))
			{
				if (currentBinInFront)
				{
					currentBinInFront.GetComponent<BinManager>().ThrowItemToBin(itemInHold);
					ResetHolding();
					return;
				}
				
				if (currentCustomerInFront == null && currentBinInFront == null)
				{
					UnHoldItem(target);
				}
				else if (currentCustomerInFront)
				{
					if (target.GetComponent<FoodItem>())
					{
						foodInHoldId = target.GetComponent<FoodItem>().GetFoodItemId();
						if (currentCustomerInFront.RecieveOrder(foodInHoldId))
						{
							Destroy(target);
							ResetHolding();
						}
					}
					else if (target.GetComponent<TrayItem>())
					{
						target.GetComponent<TrayItem>().DeliverFoodViaTray(currentCustomerInFront);
					}
                    else
					{
						UnHoldItem(target);
					}
				}
                else if (currentBinInFront)
				{
					currentBinInFront.GetComponent<BinManager>().ThrowItemToBin(target);
					ResetHolding();
				}
			}
		}
	}

	private void UnHoldItem(GameObject target)
	{
		target.transform.parent = null;
		target.GetComponent<Collider>().enabled = true;
		ResetHolding();
	}
	
	private void TakeObjIntoHold(GameObject target)
	{
		target.transform.parent = transform;
		Vector3 temp = target.transform.localPosition;
		temp.y = 0;
		temp.x = 0;
		temp.z = 1.5f;
		target.transform.localPosition = temp;
		itemInHold = target;
		itemInHold.GetComponent<Collider>().enabled = false;
		holding = true;
	}

	private void ResetHolding()
	{
		currentFoodInFront = null;
		currentCustomerInFront = null;
		currentTrayInFront = null;
		itemInHold = null;
		holding = false;
		currentBinInFront = null;
	}
}
