namespace HomeTakeover.Management
{
    using UnityEngine;

    public class TimeTracker : MonoBehaviour
    {
        public float time;
        public bool isWin;

        private void Start()
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
