using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DumplingSteamedManager : InteractableManager
{
    [SerializeField] private int maxIngredientsAmount;
    
    [SerializeField] private List<FoodItem> ingredientsContainer = new List<FoodItem>();

    [SerializeField] private int[] flourPairID; 
    
    [SerializeField] private int[] ingredientsPairID;

    [SerializeField] private GameObject foodInDumplingSteamedImagePrefab;

    [SerializeField] private Slider timerSlider;

    [SerializeField] private GameObject dumplingSteamedPanel;

    private Dictionary<int,int> dumplingSets = new Dictionary<int, int>();

    void Start()
    {
        timerSlider.value = 0;
        timerSlider.gameObject.SetActive(false);

        CreatingPairIngredients();
    }

    void Update()
    {
        if (ingredientsContainer.Count == 0)
        {
            dumplingSteamedPanel.SetActive(false);
        }
        else
        {
            dumplingSteamedPanel.SetActive(true);
        }
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
        FoodItem food = target.gameObject.GetComponent<FoodItem>();
        if (food == null) return;
        
        int foodID = food.GetFoodItemId();

        if (dumplingSets.ContainsKey(foodID) || dumplingSets.ContainsValue(foodID)) //If foodItem is one of the ingredient needed for dumpling pair
        {
            if (dumplingSets.ContainsKey(foodID)) //if foodItem IS one of the flour type
            {
                Debug.LogError("yr " + food.GetFoodItemName() + " is a fucking flour");
            }


            if (dumplingSets.ContainsValue(foodID)) //if foodItem IS one of the ingredient pair type
            {
                Debug.LogError("yr " + food.GetFoodItemName() + " is a fucking ingredient");
            }
               
            if (!ingredientsContainer.Contains(food) && ingredientsContainer.Count < maxIngredientsAmount)  //Check that container amount and add item
            {
                Debug.LogError("adding " + food.GetFoodItemName() + " to container");
                ingredientsContainer.Add(food);
                SetTargetPosition(food.transform);
                food.SetFoodIntoDumplingSteamed(true);
                FoodInDumplingSteamedAmount(food.GetFoodItemId());
                Destroy(food.gameObject);
                holding = false;
            }
            
            if (ingredientsContainer.Count >= maxIngredientsAmount) //In case container if full , then take action
            {
                SteamTheFuckOutOfYourDumpling();
            }
        }

    }

    private void SteamTheFuckOutOfYourDumpling()
    {
        Debug.LogError("Try steaming yo shit");

        int currentIngredientPairID = 0;
        FoodItem tempFood = new FoodItem();

        foreach (var item in ingredientsContainer)
        {
            if (dumplingSets.TryGetValue(item.GetFoodItemId(),out currentIngredientPairID) ) // Check if the food in the container can fetch the pair within the container
            {
                tempFood = ingredientsContainer.Find(x => x.GetFoodItemId() == currentIngredientPairID);
                if (tempFood) // Check if the container really have the pair item added inside
                {
                    item.PutFoodInTheDumplingSteamed(); //TODO make this into one Object and use only one UI pop UP // also return this as one foodItem
                    tempFood.PutFoodInTheDumplingSteamed(); // If these all return true == all done and can be create as new FoodItem (according to pair)
                }
            }
        }
    }

    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    private void SetShowTimerSlider(bool show)
    {
        timerSlider.gameObject.SetActive(show);
    }

    
    public void FoodInDumplingSteamedAmount(int foodIndex)
    {
        GameObject spawnOrderPicture = Instantiate(foodInDumplingSteamedImagePrefab);
        spawnOrderPicture.GetComponent<Image>().sprite = GameSceneManager.GetInstance().GetFoodPictureById(foodIndex);
        spawnOrderPicture.transform.parent = dumplingSteamedPanel.transform;
        spawnOrderPicture.GetComponent<FoodInDumplingSteamed>().SetOrder(foodIndex);
    }
}
