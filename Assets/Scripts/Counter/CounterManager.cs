using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterManager : MonoBehaviour
{
    public void PlaceFoodOnCounter(GameObject target, ref bool holding)
    {
        target.transform.parent = transform;
        Vector3 temp = target.transform.localPosition;
        temp.y = 0.139f;
        temp.x = 0;
        temp.z = 0;
        target.transform.localPosition = temp;
        Quaternion tempQuaternion = new Quaternion(0f, 0f, 0f, 0f);
        target.transform.localRotation = tempQuaternion;
        holding = false;
        
        if (target.GetComponent<FoodItem>() != null) // Can be a TrayItem
            target.GetComponent<FoodItem>().SetFoodOnCounter(true);
    }
}
