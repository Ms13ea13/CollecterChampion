using UnityEngine;

namespace SpawnItem
{
    public class SpawnRiceBoiled : MonoBehaviour
    {
        [SerializeField] private GameObject prefabToInstantiate;

        public void SpawnRiceBoil()
        {
            var spawnGameObj = Instantiate(prefabToInstantiate);
            spawnGameObj.transform.parent = transform;
            spawnGameObj.transform.localScale = new Vector3(1, 1, 1);

            var spawnPos = Vector3.zero;
            spawnGameObj.transform.localScale = new Vector3(100, 100, 100);
            spawnGameObj.transform.localPosition = spawnPos;
        }
    }
}

