using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveManager : MonoBehaviour
{
    public void GrillFood(GameObject target)
    {
        if (!target)
            return;

        FoodChangeMat(target);
    }

    private void FoodChangeMat(GameObject food)
    {
        //Destroy(food);
        food.GetComponent<Renderer>().material.color = new Color(69, 228, 213);
    }
}
