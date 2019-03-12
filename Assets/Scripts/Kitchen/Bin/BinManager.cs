using UnityEngine;

public class BinManager : InteractableManager
{
	public override void Interact(GameObject target  , ref bool holding )
	{
		if (!target)
			return;
		
		PlateItem plateOnHold = target.GetComponent<PlateItem>();
		if (plateOnHold != null)
		{
                        
			if (plateOnHold.ItemInPlate().Count > 0)
			{
				plateOnHold.ClearAllItemInPlate();
			}
			else
				return;
		}
		else
		{
			ThowItemToTrash(target);
		}
	}

	private void ThowItemToTrash(GameObject item)
	{
		//Set fall down position here
		Destroy(item);
	}
}
