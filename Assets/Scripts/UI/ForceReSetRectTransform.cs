using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceReSetRectTransform : MonoBehaviour {
	
	void Start()
	{
		if (gameObject.activeInHierarchy)
		{
			GetComponent<RectTransform>().transform.localScale = new Vector3(1,1,1);
			GetComponent<RectTransform>().transform.localRotation = Quaternion.Euler(Vector3.zero);
			Vector3 temp = GetComponent<RectTransform>().transform.localPosition;
			temp.z = 0f;
			GetComponent<RectTransform>().transform.localPosition = temp;
		}
		else
		{
			return;
		}
		
	}
}
