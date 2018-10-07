using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Target : MonoBehaviour
{ 
    private bool _isCheck = false;
    private bool destroyItem;
    
    public bool GetIsCheck()
    {
        return _isCheck;
    }

    public void SetIsCheck(bool check)
    {
        _isCheck = check;
    }

    public void Throw(float force)
    {
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().AddForce(transform.forward * force,ForceMode.Impulse);
    }

    public void SetDestroy(bool set)
    {
        destroyItem = set;
        if (set)
        {
            Destroy(gameObject);
        }
    }

    public bool GetDestroy()
    {
        return destroyItem;
    }
}
