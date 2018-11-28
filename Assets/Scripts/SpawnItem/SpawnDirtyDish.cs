using UnityEngine;

namespace SpawnItem
{
    public class SpawnDirtyDish : MonoBehaviour
    {
        [SerializeField] private GameObject prefabToInstantiate;

        public void SpawnDish()
        {
            var spawnGameObj = Instantiate(prefabToInstantiate);
            spawnGameObj.transform.parent = transform;
            spawnGameObj.transform.localScale = new Vector3(1, 1, 1);

            var spawnPos = Vector3.zero;

            spawnGameObj.transform.localScale = new Vector3(100, 100, 100);
            spawnGameObj.transform.localPosition = spawnPos;
        }

        public void DelaySpawnDish(float timeToSpawn)
        {
            LeanTween.delayedCall(timeToSpawn, SpawnDish);
        }
    }
}