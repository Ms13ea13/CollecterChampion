using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveManager : InteractableManager
{
    public override void Interact(GameObject target  , ref bool holding)
    {
        FoodItem food = target.gameObject.GetComponent<FoodItem>();
        if (food == null) return;
        
        int foodID = food.GetFoodItemId();

        if (foodID == 0 || foodID == 2)
        {
            SetTargetPosition(food.transform);
            holding = false;
            food.SetFoodOnStove(true);
            food.PutFoodInTheStove();
        }
    }
}
