using SpawnItem;
using UnityEngine;

public class PlayerRayCast : MonoBehaviour
{
    [SerializeField] private GameObject itemInHold;

    [SerializeField] private FoodItem currentFoodInFront;

    [SerializeField] private CustomerManager currentCustomerInFront;

    [SerializeField] private BinManager currentBinInFront;

    [SerializeField] private TrayItem currentTrayInFront;

    [SerializeField] private StoveManager currentStoveInFront;

    [SerializeField] private ChoppingBoardManager currentChoppingBoardInFront;

    [SerializeField] private CounterManager currentCounterInFront;

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

    void Start()
    {
        holding = false;
        distanceToObstacle = 0;
    }

    public void ShootRayCast()
    {
        p1 = transform.position + charContr.center + Vector3.up * -charContr.height * 0.5F;
        p2 = p1 + Vector3.up * charContr.height;

        if (holding)
        {
            DropObj(ref itemInHold);
            GetCustomerInFront();
            GetBinInFront();
            GetStoveInFront();
            GetChoppingBoardInFront();
            GetCounterInFront();
            GetPotInFront();
            GetSinkInFront();
        }
        else
            GetTrayHolderInFront();

        GetFoodInFront();


        if (Input.GetKeyUp(KeyCode.Space))
            PickUpObj();

        if (Input.GetKey(KeyCode.H))
        {
            FoodActions();
            TrayActions();
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

    private void GetTrayHolderInFront()
    {
        // Cast character controller shape 10 meters forward to see if it is about to hit anything.
        if ((Physics.CapsuleCast(p1, p2, charContr.radius, transform.forward, out hit, 10) &&
             hit.transform.tag == "Tray"))
        {
            distanceToObstacle = hit.distance;
            currentTrayInFront = hit.transform.gameObject.GetComponent<TrayItem>();
        }
        else
            currentTrayInFront = null;
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
            if (currentTrayInFront)
                if (currentFoodInFront)
                {
                    currentFoodInFront.SetDefaultFoodUI();
                    currentTrayInFront.GetComponent<TrayItem>().AddFoodToTray(currentFoodInFront.gameObject);
                }
        }
        else
        {
            if (currentTrayInFront)
                TakeObjIntoHold(currentTrayInFront.gameObject);

            if (currentFoodInFront)
            {
                if (currentFoodInFront.GetComponent<FoodItem>().CanPickupWithHands())
                {
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
            if (Input.GetKeyUp(KeyCode.B))
            {
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
                    else if (holdingItem.GetComponent<TrayItem>())
                    {
                        if (holdingItem.GetComponent<TrayItem>().DeliverFoodViaTray(currentCustomerInFront))
                            ResetHolding();
                    }
                    else
                        UnHoldItem(holdingItem);
                }
                else if (currentBinInFront)
                {
                    currentBinInFront.GetComponent<BinManager>().ThrowItemToBin(itemInHold);
                    ResetHolding();
                }
                else if (currentStoveInFront)
                {
                    if (holdingItem.GetComponent<FoodItem>().GetFoodItemId() == 0)
                    {
                        currentStoveInFront.PlaceObjIntoStove(holdingItem, ref holding);
                        holdingItem.GetComponent<FoodItem>().PutFoodInTheStove();
                        UnHoldItem(holdingItem);
                    }
                }
                else if (currentChoppingBoardInFront)
                {
                    if (holdingItem.GetComponent<FoodItem>().GetFoodItemId() == 0)
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
                else if (currentPotInFront)
                {
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
                        currentSinkInFront.PlaceTrayIntoSink(holdingItem, ref holding);
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
        {
            if (currentFoodInFront)
            {
                if (currentFoodInFront.GetFoodOnChoppingBoard())
                    currentFoodInFront.GetComponent<FoodItem>().ChopFood();
            }
        }
    }

    private void TrayActions()
    {
        if (!holding)
        {
            if (currentTrayInFront)
            {
                if (currentTrayInFront.GetTrayIntoSink())
                {
                    currentTrayInFront.GetComponent<TrayItem>().WashTray();

                    if (currentTrayInFront.washDone == true)
                    {
                        spawnCleanDish.SpawnDish();
                        currentTrayInFront.washDone = false;
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
        target.transform.parent = transform;
        Vector3 temp = target.transform.localPosition;
        temp.y = 0.165f;
        temp.x = 0;
        temp.z = 0.185f;
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
        currentTrayInFront = null;
        currentBinInFront = null;
        currentStoveInFront = null;
        currentChoppingBoardInFront = null;
        currentCounterInFront = null;
        currentPotInFront = null;
        currentSinkInFront = null;
    }
}