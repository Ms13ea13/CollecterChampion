using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectStage : MonoBehaviour
{
    [SerializeField] private LevelLoader loadingStage;

    [SerializeField] private int sceneNumber;
 
    void OnMouseDown()
    { 
        loadingStage.Loadlevel(sceneNumber);
    }
}
