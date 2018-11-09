﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    /*[SerializeField]
    private FoodStockManager[] foodStockManager;*/

    [SerializeField] private CharacterController charContr;

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
            DropOBj(ref itemInHold);
            GetCustomerInFront();
            GetBinInFront();
            GetStoveInFront();
            GetChoppingBoardInFront();
            GetCounterInFront();
            GetPotInFront();
        }
        else
            GetTrayHolderInFront();

        GetFoodInFront();
        

        if (Input.GetKeyUp(KeyCode.Space))
            PickUpObj();

        if (Input.GetKey(KeyCode.H))
            FoodActions();
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
                currentFoodInFront.SetDefaultFoodUI();
                TakeObjIntoHold(currentFoodInFront.gameObject);
            }
              
        }
    }

    private void DropOBj(ref GameObject target)
    {
        if (target && holding)
        {
            if (Input.GetKeyUp(KeyCode.B))
            {
                if (currentCustomerInFront)
                {
                    if (target.GetComponent<FoodItem>())
                    {
                        FoodItem foodToServe = target.GetComponent<FoodItem>();
                        if (currentCustomerInFront.RecieveOrder(foodToServe))
                        {
                            Destroy(target);
                            ResetHolding();
                        }
                    }
                    else if (target.GetComponent<TrayItem>())
                    {
                        target.GetComponent<TrayItem>().DeliverFoodViaTray(currentCustomerInFront);
                        ResetHolding();
                    }
                    else
                        UnHoldItem(target);
                }
                else if (currentBinInFront)
                {
                    currentBinInFront.GetComponent<BinManager>().ThrowItemToBin(itemInHold);
                    ResetHolding();
                }
                else if (currentStoveInFront)
                {
                    if (target.GetComponent<FoodItem>().GetFoodItemId() == 0)
                    {
                        currentStoveInFront.PlaceObjIntoStove(target, ref holding);
                        target.GetComponent<FoodItem>().PutFoodInTheStove();
                        UnHoldItem(target);
                    }
                }
                else if (currentChoppingBoardInFront)
                {
                    if (target.GetComponent<FoodItem>().GetFoodItemId() == 0)
                    {
                        currentChoppingBoardInFront.PlaceFoodOnChoppingBoard(target, ref holding);
                        UnHoldItem(target);
                    }
                }
                else if (currentCounterInFront)
                {
                    if (target)
                    {
                        currentCounterInFront.PlaceFoodOnCounter(target, ref holding);
                        UnHoldItem(target);
                    }
                }
                else if (currentPotInFront)
                {
                    if (target.GetComponent<FoodItem>().GetFoodItemId() == 1)
                    {
                        currentPotInFront.PlaceFoodIntoPot(target, ref holding);
                        target.GetComponent<FoodItem>().PutFoodInThePot();
                        UnHoldItem(target);
                    }
                }
                else
                    UnHoldItem(target);
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
                    currentFoodInFront.GetComponent<FoodItem>().ChopFood(currentFoodInFront.gameObject);

                    
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
    }
}