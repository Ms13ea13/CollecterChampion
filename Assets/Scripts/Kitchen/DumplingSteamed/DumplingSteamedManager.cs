﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using System;
using System.Linq;

public class DumplingSteamedManager : InteractableManager
{
    [SerializeField] private int maxIngredientsAmount;
    
    [SerializeField] private List<FoodItem> ingredientsContainer = new List<FoodItem>();

    [SerializeField] private int[] flourPairID; 
    
    [SerializeField] private int[] ingredientsPairID;

    [SerializeField] private GameObject foodInDumplingSteamedImagePrefab;

    [SerializeField] private GameObject[] foodInDumplingSteamedPrefab;
  
    [SerializeField] private Slider timerSlider;
    [SerializeField] private Image foodStateUI;
    [SerializeField] private Sprite onFirePicture;
    [SerializeField] private Sprite alertPicture;
    [SerializeField] private Sprite steamedPicture;

    [SerializeField] private GameObject dumplingSteamedPanel;
    private FoodItem steamedDumplingFoodItem;

    private Dictionary<int,int> dumplingSets = new Dictionary<int, int>();

    [SerializeField]
    private bool wrongPair = false;
    
    [SerializeField]
    private bool doneCooking = false;
    
    private int leantweenID;

    private float cookTimer = 15f;
    [SerializeField] private float tempSliderValue ;
    private float SetFoodOnFireValue = 150;
    
    private AudioSource FoodItemAudioSource;
    
    public AudioClip steaming,complete, Alert_fire;

    void Start()
    {
        timerSlider.value = 0;
        timerSlider.maxValue = SetFoodOnFireValue;
        timerSlider.gameObject.SetActive(false);
        foodStateUI.gameObject.SetActive(false);
        dumplingSteamedPanel.SetActive(false);
        FoodItemAudioSource = GetComponent<AudioSource>();
        CreatingPairIngredients();
    }

    private void CreatingPairIngredients() // Create dumpling set from flour type and meat type
    {
        for (int i = 0; i < flourPairID.Length; i++)
        {
            dumplingSets.Add(flourPairID[i], ingredientsPairID[i]);
        }
    }

    public override GameObject InteractWithPlate(PlateItem plateItem,ref bool holding , PlayerController player = null)
    {
        if (plateItem != null)
        {
           
            if (steamedDumplingFoodItem && steamedDumplingFoodItem.IsFoodDoneCooking())
            {
                Debug.Log("Pick up thing from dumpling");
                LeanTween.cancel(leantweenID);
                RemovePair();
                var tempFood = steamedDumplingFoodItem;
                steamedDumplingFoodItem = null;
                return tempFood.gameObject;
            }
            else
                return null;
           
        }
        else

        {
//            Debug.Log("plate is null");
            return null;
        }
    }

    public override bool Interact (GameObject target,ref bool holding, PlayerController player)
    {
        if (!wrongPair)
        {
            if (target == null)
            {
                if (doneCooking && !steamedDumplingFoodItem.IsFoodDoneCooking())
                {
                    var tempFood = steamedDumplingFoodItem;
                    steamedDumplingFoodItem = null;
                    player.GetPlayerRayCast().TakeObjIntoHold(tempFood.gameObject);
                    RemovePair();
                    LeanTween.cancel(leantweenID);
                }
                return false;
            }
            
            Debug.Log("try put food in steamer");
            FoodItem food = target.gameObject.GetComponent<FoodItem>();
            if (food == null) return false;
        
            int foodID = food.GetFoodItemId();

            if (dumplingSets.ContainsKey(foodID) || dumplingSets.ContainsValue(foodID)) //If foodItem is one of the ingredient needed for dumpling pair
            {
                if (!ingredientsContainer.Contains(food) && ingredientsContainer.Count < maxIngredientsAmount)  //Check that container amount and add item
                {
                    if (food.IsFoodChopped())
                    {
                        dumplingSteamedPanel.SetActive(true);
                        Debug.Log("adding " + food.GetFoodItemName() + " to container");

                        ingredientsContainer.Add(food);

                        SetTargetPosition(food.transform);

                        food.SetFoodIntoDumplingSteamed(true);
                        FoodInDumplingSteamedAmount(food.GetFoodItemId());

                        food.gameObject.tag = "Untagged";
                        food.DestroyFoodItemCollider();
                        food.enabled = false;
                        //                    Destroy(food.gameObject);
                        holding = false;
                    }
                    else
                        return false;
                }
            
                if (ingredientsContainer.Count >= maxIngredientsAmount) //In case container if full , then take action
                {
                    if (food.IsFoodChopped())
                    {
                        wrongPair = SteamTheFuckOutOfYourDumpling();
                    }
                }
            }
        }
        else
        {
            RemovePair();
            
        }
        
        return true;
    }

