﻿namespace HomeTakeover.UI
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

        public float Percent
        {
            get { return this.percent; }
            set
            {
                if (init)
                {
                    this.percent = value;
                    Debug.Log(value);
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