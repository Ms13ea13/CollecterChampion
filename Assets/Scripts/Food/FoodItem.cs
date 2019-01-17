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

    [SerializeField] private bool foodOnPan;

    [SerializeField] private bool foodOnCounter;

    [SerializeField] private bool foodIntoPot;

    public AudioClip chopping, Alert_fire, complete, grilling;

    public enum FoodState
    {
        Raw,
        Grilled,
        Chopped,
        Fried,
        Boiled,
        Alert,
        OnFire,
        Done
    }

    [SerializeField] private GameObject rawModel;

    [SerializeField] private GameObject grilledModel;

    [SerializeField] private GameObject friedModel;

    [SerializeField] private GameObject chopModel;

    [SerializeField] private GameObject boiledModel;

    [SerializeField] private GameObject modelContainer;

    private GameObject currentFoodModel;

  
    [SerializeField] private Slider timerSlider;
    [SerializeField] private Image foodStateUI;
    [SerializeField] private Sprite cookedPicture;
    [SerializeField] private Sprite onFirePicture;
    [SerializeField] private Sprite alertPicture;
    [SerializeField] private Sprite friedPicture;

    private int leantweenID;
    private const float cookTimer = 20f;

    [SerializeField] private int min = 0;

    [FormerlySerializedAs("max")] [SerializeField]
    private int maxFoodCookLevel = 150;

    [SerializeField] private float percentage;

    [FormerlySerializedAs("foodValue")] [SerializeField]
    private float currentFoodCookLevel;

    [SerializeField] private float onFireValue;
    [SerializeField] private float tempSliderValue;
    private float SetFoodOnFireValue;

    private AudioSource FoodItemAudioSource;

    private float soundLength;
    private float soundStart = 0f;
    [SerializeField] private FoodState DoneState;
    [SerializeField] private FoodState currentFoodState;

    void Start()
    {
        timerSlider.value = 0;

        onFireValue = 2;

        currentFoodState = FoodState.Raw;
        ChangeFoodVisualAccordingToStates();
        timerSlider.wholeNumbers = false;
        SetDefaultFoodUI();
        FoodItemAudioSource = GetComponent<AudioSource>(); //
    }

    public FoodState GetFoodItemState()
    {
        return currentFoodState;
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
            throw new Exception("Food state UI is null");

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
            case FoodState.Fried:
                foodStateUI.sprite = friedPicture;
                break;
            case FoodState.Done:
                foodStateUI.sprite = cookedPicture;
                break;
            default:
                foodStateUI.gameObject.SetActive(false);
                break;
        }
    }

    public void PutFoodInTheStove()
    {
        if (tempSliderValue != 0)
            timerSlider.value = tempSliderValue;

        SetFoodOnFireValue = maxFoodCookLevel;
        if (!CheckFoodStateBeforeActions())
            return;

        leantweenID = LeanTween.value(tempSliderValue, SetFoodOnFireValue + 100f, cookTimer).setOnUpdate((Value) =>
        {
            if (!FoodItemAudioSource.isPlaying && grilling != null)
                FoodItemAudioSource.PlayOneShot(grilling);

            tempSliderValue = Value;
            if (timerSlider.value <= timerSlider.maxValue && currentFoodState == FoodState.Raw)
            {
                if (!timerSlider.gameObject.activeInHierarchy)
                    SetShowTimerSlider(true);
                timerSlider.value = Value;
            }
            else
                SetShowTimerSlider(false);

            if (timerSlider.value >= timerSlider.maxValue && currentFoodState == FoodState.Raw)
            {
                currentFoodState = FoodState.Grilled;
                FoodItemAudioSource.PlayOneShot(complete);
            }
            
            if (tempSliderValue <= SetFoodOnFireValue +50 && currentFoodState != FoodState.Raw )
            {
                if (CompareCurrentFoodState(DoneState))
                {
                    currentFoodState = FoodState.Done;   
                    Debug.LogError("set done");;
                }
            }
            

            if (tempSliderValue >= SetFoodOnFireValue + 50f && currentFoodState == FoodState.Grilled)
            {
                currentFoodState = FoodState.Alert;
                timerSlider.value = 0;
                FoodItemAudioSource.PlayOneShot(Alert_fire); 
            }

            ChangeFoodVisualAccordingToStates();
            SetFoodUIState();
        }).setOnComplete(() =>
        {
            currentFoodState = FoodState.OnFire;
            ChangeFoodVisualAccordingToStates();
            tempSliderValue = 0f;
            SetFoodUIState();
        }).id;
    }

    public void PutFoodInThePan()
    {
        if (tempSliderValue != 0)
            timerSlider.value = tempSliderValue;

        SetFoodOnFireValue = maxFoodCookLevel;
        if (!CheckFoodStateBeforeActions())
            return;

        leantweenID = LeanTween.value(tempSliderValue, SetFoodOnFireValue + 100f, cookTimer).setOnUpdate((Value) =>
        {
            if (!FoodItemAudioSource.isPlaying && grilling != null)
                FoodItemAudioSource.PlayOneShot(grilling);

            tempSliderValue = Value;
            if (timerSlider.value <= timerSlider.maxValue && currentFoodState == FoodState.Raw)
            {
                if (!timerSlider.gameObject.activeInHierarchy)
                    SetShowTimerSlider(true);
                timerSlider.value = Value;
            }
            else
                SetShowTimerSlider(false);

            if (timerSlider.value >= timerSlider.maxValue && currentFoodState == FoodState.Raw)
            {
                currentFoodState = FoodState.Fried;
                FoodItemAudioSource.PlayOneShot(complete);
            }
            
            if (tempSliderValue <= SetFoodOnFireValue +50 && currentFoodState != FoodState.Raw )
            {
                if (CompareCurrentFoodState(DoneState))
                {
                    currentFoodState = FoodState.Done;   
                    Debug.LogError("set done");;
                }
            }

            if (tempSliderValue >= SetFoodOnFireValue + 50f && currentFoodState == FoodState.Fried)
            {
                currentFoodState = FoodState.Alert;
                timerSlider.value = 0;
                FoodItemAudioSource.PlayOneShot(Alert_fire);
            }

            ChangeFoodVisualAccordingToStates();
            SetFoodUIState();
        }).setOnComplete(() =>
        {
            currentFoodState = FoodState.OnFire;
            ChangeFoodVisualAccordingToStates();
            tempSliderValue = 0f;
            SetFoodUIState();
        }).id;
    }

    private bool CheckFoodStateBeforeActions()
    {
        switch (currentFoodState)
        {
            case FoodState.Grilled:
                tempSliderValue = SetFoodOnFireValue + 50f;
                ForceFoodState();
                return false;
            case FoodState.Boiled:
                tempSliderValue = SetFoodOnFireValue + 50f;
                ForceFoodState();
                return false;
            case FoodState.Alert:
                currentFoodState = FoodState.OnFire;
                ChangeFoodVisualAccordingToStates();
                SetFoodUIState();
                return false;
            case FoodState.OnFire:
                currentFoodState = FoodState.OnFire;
                ChangeFoodVisualAccordingToStates();
                SetFoodUIState();
                return false;
            case FoodState.Fried:
                tempSliderValue = SetFoodOnFireValue + 50f;
                ForceFoodState();
                return false;
            default:
                return true;
        }
    }

    private void ForceFoodState()
    {
        SetShowTimerSlider(false);
        soundStart = 0;
        soundLength = chopping.length;
       
        leantweenID = LeanTween.value(tempSliderValue, SetFoodOnFireValue + 100f, 3f).setOnUpdate((float Value) =>
        {
            if (!FoodItemAudioSource.isPlaying && grilling != null)
                FoodItemAudioSource.PlayOneShot(grilling);

            currentFoodState = FoodState.Alert;
            timerSlider.value = 0;
            
            soundStart += Time.deltaTime;
            if (soundStart >= soundLength && currentFoodState != FoodState.OnFire)
            {
                FoodItemAudioSource.PlayOneShot(Alert_fire);
                soundStart = 0;
            }
           
            ChangeFoodVisualAccordingToStates();
            SetFoodUIState();
        }).setOnComplete(() =>
        {
            currentFoodState = FoodState.OnFire;
            ChangeFoodVisualAccordingToStates();
            tempSliderValue = 0f;
            SetFoodUIState();
        }).id;
    }

    public void ChopFood()
    {
        if (timerSlider.value <= maxFoodCookLevel && CompareCurrentFoodState(FoodState.Grilled) ||
            timerSlider.value <= maxFoodCookLevel && CompareCurrentFoodState(FoodState.Fried) ||
            timerSlider.value <= maxFoodCookLevel && CompareCurrentFoodState(FoodState.Alert))
        {
            if (!timerSlider.gameObject.activeInHierarchy)
                SetShowTimerSlider(true);

            currentFoodCookLevel += Time.deltaTime * 40f;
            percentage = (currentFoodCookLevel / maxFoodCookLevel) * 100;
            timerSlider.value = percentage;
            tempSliderValue = percentage;

            soundLength = chopping.length;

            if (percentage >= 100)
            {
                currentFoodState = FoodState.Chopped;
                if (CompareCurrentFoodState(DoneState))
                    currentFoodState = FoodState.Done;
                timerSlider.value = 0;
                SetShowTimerSlider(false);
                ChangeFoodVisualAccordingToStates();
            }
            else
            {
                soundStart += Time.deltaTime;
                if (soundStart >= soundLength)
                {
                    FoodItemAudioSource.PlayOneShot(chopping);
                    soundStart = 0;
                }
            }
        }
    }

    public void PutFoodInThePot()
    {
        if (currentFoodState != FoodState.Raw)
            return;

        if (tempSliderValue != 0)
            timerSlider.value = tempSliderValue;

        SetFoodOnFireValue = maxFoodCookLevel;

        leantweenID = LeanTween.value(tempSliderValue, SetFoodOnFireValue + 100f, cookTimer).setOnUpdate(
            (float Value) =>
            {
                tempSliderValue = Value;
                if (timerSlider.value <= timerSlider.maxValue && currentFoodState == FoodState.Raw)
                {
                    if (!timerSlider.gameObject.activeInHierarchy)
                        SetShowTimerSlider(true);
                    timerSlider.value = Value;
                }
                else
                    SetShowTimerSlider(false);

                if (timerSlider.value >= timerSlider.maxValue && currentFoodState == FoodState.Raw  && currentFoodState != FoodState.Done)
                {
                    timerSlider.value = 0;

                    currentFoodState = FoodState.Boiled;
                    FoodItemAudioSource.PlayOneShot(complete);
                }

                if (tempSliderValue <= SetFoodOnFireValue +50 && currentFoodState != FoodState.Raw )
                {
                    if (CompareCurrentFoodState(DoneState))
                    {
                        currentFoodState = FoodState.Done;   
                        Debug.LogError("set done");;
                    }
                }

                if (tempSliderValue >= SetFoodOnFireValue + 50f)
                {
                    currentFoodState = FoodState.Alert;
                    FoodItemAudioSource.PlayOneShot(Alert_fire);
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

            case FoodState.Chopped:
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

            case FoodState.Fried:
                if (friedModel == null) return;
                SelectFoodModel(friedModel);
                break;
            case FoodState.Done:
                SetFoodDoneVisual();
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

    private void SetFoodDoneVisual()
    {
        if (friedModel != null) 
            SelectFoodModel(friedModel);
        
        if (grilledModel != null) 
        SelectFoodModel(grilledModel);
        
        if (boiledModel != null) 
        SelectFoodModel(boiledModel);
        
        if (chopModel != null) 
        SelectFoodModel(chopModel);
        
    }

    private void SelectFoodModel(GameObject targetFoodModel)
    {
        currentFoodModel = targetFoodModel;
        targetFoodModel.SetActive(true);
        DisableUnusedModel(targetFoodModel);
    }

    public void StopFoodItemSoundEffect()
    {
        FoodItemAudioSource.Stop();
    }

    private void DisableUnusedModel(GameObject exceptionChild)
    {
        for (int i = 0; i < modelContainer.transform.childCount; i++)
        {
            if (modelContainer.transform.GetChild(i).gameObject != exceptionChild)
                modelContainer.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public bool IsFoodDoneCooking()
    {
        return CompareCurrentFoodState(FoodState.Done);
    }

    void OnDestroy()
    {
        LeanTween.cancel(leantweenID);
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

    public void SetFoodOnPan(bool isOnPan)
    {
        foodOnPan = isOnPan;
    }

    public void SetFoodOnCounter(bool isOnCounter)
    {
        foodOnCounter = isOnCounter;
    }

    public void SetFoodIntoPot(bool isIntoPot)
    {
        foodIntoPot = isIntoPot;
    }
    
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
        return currentFoodState == FoodState.Chopped;
    }

    public bool IsFoodGriled()
    {
        return currentFoodState == FoodState.Grilled;
    }

    public bool IsFoodFried()
    {
        return currentFoodState == FoodState.Fried;
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

    public bool GetFoodOnPan()
    {
        return foodOnPan;
    }

    public bool CompareCurrentFoodState(FoodState foodState)
    {
        return currentFoodState == foodState;
    }
}