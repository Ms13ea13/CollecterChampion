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
    private float orderTimer = 30f;
    private int leantweenID;
    
    [SerializeField] private int minWaitLevel = 0;

    [SerializeField] private int maxWaitLevel = 100;
    private bool timerSet = false;
    [SerializeField]
    private int tempValue ;
    
    [SerializeField]
    private Image foodImage;

    [SerializeField]
    private Slider orderSliderTimer;

    [SerializeField]
    private FoodType.FoodItemType orderFoodType;

    public bool GetTimerSet()
    {
        return timerSet;
    }

    void Start()
    {
        Initiate();
    }
    
    private void Initiate()
    {
        tempValue = -5;
        orderSliderTimer.minValue = minWaitLevel;
        orderSliderTimer.value = tempValue;
        orderSliderTimer.maxValue = maxWaitLevel;
    }

    void Update()
    {
        if (!HowToCook.IsShowingTutorial() || Time.timeScale != 0)
        {
            if (!timerSet)
                SetTimer();
        }
    }

    void SetTimer()
    {
        
        timerSet = true;
        leantweenID = LeanTween.value(tempValue, maxWaitLevel, orderTimer).setOnUpdate((tempValue) =>
        {
            orderSliderTimer.value = tempValue;
            
        }).setOnComplete(() =>
        {
            CustomerManager.ClearCustomerOrderWhenNotSendFood(this);
            CustomerManager.GetInstance().OrderingFood();
            Destroy(gameObject);
        }).id;
    }

    public FoodType.FoodItemType SetOrder(int id)
    {
        orderId = id;
        Price = GameSceneManager.GetInstance().GetFoodPriceById(id);
        orderName = GameSceneManager.GetInstance().GetFoodNameById(id);
        foodImage.sprite = GameSceneManager.GetInstance().GetFoodPictureById(id);
        orderFoodType = GameSceneManager.GetInstance().GetFoodTypeById(id);
        Debug.LogError("order : "  + orderName +" type : " + orderFoodType.ToString() );
        return orderFoodType;
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
