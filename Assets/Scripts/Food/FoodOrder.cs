using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Serialization;

[Serializable]
public class FoodOrder : MonoBehaviour
{
    [SerializeField]
    private int orderId;

    [SerializeField]
    private string orderName;

    [SerializeField]
    private int Price;
    
    [SerializeField]
    private float orderTimer = 180f;
    private int leantweenID;
    
    [SerializeField] private int minWaitLevel = 0;

    [SerializeField] private int maxWaitLevel = 100;
    [SerializeField]
    private bool initiated = false;
    [SerializeField]
    private int tempValue ;
    
    [SerializeField]
    private Image foodImage;

    [SerializeField]
    private Slider orderSliderTimer;

    [SerializeField]
    private FoodType.FoodItemType orderFoodType;

    public void SetOrder(int id)
    {
        orderId = id;
        Price = GameSceneManager.GetInstance().GetFoodPriceById(id);
        orderName = GameSceneManager.GetInstance().GetFoodNameById(id);
        foodImage.sprite = GameSceneManager.GetInstance().GetFoodPictureById(id);
        orderFoodType = GameSceneManager.GetInstance().GetFoodTypeById(id);
        Debug.LogError("order type " + orderFoodType.ToString());
        Initiate();
    }
    private void Initiate()
    {
        tempValue = -5;
        orderSliderTimer.minValue = minWaitLevel;
        orderSliderTimer.value = tempValue;
        orderSliderTimer.maxValue = maxWaitLevel;
        SetTimer();
    }

    void SetTimer()
    {
        leantweenID = LeanTween.value(tempValue, maxWaitLevel, orderTimer).setOnStart(() =>
        {
            initiated = true;
        }).setOnUpdate((tempValue) =>
        {
            orderSliderTimer.value = tempValue;
            
        }).setOnComplete(() =>
        {
            CustomerManager.RemoveOrder(this);
            CustomerManager.GetInstance().OrderingFood();
            Destroy(gameObject);
        }).id;
    }
    
    public bool IsInitiated()
    {
        return initiated;
    }

    public FoodType.FoodItemType GetOrderFoodItemType()
    {
        return orderFoodType;
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

    private void OnDestroy()
    {
        LeanTween.cancel(leantweenID);
    }
}
