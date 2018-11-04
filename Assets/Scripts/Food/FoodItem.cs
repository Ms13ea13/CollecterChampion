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

    [SerializeField] private bool grilledFood;

    [SerializeField] private bool cooked;

    [SerializeField] private Slider timerSlider;
    [SerializeField] private int min;
    [SerializeField] private int max;

    [SerializeField] private float tempSliderValue;

    void Start()
    {
        timerSlider.value = 0;
        grilledFood = false;
        cooked = false;
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
            Debug.LogError("Hold H");
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
            Debug.LogError("Hold H");
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


    private void ChangeFoodVisualAccordingToStates(GameObject food)
    {
        if (grilledFood && !cooked)
            food.GetComponent<Renderer>().material.color = Color.green;
        if (cooked)
            food.GetComponent<Renderer>().material.color = Color.blue;
    }
}