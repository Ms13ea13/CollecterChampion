using UnityEngine;

public class ChoppingBoardManager : MonoBehaviour
{
    public void PlaceFoodOnChoppingBoard(GameObject target  , ref bool holding)
    {
        target.transform.parent = transform;
        Vector3 temp = target.transform.localPosition;
        temp.y = 0.148f;
        temp.x = 0;
        temp.z = 0;
        target.transform.localPosition = temp;
        Quaternion tempQuaternion = new Quaternion(0f,0f,0f,0f);
        target.transform.localRotation = tempQuaternion;
        holding = false;
        target.GetComponent<FoodItem>().SetFoodOnChoppingBoard(true);
    }
}
