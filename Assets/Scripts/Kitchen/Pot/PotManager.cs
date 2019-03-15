using UnityEngine;

public class PotManager : InteractableManager
{
    public override bool Interact (GameObject target, ref bool holding , PlayerController player = null)
    {
        FoodItem food = target.gameObject.GetComponent<FoodItem>();
        if (food == null) return false;
        
        int foodID = food.GetFoodItemId();

        if (foodID == 1 || foodID == 5)
        {
            SetTargetPosition(food.transform);
            holding = false;
            food.SetFoodIntoPot(true);
            food.PutFoodInThePot();
        }

        return true;
    }
}
