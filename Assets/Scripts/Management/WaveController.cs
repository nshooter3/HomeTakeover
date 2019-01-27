namespace HomeTakeover.Management
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class WaveController : MonoBehaviour
    {
        public Util.ObjectPooling.ObjectPool[] pools;
        public Spawner.EnemySpawner[] spawners;
        public Spawner.FurnitureSpawner[] furnitureSpawners;
        public Spawner.Wave[] waves;
        public float timeBetweenWaves;
        public float timeBetweenStress;
        public float timeBetweenFurniture;
        public TimeTracker timetracker;
        public int maxStress;

        public float stress;

        private float time;
        private float stressTime;
        private float furnitureTime;
        private int wave;
        private List<GameObject> spawns;
        private List<GameObject> furntitureSpawns;
        private int spawner;
        private int furniture;

        private void Start()
        {
            foreach (Util.ObjectPooling.ObjectPool p in pools)
                p.Init();

            time = timeBetweenWaves;
            furnitureTime = timeBetweenFurniture;
            stressTime = 0;
            wave = 0;
            spawner = 0;
            stress = 0;
            spawns = new List<GameObject>();
        }

        private void Update()
        {
            timetracker.time += Time.deltaTime;
            if ((time += Time.deltaTime) > timeBetweenWaves)
            {
                if (wave >= waves.Length)
                {
                    timetracker.isWin = true;
                    SceneManager.LoadScene("End");
                    return;
                }
                foreach (Enemies.EnemyPool.EnemyTypes t in waves[wave].enemies)
                {
                    GameObject g = spawners[spawner].Spawn(t);
                    if (g != null)
                        spawns.Add(g);

                    spawner++;
                    if (spawner >= spawners.Length)
                        spawner = 0;
                }

                wave++;

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

                if (spawns.Count > stress)
                    stress++;
                else if (spawns.Count < stress)
                    stress--;

                if(stress > maxStress)
                {
                    timetracker.isWin = false;
                    SceneManager.LoadScene("End");
                }

                stressTime = 0;
            }
            if((furnitureTime += Time.deltaTime) > timeBetweenFurniture)
            {
                if(furntitureSpawns == null)
                {
                    furntitureSpawns = new List<GameObject>();
                    foreach(Spawner.FurnitureSpawner furnitureSpawner in furnitureSpawners)
                    {
                        GameObject g = furnitureSpawner.Spawn();
                        if (g != null)
                            furntitureSpawns.Add(g);
                    }
                }
                else
                {
                    List<GameObject> temp = new List<GameObject>();
                    foreach (GameObject g in furntitureSpawns)
                    {
                        if (g.activeSelf)
                            temp.Add(g);
                    }

                    furntitureSpawns = temp;
                    if(furntitureSpawns.Count == 0)
                    {
                        GameObject g = furnitureSpawners[furniture].Spawn();
                        if (g != null)
                            furntitureSpawns.Add(g);

                        furniture++;
                        if (furniture >= furnitureSpawners.Length)
                            furniture = 0;
                    }
                }

                furnitureTime = 0;
            }
        }
    }
}
