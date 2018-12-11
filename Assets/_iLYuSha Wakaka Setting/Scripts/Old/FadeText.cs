using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AirSupremacy
{
    public class FadeText : MonoBehaviour
    {
        public bool tips;
        float timer;
        float fadeTime = 0.37f;
        public float lightTime = 5.0f;
        [HideInInspector]
        public ObjectPoolData objPoolData;

        void OnEnable()
        {
            timer = Time.time + lightTime + fadeTime;
        }
        void Update()
        {
            if (timer - Time.time <= fadeTime)
            {
                GetComponent<CanvasGroup>().alpha -= (1 / fadeTime) * Time.deltaTime;
            }

            if (Time.time > timer && !tips)
            {
                if (tips)
                    DestroyImmediate(gameObject);
                else
                    Recovery();
            }
        }
        void Recovery()
        {
            //objPoolData.Recovery(gameObject);
        }
    }

}
