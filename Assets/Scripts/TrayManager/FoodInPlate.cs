using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public class FoodInPlate : MonoBehaviour
{
    [SerializeField]
    private int orderId;

    [SerializeField]
    private string orderName;

    [SerializeField]
    private Image foodImage;

    public void SetOrder(int id)
    {
        orderId = id;
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

    public Sprite GetOrderPicture()
    {
        return foodImage.sprite;
    }
}
