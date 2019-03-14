using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableManager : MonoBehaviour
{
	public virtual void Interact (GameObject target , ref bool holding)
	{
		
	}

	public void SetTargetPosition(Transform targetTransform , bool notSetParent = false)
	{
		targetTransform.parent = transform;
		Vector3 temp = targetTransform.localPosition;
		temp.x = 0;
        temp.y = 0.16f;
        temp.z = 0;
		if (!notSetParent)
		targetTransform.localPosition = temp;
		Quaternion tempQuaternion = new Quaternion(0f, 0f, 0f, 0f);
		targetTransform.localRotation = tempQuaternion;
	}
}
