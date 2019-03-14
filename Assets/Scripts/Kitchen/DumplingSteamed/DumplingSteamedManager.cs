using System;
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
        FoodItemAudioSource = GetComponent<AudioSource>();
        _currentFoodState = FoodState.Chopped;
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
        SteamedFood();

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

    //------------------------------------------------------------------------------

    [SerializeField] private Image foodStateUI;
    [SerializeField] private Sprite cookedPicture;
    [SerializeField] private Sprite onFirePicture;
    [SerializeField] private Sprite alertPicture;
    [SerializeField] private Sprite steamdPicture;

    private float SetFoodOnFireValue;

    [FormerlySerializedAs("max")]
    [SerializeField]
    private int maxFoodCookLevel = 150;

    private const float cookTimer = 20f;

    private AudioSource FoodItemAudioSource;
    public AudioClip steamed, Alert_fire, complete;

    //-------------------------------------------------------------------------------

    public enum FoodState
    {
        Chopped,
        Steamed,
        Alert,
        OnFire,
        Done
    }

    [SerializeField] private FoodState DoneState;
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
        if (ingredientsContainer.Count == 0)
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
        foodStateUI.gameObject.SetActive(false);
    }

    public bool CompareCurrentFoodState(FoodState foodState)
    {
        return _currentFoodState == foodState;
    }

    public void SteamedFood()
    {
        if (_currentFoodState != FoodState.Chopped)
            return;

        if (tempSliderValue != 0)
            timerSlider.value = tempSliderValue;

        SetFoodOnFireValue = maxFoodCookLevel;

        leantweenID = LeanTween.value(tempSliderValue, SetFoodOnFireValue + 100f, cookTimer).setOnUpdate(
            (float Value) =>
            {
                if (!FoodItemAudioSource.isPlaying && steamed != null)
                    FoodItemAudioSource.PlayOneShot(steamed);

                tempSliderValue = Value;
                if (timerSlider.value <= timerSlider.maxValue && _currentFoodState == FoodState.Chopped)
                {
                    if (!timerSlider.gameObject.activeInHierarchy)
                        SetShowTimerSlider(true);
                    timerSlider.value = Value;
                }
                else
                    SetShowTimerSlider(false);

                if (timerSlider.value >= timerSlider.maxValue && _currentFoodState == FoodState.Chopped && _currentFoodState != FoodState.Done)
                {
                    timerSlider.value = 0;

                    _currentFoodState = FoodState.Steamed;
                    FoodItemAudioSource.PlayOneShot(complete);
                }

                if (tempSliderValue <= SetFoodOnFireValue + 50 && _currentFoodState != FoodState.Chopped)
                {
                    if (CompareCurrentFoodState(DoneState))
                    {
                        _currentFoodState = FoodState.Done;
                        Debug.LogError("set done"); ;
                    }
                }

                if (tempSliderValue >= SetFoodOnFireValue + 50f)
                {
                    _currentFoodState = FoodState.Alert;
                    FoodItemAudioSource.PlayOneShot(Alert_fire);
                }

                //ChangeFoodVisualAccordingToStates();
                SetFoodUIState();
            }).setOnComplete(() =>
            {
                _currentFoodState = FoodState.OnFire;
                //ChangeFoodVisualAccordingToStates();
                SetFoodUIState();
            }).id;
    }

    private void SetFoodUIState()
    {
        if (foodStateUI == null)
            throw new Exception("Food state UI is null");

        foodStateUI.gameObject.SetActive(true);
        switch (_currentFoodState)
        {
            case FoodState.Alert:
                foodStateUI.sprite = alertPicture;
                break;
            case FoodState.OnFire:
                foodStateUI.sprite = onFirePicture;
                break;
            case FoodState.Steamed:
                foodStateUI.sprite = cookedPicture;
                break;
            case FoodState.Done:
                foodStateUI.sprite = cookedPicture;
                break;
            default:
                foodStateUI.gameObject.SetActive(false);
                break;
        }
    }

    public void FoodInDumplingSteamedAmount(int foodIndex)
    {
        GameObject spawnOrderPicture = Instantiate(foodInDumplingSteamedImagePrefab);
        spawnOrderPicture.GetComponent<Image>().sprite = GameSceneManager.GetInstance().GetFoodPictureById(foodIndex);
        spawnOrderPicture.transform.parent = dumplingSteamedPanel.transform;
        spawnOrderPicture.GetComponent<FoodInDumplingSteamed>().SetOrder(foodIndex);
    }
}