    private void RemovePair()
    {
//        Debug.Log("RemovePair");
        ingredientsContainer.Clear();
        wrongPair = false;
      
        for (int i= 0; i < dumplingSteamedPanel.transform.childCount;i++)
        {
            Destroy(dumplingSteamedPanel.transform.GetChild(i).gameObject);
        }

        if (dumplingSteamedPanel.transform.childCount > 0)
        {
            dumplingSteamedPanel.SetActive(false);
        }

        doneCooking = false;
    
        timerSlider.gameObject.SetActive(false);
        foodStateUI.gameObject.SetActive(false);
        
    }

    private bool SteamTheFuckOutOfYourDumpling()
    {
        int currentIngredientPairID = 0;
        FoodItem tempFood = new FoodItem();

        foreach (var item in ingredientsContainer)
        {
            if (dumplingSets.TryGetValue(item.GetFoodItemId(), out currentIngredientPairID) ) // Check if the food in the container can fetch the pair within the container
            {
                tempFood = ingredientsContainer.Find(x => x.GetFoodItemId() == currentIngredientPairID);
                if (tempFood) // Check if the container really have the pair item added inside
                {
                    SteamingDumplingPair();
                    return false;
                }
            }
        }
        return true;
    }

    private void SteamingDumplingPair()
    {
        if (timerSlider.value != 0)
            timerSlider.value = tempSliderValue;
        
        leantweenID = LeanTween.value(tempSliderValue, SetFoodOnFireValue + 100f, cookTimer).setOnUpdate((tempSliderValue) =>
        {
            if (!FoodItemAudioSource.isPlaying && steaming != null)
                FoodItemAudioSource.PlayOneShot(steaming);



            if (tempSliderValue <= timerSlider.maxValue)
            {
                timerSlider.value = tempSliderValue;
                timerSlider.gameObject.SetActive(true);
            }
           
            
            if (tempSliderValue> SetFoodOnFireValue && tempSliderValue < SetFoodOnFireValue + 70)
            {
                if (!doneCooking)
                {
                    Debug.Log("done steaming");
                    DoneSteamingDumpling();
                    SetFoodUIState(FoodStateGlobal.FoodState.Done);
                    steamedDumplingFoodItem.CurrentFoodState = FoodStateGlobal.FoodState.Done;
                    steamedDumplingFoodItem.ChangeFoodVisualAccordingToStates();
                    timerSlider.gameObject.SetActive(false);
                    foodStateUI.gameObject.SetActive(true);
                    timerSlider.value = 0;
                    doneCooking = true;
                }
               
               

            }
            else  if (tempSliderValue > SetFoodOnFireValue + 71 && tempSliderValue < SetFoodOnFireValue + 90)
            {
              
                if (steamedDumplingFoodItem.CurrentFoodState != FoodStateGlobal.FoodState.Alert)
                {
                    steamedDumplingFoodItem.CurrentFoodState = FoodStateGlobal.FoodState.Alert;
                    SetFoodUIState(FoodStateGlobal.FoodState.Alert);
                    Debug.Log("steaming alert");
                    steamedDumplingFoodItem.ChangeFoodVisualAccordingToStates();
                    tempSliderValue = 0;
                }
               
            }else if (tempSliderValue > SetFoodOnFireValue + 91)
            {
                if (steamedDumplingFoodItem.CurrentFoodState != FoodStateGlobal.FoodState.OnFire)
                {
                    Debug.Log("onfire steaming");
                    SetFoodUIState(FoodStateGlobal.FoodState.OnFire);
                    steamedDumplingFoodItem.CurrentFoodState = FoodStateGlobal.FoodState.OnFire;
                    steamedDumplingFoodItem.ChangeFoodVisualAccordingToStates();
                }
            }
            

        }).setOnComplete(() => { tempSliderValue = 0; }).id;
    }

