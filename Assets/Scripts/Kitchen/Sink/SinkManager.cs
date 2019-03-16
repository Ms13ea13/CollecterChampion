using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkManager : InteractableManager
{
//    public override bool Interact(GameObject target, ref bool holding, PlayerController player = null)
//    {
//        var plate = target.GetComponent<PlateItem>();
//        if (plate == null) return false;
//       SetTargetPosition(plate.transform);
//        holding = false;
//        
//        plate.SetPlateIntoSink(true);
//        return true;
//    }
    
    public override GameObject InteractWithPlate(PlateItem plateItem,ref bool holding , PlayerController player = null)
    {
        if (plateItem != null)
        {
            SetTargetPosition(plateItem.transform);
            plateItem.SetPlateIntoSink(true);
            plateItem.GetComponent<Collider>().enabled = true;
            holding = false;
        }

        return null;
    }
}
