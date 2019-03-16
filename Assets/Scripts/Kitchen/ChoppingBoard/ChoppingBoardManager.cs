using UnityEngine;

public class ChoppingBoardManager : InteractableManager
{
    public override bool Interact(GameObject target  , ref bool holding, PlayerController player)
    {
        FoodItem food = target.gameObject.GetComponent<FoodItem>();
        if (food == null) return false;
        
        int foodID = food.GetFoodItemId();

        if (foodID == 0 || foodID == 3 || foodID == 4 || foodID == 6 || 
            foodID == 7 || foodID == 8 || foodID == 9 || foodID == 12 || foodID == 13)
        {
            SetTargetPosition(food.transform);
            holding = false;
            food.SetFoodOnChoppingBoard(true);
        }

        return true;
    }
}
