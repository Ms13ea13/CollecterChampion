using System.Collections.Generic;
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

    [SerializeField] private GameObject dumplingSteamedPanel;
    private GameObject steamedDumpling;
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
    
    public AudioClip steaming,complete;

    void Start()
    {
        timerSlider.value = 0;
        timerSlider.gameObject.SetActive(false);
        dumplingSteamedPanel.SetActive(false);
        FoodItemAudioSource = GetComponent<AudioSource>();
        CreatingPairIngredients();
    }

    private GameObject GetDumplingPrefabByIndex(int index)
    {
        return foodInDumplingSteamedPrefab[index];
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
            Debug.LogError("Pick up thing from dumpling");
            RemovePair();
            return steamedDumplingFoodItem.gameObject;
           
        }
        else

        {
//            Debug.LogError("plate is null");
            return null;
        }
         
           

    }

    public override bool Interact (GameObject target,ref bool holding, PlayerController player)
    {
        if (!wrongPair)
        {
            if (target == null)
            {
                return false;
            }
            
            Debug.LogError("try put food in steamer");
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
                        Debug.LogError("adding " + food.GetFoodItemName() + " to container");

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
//        Debug.LogError("RemovePair");
        ingredientsContainer.Clear();
        wrongPair = false;
        for (int i= 0; i < dumplingSteamedPanel.transform.childCount;i++)
        {
            Destroy(dumplingSteamedPanel.transform.GetChild(i).gameObject);
        }

        if (dumplingSteamedPanel.transform.childCount > 0)
        {
            dumplingSteamedPanel.SetActive(false);
//            Debug.LogError("panel doesn't have child");
        }
        
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
//                    Debug.LogError("Try steaming yo shit");
                    SteamingDumplingPair();
                    return false;
                }
            }
        }

        ShowWrongPairUI();
        return true;
    }

    private void SteamingDumplingPair()
    {
//        Debug.LogError("right pair");
        tempSliderValue = timerSlider.value;
        leantweenID = LeanTween.value(tempSliderValue, SetFoodOnFireValue + 100f, cookTimer).setOnUpdate((tempSliderValue) =>
        {
            if (!FoodItemAudioSource.isPlaying && steaming != null)
                FoodItemAudioSource.PlayOneShot(steaming);

            timerSlider.gameObject.SetActive(true);
            timerSlider.value = tempSliderValue;

        }).setOnComplete(() =>
        {
            

//            Debug.LogError("done steaming");
            timerSlider.value = 0;
            tempSliderValue = 0;
            timerSlider.gameObject.SetActive(false);

            //--------------------------------------------------

            if (ingredientsContainer.Find(x => x.GetFoodItemId() == 6) && ingredientsContainer.Find(x => x.GetFoodItemId() == 8))
            {
//                Debug.LogError("SpawnShrimpDumpling");
                spawnShrimpDumpling();
            }
            else if (ingredientsContainer.Find(x => x.GetFoodItemId() == 7) && ingredientsContainer.Find(x => x.GetFoodItemId() == 9))
            {
//                Debug.LogError("SpawnPigDumpling");
                spawnPorkDumpling();
            }

            //---------------------------------------------------

            //GameObject steamedDumpling = Instantiate(foodInDumplingSteamedPrefab[]);

            //steamedDumpling.transform.position = new Vector3(transform.position.x,0.245f,transform.position.z);
            //steamedDumplingFoodItem = steamedDumpling.GetComponent<FoodItem>();
            //            SetTargetPosition(steamedDumpling.transform,true);

            RemovePair();
            steamedDumplingFoodItem.CurrentFoodState = steamedDumplingFoodItem.GetDoneState();
            steamedDumplingFoodItem.EnableFoodItemCollider(false);
            FoodInDumplingSteamedAmount(steamedDumplingFoodItem.GetFoodItemId());
            Debug.Log(steamedDumplingFoodItem.GetFoodItemId());

            dumplingSteamedPanel.SetActive(true);

        }).id;
    }

    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    private void SetShowTimerSlider(bool show)
    {
        timerSlider.gameObject.SetActive(show);
    }

    private void spawnShrimpDumpling()
    {
        GameObject a = Instantiate(foodInDumplingSteamedPrefab[0]);
        a.transform.position = new Vector3(transform.position.x, 0.245f, transform.position.z);
        steamedDumplingFoodItem = a.GetComponent<FoodItem>();
    }

    private void spawnPorkDumpling()
    {
        GameObject b = Instantiate(foodInDumplingSteamedPrefab[1]);
        b.transform.position = new Vector3(transform.position.x, 0.245f, transform.position.z);
        steamedDumplingFoodItem = b.GetComponent<FoodItem>();
    }

    private void ShowWrongPairUI()
    {
        //show wrong pair UI here
    }

    public void FoodInDumplingSteamedAmount(int foodIndex)
    {
        GameObject spawnOrderPicture = Instantiate(foodInDumplingSteamedImagePrefab);
        spawnOrderPicture.GetComponent<Image>().sprite = GameSceneManager.GetInstance().GetFoodPictureById(foodIndex);
        spawnOrderPicture.transform.parent = dumplingSteamedPanel.transform;
        spawnOrderPicture.GetComponent<FoodInDumplingSteamed>().SetOrder(foodIndex);
    }
}
