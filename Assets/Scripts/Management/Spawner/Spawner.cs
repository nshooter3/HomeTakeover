namespace HomeTakeover.Management.Spawner
{
    using UnityEngine;

    public class Spawner : MonoBehaviour
    {
        public GameObject spawn;

        private void Start()
        {
            if (spawn != null)
                Spawn(spawn);
        }

        public GameObject Spawn(GameObject spawn)
        {
            GameObject obj = Instantiate(spawn);
            obj.transform.position = this.transform.position;
            return obj;
        }
    }
}
