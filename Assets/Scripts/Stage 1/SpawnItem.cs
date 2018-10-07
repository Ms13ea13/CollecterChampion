using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour
{
    public string _wantString;

    public GameObject[] ItemStage1Prefab;
    public Transform[] ItemSpawnPoint;
    GameObject cloneItemStage1;

    public Sprite[] ItemStage1Picture;

    void Start()
    {
        InvokeRepeating("generateItem", 1f, 0.75f);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            SpawnNewPicObj();
        }
    }

    public void generateItem()
    {
        int spRandom1 = Random.Range(0, 2);
        int spRandom2 = Random.Range(0, 3);

        cloneItemStage1 = Instantiate(ItemStage1Prefab[spRandom2]);
        cloneItemStage1.transform.position = ItemSpawnPoint[spRandom1].position;
    }

    void SpawnNewPicObj()
    {
        RandomTarget();
        GameManagerStage1.UpdateTargetPicture(_wantString);
    }

    void RandomTarget()
    {
        int _rand = Random.Range(0, 3);

        if (_rand == 0)
        {
            _wantString = "Item1";
            GameManagerStage1.GetInstance().UpdatePicture(ItemStage1Picture[0]);
        }
        else if (_rand == 1)
        {
            _wantString = "Item2";
            GameManagerStage1.GetInstance().UpdatePicture(ItemStage1Picture[1]);
        }
        else
        {
            _wantString = "Item3";
            GameManagerStage1.GetInstance().UpdatePicture(ItemStage1Picture[2]);
        }
    }
}
