using UnityEngine;

public class PotManager : InteractableManager
{
    public override void Interact (GameObject target, ref bool holding)
    {
        
        
        FoodItem food = target.gameObject.GetComponent<FoodItem>();
        if (food == null) return;
        
        int foodID = food.GetFoodItemId();

        if (foodID == 1 || foodID == 5)
        {
            SetTargetPosition(food.transform);
            holding = false;
            food.SetFoodIntoPot(true);
            food.PutFoodInThePot();
        }
    }
}
