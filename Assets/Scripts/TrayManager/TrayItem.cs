using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TrayItem : MonoBehaviour
{
    [SerializeField] private List<FoodInTray> foodInTrayOrders;

    [SerializeField] private GameObject foodInTrayImagePrefab;

    [SerializeField] private GameObject trayPanel;

    [SerializeField] private List<GameObject> itemInTray;

    [SerializeField] private int currentIndex;

    public enum TrayState
    {
        Clean,
        Dirty
    }

    [SerializeField] private TrayState currentTrayState;
    [SerializeField] private Slider timerSlider;
    private int leantweenID;
    private const float trayTimer = 20f;

    [SerializeField] private int min = 0;
    [FormerlySerializedAs("max")]
    [SerializeField] private int maxTrayCleanLevel = 100;

    [SerializeField] private float percentage;
    [FormerlySerializedAs("trayValue")]
    [SerializeField] private float currentTrayCleanLevel;

    [SerializeField] private bool trayIntoSink;
    [SerializeField] private float tempSliderValue;
    
    private bool onHold;
    private Vector3 temp;

    void Start()
    {
        currentIndex = 0;
        itemInTray = new List<GameObject>();

        timerSlider.value = 0;
        SetDefaultTrayUI();
    }

    private void SetShowTimerSlider(bool show)
    {
        timerSlider.gameObject.SetActive(show);
    }

    public void SetDefaultTrayUI()
    {
        if (currentTrayState == TrayState.Dirty && timerSlider.value > 0)
            SetShowTimerSlider(true);
        else
            SetShowTimerSlider(false);

        LeanTween.cancel(leantweenID);
    }

    public void SetTrayIntoSink(bool isIntoSink)
    {
        trayIntoSink = isIntoSink;
    }

    public bool GetTrayIntoSink()
    {
        return trayIntoSink;
    }

    public bool CompareCurrentTrayState(TrayState trayState)
    {
        return currentTrayState == trayState;
    }

    public void WashTray()
    {
        if (timerSlider.value <= maxTrayCleanLevel && CompareCurrentTrayState(TrayState.Dirty))
        {
            if (!timerSlider.gameObject.activeInHierarchy)
                SetShowTimerSlider(true);

            currentTrayCleanLevel += Time.deltaTime * 40f;
            percentage = (currentTrayCleanLevel / maxTrayCleanLevel) * 100;
            timerSlider.value = percentage;
            tempSliderValue = percentage;

            if (percentage >= 100)
            {
                currentTrayState = TrayState.Clean;
                timerSlider.value = 0;
                SetShowTimerSlider(false);
                Destroy(gameObject);
                Debug.Log(percentage + "Tray is clean");
            }
        }
    }

    public void AddFoodToTray(GameObject food)
    {
        var foodItem = food.GetComponent<FoodItem>();
        
        if (foodItem.CompareCurrentFoodState(FoodItem.FoodState.Raw) ||
            foodItem.CompareCurrentFoodState(FoodItem.FoodState.Grilled))
            return;

        if (itemInTray.Count < 3)
        {
            itemInTray.Add(food);
            food.transform.parent = transform;
            food.GetComponent<Collider>().enabled = false;
            food.transform.localPosition = StackFoodVisually(currentIndex, food.transform);
            foodItem.SetBannedId(currentIndex);
            FoodInTrayAmount(foodItem.GetFoodItemId());
           
        }
    }

    public void DeliverFoodViaTray(CustomerManager customer)
    {
        /*foreach (var item in itemInTray)
        {
            if (customer.RecieveOrder(item.gameObject.GetComponent<FoodItem>()))
            {
                itemInTray.Remove(item);
                Destroy(item.gameObject);
                ClearTargetOrderPanel(item.gameObject.GetComponent<FoodItem>().GetFoodItemId());
                currentIndex -= 1;

                if (currentIndex <= 0)
                {
                    Destroy(gameObject);
                }

                break;
            }
        }*/

        for (int i = itemInTray.Count - 1; i >= 0; i--)
        {
            if (customer.RecieveOrder(gameObject.GetComponent<FoodItem>()))
            {
                itemInTray.Remove(gameObject);
                Destroy(gameObject);
                ClearTargetOrderPanel(gameObject.GetComponent<FoodItem>().GetFoodItemId());
                currentIndex -= 1;

                if (currentIndex <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    public void RemoveAllFoodFromTray()
    {
        itemInTray.Clear();
        currentIndex = 0;
    }

    private Vector3 StackFoodVisually(int index, Transform targetTransform)
    {
        temp = targetTransform.localPosition;
        switch (index)
        {
            case 0:
            {
                temp.z = 0f;
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

        temp.x = 0f;
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

    public void FoodInTrayAmount(int foodIndex)
    {
        GameObject spawnOrderPicture = Instantiate(foodInTrayImagePrefab);
        spawnOrderPicture.GetComponent<Image>().sprite = GameSceneManager.GetInstance().GetFoodPictureById(foodIndex);
        spawnOrderPicture.transform.parent = trayPanel.transform;
        spawnOrderPicture.GetComponent<FoodInTray>().SetOrder(foodIndex);
    }

    public void ClearTargetOrderPanel(int id)
    {
        if (trayPanel.transform.childCount <= 0)
            return;

        for (int i = 0; i < trayPanel.transform.childCount; i++)
        {
            if (id == trayPanel.transform.GetChild(i).gameObject.GetComponent<FoodInTray>().GetOrderId())
            {
                Destroy(trayPanel.transform.GetChild(i).gameObject);
                return;
            }
        }
    }

    public void SetFoodOnCounter(bool b)
    {
        throw new System.NotImplementedException();
    }
}