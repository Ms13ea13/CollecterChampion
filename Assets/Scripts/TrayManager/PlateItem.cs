﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlateItem : MonoBehaviour
{
    [SerializeField] private GameObject foodInPlateImagePrefab;

    [SerializeField] private GameObject platePanel;

    [FormerlySerializedAs("itemInTray")] [SerializeField]
    private List<GameObject> itemsInPlate;

    public List<GameObject> ItemInPlate()
    {
        return itemsInPlate;
    }

    [SerializeField] private int currentIndex;

    public bool washDone = false;

    public enum PlateState
    {
        Clean,
        Dirty
    }

    [SerializeField] private PlateState _currentPlateState;
    [SerializeField] private Slider timerSlider;
    private int leantweenID;
    private const float plateTimer = 20f;

    [SerializeField] private int min = 0;

    [FormerlySerializedAs("max")] [SerializeField]
    private int maxPlateCleanLevel = 100;

    [SerializeField] private float percentage;

    [FormerlySerializedAs("trayValue")] [SerializeField]
    private float currentPlateCleanLevel;

    [SerializeField] private bool plateIntoSink;
    [SerializeField] private float tempSliderValue;

    private bool onHold;
    private Vector3 temp;

    void Start()
    {
        currentIndex = 0;
        itemsInPlate = new List<GameObject>();

        timerSlider.value = 0;
        SetDefaultPlateUI();
    }

    private void SetShowTimerSlider(bool show)
    {
        timerSlider.gameObject.SetActive(show);
    }

    public void SetDefaultPlateUI()
    {
        if (_currentPlateState == PlateState.Dirty && timerSlider.value > 0)
            SetShowTimerSlider(true);
        else
            SetShowTimerSlider(false);

        LeanTween.cancel(leantweenID);
    }

    public void SetPlateIntoSink(bool isIntoSink)
    {
        plateIntoSink = isIntoSink;
    }

    public bool GetPlateIntoSink()
    {
        return plateIntoSink;
    }

    public bool CompareCurrentPlateState(PlateState plateState)
    {
        return _currentPlateState == plateState;
    }

    public void WashPlate()
    {
        if (timerSlider.value <= maxPlateCleanLevel && CompareCurrentPlateState(PlateState.Dirty))
        {
            if (!timerSlider.gameObject.activeInHierarchy)
                SetShowTimerSlider(true);

            currentPlateCleanLevel += Time.deltaTime * 40f;
            percentage = (currentPlateCleanLevel / maxPlateCleanLevel) * 100;
            timerSlider.value = percentage;
            tempSliderValue = percentage;

            if (percentage >= 100)
            {
                _currentPlateState = PlateState.Clean;
                timerSlider.value = 0;
                SetShowTimerSlider(false);
                Destroy(gameObject);
                washDone = true;
                Debug.Log(percentage + "Plate is clean");
            }
        }
    }

    public void AddFoodToPlate(GameObject food)
    {
        var foodItem = food.GetComponent<FoodItem>();

        if (foodItem.CompareCurrentFoodState(FoodItem.FoodState.Raw) ||
            foodItem.CompareCurrentFoodState(FoodItem.FoodState.Grilled))
            return;

        if (itemsInPlate.Count < 3)
        {
            itemsInPlate.Add(food);
            food.transform.parent = transform;
            food.GetComponent<Collider>().enabled = false;
            food.transform.localPosition = StackFoodVisually(currentIndex, food.transform);
            foodItem.SetBannedId(currentIndex);
            FoodInPlateAmount(foodItem.GetFoodItemId());
        }
    }

    private Dictionary<int, int> GetFoodInPlate()
    {
        var plateDict = new Dictionary<int, int>();

        foreach (var foodObj in itemsInPlate)
        {
            var foodId = foodObj.GetComponent<FoodItem>().GetFoodItemId();
            if (plateDict.ContainsKey(foodId))
            {
                plateDict[foodId] += 1;
            }
            else
            {
                plateDict.Add(foodId, 1);
            }
        }

        return plateDict;
    }

    public bool DeliverFoodViaPlate(CustomerManager customer)
    {
        if (!customer.ReceiveOrder(GetFoodInPlate())) return false;

        for (var i = itemsInPlate.Count - 1; i >= 0; i--)
        {
            var item = itemsInPlate[i];
            itemsInPlate.Remove(item);
            ClearTargetOrderPanel(item.GetComponent<FoodItem>().GetFoodItemId());
            Destroy(item.gameObject);
        }

        Destroy(gameObject);
        return true;
    }

    private Vector3 StackFoodVisually(int index, Transform targetTransform)
    {
        temp = targetTransform.localPosition;
        switch (index)
        {
            case 0:
            {
                temp.z = 0;
                break;
            }
            case 1:
            {
                temp.z = -0.12f;
                break;
            }
            case 2:
            {
                temp.z = 0.12f;
                break;
            }
            default:
                break;
        }

        temp.x = 0;
        temp.y = 0.013f;
        currentIndex += 1;
        return temp;
    }

    public void SetOnHold(bool hold)
    {
        onHold = hold;
    }

    public bool GetOnHold()
    {
        return onHold;
    }

    public void FoodInPlateAmount(int foodIndex)
    {
        GameObject spawnOrderPicture = Instantiate(foodInPlateImagePrefab);
        spawnOrderPicture.GetComponent<Image>().sprite = GameSceneManager.GetInstance().GetFoodPictureById(foodIndex);
        spawnOrderPicture.transform.parent = platePanel.transform;
        spawnOrderPicture.GetComponent<FoodInPlate>().SetOrder(foodIndex);
    }

    public void ClearTargetOrderPanel(int id)
    {
        if (platePanel.transform.childCount <= 0)
            return;

        for (int i = 0; i < platePanel.transform.childCount; i++)
        {
            if (id == platePanel.transform.GetChild(i).gameObject.GetComponent<FoodInPlate>().GetOrderId())
            {
                Destroy(platePanel.transform.GetChild(i).gameObject);
                return;
            }
        }
    }

    private void ClearOrderPanel()
    {
        if (platePanel.transform.childCount <= 0)
            return;


        for (int i = 0; i < platePanel.transform.childCount; i++)
        {
            Destroy(platePanel.transform.GetChild(i).gameObject);
        }
    }

    private void ClearItemsInPlate()
    {
        if (itemsInPlate.Count <=0)
            return;


        foreach (var foodObj in itemsInPlate)
        {
            Destroy(foodObj);
        }
    }

    public void ClearAllItemInPlate()
    {
        ClearOrderPanel();
        ClearItemsInPlate();
        currentIndex = 0;
    }

    public void SetFoodOnCounter(bool b)
    {
        throw new System.NotImplementedException();
    }
}