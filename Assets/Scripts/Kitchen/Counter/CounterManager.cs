using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterManager : InteractableManager
{
    public override void Interact(GameObject target, ref bool holding)
    {
        if (target == null) return;
        
        SetTargetPosition(target.transform);
        holding = false;
        
        if (target.GetComponent<FoodItem>() != null) // Can be a TrayItem
            target.GetComponent<FoodItem>().SetFoodOnCounter(true);
    }
}
