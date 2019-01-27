namespace HomeTakeover.Util 
{
    using UnityEngine;
    using Util.ObjectPooling;

    public class SoundPool : ObjectPool
    {
        [SerializeField]
        private SoundPlayer[] soundPools;

        [SerializeField]
        private int[] soundPoolSizes;

        // public enum SoundTypes { armstretch, charge1, charge2, charge3, charge4, charge5,
        //                          drop, enemydefeat, enemyhit, furnace, jump, pickup, playerhit, punch,
        //                          repair, shoot1, shoot2, shoot3, shoot4, throwe }

        // armstretch = 0, charge1 = 1, charge2 = 2, charge3 = 3, charge4 = 4, charge5 = 5,
        // drop = 6, enemydefeat = 7, enemyhit = 8, furnace = 9, jump = 10, pickup = 11, playerhit = 12, punch = 13,
        // repair = 14, shoot1 = 15, shoot2 = 16, shoot3 = 17, shoot4 = 18, throw = 19, 

        /// <summary> Sound Playing Singleton instance
        public static SoundPool Instance { get; private set; }

        protected override void PreInit()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("Duplicate Sound Pool detected: removing " + this.gameObject.name);
                Destroy(this.gameObject);
                return;
            }

            Instance = this;
        }

        protected override IPoolable[] GetTemplets()
        {
            return this.soundPools;
        }

        protected override int[] GetPoolSizes()
        {
            return this.soundPoolSizes;
        }

        public GameObject PlaySound(int type)
        {
            IPoolable entity = AllocateEntity(soundPools[(int)type]);
            if (entity == null)
                return null;
            entity.Initialize();
            return entity.GetGameObject();
        }

        public void ReturnSound(int type, GameObject sound)
        {
            IPoolable entity = sound.GetComponent<IPoolable>();
            DeallocateEntity(soundPools[(int)type], entity);
        }
    }
}