using UnityEngine;

namespace AirSupremacy
{
    public class Crash : ObjectRecycleSystem
    {
        public float maxLifeTime;
        protected float timeRecovery;

        private void OnEnable()
        {
            ResetObject();
        }

        protected void ResetObject()
        {
            timeRecovery = Time.time + maxLifeTime;
        }

        void Update()
        {
            if (Time.time > timeRecovery)
                Recycle(gameObject);
        }
    }
}
