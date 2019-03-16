using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterManager : InteractableManager
{
    public override bool Interact(GameObject target, ref bool holding , PlayerController player = null)
    {
        
        if (target == null) return false;
        
        Debug.Log("counter with target item");
        if (target.GetComponent<FoodItem>() != null) // Can be a TrayItem
            target.GetComponent<FoodItem>().SetFoodOnCounter(true);
        
        SetTargetPosition(target.transform);
        holding = false;
        return true;
    }
    
    public override GameObject InteractWithPlate(PlateItem plateItem,ref bool holding , PlayerController player = null)
    {
        if (plateItem != null)
        {
            SetTargetPosition(plateItem.transform);
            plateItem.GetComponent<Collider>().enabled = true;
            holding = false;
        }

        return null;
    }
}
