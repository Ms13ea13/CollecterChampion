using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodOrder : MonoBehaviour
{
    [SerializeField]
    private int orderId;
    [SerializeField]
    private string orderName;

    public void SetOrder(int id)
    {
        orderId = id;
        orderName = GameSceneManager.GetInstance().GetComponent<FoodNameOrder>().GetStringFoodName(id);
    }

    public string GetFoodName()
    {
        return orderName;
    }

    public int GetOrderId()
    {
        return orderId;
    }
}
