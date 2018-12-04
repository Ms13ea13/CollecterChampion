using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TrayItem : MonoBehaviour
{
    [SerializeField] private List<FoodInTray> foodInTrayOrders;

    [SerializeField] private GameObject foodInTrayImagePrefab;

    [SerializeField] private GameObject trayPanel;

    [FormerlySerializedAs("itemInTray")]
    [SerializeField] private List<GameObject> itemsInTray;

    [SerializeField] private int currentIndex;

    public bool washDone = false;

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
        itemsInTray = new List<GameObject>();

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
                washDone = true;
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

        if (itemsInTray.Count < 3)
        {
            itemsInTray.Add(food);
            food.transform.parent = transform;
            food.GetComponent<Collider>().enabled = false;
            food.transform.localPosition = StackFoodVisually(currentIndex, food.transform);
            foodItem.SetBannedId(currentIndex);
            FoodInTrayAmount(foodItem.GetFoodItemId());
        }
    }

    private Dictionary<int, int> GetFoodInTray()
    {
        var trayDict = new Dictionary<int, int>();

        foreach (var foodObj in itemsInTray)
        {
            var foodId = foodObj.GetComponent<FoodItem>().GetFoodItemId();
            if (trayDict.ContainsKey(foodId))
            {
                trayDict[foodId] += 1;
            }
            else
            {
                trayDict.Add(foodId, 1);
            }
        }

        return trayDict;
    }

    public bool DeliverFoodViaTray(CustomerManager customer)
    {
        if (!customer.ReceiveOrder(GetFoodInTray())) return false;
        
        for (var i = itemsInTray.Count - 1; i >= 0; i--)
        {
            var item = itemsInTray[i];
            itemsInTray.Remove(item);
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