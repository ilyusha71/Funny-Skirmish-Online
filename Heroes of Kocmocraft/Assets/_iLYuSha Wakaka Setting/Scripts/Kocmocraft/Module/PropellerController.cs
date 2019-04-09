using UnityEngine;

namespace Kocmoca
{
    public class PropellerController : MonoBehaviour
    {
        public float coefficient = 1.0f;
        public bool isReverse;
        private Animation anim;
        private string nameClip;

        void Awake()
        {
            anim = GetComponent<Animation>();
            nameClip = anim.clip.name;
            anim[nameClip].speed = isReverse ? coefficient : -coefficient;
        }

        public void  SetAnimationSpeed(float power)
        {
            anim[nameClip].speed = (isReverse ? coefficient : -coefficient) * power;
        }
    }
}