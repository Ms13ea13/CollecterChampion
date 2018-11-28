using UnityEngine;

public class BinManager : MonoBehaviour
{
	public void ThrowItemToBin(GameObject target)
	{
		if (!target)
			return;

		PlayFoodGoDownTrash(target);
	}

	private void PlayFoodGoDownTrash(GameObject tray)
	{
		//Set fall down position here
		Destroy(tray);
	}
}
