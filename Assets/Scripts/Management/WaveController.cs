namespace HomeTakeover.Management
{
    using UnityEngine;

    public class WaveController : MonoBehaviour
    {
        public Spawner.Spawner spawner;
        public Spawner.Wave[] waves;
        public float timeBetweenWaves;

        private float time;
        private int wave;

        private void Start()
        {
            time = 0;
            wave = 0;
        }

        private void Update()
        {
            if((time += Time.deltaTime) > timeBetweenWaves)
            {
                foreach (GameObject g in waves[wave].objects)
                    spawner.Spawn(g);

                wave++;
                if (wave >= waves.Length)
                    wave = 0;
            }
        }
    }
}
