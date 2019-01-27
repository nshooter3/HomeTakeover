﻿namespace HomeTakeover.Management.Spawner
{
    using UnityEngine;
    using Enemies;
    using System.Collections;

    public class EnemySpawner : MonoBehaviour
    {
        public EnemyPool.EnemyTypes type;
        public bool spawnOnStart;

        private void Start()
        {
            if (spawnOnStart)
                StartCoroutine(SpawnOnNextFrame());
        }

        public GameObject Spawn()
        {
            GameObject obj = EnemyPool.Instance.GetEnemy(this.type);
            if (obj != null)
                obj.transform.position = this.transform.position;
            return obj;
        }

        public GameObject Spawn(EnemyPool.EnemyTypes type)
        {
            GameObject obj = EnemyPool.Instance.GetEnemy(type);
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
