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

    void Start()
    {
        holding = false;
        distanceToObstacle = 0;
        playerAudioSource = GetComponent<AudioSource>();//

        animPlayer = GetComponent<Animator>();
    }

    public void ShootRayCast()
    {
        p1 = transform.position + charContr.center + Vector3.up * -charContr.height * 0.5F;
        p2 = p1 + Vector3.up * charContr.height;

        if (holding)
        {
            DropObj(ref itemInHold);
            GetCustomerInFront();
            GetInteractableInFront();
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
        if ((Physics.CapsuleCast(p1, p2, charContr.radius, transform.forward, out hit, 0.001f) &&
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
        //animPlayer.Play("PickupItem");
        if (holding)
        {
            if (currentPlateInFront) //Holding a plate
                if (currentFoodInFront)
                {
                    bool added = currentPlateInFront.AddFoodToPlate(currentFoodInFront.gameObject);
                    
                    if (added)
                    {
                        currentFoodInFront.StopFoodItemSoundEffect();
                        currentFoodInFront.SetDefaultFoodUI();
                    }
                }
        }
        else
        {
            if (currentPlateInFront)
                TakeObjIntoHold(currentPlateInFront.gameObject);

            if (currentFoodInFront)
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
                    interactableManagerInFront.Interact(holdingItem,ref holding);
                    UnHoldItem(holdingItem);
                }
                else
                    UnHoldItem(holdingItem);
            }
        }
    }

    private void FoodActions()
    {
        if (!holding)
            if (currentFoodInFront)
                if (currentFoodInFront.GetFoodOnChoppingBoard())
                {
                    //animPlayer.SetBool("isChop", true);
                    currentFoodInFront.ChopFood();
                    //animPlayer.SetBool("isChop", false);
                }
    }

    private void PlateActions()
    {
        if (!holding)
        {
            if (currentPlateInFront)
            {
                if (currentPlateInFront.GetPlateIntoSink())
                {
                    currentPlateInFront.WashPlate();

                    if (currentPlateInFront.washDone)
                    {
                        spawnCleanDish.SpawnDish();
                        currentPlateInFront.washDone = false;
                    }
                }
            }
        }
    }

    private void UnHoldItem(GameObject target)
    {
        target.transform.parent = null;
        target.GetComponent<Collider>().enabled = true;
        ResetHolding();
    }

    private void TakeObjIntoHold(GameObject target)
    {
        playerAudioSource.PlayOneShot(pick_up);//
        target.transform.parent = transform;
        Vector3 temp = target.transform.localPosition;
        temp.y = 15.9f;
        temp.x = 0;
        temp.z = 15.4f;
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
