using UnityEngine;

public class BinManager : MonoBehaviour
{
	public void ThrowItemToBin(GameObject target)
	{
		if (!target)
			return;

		PlayFoodGoDownTrash(target);
	}

	private void PlayFoodGoDownTrash(GameObject item)
	{
		//Set fall down position here
		Destroy(item);
	}
}
