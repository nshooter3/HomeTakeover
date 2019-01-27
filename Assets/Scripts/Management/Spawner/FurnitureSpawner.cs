namespace HomeTakeover.Management.Spawner
{
    using UnityEngine;
    using Furniture;
    using System.Collections;

    public class FurnitureSpawner : MonoBehaviour
    {
        public FurniturePool.FurnitureTypes type;
        public bool spawnOnStart;

        private void Start()
        {
            if (spawnOnStart)
                StartCoroutine(SpawnOnNextFrame());
        }

        public GameObject Spawn()
        {
            GameObject obj = FurniturePool.Instance.GetFurniture(this.type);
            if (obj != null)
                obj.transform.position = this.transform.position;
            return obj;
        }

        public GameObject Spawn(FurniturePool.FurnitureTypes type)
        {
            GameObject obj = FurniturePool.Instance.GetFurniture(type);
            if (obj != null)
                obj.transform.position = this.transform.position;
            return obj;
        }

        public IEnumerator SpawnOnNextFrame()
        {
            yield return new WaitForEndOfFrame();
            Spawn();
        }
    }
}
