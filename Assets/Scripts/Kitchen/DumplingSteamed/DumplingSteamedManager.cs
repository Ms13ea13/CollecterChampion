using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumplingSteamedManager : InteractableManager
{
    public override void Interact(GameObject target, ref bool holding)
    {
        FoodItem food = target.gameObject.GetComponent<FoodItem>();
        if (food == null) return;
        
        int foodID = food.GetFoodItemId();

        if (foodID == 6 || foodID == 7 || foodID == 8 || foodID == 9)
        {
            SetTargetPosition(food.transform);
            holding = false;
            food.SetFoodIntoDumplingSteamed(true);
            food.PutFoodInTheDumplingSteamed();
        }
    }
}
