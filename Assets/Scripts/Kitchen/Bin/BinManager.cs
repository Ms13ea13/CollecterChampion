using UnityEngine;

public class BinManager : InteractableManager
{

	public override GameObject InteractWithPlate(PlateItem plateItem, PlayerController player = null)
	{
		if (plateItem != null)
		{
                        
			if (plateItem.ItemInPlate().Count > 0)
			{
				plateItem.ClearAllItemInPlate();
				return plateItem.gameObject;
			}
			else
			{
				ThowItemToTrash(plateItem.gameObject);
			}
		}

		return null;
	}
	
	public override bool Interact(GameObject target  , ref bool holding, PlayerController player )
	{
		if (!target)
			return false;
		
		
			ThowItemToTrash(target);

		return true;
	}

	private void ThowItemToTrash(GameObject item)
	{
		//Set fall down position here
		Destroy(item);
	}
}
