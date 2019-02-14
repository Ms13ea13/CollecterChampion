using System;
using System.Collections;
using System.Collections.Generic;
using SpawnItem;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class CustomerManager : MonoBehaviour
{
    [SerializeField] private static List<FoodOrder> customerOrders = new List<FoodOrder>();


    [SerializeField] private Image showStatus;

    [SerializeField] private GameObject orderPrefab;

    [SerializeField] private Sprite[] statusImages;

    [SerializeField] private GameObject customerPanel;

    [SerializeField] private SpawnDirtyDish spawnDirtyDish;
    private static CustomerManager instance;

    //--------------------------------------------------------------------------------------------

//    private int leantweenID;
//    private const float waitTimer = 60f;
//
//    [SerializeField] private int minWaitLevel = 0;
//
//    [SerializeField] private int maxWaitLevel = 100;
//
//    [SerializeField] private float percentage;
//
//    [SerializeField] private float currentWaitLevel;
//
//    [SerializeField] private float changeMenuValue;
//    [SerializeField] private float tempSliderValue;
//
//    [SerializeField] private Slider timerSlider;
    private int maxOrder = 5;
    private int currentOrderAmount;

    private float delayOrderTimer = 5f;

    private void Awake()
    {
        instance = this;
        
        if (customerPanel == null)
        {
            customerPanel = GameObject.Find("OrderPanel");
        }
    }
    
    public static CustomerManager GetInstance()
    {
        return instance;
    }

    public void Initiate()
    {
        
        OrderingFood();
        
        if (HowToCook.IsShowingTutorial())
        {
            customerPanel.SetActive(false);
        }
//        timerSlider.value = 0;
//        changeMenuValue = 2;
//        
//        timerSlider.wholeNumbers = false;
    }

//    void Update()
//    {
//        if (!HowToCook.IsShowingTutorial())
//        {
//            customerPanel.SetActive(true);
//            if (customerOrders.Count == 0)
//            {
//                if (customerOrders.Count < maxOrder)
//                     OrderingFood(delayOrderTimer);
//            }
//            else
//            {
//                AutoOrderingFood();
//            }
//        }
//    }

    private void AutoOrderingFood()
    {
        LeanTween.delayedCall(delayOrderTimer, OrderingFood);
    }

    private void RandomFoodOrder()
    {
        GameObject spawnOrderPicture = Instantiate(orderPrefab);
        
        if (customerPanel != null)
        {
            spawnOrderPicture.transform.parent = customerPanel.transform;
            customerOrders.Add(spawnOrderPicture.GetComponent<FoodOrder>());
            
        }
        else 
            Debug.LogError("customerPanel is null");
        
           
        
//        for (int i = 0; i < maxOrder; i++)
//        {
//            GameObject spawnOrderPicture = Instantiate(orderPrefab);
//            if (customerPanel != null)
//            spawnOrderPicture.transform.parent = customerPanel.transform;
//            else 
//            Debug.LogError("customerPanel is null");
//            customerOrders.Add(spawnOrderPicture.GetComponent<FoodOrder>());
//        }
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

    public List<GameObject> ProcessingFoodOnPlate(List<GameObject> item, PlateItem plate)
    {
        if (item.Count == 0)
            return null;
        
        Debug.LogError("item in plate count : " + item.Count);
        for (int i = 0; i < item.Count; i++)
        {

            var fooditem = item[i].GetComponent<FoodItem>();

            for (int j = 0; j < customerOrders.Count; j++)
            {
                if (customerOrders[j].GetOrderFoodItemType() == fooditem.GetFoodType())
                {
                    Debug.LogError("matching food order : " + customerOrders[j].GetOrderName() +"with item : " + item[i].GetComponent<FoodItem>().GetFoodType());
                    ClearCustomerOrderWhenNotSendFood(customerOrders[j]);
                    item.Remove(item[i]);
                    plate.ClearTargetOrderPanel(fooditem.GetFoodItemId());
                    Destroy(item[i]);
                    spawnDirtyDish.DelaySpawnDish(3f);
                    break;
                }
            }
        }

        if (item.Count == 0)
            return null;
        else
            return item;
       
        
//        for (int i = 0; i < customerOrders.Count; i++)
//        {
//            if (item.Count == 0)
//                return true;
//            
//            for (int j = 0; j < item.Count; j++)
//            {
//                if (customerOrders[i]. == item[j].GetComponent<FoodItem>().GetFoodType())
//                {
//                    ClearCustomerOrderWhenNotSendFood(customerOrders[i]);
//                    spawnDirtyDish.DelaySpawnDish(3f);
//                }
//                else
//                {
//                    Debug.LogError("ordertype : " + customerOrders[i].GetOrderFoodItemType()  + " || item type : " +  item[j].GetComponent<FoodItem>().GetFoodType());
//                }
//
//            }
//        }

//        var orderValid = DoesTrayMatchOrder();
//
//        if (orderValid)
//        {
//            ClearCustomerOrder();
//            spawnDirtyDish.DelaySpawnDish(3f);
//            
////            SetShowTimerSlider(false);
//        }
//
//        return orderValid;
    }
    
//    private bool DoesTrayMatchOrder()
//    {
//        var customerDict = GetCustomerOrderDict();
//
//        if (trayDict.Count != customerDict.Count) return false;
//
//        var isEqual = true;
//        foreach (var pair in trayDict)
//        {
//            int value;
//            if (customerDict.TryGetValue(pair.Key, out value))
//            {
//                if (value == pair.Value)
//                    continue;
//                isEqual = false;
//                break;
//            }
//
//            isEqual = false;
//            break;
//        }
//
//        return isEqual;
//    }

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

    public static void ClearCustomerOrderWhenNotSendFood(FoodOrder order)
    {
//        for (var i = customerOrders.Count - 1; i >= 0; i--)
//        {
//            var order = customerOrders[i];
            customerOrders.Remove(order);
//            if (order.gameObject != null)
//                LeanTween.cancel(order.gameObject);
//            Destroy(order.gameObject);
//        }
    }

    public bool ReceiveOrder(FoodItem foodReceive)
    {
        if (customerOrders.Count > 0 && foodReceive)
        {
            
            Debug.LogError("ReceiveOrder");
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
                    
//                    SetShowTimerSlider(false);
                    return true;
                }
            }

            return false;
        }

        return false;
    }

    public void OrderingFood()
    {
        if (customerOrders.Count >= maxOrder) return;
        
        RandomFoodOrder();
        var seq = LeanTween.sequence();
        seq.append(delayOrderTimer);
        seq.append(() =>
        {
            if (customerOrders.Count > 0)
            {
                foreach (var foodOrder in customerOrders)
                {
                    if (!foodOrder.GetTimerSet())
                    {
                        var foodItemType = foodOrder.SetOrder(GameSceneManager.GetInstance().RandomFoodOrderID());
                    }
                }
            }
        });
        AutoOrderingFood();


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
               AutoOrderingFood();
        });
    }

    //-------------------------------------------------------------------------------------------

//    private void SetShowTimerSlider(bool show)
//    {
//        if (timerSlider != null)
//        timerSlider.gameObject.SetActive(show);
//        else
//        {
//            Debug.LogError("timerslider is null");
//        }
//    }

//    private void customerOrderWait()
//    {

//        float SetChangeMenuValue = maxWaitLevel + 0.000001f;
//        SetShowTimerSlider(true);
//
//        leantweenID = LeanTween.value(tempSliderValue, SetChangeMenuValue + 0.001f, waitTimer).setOnUpdate((Value) =>
//        {
//            tempSliderValue = Value;
//            if (timerSlider.value <= timerSlider.maxValue)
//                timerSlider.value = Value;
//
//            if (timerSlider.value >= timerSlider.maxValue)
//            {
//                Debug.Log("Change Order");
//                ClearCustomerOrderWhenNotSendFood();
//                SetShowTimerSlider(false);
//                OrderingFood();
//               
//            }
//        }).id;
//    }
}