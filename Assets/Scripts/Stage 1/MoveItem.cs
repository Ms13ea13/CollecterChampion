using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveItem : MonoBehaviour
{
    public float _itemspeed;
	
	void Update ()
    {
        transform.position += transform.right * Time.deltaTime * -1 * _itemspeed;

        if (transform.position.x <= -4.33f && transform.position.z == -4.41f)
        {
            float yRotation = 90.0f;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, yRotation, transform.eulerAngles.z);
        }

        if (transform.position.x <= -4.33f && transform.position.z == 4.21f)
        {
            float yRotation = -90.0f;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, yRotation, transform.eulerAngles.z);
        }
    }

    void OnTriggerEnter(Collider hit)
    {
        if (hit.gameObject.tag == "Bin")
        {
            Destroy(gameObject);
        }
    }
}
