namespace HomeTakeover.Enemies
{
    using UnityEngine;
    using Util.ObjectPooling;

    public class EnemyPool : ObjectPool
    {
        [SerializeField]
        private Enemy[] enemyPools;
        [SerializeField]
        private int[] enemyPoolSizes;

        public enum EnemyTypes { Blender, Chair, Stool, Vacuum }

        /// <summary> Singleton instance for this object pool. </summary>
        public static EnemyPool Instance { get; private set; }

        protected override void PreInit()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("Duplicate Enemy Pool detected: removing " + this.gameObject.name);
                Destroy(this.gameObject);
                return;
            }

            Instance = this;
        }

        protected override IPoolable[] GetTemplets()
        {
            return this.enemyPools;
        }

        protected override int[] GetPoolSizes()
        {
            return this.enemyPoolSizes;
        }

        public GameObject GetEnemy(EnemyTypes type)
        {
            IPoolable entity = AllocateEntity(enemyPools[(int)type]);
            if (entity == null)
                return null;

            return entity.GetGameObject();
        }

        public void ReturnEnemy(EnemyTypes type, GameObject enemy)
        {
            IPoolable entity = enemy.GetComponent<IPoolable>();
            DeallocateEntity(enemyPools[(int)type], entity);
        }
    }
}
