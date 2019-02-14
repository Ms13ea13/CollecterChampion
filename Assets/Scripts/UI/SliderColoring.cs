using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SliderColoring : MonoBehaviour {

	[SerializeField]
	private Image targetImage;
	
	void Update()
	{
		Slider target = GetComponent<Slider>();
		if (target == null)
		{
			Destroy(this);
		}

		var halfValue = target.maxValue / 2;
			
		if (target.value > (halfValue/2) && target.value < halfValue)
		{
			targetImage.color = Color.yellow;
		}else if (target.value > halfValue)
		{
			targetImage.color = Color.red;
		}
	}
}
