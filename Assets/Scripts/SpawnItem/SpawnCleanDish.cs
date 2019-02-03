using UnityEngine;

namespace SpawnItem
{
    public class SpawnCleanDish : MonoBehaviour
    {
        [SerializeField] private GameObject prefabToInstantiate;

        private Vector3 temp;
        private int currentIndex;

        void Start()
        {
            currentIndex = 0;
        }

        public void SpawnDish()
        {
            var spawnGameObj = Instantiate(prefabToInstantiate);
            spawnGameObj.transform.parent = transform;
            spawnGameObj.transform.localScale = new Vector3(1, 1, 1);

            var spawnPos = Vector3.zero;
            spawnGameObj.transform.localScale = new Vector3(1, 1, 1);
            spawnGameObj.transform.localPosition = StackDirtyDishes(currentIndex, spawnGameObj.transform);
        }

        private Vector3 StackDirtyDishes(int index, Transform targetTransform)
        {
            temp = targetTransform.localPosition;
            switch (index)
            {
                case 0:
                    {
                        temp.y = 0;
                        break;
                    }
                case 1:
                    {
                        temp.y = 0.0205f;
                        currentIndex = 0;
                        break;
                    }
                default:
                    break;
            }

            temp.x = 0;
            temp.z = 0f;

            if (currentIndex <= 1)
            {
                currentIndex += 1;
            }
            else
            {
                currentIndex = 0;
            }

            return temp;
        }
    }
}
