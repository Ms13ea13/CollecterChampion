using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] private bool grilledFood;

    [SerializeField] private bool cooked;

    [SerializeField] private bool boiled;

    [SerializeField] private Slider timerSlider;
    [SerializeField] private int min;
    [SerializeField] private int max;

    [SerializeField] private float tempSliderValue;

    void Start()
    {
        timerSlider.value = 0;
        grilledFood = false;
        cooked = false;
        boiled = false;
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

    public bool GetCookState()
    {
        return cooked;
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

    public bool GetFoodIsGrilled()
    {
        return grilledFood;
    }

    public void PrepareFood(GameObject target)
    {
        if (!target)
            return;

        if (timerSlider.value > 0)
            SetShowTimerSlider(true);

        if (timerSlider.value < max && !grilledFood && !cooked)
        {
            timerSlider.value += Time.deltaTime;

            if (timerSlider.value >= max)
            {
                grilledFood = true;
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

        if (timerSlider.value < max && grilledFood && !cooked)
        {
            timerSlider.value += Time.deltaTime;

            if (timerSlider.value >= max)
            {
                cooked = true;
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

        if (timerSlider.value < max && !boiled)
        {
            timerSlider.value += Time.deltaTime;

            if (timerSlider.value >= max)
            {
                boiled = true;
                SetShowTimerSlider(false);
                ChangeFoodVisualAccordingToStates(target);
                timerSlider.value = 0;
            }

            tempSliderValue = timerSlider.value;
        }
    }

    private void ChangeFoodVisualAccordingToStates(GameObject food)
    {
        if (grilledFood && !cooked)
            food.GetComponent<Renderer>().material.color = Color.green;
        if (cooked)
            food.GetComponent<Renderer>().material.color = Color.blue;
        if (boiled)
            food.GetComponent<Renderer>().material.color = Color.red;
    }
}