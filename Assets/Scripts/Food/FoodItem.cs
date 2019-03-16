using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class FoodItem : MonoBehaviour
{
    [SerializeField] private Collider collider;
    
    [SerializeField] private int foodID;

    [SerializeField] private string FoodName;

    [SerializeField] private int trayBandedId;

    [SerializeField] private bool foodOnStove;

    [SerializeField] private bool foodIntoDumplingSteamed;

    [SerializeField] private bool foodOnChoppingBoard;

    [SerializeField] private bool foodOnPan;

    [SerializeField] private bool foodOnCounter;

    [SerializeField] private bool foodIntoPot;

    public AudioClip chopping, Alert_fire, complete, grilling, boiling;

//    public enum FoodState
//    {
//        Raw,
//        Grilled,
//        Chopped,
//        Fried,
//        Boiled,
//        Alert,
//        OnFire,
//        Steamed,
//        Done
//    }
    
    [SerializeField] private GameObject rawModel;

    [SerializeField] private GameObject grilledModel;

    [SerializeField] private GameObject friedModel;

    [SerializeField] private GameObject chopModel;

    [SerializeField] private GameObject boiledModel;

    [SerializeField] private GameObject steamedModel;

    [SerializeField] private GameObject modelContainer;

    private GameObject currentFoodModel;

  
    [SerializeField] private Slider timerSlider;
    [SerializeField] private Image foodStateUI;
    [SerializeField] private Sprite cookedPicture;
    [SerializeField] private Sprite onFirePicture;
    [SerializeField] private Sprite alertPicture;
    [SerializeField] private Sprite friedPicture;
    [SerializeField] private Sprite steamedPicture;

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
    [SerializeField] private FoodType.FoodItemType foodtype;

    public FoodType.FoodItemType GetFoodType()
    {
        return foodtype;
    }

    [SerializeField] private FoodStateGlobal.FoodState doneState;
    public FoodStateGlobal.FoodState CurrentFoodState;
    

    void Start()
    {
        timerSlider.value = 0;

        onFireValue = 2;

//        CurrentFoodState = FoodStateGlobal.FoodState.Raw;
        ChangeFoodVisualAccordingToStates();
        timerSlider.wholeNumbers = false;
        SetDefaultFoodUI();
        FoodItemAudioSource = GetComponent<AudioSource>(); //
    }

    public void SetDefaultFoodUI()
    {
        if (CurrentFoodState == FoodStateGlobal.FoodState.Raw && timerSlider.value > 0)
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
        switch (CurrentFoodState)
        {
            case FoodStateGlobal.FoodState.Alert:
                foodStateUI.sprite = alertPicture;
                break;
            case FoodStateGlobal.FoodState.OnFire:
                foodStateUI.sprite = onFirePicture;
                break;
            case FoodStateGlobal.FoodState.Boiled:
                foodStateUI.sprite = cookedPicture;
                break;
            case FoodStateGlobal.FoodState.Grilled:
                foodStateUI.sprite = cookedPicture;
                break;
            case FoodStateGlobal.FoodState.Fried:
                foodStateUI.sprite = friedPicture;
                break;
            case FoodStateGlobal.FoodState.Steamed:
                foodStateUI.sprite = steamedPicture;
                break;
            case FoodStateGlobal.FoodState.Done:
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
            if (timerSlider.value <= timerSlider.maxValue && CurrentFoodState == FoodStateGlobal.FoodState.Raw)
            {
                if (!timerSlider.gameObject.activeInHierarchy)
                    SetShowTimerSlider(true);
                timerSlider.value = Value;
            }
            else
                SetShowTimerSlider(false);

            if (timerSlider.value >= timerSlider.maxValue && CurrentFoodState == FoodStateGlobal.FoodState.Raw)
            {
                CurrentFoodState = FoodStateGlobal.FoodState.Grilled;
                FoodItemAudioSource.PlayOneShot(complete);
            }
            
            if (tempSliderValue <= SetFoodOnFireValue +50 && CurrentFoodState != FoodStateGlobal.FoodState.Raw )
            {
                if (CompareCurrentFoodState(doneState))
                {
                    CurrentFoodState = FoodStateGlobal.FoodState.Done;   
                    Debug.LogError("set done");;
                }
            }
            
            if (tempSliderValue >= SetFoodOnFireValue + 50f && CurrentFoodState == FoodStateGlobal.FoodState.Grilled)
            {
                CurrentFoodState = FoodStateGlobal.FoodState.Alert;
                timerSlider.value = 0;
                FoodItemAudioSource.PlayOneShot(Alert_fire); 
            }

            ChangeFoodVisualAccordingToStates();
            SetFoodUIState();
        }).setOnComplete(() =>
        {
            CurrentFoodState = FoodStateGlobal.FoodState.OnFire;
            ChangeFoodVisualAccordingToStates();
            tempSliderValue = 0f;
            SetFoodUIState();
        }).id;
    }

    //-------------------------------------------------------------------------------------------------------------------------------
    public bool PutFoodInTheDumplingSteamed()
    {
        //TODO make leantwen count down here then return
        Debug.LogError("Put this " + GetFoodItemName()+  "id : " + GetFoodItemId().ToString() + "into the steamer");
        return false;
    }
    //-------------------------------------------------------------------------------------------------------------------------------

    public void PutFoodInThePan()
    {
        if (tempSliderValue != 0)
            timerSlider.value = tempSliderValue;

        SetFoodOnFireValue = maxFoodCookLevel;

        if (!CheckFoodStateBeforeActions())
            return;

        if (foodID == 3)
        {
            leantweenID = LeanTween.value(tempSliderValue, SetFoodOnFireValue + 100f, cookTimer).setOnUpdate((Value) =>
            {
                if (!FoodItemAudioSource.isPlaying && grilling != null)
                    FoodItemAudioSource.PlayOneShot(grilling);

                tempSliderValue = Value;

                if (timerSlider.value <= timerSlider.maxValue && CurrentFoodState == FoodStateGlobal.FoodState.Raw)
                {
                    if (!timerSlider.gameObject.activeInHierarchy)
                        SetShowTimerSlider(true);
                    timerSlider.value = Value;
                }
                else
                    SetShowTimerSlider(false);

                if (timerSlider.value >= timerSlider.maxValue && CurrentFoodState == FoodStateGlobal.FoodState.Raw)
                {
                    CurrentFoodState = FoodStateGlobal.FoodState.Fried;
                    FoodItemAudioSource.PlayOneShot(complete);
                }

                if (tempSliderValue <= SetFoodOnFireValue + 50 && CurrentFoodState != FoodStateGlobal.FoodState.Raw)
                {
                    if (CompareCurrentFoodState(doneState))
                    {
                        CurrentFoodState = FoodStateGlobal.FoodState.Done;
                        Debug.LogError("set done"); ;
                    }
                }

                if (tempSliderValue >= SetFoodOnFireValue + 50f && CurrentFoodState == FoodStateGlobal.FoodState.Fried)
                {
                    CurrentFoodState = FoodStateGlobal.FoodState.Alert;
                    timerSlider.value = 0;
                    FoodItemAudioSource.PlayOneShot(Alert_fire);
                }

                ChangeFoodVisualAccordingToStates();
                SetFoodUIState();
            }).setOnComplete(() =>
            {
                CurrentFoodState = FoodStateGlobal.FoodState.OnFire;
                ChangeFoodVisualAccordingToStates();
                tempSliderValue = 0f;
                SetFoodUIState();
            }).id;
        }
        else if (foodID == 4)
        {
            leantweenID = LeanTween.value(tempSliderValue, SetFoodOnFireValue + 100f, cookTimer).setOnUpdate((Value) =>
            {
                if (!FoodItemAudioSource.isPlaying && grilling != null)
                    FoodItemAudioSource.PlayOneShot(grilling);

                tempSliderValue = Value;

                if (timerSlider.value <= timerSlider.maxValue && CurrentFoodState == FoodStateGlobal.FoodState.Chopped)
                {
                    if (!timerSlider.gameObject.activeInHierarchy)
                        SetShowTimerSlider(true);
                    timerSlider.value = Value;
                }
                else
                    SetShowTimerSlider(false);

                if (timerSlider.value >= timerSlider.maxValue && CurrentFoodState == FoodStateGlobal.FoodState.Chopped)
                {
                    CurrentFoodState = FoodStateGlobal.FoodState.Fried;
                    FoodItemAudioSource.PlayOneShot(complete);
                }

                if (tempSliderValue <= SetFoodOnFireValue + 50 && CurrentFoodState != FoodStateGlobal.FoodState.Chopped)
                {
                    if (CompareCurrentFoodState(doneState))
                    {
                        CurrentFoodState = FoodStateGlobal.FoodState.Done;
                        Debug.LogError("set done"); ;
                    }
                }

                if (tempSliderValue >= SetFoodOnFireValue + 50f && CurrentFoodState == FoodStateGlobal.FoodState.Fried)
                {
                    CurrentFoodState = FoodStateGlobal.FoodState.Alert;
                    timerSlider.value = 0;
                    FoodItemAudioSource.PlayOneShot(Alert_fire);
                }

                ChangeFoodVisualAccordingToStates();
                SetFoodUIState();
            }).setOnComplete(() =>
            {
                CurrentFoodState = FoodStateGlobal.FoodState.OnFire;
                ChangeFoodVisualAccordingToStates();
                //tempSliderValue = 0f;
                SetFoodUIState();
            }).id;
        }
    }

    private bool CheckFoodStateBeforeActions()
    {
        switch (CurrentFoodState)
        {
            case FoodStateGlobal.FoodState.Grilled:
                tempSliderValue = SetFoodOnFireValue + 50f;
                ForceFoodState();
                return false;
            case FoodStateGlobal.FoodState.Boiled:
                tempSliderValue = SetFoodOnFireValue + 50f;
                ForceFoodState();
                return false;
            case FoodStateGlobal.FoodState.Alert:
                CurrentFoodState = FoodStateGlobal.FoodState.OnFire;
                ChangeFoodVisualAccordingToStates();
                SetFoodUIState();
                return false;
            case FoodStateGlobal.FoodState.OnFire:
                CurrentFoodState = FoodStateGlobal.FoodState.OnFire;
                ChangeFoodVisualAccordingToStates();
                SetFoodUIState();
                return false;
            case FoodStateGlobal.FoodState.Fried:
                tempSliderValue = SetFoodOnFireValue + 50f;
                ForceFoodState();
                return false;
            case FoodStateGlobal.FoodState.Steamed:
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

            CurrentFoodState = FoodStateGlobal.FoodState.Alert;
            timerSlider.value = 0;
            
            soundStart += Time.deltaTime;
            if (soundStart >= soundLength && CurrentFoodState != FoodStateGlobal.FoodState.OnFire)
            {
                FoodItemAudioSource.PlayOneShot(Alert_fire);
                soundStart = 0;
            }
           
            ChangeFoodVisualAccordingToStates();
            SetFoodUIState();
        }).setOnComplete(() =>
        {
            CurrentFoodState = FoodStateGlobal.FoodState.OnFire;
            ChangeFoodVisualAccordingToStates();
            tempSliderValue = 0f;
            SetFoodUIState();
        }).id;
    }

    public void ChopFood()
    {
        if (foodID == 4 || foodID == 6 || foodID == 7 || foodID == 8 || foodID == 9 || foodID == 12 || foodID == 13)
        {
            if (timerSlider.value <= maxFoodCookLevel && CompareCurrentFoodState(FoodStateGlobal.FoodState.Raw))
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
                    CurrentFoodState = FoodStateGlobal.FoodState.Chopped;
                    /*if (CompareCurrentFoodState(DoneState))
                        currentFoodState = FoodState.Done;*/
                    timerSlider.value = 0;
                    SetShowTimerSlider(false);
                    ChangeFoodVisualAccordingToStates();
                    tempSliderValue = 0f;
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
        else
        {
            if (timerSlider.value <= maxFoodCookLevel && CompareCurrentFoodState(FoodStateGlobal.FoodState.Grilled) ||
            timerSlider.value <= maxFoodCookLevel && CompareCurrentFoodState(FoodStateGlobal.FoodState.Fried) ||
            timerSlider.value <= maxFoodCookLevel && CompareCurrentFoodState(FoodStateGlobal.FoodState.Alert))
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
                    CurrentFoodState = FoodStateGlobal.FoodState.Chopped;
                    if (CompareCurrentFoodState(doneState))
                        CurrentFoodState = FoodStateGlobal.FoodState.Done;
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
    }

    public void PutFoodInThePot()
    {
        if (CurrentFoodState != FoodStateGlobal.FoodState.Raw)
            return;

        if (tempSliderValue != 0)
            timerSlider.value = tempSliderValue;

        SetFoodOnFireValue = maxFoodCookLevel;

        leantweenID = LeanTween.value(tempSliderValue, SetFoodOnFireValue + 100f, cookTimer).setOnUpdate(
            (float Value) =>
            {
                if (!FoodItemAudioSource.isPlaying && boiling != null)
                    FoodItemAudioSource.PlayOneShot(boiling);

                tempSliderValue = Value;
                if (timerSlider.value <= timerSlider.maxValue && CurrentFoodState == FoodStateGlobal.FoodState.Raw)
                {
                    if (!timerSlider.gameObject.activeInHierarchy)
                        SetShowTimerSlider(true);
                    timerSlider.value = Value;
                }
                else
                    SetShowTimerSlider(false);

                if (timerSlider.value >= timerSlider.maxValue && CurrentFoodState == FoodStateGlobal.FoodState.Raw  && CurrentFoodState != FoodStateGlobal.FoodState.Done)
                {
                    timerSlider.value = 0;

                    CurrentFoodState = FoodStateGlobal.FoodState.Boiled;
                    FoodItemAudioSource.PlayOneShot(complete);
                }

                if (tempSliderValue <= SetFoodOnFireValue +50 && CurrentFoodState != FoodStateGlobal.FoodState.Raw )
                {
                    if (CompareCurrentFoodState(doneState))
                    {
                        CurrentFoodState = FoodStateGlobal.FoodState.Done;   
                        Debug.LogError("set done");;
                    }
                }

                if (tempSliderValue >= SetFoodOnFireValue + 50f)
                {
                    CurrentFoodState = FoodStateGlobal.FoodState.Alert;
                    FoodItemAudioSource.PlayOneShot(Alert_fire);
                }

                ChangeFoodVisualAccordingToStates();
                SetFoodUIState();
            }).setOnComplete(() =>
        {
            CurrentFoodState = FoodStateGlobal.FoodState.OnFire;
            ChangeFoodVisualAccordingToStates();
            SetFoodUIState();
        }).id;
    }

    public void ChangeFoodVisualAccordingToStates() // show visual
    {
        switch (CurrentFoodState)
        {
            case FoodStateGlobal.FoodState.Raw:
                if (rawModel == null) return;
                SelectFoodModel(rawModel);
                break;

            case FoodStateGlobal.FoodState.Chopped:
                if (chopModel == null) return;
                SelectFoodModel(chopModel);
                break;

            case FoodStateGlobal.FoodState.Boiled:
                if (boiledModel == null) return;
                SelectFoodModel(boiledModel);
                break;

            case FoodStateGlobal.FoodState.Grilled:
                if (grilledModel == null) return;
                SelectFoodModel(grilledModel);
                break;

            case FoodStateGlobal.FoodState.Fried:
                if (friedModel == null) return;
                SelectFoodModel(friedModel);
                break;

            case FoodStateGlobal.FoodState.Steamed:
                if (friedModel == null) return;
                SelectFoodModel(steamedModel);
                break;

            case FoodStateGlobal.FoodState.Done:
                SetFoodDoneVisual();
                break;

            case FoodStateGlobal.FoodState.Alert:
                if (currentFoodModel != null)
                currentFoodModel.GetComponent<Renderer>().material.color = Color.Lerp(Color.yellow, Color.red, 3f);
                else
                {
                    GetComponent<Renderer>().material.color = Color.Lerp(Color.yellow, Color.red, 3f);
                }
                break;

            case FoodStateGlobal.FoodState.OnFire:
                if (currentFoodModel != null)
                currentFoodModel.GetComponent<Renderer>().material.color = Color.Lerp(Color.red, Color.black, 3f);
                else
                {
                    GetComponent<Renderer>().material.color = Color.Lerp(Color.red, Color.black, 3f);
                }
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
        return CompareCurrentFoodState(FoodStateGlobal.FoodState.Done);
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

    //-----------------------------------------------------------------------
    public void SetFoodIntoDumplingSteamed(bool isIntoDumplingSteamed)
    {
        foodIntoDumplingSteamed = isIntoDumplingSteamed;
    }
    //-----------------------------------------------------------------------

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
        return CurrentFoodState == FoodStateGlobal.FoodState.Chopped;
    }

    public bool IsFoodGriled()
    {
        return CurrentFoodState == FoodStateGlobal.FoodState.Grilled;
    }

    public bool IsFoodFried()
    {
        return CurrentFoodState == FoodStateGlobal.FoodState.Fried;
    }

    public bool IsFoodOnFire()
    {
        return CurrentFoodState == FoodStateGlobal.FoodState.OnFire;
    }

    public bool IsFoodBoiled()
    {
        return CurrentFoodState == FoodStateGlobal.FoodState.Boiled;
    }

    public bool IsFoodAlert()
    {
        return CurrentFoodState == FoodStateGlobal.FoodState.Alert;
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

    public bool CompareCurrentFoodState(FoodStateGlobal.FoodState foodStateGlobal)
    {
        return CurrentFoodState == foodStateGlobal;
    }

    public FoodStateGlobal.FoodState GetDoneState()
    {
        return doneState;
    }

    public bool FoodIsDone()
    {
        return CurrentFoodState == doneState;
    }

    public void EnableFoodItemCollider(bool colliderEnable)
    {
        collider.enabled = colliderEnable;
    }

    public void DestroyFoodItemCollider()
    {
        Destroy(collider);
    }
}