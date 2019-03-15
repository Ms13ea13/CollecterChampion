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
    private GameObject GetDumplingPrefabByIndex(int index)
    {
        return foodInDumplingSteamedPrefab[index];
    }

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

    private float cookTimer = 2f;
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

    void Update()
    {
//        if (ingredientsContainer.Count == 0)
//        {
//            dumplingSteamedPanel.SetActive(false);
//        }
//        else
//        {
//            dumplingSteamedPanel.SetActive(true);
//        }
    }

    private void CreatingPairIngredients() // Create dumpling set from flour type and meat type
    {
        for (int i = 0; i < flourPairID.Length; i++)
        {
            dumplingSets.Add(flourPairID[i], ingredientsPairID[i]);
        }
    }

    public override void Interact(GameObject target, ref bool holding)
    {
        if (!wrongPair)
        {
            if (target == null)
            {
                Debug.LogError("Pick up thing from dumpling");
                RemovePair();
                return;
            }
            FoodItem food = target.gameObject.GetComponent<FoodItem>();
            if (food == null) return;
        
            int foodID = food.GetFoodItemId();

            if (dumplingSets.ContainsKey(foodID) || dumplingSets.ContainsValue(foodID)) //If foodItem is one of the ingredient needed for dumpling pair
            {
//                if (dumplingSets.ContainsKey(foodID)) //if foodItem IS one of the flour type
//                {
//                    Debug.LogError("yr " + food.GetFoodItemName() + " is a fucking flour id :" + food.GetFoodItemId());
//                }
//
//
//                if (dumplingSets.ContainsValue(foodID)) //if foodItem IS one of the ingredient pair type
//                {
//                    Debug.LogError("yr " + food.GetFoodItemName() + " is a fucking ingredient id :" + food.GetFoodItemId());
//                }
               
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
    }

    private void RemovePair()
    {
        Debug.LogError("RemovePair");
        ingredientsContainer.Clear();
        wrongPair = false;
        for (int i= 0; i < dumplingSteamedPanel.transform.childCount;i++)
        {
            Destroy(dumplingSteamedPanel.transform.GetChild(i).gameObject);
        }

        if (dumplingSteamedPanel.transform.childCount > 0)
        {
            dumplingSteamedPanel.SetActive(false);
            Debug.LogError("panel doesn't have child");
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
                    Debug.LogError("Try steaming yo shit");
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
        Debug.LogError("right pair");
        tempSliderValue = timerSlider.value;
        leantweenID = LeanTween.value(tempSliderValue, SetFoodOnFireValue + 100f, cookTimer).setOnUpdate((tempSliderValue) =>
        {
            if (!FoodItemAudioSource.isPlaying && steaming != null)
                FoodItemAudioSource.PlayOneShot(steaming);

            timerSlider.gameObject.SetActive(true);
            timerSlider.value = tempSliderValue;

        }).setOnComplete(() =>
        {
            

            Debug.LogError("done steaming");
            timerSlider.gameObject.SetActive(false);

            //--------------------------------------------------

            if (ingredientsContainer.Find(x => x.GetFoodItemId() == 6) && ingredientsContainer.Find(x => x.GetFoodItemId() == 8))
            {
                Debug.LogError("SpawnShrimpDumpling");
                spawnShrimpDumpling();
            }
            else if (ingredientsContainer.Find(x => x.GetFoodItemId() == 7) && ingredientsContainer.Find(x => x.GetFoodItemId() == 9))
            {
                Debug.LogError("SpawnPigDumpling");
                spawnPorkDumpling();
            }

            //---------------------------------------------------

            //GameObject steamedDumpling = Instantiate(foodInDumplingSteamedPrefab[]);

            //steamedDumpling.transform.position = new Vector3(transform.position.x,0.245f,transform.position.z);
            //steamedDumplingFoodItem = steamedDumpling.GetComponent<FoodItem>();
            //            SetTargetPosition(steamedDumpling.transform,true);

            RemovePair();
            steamedDumplingFoodItem.CurrentFoodState = steamedDumplingFoodItem.GetDoneState();

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
