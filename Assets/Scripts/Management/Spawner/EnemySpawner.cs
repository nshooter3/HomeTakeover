namespace HomeTakeover.Management.Spawner
{
    using UnityEngine;
    using Enemies;

    public class EnemySpawner : MonoBehaviour
    {
        public EnemyPool.EnemyTypes type;
        public bool spawnOnStart;

        private void Start()
        {
            if (spawnOnStart)
                Spawn();
        }

        public GameObject Spawn()
        {
            GameObject obj = EnemyPool.Instance.GetEnemy(type);
            if(obj != null)
                obj.transform.position = this.transform.position;
            return obj;
        }
    }
}
