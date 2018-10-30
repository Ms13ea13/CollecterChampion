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
        foodStock -= minusFood;
        SetFoodContent(foodStock.ToString());
    }

    private void SetFoodContent(string message)
    {
        foodStockText.text = message;
    }
}
