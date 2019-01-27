namespace HomeTakeover.Management
{
    using System.Collections.Generic;
    using UnityEngine;

    public class WaveController : MonoBehaviour
    {
        public Util.ObjectPooling.ObjectPool[] pools;
        public Spawner.EnemySpawner spawner;
        public Spawner.Wave[] waves;
        public float timeBetweenWaves;
        public float timeBetweenStress;

        public float stress;

        private float time;
        private float stressTime;
        private int wave;
        private List<GameObject> spawns;

        private void Start()
        {
            foreach (Util.ObjectPooling.ObjectPool p in pools)
                p.Init();

            time = timeBetweenWaves;
            stressTime = 0;
            wave = 0;
            stress = 0;
            spawns = new List<GameObject>();
        }

        private void Update()
        {
            if ((time += Time.deltaTime) > timeBetweenWaves)
            {
                foreach (Enemies.EnemyPool.EnemyTypes t in waves[wave].enemies)
                {
                    GameObject g = spawner.Spawn(t);
                    if (g != null)
                        spawns.Add(g);
                }

                wave++;
                if (wave >= waves.Length)
                    wave = 0;
                time = 0;
            }
            if ((stressTime += Time.deltaTime) > timeBetweenStress)
            {
                List<GameObject> temp = new List<GameObject>();
                foreach(GameObject g in spawns)
                {
                    if (g.activeSelf)
                        temp.Add(g);
                }

                spawns = temp;

                if (stress < spawns.Count)
                    stress++;
                else if (stress > spawns.Count)
                    stress--;

                stressTime = 0;
            }
        }
    }
}
