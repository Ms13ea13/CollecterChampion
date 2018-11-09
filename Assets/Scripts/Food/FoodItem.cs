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
    [SerializeField] private int min = 0;
   [FormerlySerializedAs("max")] [SerializeField] private int maxFoodCookLevel = 100;

    [SerializeField] private float percentage;
    [FormerlySerializedAs("foodValue")] [SerializeField] private float currentFoodCookLevel;
    [SerializeField] private float onFireValue;
    [SerializeField] private float tempSliderValue;

    void Start()
    {
        timerSlider.value = 0;
        onFireValue = 2;

        currentFoodState = FoodState.Raw;
        timerSlider.wholeNumbers = false;
        SetShowTimerSlider(false);
    }

    private void SetShowTimerSlider(bool show)
    {
        timerSlider.gameObject.SetActive(show);
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

    public void PrepareFood(GameObject target)
    {
        currentFoodCookLevel += Time.deltaTime;
        percentage = (currentFoodCookLevel / onFireValue) * 100;

        if (!target)
            return;

        if (timerSlider.value > 0)
            SetShowTimerSlider(true);

        if (timerSlider.value < maxFoodCookLevel && !CompareCurrentFoodState(FoodState.Grilled) && !CompareCurrentFoodState(FoodState.Chop) && !CompareCurrentFoodState(FoodState.OnFire))
        {
            timerSlider.value += Time.deltaTime;
            
            if (percentage >= 50)
            {
                currentFoodState = FoodState.Grilled;
                SetShowTimerSlider(false);
                ChangeFoodVisualAccordingToStates(target);
                timerSlider.value = 0;
            }

            tempSliderValue = percentage;
            Debug.Log(percentage + "Grilled");
        }

        else if (timerSlider.value < max && CompareCurrentFoodState(FoodState.Grilled) && !CompareCurrentFoodState(FoodState.Chop) && !CompareCurrentFoodState(FoodState.OnFire))
        {
            timerSlider.value += Time.deltaTime;

            if (percentage >= 100)
            {
                currentFoodState = FoodState.OnFire;
                SetShowTimerSlider(false);
                ChangeFoodVisualAccordingToStates(target);
                timerSlider.value = 0;
            }

            tempSliderValue = percentage;
            Debug.Log(percentage + "Onfire");
        }
    }

    public void ChopFood(GameObject target)
    {
        currentFoodCookLevel += Time.deltaTime;
        percentage = (currentFoodCookLevel / onFireValue) * 100;

        if (!target)
            return;

        if (timerSlider.value > 0)
            SetShowTimerSlider(true);

        if (timerSlider.value < maxFoodCookLevel && CompareCurrentFoodState(FoodState.Grilled) && !CompareCurrentFoodState(FoodState.Chop) && !CompareCurrentFoodState(FoodState.OnFire))
        {
            timerSlider.value += Time.deltaTime;

            if (percentage >= 100)
            {
                currentFoodState = FoodState.Chop;
                SetShowTimerSlider(false);
                ChangeFoodVisualAccordingToStates(target);
                timerSlider.value = 0;
            }

            tempSliderValue = percentage;
            Debug.Log(percentage + "Chopped");
        }
    }

    public void BoilFood(GameObject target)
    {
        if (!target)
            return;

        if (timerSlider.value > 0)
            SetShowTimerSlider(true);

        if (timerSlider.value < maxFoodCookLevel && !CompareCurrentFoodState(FoodState.Boiled))
        {
            timerSlider.value += Time.deltaTime;

            if (timerSlider.value >= 1)
            {
                currentFoodState = FoodState.Boiled;
                SetShowTimerSlider(false);
                ChangeFoodVisualAccordingToStates(target);
                timerSlider.value = 0;
            }

            tempSliderValue = timerSlider.value;
            Debug.Log("Boiled");
        }
    }

    private void ChangeFoodVisualAccordingToStates(GameObject food)
    {
        if (CompareCurrentFoodState(FoodState.Grilled) && !IsFoodChopped() && !IsFoodOnFire())
            food.GetComponent<Renderer>().material.color = Color.green;

        if (CompareCurrentFoodState(FoodState.OnFire) && !IsFoodChopped() && IsFoodOnFire())
            food.GetComponent<Renderer>().material.color = Color.red;

        if (IsFoodChopped())
            food.GetComponent<Renderer>().material.color = Color.blue;

        if (CompareCurrentFoodState(FoodState.Boiled))
            food.GetComponent<Renderer>().material.color = Color.blue;
    }
}