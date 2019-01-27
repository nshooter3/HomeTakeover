namespace HomeTakeover.Util
{
    using HomeTakeover.Util.ObjectPooling;
    using UnityEngine;

    public class SoundPlayer : MonoBehaviour, IPoolable
    {
        [SerializeField]
        private SoundPool.SoundTypes type;
        [SerializeField]
        private AudioSource soundPlayer;
        [SerializeField]
        private int referenceIndex = 0;

        public void Initialize()
        {
            soundPlayer.Play();
        }

        public void ReInitialize()
        {
            this.gameObject.SetActive(true);
        }

        public void Deallocate()
        {
            this.gameObject.SetActive(false);
        }

        public void Delete()
        {
            Destroy(this.gameObject);
        }

        void Update()
        {
            if(!soundPlayer.isPlaying)
            {
                ReturnSound();
            }
        }

        public IPoolable SpawnCopy(int referenceIndex)
        {
            SoundPlayer Sound = Instantiate<SoundPlayer>(this);
            Sound.referenceIndex = referenceIndex;
            return Sound;
        }

        public GameObject GetGameObject()
        {
            return this.gameObject;
        }

        public int GetReferenceIndex()
        {
            return this.referenceIndex;
        }

        private void ReturnSound()
        {
            Debug.Log("Returning");
            SoundPool.Instance.ReturnSound(this.type, this.gameObject);
        }
    }
}
