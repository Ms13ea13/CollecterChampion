using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.WSA.Input;

public class PlayerRayCast : MonoBehaviour
{
	[SerializeField]
	private GameObject foodItemInHold;
	[SerializeField]
	private GameObject currentItemInFront;

	[SerializeField]
	private GameObject currentCustomer;
    private bool holding;

    [SerializeField]
	private CharacterController charContr;
	
	[SerializeField]
	private float playerSightLength = 10f;

	[SerializeField]
	private float distanceToObstacle;

	void Start()
	{
		holding = false;
	}

	void Update()
	{
		if (!foodItemInHold)
			ResetHolding();

		if (holding)
		{
			DropOBj(ref foodItemInHold);
			currentCustomer = GetCustomerInFront(holding);
		}
		else
		{
			PickUpObj(GetGameObjectInFront(holding));
		}
		
	}

	private GameObject GetGameObjectInFront(bool hold)
	{
		if (hold)
			return null;
		
		
		RaycastHit hit;
		Vector3 p1 = transform.position + charContr.center + Vector3.up * -charContr.height * 0.5F;
		Vector3 p2 = p1 + Vector3.up * charContr.height;
		distanceToObstacle = 0;

		
	
		// Cast character controller shape 10 meters forward to see if it is about to hit anything.
		if ((Physics.CapsuleCast(p1, p2, charContr.radius, transform.forward, out hit, 10) && hit.transform.tag == "Food") )
		{
			distanceToObstacle = hit.distance;
			currentItemInFront = hit.transform.gameObject;
			return hit.transform.gameObject;
			
		}
		else
		{
			currentItemInFront = null;
			return null;
		}
		
	}
	
	private GameObject GetCustomerInFront(bool hold)
	{
		if (!hold)
			return null;
		
		
		RaycastHit hit;
		Vector3 p1 = transform.position + charContr.center + Vector3.up * -charContr.height * 0.5F;
		Vector3 p2 = p1 + Vector3.up * charContr.height;
		distanceToObstacle = 0;

		
	
		// Cast character controller shape 10 meters forward to see if it is about to hit anything.
		if ((Physics.CapsuleCast(p1, p2, charContr.radius, transform.forward, out hit, 10) && hit.transform.tag == "Customer") )
		{
			distanceToObstacle = hit.distance;
			return hit.transform.gameObject;
			
		}
		else
		{
			currentItemInFront = null;
			return null;
		}
		
	}

	private void PickUpObj(GameObject target)
	{
		if (target != null && !holding)
		{
			if (Input.GetKeyUp(KeyCode.Space))
			{
				TakeObjIntoHold(target);
			}
		}
	}

	private void DropOBj(ref GameObject target)
	{
		if (target && holding)
		{
			if (Input.GetKeyDown(KeyCode.B))
			{
				if (currentCustomer == null)
				{
					target.transform.parent = null;
					target.GetComponent<Collider>().enabled = true;
					ResetHolding();
				}
				else
				{
					int id = target.GetComponent<FoodItem>().GetFoodItemId();

					if (GetCustomerInFront(holding).GetComponent<CustomerManager>().RecieveOrder(id))
					{
						Destroy(target);
						ResetHolding();
					}
					else
					{
						target.transform.parent = null;
						target.GetComponent<Collider>().enabled = true;
						ResetHolding();
					}
						
				}
			}
		}
	}

	private void TakeObjIntoHold(GameObject target)
	{
		target.transform.parent = transform;
		foodItemInHold = target;
		foodItemInHold.GetComponent<Collider>().enabled = false;
		holding = true;
	}

	private void ResetHolding()
	{
		currentItemInFront = null;
		foodItemInHold = null;
		holding = false;
	}
	
}
