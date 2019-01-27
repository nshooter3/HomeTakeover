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

        public enum SoundTypes { armstretch, enemyhit, charge1, charge2, charge3, charge4, charge5,
                                 drop, enemydefeat, furnace, jump, music, pickup, playerhit, punch,
                                 repair, shoot1, shoot2, shoot3, shoot4, throwe }

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

        public GameObject PlaySound(SoundTypes type)
        {
            IPoolable entity = AllocateEntity(soundPools[(int)type]);
            if (entity == null)
                return null;
            entity.Initialize();
            return entity.GetGameObject();
        }

        public void ReturnSound(SoundTypes type, GameObject sound)
        {
            IPoolable entity = sound.GetComponent<IPoolable>();
            DeallocateEntity(soundPools[(int)type], entity);
        }
    }
}