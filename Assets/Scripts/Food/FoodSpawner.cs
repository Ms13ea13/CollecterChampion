using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    [SerializeField]
    private int foodID;
    [SerializeField]
    private GameObject foodPrefab;

    void Update()
    {
        SpawnFood();
    }

    private void SpawnFood()
    {
        if (transform.childCount == 0)
        {
            GameObject spawnFood = Instantiate(foodPrefab);
            spawnFood.GetComponent<FoodItem>().SetUpFoodItem(foodID);
            spawnFood.transform.parent = transform;
            spawnFood.transform.localPosition = Vector3.zero;
        }
    }
}