    private void DoneSteamingDumpling()
    {
        if (steamedDumplingFoodItem != null) return;
        if (ingredientsContainer.Find(x => x.GetFoodItemId() == 6) && ingredientsContainer.Find(x => x.GetFoodItemId() == 8) ||
            ingredientsContainer.Find(x => x.GetFoodItemId() == 9) && ingredientsContainer.Find(x => x.GetFoodItemId() == 12))
        {
            spawnShrimpDumpling();
        }
        else if (ingredientsContainer.Find(x => x.GetFoodItemId() == 7) && ingredientsContainer.Find(x => x.GetFoodItemId() == 9) ||
                 ingredientsContainer.Find(x => x.GetFoodItemId() == 13) && ingredientsContainer.Find(x => x.GetFoodItemId() == 13))
        {
            spawnPorkDumpling();
        }

        RemovePair();
        steamedDumplingFoodItem.CurrentFoodState = steamedDumplingFoodItem.GetDoneState();
        steamedDumplingFoodItem.EnableFoodItemCollider(false);
        FoodInDumplingSteamedAmount(steamedDumplingFoodItem.GetFoodItemId());
        Debug.Log(steamedDumplingFoodItem.GetFoodItemId());

        dumplingSteamedPanel.SetActive(true);
    }

    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
   
    private void spawnShrimpDumpling()
    {
        GameObject a = Instantiate(foodInDumplingSteamedPrefab[0]);
        a.transform.position = new Vector3(transform.position.x, 0.245f, transform.position.z);
        steamedDumplingFoodItem = a.GetComponent<FoodItem>();
        steamedDumplingFoodItem.GetComponent<Renderer>().material.color = Color.white;
    }

    private void spawnPorkDumpling()
    {
        GameObject b = Instantiate(foodInDumplingSteamedPrefab[1]);
        b.transform.position = new Vector3(transform.position.x, 0.245f, transform.position.z);
        steamedDumplingFoodItem = b.GetComponent<FoodItem>();
        steamedDumplingFoodItem.GetComponent<Renderer>().material.color = Color.white;
    }

    public void FoodInDumplingSteamedAmount(int foodIndex)
    {
        GameObject spawnOrderPicture = Instantiate(foodInDumplingSteamedImagePrefab);
        spawnOrderPicture.GetComponent<Image>().sprite = GameSceneManager.GetInstance().GetFoodPictureById(foodIndex);
        spawnOrderPicture.transform.parent = dumplingSteamedPanel.transform;
        spawnOrderPicture.GetComponent<FoodInDumplingSteamed>().SetOrder(foodIndex);
    }

    private void SetFoodUIState(FoodStateGlobal.FoodState state)
    {
       
        switch (state)
        {
            case FoodStateGlobal.FoodState.Alert:
                foodStateUI.sprite = alertPicture;
                break;
            case FoodStateGlobal.FoodState.OnFire:
                foodStateUI.sprite = onFirePicture;
                break;
            case FoodStateGlobal.FoodState.Done:
                foodStateUI.sprite = steamedPicture;
                break;
            default:
                break;
        }

    }
}