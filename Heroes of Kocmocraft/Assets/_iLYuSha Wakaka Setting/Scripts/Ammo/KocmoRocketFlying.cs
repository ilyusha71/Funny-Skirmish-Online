using System.Collections;
using UnityEngine;

namespace Kocmoca
{
    public class KocmoRocketFlying : Ammo
    {
        public ParticleSystem[] vfx;

        // Target param.
        private Rigidbody targetRigid;

        private void Awake()
        {
            InitializeAmmo();
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
            yield return null;
            if (target)
            {
                targetRigid = target.GetComponent<Rigidbody>();
                float expectedTime = Mathf.Sqrt(Vector3.SqrMagnitude(target.position - myTransform.position) / (KocmoRocketLauncher.flightVelocity * KocmoRocketLauncher.flightVelocity - targetRigid.velocity.sqrMagnitude));

                Vector3 expectedTargetPosition = target.position + targetRigid.velocity * expectedTime;
                Vector3 expectedTargetDirection = (expectedTargetPosition - myTransform.position).normalized;
                myTransform.forward = expectedTargetDirection;
            }
            myTransform.localRotation *= Quaternion.Euler(0, projectileSpread, 0);
            myRigidbody.velocity = myTransform.forward * KocmoRocketLauncher.flightVelocity;
            for (int i = 0; i < vfx.Length; i++)
            {
                vfx[i].Play();
            }
            yield return SatelliteCommander.waitRocketRecovery;
            Recycle(gameObject);
        }

        private void FixedUpdate()
        {
            DetectCollisionByLinecast();
        }

        protected override void DetectCollisionByLinecast()
        {
            if (Physics.Linecast(pointStarting, myTransform.position, out raycastHit))
            {
                AvionicsSystem hull = raycastHit.transform.GetComponent<AvionicsSystem>();
                if (hull)
                {
                    if (hull.kocmoNumber == shooter) return;
                    float basicDamage = myRigidbody.velocity.magnitude * KocmoRocketLauncher.coefficientDamageBasic;
                    hull.Hit(new DamagePower()
                    {
                        Attacker = owner,
                        Hull = (int)(basicDamage * KocmoRocketLauncher.coefficientDamageHull),
                        Shield = (int)(basicDamage * KocmoRocketLauncher.coefficientDamageShield)
                    });
                }
                ResourceManager.hitFire.Reuse(raycastHit.point, Quaternion.identity);
                Recycle(gameObject);
                return;
            }
            pointStarting = myTransform.position;
        }
    }
}