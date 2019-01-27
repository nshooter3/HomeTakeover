namespace HomeTakeover.Furniture
{
    using UnityEngine;
    using Util.ObjectPooling;

    public class FurniturePool : ObjectPool
    {
        [SerializeField]
        private Furniture[] furniturePools;
        [SerializeField]
        private int[] furniturePoolSizes;

        public enum FurnitureTypes { Box , Blender, Stool, Vacuum, Armchair }

        /// <summary> Singleton instance for this object pool. </summary>
        public static FurniturePool Instance { get; private set; }

        protected override void PreInit()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("Duplicate Furniture Pool detected: removing " + this.gameObject.name);
                Destroy(this.gameObject);
                return;
            }

            Instance = this;
        }

        protected override IPoolable[] GetTemplets()
        {
            return this.furniturePools;
        }

        protected override int[] GetPoolSizes()
        {
            return this.furniturePoolSizes;
        }

        public GameObject GetFurniture(FurnitureTypes type)
        {
            IPoolable entity = AllocateEntity(furniturePools[(int)type]);
            if (entity == null)
                return null;

            return entity.GetGameObject();
        }

        public void ReturnFurniture(FurnitureTypes type, GameObject enemy)
        {
            IPoolable entity = enemy.GetComponent<IPoolable>();
            DeallocateEntity(furniturePools[(int)type], entity);
        }
    }
}
