using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public class FoodOrder : MonoBehaviour
{
    [SerializeField]
    private int orderId;
    [SerializeField]
    private string orderName;

    [SerializeField]
    private Image foodImage;

    [SerializeField]
    private int Price;

    public void SetOrder(int id)
    {
        orderId = id;
        Price = GameSceneManager.GetInstance().GetFoodPriceById(id);
        orderName = GameSceneManager.GetInstance().GetFoodNameById(id);
        foodImage.sprite = GameSceneManager.GetInstance().GetFoodPictureById(id);
    }

    public string GetOrderName()
    {
        return orderName;
    }
   
    public int GetOrderId()
    {
        return orderId;
    }
    
    public int GetOrderPrice()
    {
        return Price;
    }
    
    public Sprite GetOrderPicture()
    {
        return foodImage.sprite;
    }
}
