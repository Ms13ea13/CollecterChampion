﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveItem : MonoBehaviour
{
    public float _itemspeed;

    //private bool checkHit;
    
    /*[SerializeField]
    private Transform[] targetWayPoints;

    private int indeXWayPoint;*/

    void Start()
    {
        /*indeXWayPoint = 0;
        StartCoroutine(GetToWayPoint());*/
    }


    /*private IEnumerator GetToWayPoint()
    {
        while (checkHit == false)
        {
            if (indeXWayPoint < targetWayPoints.Length)
            {
                Vector3 target = targetWayPoints[indeXWayPoint].position;

                if (transform.position != targetWayPoints[indeXWayPoint].position)
                {
                    LerpObj();
                }

                if (transform.position == target)
                {
                    indeXWayPoint += 1;
                }
            }
            else
            {
                indeXWayPoint = 0;
            }
            
            yield return null;
        }
    }*/

    void Update ()
    {
        transform.position += transform.right * Time.deltaTime * -1 * _itemspeed;

        if (transform.position.x <= -7.15f && transform.position.z == -9.1f)
        {
            float yRotation = 90.0f;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, yRotation, transform.eulerAngles.z);
        }

        if (transform.position.x <= -7.15f && transform.position.z == 5.004786f)
        {
            float yRotation = -90.0f;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, yRotation, transform.eulerAngles.z);
        }
    }


   /* private void LerpObj()
    {
        
    }*/

    void OnTriggerEnter(Collider hit)
    {
        if (hit.gameObject.tag == "Bin")
        {
            //checkHit = true;
            Destroy(gameObject);
        }
    }
}
