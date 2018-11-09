using System;
using UnityEngine;
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

    [SerializeField] private Renderer foodRenderer;

    public enum FoodState
    {
        Raw,
        Grilled,
        Chop,
        Boiled,
        Alert,
        OnFire
    }

    private FoodState currentFoodState;
    
    [SerializeField] private Slider timerSlider;
    [SerializeField] private Image foodStateUI;
    [SerializeField] private Sprite cookedPicture;
    [SerializeField] private Sprite onFirePicture;
    [SerializeField] private Sprite alertPicture;
    
    [SerializeField] private int min = 0;
    [SerializeField] private int max = 100;
    [SerializeField] private const float cookTimer = 20f;

    [SerializeField] private float percentage;
    [SerializeField] private float foodValue;
    [SerializeField] private float onFireValue;
    [SerializeField] private float tempSliderValue;

    void Start()
    {
        timerSlider.value = 0;
        onFireValue = 2;

        currentFoodState = FoodState.Raw;
        timerSlider.wholeNumbers = false;
        SetDefaultFoodUI();
        foodRenderer = GetComponent<Renderer>();
        
        if (!foodRenderer)
            gameObject.AddComponent<Renderer>();

    }

    public void SetDefaultFoodUI()
    {
        SetShowTimerSlider(false);
      foodStateUI.gameObject.SetActive(false);
    }
    
    private void SetShowTimerSlider(bool show)
    {
        timerSlider.gameObject.SetActive(show);
    }

    private void SetFoodUIState()
    {
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
                foodStateUI.sprite = cookedPicture;
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

    public int GetFoodItemId()
    {
        return foodID;
    }

    public bool IsFoodChoped()
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

    public string GetFoodItemName()
    {
        return FoodName;
    }

    public int GetBanedID()
    {
        return trayBandedId;
    }

    public void SetBannedId(int id)
    {
        trayBandedId = id;
    }

    public void SetFoodOnStove(bool isOnStove)
    {
        foodOnStove = isOnStove;
    }

    public bool GetFoodOnStove()
    {
        return foodOnStove;
    }

    public void SetFoodOnChoppingBoard(bool isOnChoppingBoard)
    {
        foodOnChoppingBoard = isOnChoppingBoard;
    }

    public bool GetFoodOnChoppingBoard()
    {
        return foodOnChoppingBoard;
    }

    public void SetFoodOnCounter(bool isOnCounter)
    {
        foodOnCounter = isOnCounter;
    }

    public bool GetFoodOnCounter()
    {
        return foodOnCounter;
    }

    public void SetFoodIntoPot(bool isIntoPot)
    {
        foodIntoPot = isIntoPot;
    }

    public bool GetFoodIntoPot()
    {
        return foodIntoPot;
    }

    public bool CompareCurrentFoodState(FoodState foodState)
    {
        return currentFoodState == foodState;
    }

    public void PutFoodInTheStove()
    {
        if (currentFoodState != FoodState.Raw)
            return;

        
        SetShowTimerSlider(true);
        tempSliderValue = 0f;
        float SetFoodOnFireValue = max + 50f;
        LeanTween.value(tempSliderValue , SetFoodOnFireValue +100f, cookTimer).setOnUpdate((Value) =>
        {
            tempSliderValue = Value;
            timerSlider.value = Value;
            
            if ( timerSlider.value >= timerSlider.maxValue && currentFoodState == FoodState.Raw)
            {
                SetShowTimerSlider(false);
                timerSlider.value = 0;
                currentFoodState = FoodState.Grilled;
                ChangeFoodVisualAccordingToStates();
                Debug.Log("food is cooked");
            }
            if ( tempSliderValue >= SetFoodOnFireValue && currentFoodState == FoodState.Grilled)
            {
                currentFoodState = FoodState.Alert;
                ChangeFoodVisualAccordingToStates();
                Debug.Log("food is in Alert state");
            }
          SetFoodUIState();
        }).setOnComplete(() =>
        {
            currentFoodState = FoodState.OnFire;
            ChangeFoodVisualAccordingToStates();
            SetFoodUIState();
            Debug.Log("food is Overcooked");
        });
    }

    public void ChopFood(GameObject target)
    {
        foodValue += Time.deltaTime;
        percentage = (foodValue / onFireValue) * 100;

        if (!target)
            return;

        if (timerSlider.value > 0)
            SetShowTimerSlider(true);

        if (timerSlider.value < max && CompareCurrentFoodState(FoodState.Grilled) && !CompareCurrentFoodState(FoodState.Chop) && !CompareCurrentFoodState(FoodState.OnFire))
        {
            timerSlider.value += Time.deltaTime;

            if (percentage >= 100)
            {
                currentFoodState = FoodState.Chop;
                SetShowTimerSlider(false);
                ChangeFoodVisualAccordingToStates();
                timerSlider.value = 0;
            }

            tempSliderValue = percentage;
            Debug.Log(percentage + "Choped");
        }
    }

    public void PutFoodInThePot()
    {
        if (currentFoodState != FoodState.Raw)
            return;

        
        SetShowTimerSlider(true);
        tempSliderValue = 0f;
        float SetFoodOnFireValue = max + 50f;
        LeanTween.value(tempSliderValue , SetFoodOnFireValue +100f, cookTimer).setOnUpdate((float Value) =>
        {
            tempSliderValue = Value;
            timerSlider.value = Value;
            
            if ( timerSlider.value >= timerSlider.maxValue && currentFoodState == FoodState.Raw)
            {
                SetShowTimerSlider(false);
                timerSlider.value = 0;
                currentFoodState = FoodState.Boiled;
                ChangeFoodVisualAccordingToStates();
                Debug.Log("food is cooked");
            }
            if ( tempSliderValue >= SetFoodOnFireValue && currentFoodState == FoodState.Boiled)
            {
                currentFoodState = FoodState.Alert;
                ChangeFoodVisualAccordingToStates();
                Debug.Log("food is in Alert state");
            }
            
            SetFoodUIState();
          
        }).setOnComplete(() =>
        {
            currentFoodState = FoodState.OnFire;
            ChangeFoodVisualAccordingToStates();
            SetFoodUIState();
            Debug.Log("food is Overcooked");
        });
    }

    private void ChangeFoodVisualAccordingToStates()
    {
        
        
        if (CompareCurrentFoodState(FoodState.Grilled) && !IsFoodChoped() && !IsFoodOnFire())
            foodRenderer.material.color = Color.green;

        if (CompareCurrentFoodState(FoodState.OnFire) && !IsFoodChoped() && IsFoodOnFire())
            foodRenderer.material.color  = Color.red;

        if (IsFoodChoped())
            foodRenderer.material.color  = Color.blue;

        if (CompareCurrentFoodState(FoodState.Boiled))
            foodRenderer.material.color  = Color.blue;
    }
}