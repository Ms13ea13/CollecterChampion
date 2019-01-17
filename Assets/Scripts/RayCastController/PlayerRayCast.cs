using SpawnItem;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerRayCast : MonoBehaviour
{
    [SerializeField] private GameObject itemInHold;

    [SerializeField] private FoodItem currentFoodInFront;

    [SerializeField] private CustomerManager currentCustomerInFront;

    [SerializeField] private CustomerManagerStage2 currentCustomerStage2InFront;

    [SerializeField] private BinManager currentBinInFront;

    [SerializeField] private PlateItem currentPlateInFront;

    [SerializeField] private StoveManager currentStoveInFront;

    [SerializeField] private ChoppingBoardManager currentChoppingBoardInFront;

    [SerializeField] private CounterManager currentCounterInFront;

    [SerializeField] private PanManager currentPanInFront;

    [SerializeField] private PotManager currentPotInFront;

    [SerializeField] private SinkManager currentSinkInFront;

    [SerializeField] private CharacterController charContr;
    
    [SerializeField] private SpawnCleanDish spawnCleanDish;

    [SerializeField] private float playerSightLength = 10f;

    [SerializeField] private float distanceToObstacle;

    [SerializeField] private bool holding;

    private int foodInHoldId;
    private RaycastHit hit;
    private Vector3 p1;
    private Vector3 p2;
    public AudioClip  pick_up,sent_food;
    private AudioSource playerAudioSource;

    void Start()
    {
        holding = false;
        distanceToObstacle = 0;
        playerAudioSource = GetComponent<AudioSource>();//
    }

    public void ShootRayCast()
    {
        p1 = transform.position + charContr.center + Vector3.up * -charContr.height * 0.5F;
        p2 = p1 + Vector3.up * charContr.height;

        if (holding)
        {
            DropObj(ref itemInHold);
            GetCustomerInFront();
            GetCustomerStage2InFront();
            GetBinInFront();
            GetStoveInFront();
            GetChoppingBoardInFront();
            GetCounterInFront();
            GetPanInFront();
            GetPotInFront();
            GetSinkInFront();
        }
        else
            GetPlateHolderInFront();

        GetFoodInFront();


        if (Input.GetButtonUp("PickUp"))
            PickUpObj();
            
        if (Input.GetButton("Interact"))
        { 
            FoodActions();
            PlateActions();
        }
    }

    private void GetFoodInFront()
    {
        // Cast character controller shape 10 meters forward to see if it is about to hit anything.
        if ((Physics.CapsuleCast(p1, p2, charContr.radius, transform.forward, out hit, 10) &&
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
        if ((Physics.CapsuleCast(p1, p2, charContr.radius, transform.forward, out hit, 10) &&
             hit.transform.tag == "Customer"))
        {
            distanceToObstacle = hit.distance;
            currentCustomerInFront = hit.transform.gameObject.GetComponent<CustomerManager>();
        }
        else
            currentCustomerInFront = null;
    }

    private void GetCustomerStage2InFront()
    {
        // Cast character controller shape 10 meters forward to see if it is about to hit anything.
        if ((Physics.CapsuleCast(p1, p2, charContr.radius, transform.forward, out hit, 10) &&
             hit.transform.tag == "Customer"))
        {
            distanceToObstacle = hit.distance;
            currentCustomerStage2InFront = hit.transform.gameObject.GetComponent<CustomerManagerStage2>();
        }
        else
            currentCustomerStage2InFront = null;
    }

    private void GetBinInFront()
    {
        // Cast character controller shape 10 meters forward to see if it is about to hit anything.
        if ((Physics.CapsuleCast(p1, p2, charContr.radius, transform.forward, out hit, 10) &&
             hit.transform.tag == "Bin"))
        {
            distanceToObstacle = hit.distance;
            currentBinInFront = hit.transform.gameObject.GetComponent<BinManager>();
        }
        else
            currentBinInFront = null;
    }

    private void GetPlateHolderInFront()
    {
        // Cast character controller shape 10 meters forward to see if it is about to hit anything.
        if ((Physics.CapsuleCast(p1, p2, charContr.radius, transform.forward, out hit, 10) &&
             hit.transform.tag == "Tray"))
        {
            distanceToObstacle = hit.distance;
            currentPlateInFront = hit.transform.gameObject.GetComponent<PlateItem>();
        }
        else
            currentPlateInFront = null;
    }

    private void GetStoveInFront()
    {
        if ((Physics.CapsuleCast(p1, p2, charContr.radius, transform.forward, out hit, 10) &&
             hit.transform.tag == "Stove"))
        {
            distanceToObstacle = hit.distance;
            currentStoveInFront = hit.transform.gameObject.GetComponent<StoveManager>();
        }
        else
            currentStoveInFront = null;
    }

    private void GetChoppingBoardInFront()
    {
        if ((Physics.CapsuleCast(p1, p2, charContr.radius, transform.forward, out hit, 10) &&
             hit.transform.tag == "ChoppingBoard"))
        {
            distanceToObstacle = hit.distance;
            currentChoppingBoardInFront = hit.transform.gameObject.GetComponent<ChoppingBoardManager>();
        }
        else
            currentChoppingBoardInFront = null;
    }

    private void GetCounterInFront()
    {
        if ((Physics.CapsuleCast(p1, p2, charContr.radius, transform.forward, out hit, 10) &&
             hit.transform.tag == "Counter"))
        {
            distanceToObstacle = hit.distance;
            currentCounterInFront = hit.transform.gameObject.GetComponent<CounterManager>();
        }
        else
            currentCounterInFront = null;
    }

    private void GetPanInFront()
    {
        if ((Physics.CapsuleCast(p1, p2, charContr.radius, transform.forward, out hit, 10) &&
             hit.transform.tag == "Pan"))
        {
            distanceToObstacle = hit.distance;
            currentPanInFront = hit.transform.gameObject.GetComponent<PanManager>();
        }
        else
            currentPanInFront = null;
    }

    private void GetPotInFront()
    {
        // Cast character controller shape 10 meters forward to see if it is about to hit anything.
        if ((Physics.CapsuleCast(p1, p2, charContr.radius, transform.forward, out hit, 10) &&
             hit.transform.tag == "Pot"))
        {
            distanceToObstacle = hit.distance;
            currentPotInFront = hit.transform.gameObject.GetComponent<PotManager>();
        }
        else
            currentPotInFront = null;
    }

    private void GetSinkInFront()
    {
        if ((Physics.CapsuleCast(p1, p2, charContr.radius, transform.forward, out hit, 10) &&
             hit.transform.tag == "Sink"))
        {
            distanceToObstacle = hit.distance;
            currentSinkInFront = hit.transform.gameObject.GetComponent<SinkManager>();
        }
        else
            currentSinkInFront = null;
    }

    private void PickUpObj()
    {
        if (holding)
        {
            if (currentPlateInFront) //Holding a plate
                if (currentFoodInFront)
                {
                    currentFoodInFront.StopFoodItemSoundEffect();
                    currentFoodInFront.SetDefaultFoodUI();
                    currentPlateInFront.GetComponent<PlateItem>().AddFoodToPlate(currentFoodInFront.gameObject);
                }
        }
        else
        {
            if (currentPlateInFront)
                TakeObjIntoHold(currentPlateInFront.gameObject);

            if (currentFoodInFront)
            {
                if (currentFoodInFront.GetComponent<FoodItem>().CanPickupWithHands())
                {
                    playerAudioSource.PlayOneShot(pick_up);//

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
            if (Input.GetButtonUp("DropOBJ"))
            {
                playerAudioSource.PlayOneShot(pick_up);//
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
                        if (holdingItem.GetComponent<PlateItem>().DeliverFoodViaPlate(currentCustomerInFront))
                            ResetHolding();

                        playerAudioSource.PlayOneShot(sent_food);//
                    }
                    else
                        UnHoldItem(holdingItem);
                }
                else if (currentCustomerStage2InFront)
                {
                    if (holdingItem.GetComponent<FoodItem>())
                    {
                        FoodItem foodToServe = holdingItem.GetComponent<FoodItem>();
                        if (currentCustomerStage2InFront.ReceiveOrder(foodToServe))
                        {
                            Destroy(holdingItem);
                            ResetHolding();
                        }

                    }
                    else if (holdingItem.GetComponent<PlateItem>())
                    {
                        if (holdingItem.GetComponent<PlateItem>().DeliverFood2ViaPlate(currentCustomerStage2InFront))
                            ResetHolding();

                        playerAudioSource.PlayOneShot(sent_food);//
                    }
                    else
                        UnHoldItem(holdingItem);
                }
                else if (currentBinInFront)
                {
                    PlateItem plateOnHold= itemInHold.GetComponent<PlateItem>();
                    
                    if (plateOnHold != null)
                    {
                        
                        if (plateOnHold.ItemInPlate().Count > 0)
                        {
                            plateOnHold.ClearAllItemInPlate();
                        }
                        else
                            return;
                    }
                    else
                    {
                        currentBinInFront.GetComponent<BinManager>().ThrowItemToBin(itemInHold);
                        ResetHolding();
                    }
                        
                }
                else if (currentStoveInFront)
                {
                    if (holdingItem.GetComponent<FoodItem>().GetFoodItemId() == 0 ||
                        holdingItem.GetComponent<FoodItem>().GetFoodItemId() == 2)
                    {
                        currentStoveInFront.PlaceObjIntoStove(holdingItem, ref holding);
                        holdingItem.GetComponent<FoodItem>().PutFoodInTheStove();
                        UnHoldItem(holdingItem);
                    }
                }
                else if (currentChoppingBoardInFront)
                {
                    if (holdingItem.GetComponent<FoodItem>().GetFoodItemId() == 0 ||
                        holdingItem.GetComponent<FoodItem>().GetFoodItemId() == 3)
                    {
                        currentChoppingBoardInFront.PlaceFoodOnChoppingBoard(holdingItem, ref holding);
                        UnHoldItem(holdingItem);
                    }
                }
                else if (currentCounterInFront)
                {
                    if (holdingItem)
                    {
                        currentCounterInFront.PlaceFoodOnCounter(holdingItem, ref holding);
                        UnHoldItem(holdingItem);
                    }
                }
                else if (currentPanInFront)
                {
                    if (holdingItem.GetComponent<FoodItem>().GetFoodItemId() == 3)
                    {
                        currentPanInFront.PlaceObjIntoPan(holdingItem, ref holding);
                        holdingItem.GetComponent<FoodItem>().PutFoodInThePan();
                        UnHoldItem(holdingItem);
                    }
                }
                else if (currentPotInFront)
                {
                    if (holdingItem.GetComponent<FoodItem>() == null) return;
                    if (holdingItem.GetComponent<FoodItem>().GetFoodItemId() == 1)
                    {
                        currentPotInFront.PlaceFoodIntoPot(holdingItem, ref holding);
                        holdingItem.GetComponent<FoodItem>().PutFoodInThePot();
                        UnHoldItem(holdingItem);
                    }
                }
                else if (currentSinkInFront)
                {
                    if (holdingItem)
                    {
                        currentSinkInFront.PlacePlateIntoSink(holdingItem, ref holding);
                        UnHoldItem(holdingItem);
                    }
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
                    currentFoodInFront.GetComponent<FoodItem>().ChopFood();
    }

    private void PlateActions()
    {
        if (!holding)
        {
            if (currentPlateInFront)
            {
                if (currentPlateInFront.GetPlateIntoSink())
                {
                    currentPlateInFront.GetComponent<PlateItem>().WashPlate();

                    if (currentPlateInFront.washDone == true)
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
    }

    private void ResetHolding()
    {
        holding = false;
        itemInHold = null;
        currentFoodInFront = null;
        currentCustomerInFront = null;
        currentCustomerStage2InFront = null;
        currentPlateInFront = null;
        currentBinInFront = null;
        currentStoveInFront = null;
        currentChoppingBoardInFront = null;
        currentCounterInFront = null;
        currentPanInFront = null;
        currentPotInFront = null;
        currentSinkInFront = null;
    }
}
