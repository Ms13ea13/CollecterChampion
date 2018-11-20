using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCheckByChildren : MonoBehaviour
{
	[SerializeField]
	private GameObject prefab;

    void Start()
	{
		SpawnItemOnPosition();
	}

	public void SpawnItemOnPosition()
	{
		if (transform.childCount == 0)
		{
			GameObject spawnGameobj = Instantiate(prefab);
			spawnGameobj.transform.parent = transform;
            spawnGameobj.transform.localScale = new Vector3(1, 1, 1);
            Vector3 temp = Vector3.zero;
			temp.y = 0.14f;
			spawnGameobj.transform.localPosition = temp;
		}
	}
}
