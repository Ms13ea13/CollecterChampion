using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumplingSteamedManager : MonoBehaviour
{
    /*public void ThrowItemToBin(GameObject target)
    {
        if (!target)
            return;

        PlayFoodGoDownTrash(target);
    }

    private void PlayFoodGoDownTrash(GameObject item)
    {
        //Set fall down position here
        Destroy(item);
    }*/

    public void PlaceObjIntoDumplingSteamed(GameObject target, ref bool holding)
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
        target.GetComponent<FoodItem>().SetFoodIntoDumplingSteamed(true);
    }
}
