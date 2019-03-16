using System.Collections.Generic;
using System.Linq;
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

    [SerializeField]
    private int maxPlateCleanLevel = 100;

    [SerializeField] private float percentage;

    [SerializeField]
    private float currentPlateCleanLevel;

    [SerializeField] private bool plateIntoSink;
    [SerializeField] private float tempSliderValue;

    private AudioSource FoodItemAudioSource;
    public AudioClip washdish;
    private float soundLength;
    private float soundStart = 0f;

    private bool onHold;
    private Vector3 temp;

    void Start()
    {
        currentIndex = 0;
        itemsInPlate = new List<GameObject>();
        timerSlider.value = 0;
        SetDefaultPlateUI();
        FoodItemAudioSource = GetComponent<AudioSource>(); //
    }

    void Update()
    {
        if (itemsInPlate.Count == 0)
        {
            platePanel.SetActive(false);
        }
        else
        {
            platePanel.SetActive(true);
        }
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
            if (!FoodItemAudioSource.isPlaying && washdish != null)
                FoodItemAudioSource.PlayOneShot(washdish);

            if (!timerSlider.gameObject.activeInHierarchy)
                SetShowTimerSlider(true);

            currentPlateCleanLevel += Time.deltaTime * 40f;
            percentage = (currentPlateCleanLevel / maxPlateCleanLevel) * 100;
            timerSlider.value = percentage;
            tempSliderValue = percentage;

            soundLength = washdish.length;//

            if (percentage >= 100)
            {
                _currentPlateState = PlateState.Clean;
                timerSlider.value = 0;
                SetShowTimerSlider(false);
                Destroy(gameObject);
                washDone = true;
                Debug.Log(percentage + "Plate is clean");
            }
            else
            {
                soundStart += Time.deltaTime;
                if (soundStart >= soundLength)
                {
                    FoodItemAudioSource.PlayOneShot(washdish);
                    soundStart = 0;
                }
            }
        }
    }

    public bool AddFoodToPlate(FoodItem food)
    {

        if (food.IsFoodDoneCooking() || food.CompareCurrentFoodState(FoodStateGlobal.FoodState.Alert)  )
        {
            if (itemsInPlate.Count < 3)
            {
                itemsInPlate.Add(food.gameObject);
                food.transform.parent = transform;
                food.GetComponent<Collider>().enabled = false;
                food.transform.localPosition = StackFoodVisually(currentIndex, food.transform);
                food.SetBannedId(currentIndex);
                FoodInPlateAmount(food.GetFoodItemId());
            }
            return true;
         
        }
        else
        {
            Debug.LogError("Nope food is : " + food.CurrentFoodState.ToString());
            return false;
        }

      

       
    }

    public bool DeliverFoodViaPlate(CustomerManager customer)
    {

        if (customer.ProcessingFoodOnPlate(this) != null)
        {
            return false;
        }
        else
        {
            Destroy(gameObject);
            return true;
        }
    }

    private Vector3 StackFoodVisually(int index, Transform targetTransform)
    {
        temp = targetTransform.localPosition;
        switch (index)
        {
            case 0:
            {
                temp.y = 0f;
                break;
            }
            case 1:
            {
                temp.y = 0.045f;
                break;
            }
            case 2:
            {
                temp.y = 0.08f;
                break;
            }
            default:
                break;
        }

        temp.x = 0;
        temp.z = 0;

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