using UnityEngine;

namespace Kocmoca
{
    public class WreckageBehavior : ObjectRecycleSystem
    {
        private float maxLifeTime = 7.0f;

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