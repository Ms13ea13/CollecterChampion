using SpawnItem;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class PlayerRayCast : MonoBehaviour
{
    [SerializeField] private GameObject itemInHold;

    [SerializeField] private FoodItem currentFoodInFront;

    [SerializeField] private CustomerManager currentCustomerInFront;
    
    [SerializeField] private InteractableManager interactableManagerInFront;

    [SerializeField] private PlateItem currentPlateInFront;

    [SerializeField] private CharacterController charContr;
    
    [SerializeField] private SpawnCleanDish spawnCleanDish;

    [SerializeField] private float playerSightLength;

    [SerializeField] private float distanceToObstacle;

    [SerializeField] private bool holding;

    [SerializeField] private string PickUpKey;
    [SerializeField] private string DropKey;
    [SerializeField] private string InteractKey;

    private int foodInHoldId;
    private RaycastHit hit;

    private Vector3 p1;
    private Vector3 p2;

    public AudioClip  pick_up,sent_food;
    private AudioSource playerAudioSource;

    public Animator animPlayer;
    private PlayerController player;

    void Start()
    {
        holding = false;
        distanceToObstacle = 0;
        playerAudioSource = GetComponent<AudioSource>();
        player = GetComponent<PlayerController>();

        animPlayer = GetComponent<Animator>();
    }

    public void ShootRayCast()
    {
        p1 = transform.position + charContr.center + Vector3.up * -charContr.height * 0.5F;
        p2 = p1 + Vector3.up * charContr.height;

        GetInteractableInFront();
        
        if (holding)
        {
            DropObj(ref itemInHold);
            GetCustomerInFront();
        }
        else
            GetPlateHolderInFront();

        GetFoodInFront();

        if (Input.GetButtonUp(PickUpKey))
            PickUpObj();
            
        if (Input.GetButton(InteractKey))
        { 
            FoodActions();
            PlateActions();
        }
        else
        {
            animPlayer.SetBool("isChop", false);
            animPlayer.SetBool("isWash", false);
        }
    }

    private void GetFoodInFront()
    {
        // Cast character controller shape 10 meters forward to see if it is about to hit anything.
        if ((Physics.CapsuleCast(p1, p2, charContr.radius, transform.forward, out hit, playerSightLength) &&
             hit.transform.tag == "Food"))
        {
            distanceToObstacle = hit.distance;
            currentFoodInFront = hit.transform.gameObject.GetComponent<FoodItem>();
        }
        else
        {
            currentFoodInFront = null;
        }
    }

    private void GetCustomerInFront()
    {
        // Cast character controller shape 10 meters forward to see if it is about to hit anything.
        if ((Physics.CapsuleCast(p1, p2, charContr.radius, transform.forward, out hit, playerSightLength) &&
             hit.transform.tag == "Customer"))
        {
            distanceToObstacle = hit.distance;
            currentCustomerInFront = hit.transform.gameObject.GetComponent<CustomerManager>();
        }
        else
            currentCustomerInFront = null;
    }
    
    private void GetPlateHolderInFront()
    {
        // Cast character controller shape 10 meters forward to see if it is about to hit anything.
        if ((Physics.CapsuleCast(p1, p2, charContr.radius, transform.forward, out hit, playerSightLength) &&
             hit.transform.tag == "Tray"))
        {
            distanceToObstacle = hit.distance;
            currentPlateInFront = hit.transform.gameObject.GetComponent<PlateItem>();
        }
        else
            currentPlateInFront = null;
    }

    private void GetInteractableInFront()
    {
        if ((Physics.CapsuleCast(p1, p2, charContr.radius, transform.forward, out hit, playerSightLength) &&
             hit.transform.gameObject.GetComponent<InteractableManager>()))
        {
            distanceToObstacle = hit.distance;
            interactableManagerInFront = hit.transform.gameObject.GetComponent<InteractableManager>();
        }
        else
            interactableManagerInFront = null;
    }

    private void PickUpObj()
    {
        if (holding)
        {
            if (currentPlateInFront) //Holding a plate
            {
                if (currentFoodInFront)
                {
                    bool added = currentPlateInFront.AddFoodToPlate(currentFoodInFront);
                    
                    if (added)
                    {
                        currentFoodInFront.StopFoodItemSoundEffect();
                        currentFoodInFront.SetDefaultFoodUI();
                    }
                }
                
                if (interactableManagerInFront)
                {
                    Debug.LogError("holding plate and interact");
                  GameObject foodItemGameObj =  interactableManagerInFront.InteractWithPlate(currentPlateInFront,ref  holding ,player);
                    if (foodItemGameObj == null)
                    {
                        Debug.LogError("gameobj from interactWithPlate is null");
                        if (holding == false)
                        {
                            ResetHolding();
                        }
                        return;
                    }
                    FoodItem foodItem = foodItemGameObj.GetComponent<FoodItem>();
                    if (foodItem)
                    {
                        bool added = currentPlateInFront.AddFoodToPlate(foodItem);
                    
                        if (added)
                        {
                            Debug.Log("add item from interact to plate");
                            foodItem.StopFoodItemSoundEffect();
                            foodItem.SetDefaultFoodUI();
                        }
                        else
                        {
                            Debug.Log("didnt add shit");
                        }
                    }
                    else
                    {
                        Debug.LogError("not food item yo");
                    }
                }
            }
               
        }
        else
        {
            
            if (interactableManagerInFront)
            {
                interactableManagerInFront.Interact(null,ref holding,player);
            }
            
           else if (currentPlateInFront)
                TakeObjIntoHold(currentPlateInFront.gameObject);

           else if (currentFoodInFront && currentFoodInFront.enabled)
            {
                if (!currentFoodInFront.IsFoodDoneCooking())
                {
                    playerAudioSource.PlayOneShot(pick_up);

                    currentFoodInFront.StopFoodItemSoundEffect();
                    currentFoodInFront.SetDefaultFoodUI();
                    TakeObjIntoHold(currentFoodInFront.gameObject);
                }
            }
        }
    }

    private void DropObj(ref GameObject holdingItem)
    {
        if (holdingItem && holding)
        {
            if (Input.GetButtonUp(DropKey))
            {
                playerAudioSource.PlayOneShot(pick_up);
                if (currentCustomerInFront)
                {
                    if (holdingItem.GetComponent<FoodItem>())
                    { 
                        FoodItem foodToServe = holdingItem.GetComponent<FoodItem>();
                        if (currentCustomerInFront.ReceiveOrder(foodToServe))
                        {
                            Destroy(holdingItem);
                            ResetHolding();
                        }
                        
                    }
                    else if (holdingItem.GetComponent<PlateItem>())
                    {
                       
                        PlateItem plateToServe = holdingItem.GetComponent<PlateItem>();
                        Debug.Log("server food via plate");
                        if (plateToServe.DeliverFoodViaPlate(currentCustomerInFront))
                        {
                            ResetHolding();
                        }
                        playerAudioSource.PlayOneShot(sent_food);
                    }
                    else
                        UnHoldItem(holdingItem);
                }
                else if (interactableManagerInFront)
                {
                    bool interacted = false;
                    var plate = holdingItem.GetComponent<PlateItem>();
                    if (plate == null)
                    {
                        interacted = interactableManagerInFront.Interact(holdingItem,ref holding,player);
                        if (interacted)
                            UnHoldItem(holdingItem);
                    }
                    else
                    {
                        var emptyPlate =   interactableManagerInFront.InteractWithPlate(plate,ref  holding ,player);
                        if (emptyPlate == null)
                        UnHoldItem(holdingItem);
                        else 
                            TakeObjIntoHold(emptyPlate.gameObject);
                    }
                }
//                else
//                    UnHoldItem(holdingItem);
            }
        }
    }

    private void FoodActions()
    {
        if (!holding)
            if (currentFoodInFront)
                if (currentFoodInFront.GetFoodOnChoppingBoard())
                {
                    animPlayer.SetBool("isChop", true);
                    currentFoodInFront.ChopFood();
                }
    }

    private void PlateActions()
    {
        if (!holding)
            if (currentPlateInFront)
                if (currentPlateInFront.GetPlateIntoSink())
                {
                    animPlayer.SetBool("isWash", true);
                    currentPlateInFront.WashPlate();

                    if (currentPlateInFront.washDone)
                    {
                        spawnCleanDish.SpawnDish();
                        currentPlateInFront.washDone = false;
                    }
                }
    }

    private void UnHoldItem(GameObject target)
    {
        target.transform.parent = null;
        target.GetComponent<Collider>().enabled = true;
        ResetHolding();
    }

    public GameObject GetItemInHold()
    {
        return itemInHold;
    }

    public void TakeObjIntoHold(GameObject target)
    {
       
        
        
        Debug.LogError("takeOBJ into hold ");
        playerAudioSource.PlayOneShot(pick_up);
        target.transform.parent = transform;
        Vector3 temp = target.transform.localPosition;
        temp.x = 0;
        temp.y = 0.175f;
        temp.z = 0.185f;
        target.transform.localPosition = temp;
        itemInHold = target;
        itemInHold.GetComponent<Collider>().enabled = false;

        holding = true;
        animPlayer.SetBool("isPickup", true);
    }

    private void ResetHolding()
    {
        animPlayer.SetBool("isPickup", false);
        holding = false;

        itemInHold = null;
        currentFoodInFront = null;
        currentCustomerInFront = null;
        currentPlateInFront = null;
        interactableManagerInFront = null;
    }
}
