using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class FoodItem : MonoBehaviour

{
    [SerializeField] private int foodID;

    [SerializeField] private string FoodName;

    [SerializeField] private int trayBandedId;

    [SerializeField] private bool foodOnStove;

    [SerializeField] private bool foodOnChoppingBoard;

    [SerializeField] private bool foodOnCounter;

    [SerializeField] private bool foodIntoPot;

    public AudioClip chopping, Alert_fire, complete;//

    public string GetFoodItemName()
    {
        return FoodName;
    }

    public int GetBanedID()
    {
        return trayBandedId;
    }

    public bool GetFoodOnCounter()
    {
        return foodOnCounter;
    }

    public bool GetFoodIntoPot()
    {
        return foodIntoPot;
    }

    public int GetFoodItemId()
    {
        return foodID;
    }

    public bool IsFoodChopped()
    {
        return currentFoodState == FoodState.Chop;
    }

    public bool IsFoodOnFire()
    {
        return currentFoodState == FoodState.OnFire;
    }

    public bool IsFoodBoiled()
    {
        return currentFoodState == FoodState.Boiled;
    }

    public bool IsFoodAlert()
    {
        return currentFoodState == FoodState.Alert;
    }

    public bool GetFoodOnStove()
    {
        return foodOnStove;
    }

    public bool GetFoodOnChoppingBoard()
    {
        return foodOnChoppingBoard;
    }

    public bool CompareCurrentFoodState(FoodState foodState)
    {
        return currentFoodState == foodState;
    }

    public enum FoodState
    {
        Raw,
        Grilled,
        Chop,
        Boiled,
        Alert,
        OnFire
    }

    [SerializeField] private GameObject rawModel;

    [SerializeField] private GameObject grilledModel;

    [SerializeField] private GameObject chopModel;

    [SerializeField] private GameObject boiledModel;

    [SerializeField] private GameObject modelContainer;

    private GameObject currentFoodModel;

    [SerializeField] private FoodState currentFoodState;
    [SerializeField] private Slider timerSlider;
    [SerializeField] private Image foodStateUI;
    [SerializeField] private Sprite cookedPicture;
    [SerializeField] private Sprite onFirePicture;
    [SerializeField] private Sprite alertPicture;

    private int leantweenID;
    private const float cookTimer = 20f;

    [SerializeField] private int min = 0;

    [FormerlySerializedAs("max")]
    [SerializeField] private int maxFoodCookLevel = 100;

    [SerializeField] private float percentage;

    [FormerlySerializedAs("foodValue")]
    [SerializeField] private float currentFoodCookLevel;

    [SerializeField] private float onFireValue;
    [SerializeField] private float tempSliderValue;

    void Start()
    {
        timerSlider.value = 0;

        onFireValue = 2;

        currentFoodState = FoodState.Raw;
        ChangeFoodVisualAccordingToStates();
        timerSlider.wholeNumbers = false;
        SetDefaultFoodUI();
    }

    public void SetDefaultFoodUI()
    {
        if (currentFoodState == FoodState.Raw && timerSlider.value > 0)
            SetShowTimerSlider(true);
        else
            SetShowTimerSlider(false);

        LeanTween.cancel(leantweenID);
        foodStateUI.gameObject.SetActive(false);
    }

    private void SetShowTimerSlider(bool show)
    {
        timerSlider.gameObject.SetActive(show);
    }

    private void SetFoodUIState()
    {
        if (foodStateUI == null)
            throw new Exception("ควย null ไอเหี้ย");

        foodStateUI.gameObject.SetActive(true);
        switch (currentFoodState)
        {
            case FoodState.Alert:
                foodStateUI.sprite = alertPicture;
                break;
            case FoodState.OnFire:
                foodStateUI.sprite = onFirePicture;
                break;
            case FoodState.Boiled:
                foodStateUI.sprite = cookedPicture; //ไปเเยกมา
                break;
            case FoodState.Grilled:
                foodStateUI.sprite = cookedPicture;
                break;
            default:
                foodStateUI.gameObject.SetActive(false);
                break;
        }
    }

    public void SetUpFoodItem(int id)
    {
        foodID = id;
        FoodName = GameSceneManager.GetInstance().GetFoodNameById(foodID);
    }

    public void SetBannedId(int id)
    {
        trayBandedId = id;
    }

    public void SetFoodOnStove(bool isOnStove)
    {
        foodOnStove = isOnStove;
    }

    public void SetFoodOnChoppingBoard(bool isOnChoppingBoard)
    {
        foodOnChoppingBoard = isOnChoppingBoard;
    }

    public void SetFoodOnCounter(bool isOnCounter)
    {
        foodOnCounter = isOnCounter;
    }

    public void SetFoodIntoPot(bool isIntoPot)
    {
        foodIntoPot = isIntoPot;
    }

    public void PutFoodInTheStove()
    {
        currentFoodCookLevel += Time.deltaTime;
        percentage = (currentFoodCookLevel / onFireValue) * 100;

        SetShowTimerSlider(true);
        tempSliderValue = timerSlider.value;
        float SetFoodOnFireValue = maxFoodCookLevel + 50f;

        leantweenID = LeanTween.value(tempSliderValue, SetFoodOnFireValue + 100f, cookTimer).setOnUpdate((Value) =>
        {
            tempSliderValue = Value;
            if (timerSlider.value <= timerSlider.maxValue && currentFoodState == FoodState.Raw)
                timerSlider.value = Value;

            if (timerSlider.value >= timerSlider.maxValue && currentFoodState == FoodState.Raw)
            {
                timerSlider.value = 0;
                SetShowTimerSlider(false);
                currentFoodState = FoodState.Grilled;
                AudioSource audio = GetComponent<AudioSource>();//
                audio.PlayOneShot(complete);//
            }

            if (tempSliderValue >= SetFoodOnFireValue && currentFoodState == FoodState.Grilled)
            {
                currentFoodState = FoodState.Alert;
                AudioSource audio = GetComponent<AudioSource>();//
                audio.PlayOneShot(Alert_fire);//
            }
            ChangeFoodVisualAccordingToStates();
            SetFoodUIState();
           
        }).setOnComplete(() =>
        {
            currentFoodState = FoodState.OnFire;
            ChangeFoodVisualAccordingToStates();
            SetFoodUIState();
            

        }).id;
    }

    public void ChopFood()
    {
        if (timerSlider.value <= maxFoodCookLevel && CompareCurrentFoodState(FoodState.Grilled) ||
            timerSlider.value <= maxFoodCookLevel && CompareCurrentFoodState(FoodState.Alert))
        {
            if (!timerSlider.gameObject.activeInHierarchy)
                SetShowTimerSlider(true);

            currentFoodCookLevel += Time.deltaTime * 40f;
            percentage = (currentFoodCookLevel / maxFoodCookLevel) * 100;
            timerSlider.value = percentage;
            tempSliderValue = percentage;

            if (Input.GetKeyDown(KeyCode.H)) {
                AudioSource audio = GetComponent<AudioSource>();//
                audio.PlayOneShot(chopping);//
            }
            
            if (percentage >= 100)
            {
                currentFoodState = FoodState.Chop;
                timerSlider.value = 0;
                SetShowTimerSlider(false);
                ChangeFoodVisualAccordingToStates();
            }
        }
    }

    public void PutFoodInThePot()
    {
        if (currentFoodState != FoodState.Raw)
            return;

        SetShowTimerSlider(true);
        tempSliderValue = timerSlider.value;
        float SetFoodOnFireValue = maxFoodCookLevel + 50f;

        leantweenID = LeanTween.value(tempSliderValue, SetFoodOnFireValue + 100f, cookTimer).setOnUpdate(
            (float Value) =>
            {
                tempSliderValue = Value;
                if (timerSlider.value <= timerSlider.maxValue && currentFoodState == FoodState.Raw)
                    timerSlider.value = Value;

                if (timerSlider.value >= timerSlider.maxValue && currentFoodState == FoodState.Raw)
                {
                    timerSlider.value = 0;
                    SetShowTimerSlider(false);
                    currentFoodState = FoodState.Boiled;

                    AudioSource audio1 = GetComponent<AudioSource>();//
                    audio1.PlayOneShot(complete);//
                }

                if (tempSliderValue >= SetFoodOnFireValue && currentFoodState == FoodState.Boiled)
                {
                    currentFoodState = FoodState.Alert;
                    AudioSource audio = GetComponent<AudioSource>();//
                    audio.PlayOneShot(Alert_fire);//
                }
                ChangeFoodVisualAccordingToStates();
                SetFoodUIState();
            }).setOnComplete(() =>
        {
            currentFoodState = FoodState.OnFire;
            ChangeFoodVisualAccordingToStates();
            SetFoodUIState();
        }).id;
    }

    private void ChangeFoodVisualAccordingToStates() // show visual
    {
        switch (currentFoodState)
        {
            case FoodState.Raw:
                if (rawModel == null) return;
                SelectFoodModel(rawModel);
                break;

            case FoodState.Chop:
                if (chopModel == null) return;
                SelectFoodModel(chopModel);
                break;

            case FoodState.Boiled:
                if (boiledModel == null) return;
                SelectFoodModel(boiledModel);
                break;

            case FoodState.Grilled:
                if (grilledModel == null) return;
                SelectFoodModel(grilledModel);
                break;

            case FoodState.Alert:
                currentFoodModel.GetComponent<Renderer>().material.color = Color.Lerp(Color.yellow, Color.red, 3f);
                break;

            case FoodState.OnFire:
                currentFoodModel.GetComponent<Renderer>().material.color = Color.Lerp(Color.red, Color.black, 3f);
                break;

            default:
                Debug.LogError("Default wtf");
                break;
        }
    }

    private void SelectFoodModel(GameObject targetFoodModel)
    {
        currentFoodModel = targetFoodModel;
        targetFoodModel.SetActive(true);
        DisableUnusedModel(targetFoodModel);
    }

    private void DisableUnusedModel(GameObject exceptionChild)
    {
        for (int i = 0; i < modelContainer.transform.childCount; i++)
        {
            if (modelContainer.transform.GetChild(i).gameObject != exceptionChild)
                modelContainer.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public bool CanNotPickupWithHands()
    {
        if (CompareCurrentFoodState(FoodState.Boiled) || CompareCurrentFoodState(FoodState.Chop))
            return false;

        return true;
    }

    void OnDestroy()
    {
        LeanTween.cancel(leantweenID);
    }
}