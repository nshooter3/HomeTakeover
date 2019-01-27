namespace HomeTakeover.UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.SceneManagement;

    public class WinScreen : MonoBehaviour
    {
        public Text win;
        public Text time;
        public float waitTime;

        private float timer;

        private void Start()
        {
            var time = FindObjectOfType<Management.TimeTracker>();
            if (time.isWin)
                win.text = "You survived!";
            else
                win.text = "You died";

            this.time.text = "You survived for " + time.time + " seconds";
            Destroy(time.gameObject);
        }

        private void Update()
        {
            if((timer += Time.deltaTime) > waitTime)
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
    }
}
