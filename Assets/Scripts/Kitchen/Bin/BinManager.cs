using UnityEngine;

public class BinManager : InteractableManager
{
	public override bool Interact(GameObject target  , ref bool holding, PlayerController player )
	{
		if (!target)
			return false;
		
		PlateItem plateOnHold = target.GetComponent<PlateItem>();
		if (plateOnHold != null)
		{
                        
			if (plateOnHold.ItemInPlate().Count > 0)
			{
				plateOnHold.ClearAllItemInPlate();
			}
			else
				return false;
		}
		else
		{
			ThowItemToTrash(target);
		}

		return true;
	}

	private void ThowItemToTrash(GameObject item)
	{
		//Set fall down position here
		Destroy(item);
	}
}
