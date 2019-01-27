namespace HomeTakeover.Util.ObjectPooling
{
    using UnityEngine;

    public class PoolTester : MonoBehaviour
    {
        public ObjectPool[] pools;

        private void Start()
        {
            foreach (ObjectPool op in pools)
                op.Init();
        }
    }
}
