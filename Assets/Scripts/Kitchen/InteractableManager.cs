﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableManager : MonoBehaviour {

	public virtual void Interact (GameObject target , ref bool holding)
	{
		
	}

	public void SetTargetPosition(Transform targetTransform)
	{
		targetTransform.parent = transform;
		Vector3 temp = targetTransform.localPosition;
		temp.y = 0.139f;
		temp.x = 0;
		temp.z = 0;
		targetTransform.localPosition = temp;
		Quaternion tempQuaternion = new Quaternion(0f, 0f, 0f, 0f);
		targetTransform.localRotation = tempQuaternion;
	}
}
