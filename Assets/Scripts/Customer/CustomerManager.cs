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
    private static List<FoodOrder> customerOrders = new List<FoodOrder>();

    [SerializeField] private Image showStatus;

    [SerializeField] private GameObject orderPrefab;

    [SerializeField] private Sprite[] statusImages;

    [SerializeField] private GameObject customerPanel;

    [SerializeField] private SpawnDirtyDish spawnDirtyDish;
    private static CustomerManager instance;

    //--------------------------------------------------------------------------------------------

    private int maxOrder = 5;
    private int currentOrderAmount;

    [SerializeField]
    private float delayOrderTimer = 50f;

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
            customerPanel.SetActive(false);

    }


    private void AutoOrderingFood()
    {
        LeanTween.delayedCall(delayOrderTimer, OrderingFood);
    }

    private void RandomFoodOrder()
    {
        GameObject spawnOrderPicture = Instantiate(orderPrefab);
        FoodOrder order = spawnOrderPicture.GetComponent<FoodOrder>();
        
        if (customerPanel != null)
        {
            spawnOrderPicture.transform.parent = customerPanel.transform;
            order.SetOrder(GameSceneManager.GetInstance().RandomFoodOrderID());
            customerOrders.Add(order);
        }
        else 
            Debug.LogError("customerPanel is null");

    }

    public List<GameObject> ProcessingFoodOnPlate( PlateItem plate)
    {
        if (plate.ItemInPlate().Count == 0)
            return null;
        
        for (int i = 0; i < plate.ItemInPlate().Count; i++)
        {

            var fooditem = plate.ItemInPlate()[i].GetComponent<FoodItem>();

            for (int j = 0; j < customerOrders.Count; j++)
            {
                Debug.LogError("order food type : " + customerOrders[j].GetOrderFoodItemType().ToString());
                Debug.LogError("food type : " + fooditem.GetFoodType().ToString());
                if (customerOrders[j].GetOrderFoodItemType() == fooditem.GetFoodType())
                {
                    Debug.LogError("matching food order : " + customerOrders[j].GetOrderName() +" with item : " + fooditem.GetFoodType());
                    DelayPayment(customerOrders[j].GetOrderPrice());
                    RemoveOrder(customerOrders[j]);
                    Destroy(plate.ItemInPlate()[i]);
                    plate.ItemInPlate().Remove(plate.ItemInPlate()[i]);
                    plate.ClearTargetOrderPanel(fooditem.GetFoodItemId());
                    spawnDirtyDish.DelaySpawnDish(3f);
                    break;
                }
                else
                {
                    Debug.LogError("aint' catch shit");
                }
            }
        }

        if (plate.ItemInPlate().Count == 0)
            return null;
        else
            return plate.ItemInPlate();

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

    public static void RemoveOrder(FoodOrder order)
    {
        customerOrders.Remove(order);
        Destroy(order.gameObject);
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
        seq.append(1.5f);
        seq.append(() =>
        {
            Payment(moneyAmount);
            if (customerOrders.Count == 0)
               AutoOrderingFood();
        });
    }
}