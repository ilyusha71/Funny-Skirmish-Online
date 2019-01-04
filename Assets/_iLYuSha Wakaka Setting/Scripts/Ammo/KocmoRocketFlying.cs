using System.Collections;
using UnityEngine;

namespace Kocmoca
{
    public class KocmoRocketFlying : Ammo
    {
        public GameObject effect;
        public ObjectPoolData objPoolData;
        public ParticleSystem[] vfx;

        private void Awake()
        {
            InitializeAmmo();
            objPoolData = ObjectPoolManager.Instance.CreatObjectPool(effect, 7,140);
            vfx = GetComponentsInChildren<ParticleSystem>();
        }

        private void OnEnable()
        {
            for (int i = 0; i < vfx.Length; i++)
            {
                vfx[i].Stop();
            }
            ResetAmmo();
            StartCoroutine(FlyingInitialize());
        }

        IEnumerator FlyingInitialize()
        {
            yield return new WaitForSeconds(0.0001f);
            if (target)
            {
                float nowDistance = Vector3.Distance(target.transform.position, myTransform.position);
                float expectedTime = Mathf.Sqrt(nowDistance * nowDistance / (KocmoRocketLauncher.flightVelocity * KocmoRocketLauncher.flightVelocity - target.GetComponent<Rigidbody>().velocity.sqrMagnitude));

                Vector3 expectedTargetPosition =
                    target.transform.position +
                    target.GetComponent<Rigidbody>().velocity * expectedTime;
                Vector3 expectedTargetDirection = (expectedTargetPosition - myTransform.position).normalized;
                myTransform.forward = expectedTargetDirection;
            }
            myTransform.localRotation *= Quaternion.Euler(0, projectileSpread, 0);
            timeRecovery = Time.time + KocmoRocketLauncher.flightTime;
            for (int i = 0; i < vfx.Length; i++)
            {
                vfx[i].Play();
            }
        }

        void Update()
        {
            if (Time.time > timeRecovery)
                Recycle(gameObject);
        }

        private void FixedUpdate()
        {
            myRigidbody.velocity = myTransform.forward * KocmoRocketLauncher.thrust * Time.fixedDeltaTime;
            CollisionDetection();
        }

        protected override void CollisionDetection()
        {
            raycastHits = Physics.RaycastAll(pointStarting, transform.forward, Vector3.Distance(myTransform.position, pointStarting));
            if (raycastHits.Length > 0)
            {
                objPoolData.Reuse(raycastHits[0].point, Quaternion.identity);
                KocmocraftMechDroid hull = raycastHits[0].transform.GetComponent<KocmocraftMechDroid>();
                if (hull)
                {
                    float basicDamage = myRigidbody.velocity.magnitude * KocmoRocketLauncher.coefficientDamageBasic;
                    hull.Hit(new DamageInfo()
                    {
                        Attacker = owner,
                        Hull = (int)(basicDamage * KocmoRocketLauncher.coefficientDamageHull),
                        Shield = (int)(basicDamage * KocmoRocketLauncher.coefficientDamageShield)
                    });
                }
                Recycle(gameObject);
                return;
            }
            pointStarting = myTransform.position;
        }
    }
}