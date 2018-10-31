using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodStockManager : MonoBehaviour
{
    [SerializeField]
    private Text foodStockText;

    [SerializeField]
    private int foodStock;

    public void RemoveFoodNumber(int minusFood)
    {
        if (foodStock > 0)
        {
            foodStock -= minusFood;
            SetFoodContent(foodStock.ToString());
        }
        else
        {
            foodStock = 0;
        }
    }

    private void SetFoodContent(string message)
    {
        foodStockText.text = message;
    }
}
