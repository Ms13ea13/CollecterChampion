using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanManager : InteractableManager
{
    public override void Interact(GameObject target, ref bool holding)
    {
        FoodItem food = target.gameObject.GetComponent<FoodItem>();
        if (food == null) return;
        
        int foodID = food.GetFoodItemId();

        if (foodID == 3 || foodID == 4 )
        {
            SetTargetPosition(food.transform);
            holding = false;
            food.SetFoodOnStove(true);
            food.PutFoodInThePan();
        }
    }
}
