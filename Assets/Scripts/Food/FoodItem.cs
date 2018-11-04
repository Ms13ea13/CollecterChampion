using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodItem : MonoBehaviour
{
	[SerializeField]
	private int foodID;
	
	[SerializeField]
	private string FoodName;

	[SerializeField]
	private int trayBandedId;

    [SerializeField]
    private bool foodOnStove;

    [SerializeField]
    private bool foodOnChoppingBoard;

    [SerializeField]
    private bool grilledFood;

    [SerializeField]
    private Slider targetSlider;
    [SerializeField]
    private int min;
    [SerializeField]
    private int max;

    void Start()
    {
        targetSlider.gameObject.SetActive(false);
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

    public void SetFoodIsGrilled(bool isGrilled)
    {
        if (!isGrilled)
        {
            grilledFood = isGrilled;
        }
    }

    public bool GetFoodIsGrilled()
    {
        return grilledFood;
    }

    public void PrepareFood(GameObject target)
    {
        if (targetSlider.value < max)
        {
            targetSlider.value += Time.deltaTime;
            targetSlider.gameObject.SetActive(true);
            //SetFoodIsGrilled(false);
        }

        if (targetSlider.value >= max)
        {
            //targetSlider.gameObject.SetActive(false);
            //SetFoodIsGrilled(true);

            if (!target)
                return;

            GrillFoodChangeMat(target);
            targetSlider.value = 0;
            return;
        }
    }

    private void GrillFoodChangeMat(GameObject food)
    {
        food.GetComponent<Renderer>().material.color =  Color.green;
    }

    public void ChopFood(GameObject target)
    {
        if (targetSlider.value < max)
        {
            targetSlider.value += Time.deltaTime;
            targetSlider.gameObject.SetActive(true);
            //SetFoodIsCooked(false);
        }
        else if (targetSlider.value >= max)
        {
            targetSlider.gameObject.SetActive(false);
            //SetFoodIsCooked(true);

            if (!target)
                return;

            ChopFoodChangeMat(target);
        }
    }

    private void ChopFoodChangeMat(GameObject food)
    {
        food.GetComponent<Renderer>().material.color = Color.blue;
    }
}
