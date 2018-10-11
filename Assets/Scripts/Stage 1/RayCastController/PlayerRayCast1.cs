using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.WSA.Input;

public class PlayerRayCast1 : MonoBehaviour
{
	private GameObject temp;
	private GameObject currentItemInFront;
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
		if (!temp)
			ResetHolding();

		if (holding)
		{
			DropOBj(ref temp);
		}
		else
		{
			PickUpObj(CapsuleCast(holding));
		}
		
	}

	private GameObject CapsuleCast(bool hold)
	{
		if (hold)
			return null;
		
		
		RaycastHit hit;
		Vector3 p1 = transform.position + charContr.center + Vector3.up * -charContr.height * 0.5F;
		Vector3 p2 = p1 + Vector3.up * charContr.height;
		distanceToObstacle = 0;

		// Cast character controller shape 10 meters forward to see if it is about to hit anything.
		if ((Physics.CapsuleCast(p1, p2, charContr.radius, transform.forward, out hit, 10) &&
		    hit.transform.tag == "Item1") || (Physics.CapsuleCast(p1, p2, charContr.radius, transform.forward, out hit, 10) &&
            hit.transform.tag == "Item2") || (Physics.CapsuleCast(p1, p2, charContr.radius, transform.forward, out hit, 10) &&
            hit.transform.tag == "Item3"))
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

	private void PickUpObj(GameObject target)
	{
		if (target != null && !holding)
		{
			if (Input.GetButtonDown("PickupJoy1"))
			{
				target.transform.parent = transform;
				target.GetComponent<MoveItem>().enabled = false;
				temp = target;
				holding = true;
			}
		}
	}

	private void DropOBj(ref GameObject target)
	{
		if (target && holding)
		{
			if (Input.GetButtonDown("ThrowJoy1"))
			{
				target.transform.parent = null;
				target.GetComponent<Target>().Throw(20f);

				ResetHolding();

			}
		}
		else
		{
//			Debug.LogError("target : " + target + " hold : " + holding);
		}
	}

	private void ResetHolding()
	{
		currentItemInFront = null;
		temp = null;
		holding = false;
	}
	
}
