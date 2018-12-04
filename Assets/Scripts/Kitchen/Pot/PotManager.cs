using SpawnItem;
using UnityEngine;

public class PotManager : MonoBehaviour
{
    [SerializeField] private SpawnRiceBoiled spawnRiceObj;

    public void PlaceFoodIntoPot(GameObject target, ref bool holding)
    {
        target.transform.parent = transform;
        Vector3 temp = target.transform.localPosition;
        temp.y = 0.148f;
        temp.x = 0;
        temp.z = 0;
        target.transform.localPosition = temp;
        Quaternion tempQuaternion = new Quaternion(0f, 0f, 0f, 0f);
        target.transform.localRotation = tempQuaternion;
        holding = false;
        target.GetComponent<FoodItem>().SetFoodIntoPot(true);
    }

    public void SpawnRice()
    {
        spawnRiceObj.SpawnRiceBoil();
    }
}
