using UnityEngine;

namespace Kocmoca
{
    public class HitSpark : ObjectRecycleSystem
    {
        public float lifeTime = 0.37f;
        private void OnEnable()
        {
            timeRecovery = Time.time + lifeTime;
        }

        private void Update()
        {
            if (Time.time > timeRecovery)
                Recycle(gameObject);
        }
    }
}