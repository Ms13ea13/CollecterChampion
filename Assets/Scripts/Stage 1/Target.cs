using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private bool _isCheck = false;

    public bool GetIsCheck()
    {
        return _isCheck;
    }

    public void SetIsCheck()
    {
        _isCheck = true;
    }
}
