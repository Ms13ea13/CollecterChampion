using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DumplingSteamedManager : InteractableManager
{
    [SerializeField]
    private int maxIngredientsAmount;
    
    [SerializeField]
    private List<FoodItem> ingredientsContainer = new List<FoodItem>();

    [SerializeField]
    private int[] flourPairID; 
    
    [SerializeField]
    private int[] ingredientsPairID;
    
    private Dictionary<int,int> dumplingSets = new Dictionary<int, int>();

    void Start()
    {
        currentIndex = 0;
        itemsInDumplingSteamed = new List<GameObject>();
        timerSlider.value = 0;
        SetDefaultDumplingSteamedUI();

        CreatingPairIngredients();
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

    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    [SerializeField] private GameObject foodInDumplingSteamedImagePrefab;

    [SerializeField] private GameObject dumplingSteamedPanel;

    [FormerlySerializedAs("itemInDumplingSteamed")]
    [SerializeField]
    private List<GameObject> itemsInDumplingSteamed;

    public List<GameObject> ItemInDumplingSteamed()
    {
        return itemsInDumplingSteamed;
    }

    [SerializeField] private int currentIndex;

    public enum FoodState
    {
        Chopped,
        Steamed
    }

    [SerializeField] private FoodState _currentFoodState;

    [SerializeField] private Slider timerSlider;
    private int leantweenID;
    private const float dumplingSteamedTimer = 20f;

    [SerializeField] private int min = 0;

    [SerializeField]
    private int maxFoodLevel = 100;

    [SerializeField] private float percentage;

    [SerializeField]
    private float currentFoodLevel;

    [SerializeField] private bool plateIntoSink;
    [SerializeField] private float tempSliderValue;

    private bool onHold;
    private Vector3 temp;

    void Update()
    {
        if (itemsInDumplingSteamed.Count == 0)
        {
            dumplingSteamedPanel.SetActive(false);
        }
        else
        {
            dumplingSteamedPanel.SetActive(true);
        }
    }

    private void SetShowTimerSlider(bool show)
    {
        timerSlider.gameObject.SetActive(show);
    }

    public void SetDefaultDumplingSteamedUI()
    {
        if (_currentFoodState == FoodState.Chopped && timerSlider.value > 0)
            SetShowTimerSlider(true);
        else
            SetShowTimerSlider(false);

        LeanTween.cancel(leantweenID);
    }

    public bool CompareCurrentFoodState(FoodState foodState)
    {
        return _currentFoodState == foodState;
    }

    public void SteamedFood()
    {
        if (timerSlider.value >= maxFoodLevel && CompareCurrentFoodState(FoodState.Chopped))
        {
            if (!timerSlider.gameObject.activeInHierarchy)
                SetShowTimerSlider(true);

            currentFoodLevel += Time.deltaTime * 40f;
            percentage = (currentFoodLevel / maxFoodLevel) * 100;
            timerSlider.value = percentage;
            tempSliderValue = percentage;

            if (percentage >= 100)
            {
                _currentFoodState = FoodState.Steamed;
                timerSlider.value = 0;
                SetShowTimerSlider(false);
                Destroy(gameObject);
                Debug.Log(percentage + "Food is steamed");
            }
        }
    }

    public bool AddFoodToDumplingSteamed(GameObject food)
    {
        var foodItem = food.GetComponent<FoodItem>();

        if (!foodItem.CompareCurrentFoodState(FoodItem.FoodState.Done))
        {
            Debug.LogError("Nope food is : " + foodItem.GetFoodItemState().ToString());
            return false;
        }

        if (itemsInDumplingSteamed.Count < 2)
        {
            itemsInDumplingSteamed.Add(food);
            food.transform.parent = transform;
            food.GetComponent<Collider>().enabled = false;
            food.transform.localPosition = StackFoodVisually(currentIndex, food.transform);
            foodItem.SetBannedId(currentIndex);
            FoodInDumplingSteamedAmount(foodItem.GetFoodItemId());
        }

        return true;
    }

    private Vector3 StackFoodVisually(int index, Transform targetTransform)
    {
        temp = targetTransform.localPosition;
        switch (index)
        {
            case 0:
                {
                    temp.y = 0.013f;
                    break;
                }
            case 1:
                {
                    temp.y = 0.045f;
                    break;
                }
            default:
                break;
        }

        temp.x = 0;
        temp.z = 0;

        currentIndex += 1;
        return temp;
    }

    public void SetOnHold(bool hold)
    {
        onHold = hold;
    }

    public bool GetOnHold()
    {
        return onHold;
    }

    public void FoodInDumplingSteamedAmount(int foodIndex)
    {
        GameObject spawnOrderPicture = Instantiate(foodInDumplingSteamedImagePrefab);
        spawnOrderPicture.GetComponent<Image>().sprite = GameSceneManager.GetInstance().GetFoodPictureById(foodIndex);
        spawnOrderPicture.transform.parent = dumplingSteamedPanel.transform;
        spawnOrderPicture.GetComponent<FoodInPlate>().SetOrder(foodIndex);
    }

    public void ClearTargetOrderPanel(int id)
    {
        if (dumplingSteamedPanel.transform.childCount <= 0)
            return;

        for (int i = 0; i < dumplingSteamedPanel.transform.childCount; i++)
        {
            if (id == dumplingSteamedPanel.transform.GetChild(i).gameObject.GetComponent<FoodInPlate>().GetOrderId())
            {
                Destroy(dumplingSteamedPanel.transform.GetChild(i).gameObject);
                return;
            }
        }
    }

    private void ClearItemsInDumplingSteamed()
    {
        if (itemsInDumplingSteamed.Count <= 0)
            return;

        foreach (var foodObj in itemsInDumplingSteamed)
        {
            Destroy(foodObj);
        }
    }

    public void ClearAllItemInDumplingSteamed()
    {
        ClearItemsInDumplingSteamed();
        currentIndex = 0;
    }

    public void SetFoodOnCounter(bool b)
    {
        throw new System.NotImplementedException();
    }
}
