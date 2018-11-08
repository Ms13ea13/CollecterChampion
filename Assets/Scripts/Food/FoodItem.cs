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

    public enum FoodState
    {
        Raw,
        Grilled,
        Cooked,
        Boiled,
        Burnt
    }

    private FoodState currentFoodState;
    
    [SerializeField] private Slider timerSlider;
    [SerializeField] private int min;
    [SerializeField] private int max;

    [SerializeField] private float tempSliderValue;

    void Start()
    {
        timerSlider.value = 0;
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

    public bool IsFoodCooked()
    {
        return currentFoodState == FoodState.Cooked;
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
        if (!target)
            return;

        if (timerSlider.value > 0)
            SetShowTimerSlider(true);

        if (timerSlider.value < max && !CompareCurrentFoodState(FoodState.Grilled) && !CompareCurrentFoodState(FoodState.Cooked))
        {
            timerSlider.value += Time.deltaTime;

            if (timerSlider.value >= max)
            {
                currentFoodState = FoodState.Grilled;
                SetShowTimerSlider(false);
                ChangeFoodVisualAccordingToStates(target);
                timerSlider.value = 0;
            }

            tempSliderValue = timerSlider.value;
        }
    }

    public void ChopFood(GameObject target)
    {
        if (!target)
            return;

        if (timerSlider.value > 0)
            SetShowTimerSlider(true);

        if (timerSlider.value < max && CompareCurrentFoodState(FoodState.Grilled) && !CompareCurrentFoodState(FoodState.Cooked))
        {
            timerSlider.value += Time.deltaTime;

            if (timerSlider.value >= max)
            {
                currentFoodState = FoodState.Cooked;
                SetShowTimerSlider(false);
                ChangeFoodVisualAccordingToStates(target);
                timerSlider.value = 0;
            }

            tempSliderValue = timerSlider.value;
        }
    }

    public void BoilFood(GameObject target)
    {
        if (!target)
            return;

        if (timerSlider.value > 0)
            SetShowTimerSlider(true);

        if (timerSlider.value < max && !CompareCurrentFoodState(FoodState.Boiled))
        {
            timerSlider.value += Time.deltaTime;

            if (timerSlider.value >= max)
            {
                currentFoodState = FoodState.Boiled;
                SetShowTimerSlider(false);
                ChangeFoodVisualAccordingToStates(target);
                timerSlider.value = 0;
            }

            tempSliderValue = timerSlider.value;
        }
    }

    private void ChangeFoodVisualAccordingToStates(GameObject food)
    {
        if (CompareCurrentFoodState(FoodState.Grilled) && !IsFoodCooked())
            food.GetComponent<Renderer>().material.color = Color.green;
        if (IsFoodCooked())
            food.GetComponent<Renderer>().material.color = Color.blue;
        if (CompareCurrentFoodState(FoodState.Boiled))
            food.GetComponent<Renderer>().material.color = Color.red;
    }
}