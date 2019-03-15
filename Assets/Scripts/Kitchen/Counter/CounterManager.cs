﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterManager : InteractableManager
{
    public override bool Interact(GameObject target, ref bool holding , PlayerController player = null)
    {
        if (target == null) return false;
        
        SetTargetPosition(target.transform);
        holding = false;
        
        if (target.GetComponent<FoodItem>() != null) // Can be a TrayItem
            target.GetComponent<FoodItem>().SetFoodOnCounter(true);
        return true;
    }
}
