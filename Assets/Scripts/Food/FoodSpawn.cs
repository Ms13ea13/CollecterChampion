using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawn : MonoBehaviour
{
    [SerializeField]
    private int foodID;
    [SerializeField]
    private GameObject foodPrefab;

    void Start()
    {
        SpawnFood();
    }

    private void SpawnFood()
    {
        if (transform.childCount == 0)
        {
            GameObject spawnFood = Instantiate(foodPrefab);
            spawnFood.GetComponent<FoodItem>().SetFoodItemId(foodID);
            spawnFood.transform.parent = transform;
            spawnFood.transform.localPosition = Vector3.zero;
        }
    }
}
