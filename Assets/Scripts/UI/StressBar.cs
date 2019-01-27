namespace HomeTakeover.UI
{
    using UnityEngine;

    public class StressBar : MonoBehaviour
    {
        [SerializeField]
        private RectTransform bar;
        [SerializeField]
        private Management.WaveController waveController;
        [SerializeField]
        private float maxStress;

        private bool init = false;
        private float percent = 0f;
        private Vector3 offset;
        private Transform parent;

        float hideTimer = 0, maxHideTimer = 2.0f;

        private void Start()
        {
            this.parent = this.transform.parent;
            this.offset = this.transform.position - this.parent.position;
            this.init = true;
        }

        private void Update()
        {
            percent = waveController.stress / maxStress;
            bar.anchoredPosition = new Vector3(-190f * (1f - percent), 145f, 0f);
            bar.localScale = new Vector3(percent, 1f, 1f);
        }
    }
}