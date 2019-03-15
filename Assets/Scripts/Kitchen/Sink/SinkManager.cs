using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkManager : InteractableManager
{
    public override bool Interact(GameObject target, ref bool holding, PlayerController player = null)
    {
        var plate = target.GetComponent<PlateItem>();
        if (plate == null) return false;
       SetTargetPosition(plate.transform);
        holding = false;
        
        plate.SetPlateIntoSink(true);
        return true;
    }
}
