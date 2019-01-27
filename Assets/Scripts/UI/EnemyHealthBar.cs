namespace HomeTakeover.UI
{
    using UnityEngine;

    public class EnemyHealthBar : MonoBehaviour
    {
        [SerializeField]
        private RectTransform bar;

        private bool init = false;
        private float percent = 1f;
        private Vector3 offset;
        private Transform parent;

        float hideTimer = 0, maxHideTimer = 2.0f;

        public float Percent
        {
            get { return this.percent; }
            set
            {
                if (init)
                {
                    hideTimer = maxHideTimer;
                    this.percent = value;
                    bar.transform.parent.gameObject.SetActive(this.percent < 1f);
                    UpdateBar();
                }
                else
                    Init();
            }
        }

        private void Init()
        {
            this.parent = this.transform.parent;
            this.offset = this.transform.position - this.parent.position;
            this.init = true;
            bar.transform.parent.gameObject.SetActive(this.percent < 1f);
            this.Percent = 1f;
        }

        private void Update()
        {
            if (hideTimer > 0)
            {
                hideTimer -= Time.deltaTime;
                if (hideTimer <= 0)
                {
                    bar.transform.parent.gameObject.SetActive(false);
                }
            }
            this.transform.rotation = Quaternion.identity;
            this.transform.position = this.parent.position + this.offset;
        }

        private void UpdateBar()
        {
            bar.anchoredPosition = new Vector3(-5f * (1f - percent), 0f, 0f);
            bar.localScale = new Vector3(percent, 1f, 1f);
        }
    }
}