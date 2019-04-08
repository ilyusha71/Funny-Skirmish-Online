using UnityEngine;

namespace Kocmoca
{
    public class HitSpark : ObjectRecycleSystem
    {
        private void OnEnable()
        {
            timeRecovery = Time.time + 0.37f;
        }

        private void Update()
        {
            if (Time.time > timeRecovery)
                Recycle(gameObject);
        }
    }
}