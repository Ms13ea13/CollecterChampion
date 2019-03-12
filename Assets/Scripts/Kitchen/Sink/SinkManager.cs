using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkManager : InteractableManager
{
    public override void Interact(GameObject target, ref bool holding)
    {
       SetTargetPosition(target.transform);
        holding = false;
        target.GetComponent<PlateItem>().SetPlateIntoSink(true);
    }
}
