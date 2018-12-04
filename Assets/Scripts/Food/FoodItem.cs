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

    [SerializeField] private FoodState currentFoodState;
    [SerializeField] private Slider timerSlider;
    [SerializeField] private Image foodStateUI;
    [SerializeField] private Sprite cookedPicture;
    [SerializeField] private Sprite onFirePicture;
    [SerializeField] private Sprite alertPicture;

    private PotManager potManage;

    public bool chopDone = false;
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

        //currentFoodState = FoodState.Raw;
        timerSlider.wholeNumbers = false;
        SetDefaultFoodUI();
        foodRenderer = GetComponent<Renderer>();
        
        if (!foodRenderer)
            gameObject.AddComponent<Renderer>();
    }

    public void SetDefaultFoodUI()
    {
        if (currentFoodState == FoodState.Raw && timerSlider.value >0)
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
        currentFoodCookLevel += Time.deltaTime;
        percentage = (currentFoodCookLevel / onFireValue) * 100;
        
        SetShowTimerSlider(true);
        tempSliderValue = timerSlider.value;
        float SetFoodOnFireValue = maxFoodCookLevel + 50f;

        leantweenID = LeanTween.value(tempSliderValue , SetFoodOnFireValue + 100f, cookTimer).setOnUpdate((Value) =>
        {
            tempSliderValue = Value;
            if ( timerSlider.value <= timerSlider.maxValue && currentFoodState == FoodState.Raw)
            timerSlider.value = Value;
            
            if ( timerSlider.value >= timerSlider.maxValue && currentFoodState == FoodState.Raw)
            {
                timerSlider.value = 0;
                SetShowTimerSlider(false);
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
            
            if (percentage >= 100)
            {
                currentFoodState = FoodState.Chop;
                timerSlider.value = 0;
                SetShowTimerSlider(false);
                Destroy(gameObject);
                chopDone = true;
                Debug.Log(percentage + "Chopped");
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

        leantweenID = LeanTween.value(tempSliderValue , SetFoodOnFireValue + 100f, cookTimer).setOnUpdate((float Value) =>
        {
            tempSliderValue = Value;
            if ( timerSlider.value <= timerSlider.maxValue && currentFoodState == FoodState.Raw)
            timerSlider.value = Value;
            
            if ( timerSlider.value >= timerSlider.maxValue && currentFoodState == FoodState.Raw)
            {
                timerSlider.value = 0;
                SetShowTimerSlider(false);
                currentFoodState = FoodState.Boiled;
                //ChangeFoodVisualAccordingToStates();
                Destroy(gameObject);
                potManage.SpawnRice();
                SetFoodUIState();
                Debug.Log("food is cooked");
            }
            if ( tempSliderValue >= SetFoodOnFireValue && currentFoodState == FoodState.Boiled)
            {
                currentFoodState = FoodState.Alert;
                //ChangeFoodVisualAccordingToStates();
                SetFoodUIState();
                Debug.Log("food is in Alert state");
            }

            //SetFoodUIState();
        }).setOnComplete(() =>
        {
            currentFoodState = FoodState.OnFire;
            //ChangeFoodVisualAccordingToStates();
            SetFoodUIState();
            Debug.Log("food is Overcooked");
        }).id;
    }

    private void ChangeFoodVisualAccordingToStates()
    {
        if (CompareCurrentFoodState(FoodState.Grilled) && !IsFoodChopped() && !IsFoodOnFire())
            foodRenderer.material.color = Color.green;

        if (CompareCurrentFoodState(FoodState.OnFire) && !IsFoodChopped()&& IsFoodOnFire())
            foodRenderer.material.color  = Color.red;

        if (IsFoodChopped())
            foodRenderer.material.color = Color.blue;
            
        if (CompareCurrentFoodState(FoodState.Boiled))
            foodRenderer.material.color  = Color.blue;
    }

    public bool CanPickupWithHands()
    {
        if (FoodName == "Rice Boiled")
        {
            if (CompareCurrentFoodState(FoodState.Boiled) || CompareCurrentFoodState(FoodState.Alert))
                return false;
        }

        if (FoodName == "Duck Meat")
        {
            if (CompareCurrentFoodState(FoodState.Chop))
                return false;
        }

        return true;
    }
}
