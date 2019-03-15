using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanManager : InteractableManager
{
    public override bool Interact(GameObject target, ref bool holding, PlayerController player = null)
    {
        FoodItem food = target.gameObject.GetComponent<FoodItem>();
        if (food == null) return false;
        
        int foodID = food.GetFoodItemId();

        if (foodID == 3 || foodID == 4 )
        {
            SetTargetPosition(food.transform);
            holding = false;
            food.SetFoodOnStove(true);
            food.PutFoodInThePan();
        }

        return true;
    }
}
