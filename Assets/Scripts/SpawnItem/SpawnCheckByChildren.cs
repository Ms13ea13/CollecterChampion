using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCheckByChildren : MonoBehaviour
{

	[SerializeField]
	private GameObject prefab;

	void Update()
	{
		SpawnItemOnPosition();
	}

	public void SpawnItemOnPosition()
	{
		if (transform.childCount == 0)
		{
			GameObject spawnGameobj = Instantiate(prefab);
			spawnGameobj.transform.parent = transform;
			Vector3 temp = Vector3.zero;
			temp.y = .5f;
			spawnGameobj.transform.localPosition = temp;
		}
		
	}


}
