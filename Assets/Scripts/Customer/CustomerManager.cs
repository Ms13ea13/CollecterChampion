﻿using System.Collections.Generic;
using SpawnItem;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class CustomerManager : MonoBehaviour
{
    [SerializeField] private List<FoodOrder> customerOrders;

    [SerializeField] private int orderLength;

    [SerializeField] private Image showStatus;

    [SerializeField] private GameObject orderImagePrefab;

    [SerializeField] private Sprite[] statusImages;

    [SerializeField] private GameObject customerPanel;

    [SerializeField] private SpawnDirtyDish spawnDirtyDish;

    //--------------------------------------------------------------------------------------------

    private int leantweenID;
    private const float waitTimer = 2f;

    [SerializeField] private int minWaitLevel = 0;

    [FormerlySerializedAs("max")]
    [SerializeField] private int maxWaitLevel = 100;

    [SerializeField] private float percentage;

    [FormerlySerializedAs("foodValue")]
    [SerializeField] private float currentWaitLevel;

    [SerializeField] private float changeMenuValue;
    [SerializeField] private float tempSliderValue;

    [SerializeField] private Slider timerSlider;

    private void Start()
    {
        OrderingFood();

        timerSlider.value = 0;
        changeMenuValue = 2;
        
        timerSlider.wholeNumbers = false;
    }

    private void RandomFoodAmount()
    {
        customerOrders = new List<FoodOrder>();
        int foodAmount = Random.Range(1, 4);

        for (int i = 0; i < foodAmount; i++)
        {
            GameObject spawnOrderPicture = Instantiate(orderImagePrefab);
            if (customerPanel != null)
            spawnOrderPicture.transform.parent = customerPanel.transform;
            else 
            Debug.LogError("customerPanel is null");
            customerOrders.Add(spawnOrderPicture.GetComponent<FoodOrder>());
        }
    }

    public Dictionary<int, int> GetCustomerOrderDict()
    {
        var orderDict = new Dictionary<int, int>();

        foreach (var order in customerOrders)
        {
            var orderId = order.GetOrderId();
            if (orderDict.ContainsKey(orderId))
                orderDict[orderId] += 1;
            else
                orderDict.Add(orderId, 1);
        }

        return orderDict;
    }

    public bool ReceiveOrder(Dictionary<int, int> trayDict)
    {
        var orderValid = DoesTrayMatchOrder(trayDict);

        if (orderValid)
        {
            ClearCustomerOrder();
            spawnDirtyDish.DelaySpawnDish(3f);
            
            SetShowTimerSlider(false);
        }

        return orderValid;
    }
    
    private bool DoesTrayMatchOrder(Dictionary<int, int> trayDict)
    {
        var customerDict = GetCustomerOrderDict();

        if (trayDict.Count != customerDict.Count) return false;

        var isEqual = true;
        foreach (var pair in trayDict)
        {
            int value;
            if (customerDict.TryGetValue(pair.Key, out value))
            {
                if (value == pair.Value)
                    continue;
                isEqual = false;
                break;
            }

            isEqual = false;
            break;
        }

        return isEqual;
    }

    private static bool CheckFood(FoodOrder foodOrder, FoodItem foodReceive)
    {
        if (foodReceive.IsFoodChopped() || foodReceive.IsFoodBoiled() || foodReceive.IsFoodAlert())
        {
            if (foodOrder.GetOrderId() == foodReceive.GetFoodItemId())
            {
                return true;
            }
        }

        return false;
    }

    private void ClearCustomerOrder()
    {
        for (var i = customerOrders.Count - 1; i >= 0; i--)
        {
            var order = customerOrders[i];
            customerOrders.Remove(order);
            DelayPayment(order.GetOrderPrice());
            if (order.gameObject != null)
                LeanTween.cancel(order.gameObject);
            Destroy(order.gameObject);
        }
    }

    private void ClearCustomerOrderWhenNotSendFood()
    {
        for (var i = customerOrders.Count - 1; i >= 0; i--)
        {
            var order = customerOrders[i];
            customerOrders.Remove(order);
            if (order.gameObject != null)
                LeanTween.cancel(order.gameObject);
            Destroy(order.gameObject);
        }
    }

    public bool ReceiveOrder(FoodItem foodReceive)
    {
        if (customerOrders.Count > 0 && foodReceive)
        {
            foreach (var item in customerOrders)
            {
                if (CheckFood(item, foodReceive)) return true;

                if (item.GetOrderId() == foodReceive.GetFoodItemId() && foodReceive.IsFoodChopped() ||
                    item.GetOrderId() == foodReceive.GetFoodItemId() && foodReceive.IsFoodBoiled() ||
                    item.GetOrderId() == foodReceive.GetFoodItemId() && foodReceive.IsFoodAlert())
                {
                    customerOrders.Remove(item);
                    DelayPayment(item.GetOrderPrice());
                    if (item.gameObject != null)
                        LeanTween.cancel(item.gameObject);
                    Destroy(item.gameObject);
                    
                    SetShowTimerSlider(false);

                    return true;
                }
            }

            return false;
        }

        return false;
    }

    private void OrderingFood()
    {
        LeanTween.cancel(leantweenID);
        RandomFoodAmount();
        tempSliderValue = 0;
        timerSlider.value = 0;
        if (customerOrders.Count > 0)
        {
            foreach (var item in customerOrders)
            {
                item.SetOrder(GameSceneManager.GetInstance().RandomFoodOrderByOne());
               
            }
            customerOrderWait();
        }
    }

    private void Payment(int moneyAmount)
    {
        //Play coin vfx here
        GameSceneManager.GetInstance().CustomerPayMoneyToStore(moneyAmount);
    }

    void DelayPayment(int moneyAmount)
    {
        var seq = LeanTween.sequence();
        seq.append(3f);
        seq.append(() =>
        {
            Payment(moneyAmount);
            if (customerOrders.Count == 0)
                OrderingFood();
        });
    }

    //-------------------------------------------------------------------------------------------

    private void SetShowTimerSlider(bool show)
    {
        if (timerSlider != null)
        timerSlider.gameObject.SetActive(show);
        else
        {
            Debug.LogError("timerslider is null");
        }
    }

    private void customerOrderWait()
    {

        float SetChangeMenuValue = maxWaitLevel + 0.000001f;
        SetShowTimerSlider(true);

        leantweenID = LeanTween.value(tempSliderValue, SetChangeMenuValue + 0.001f, waitTimer).setOnUpdate((Value) =>
        {
            tempSliderValue = Value;
            if (timerSlider.value <= timerSlider.maxValue)
                timerSlider.value = Value;

            if (timerSlider.value >= timerSlider.maxValue)
            {
//                Debug.Log("Change Order");
                ClearCustomerOrderWhenNotSendFood();
                SetShowTimerSlider(false);
                OrderingFood();
               
            }
        }).id;
    }
}