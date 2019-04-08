﻿using System.Collections;
using UnityEngine;

namespace Kocmoca
{
    public class KocmoRocketFlying : Ammo
    {
        public GameObject effect;
        public ObjectPoolData objPoolData;
        public ParticleSystem[] vfx;

        // Target param.
        private Rigidbody targetRigid;

        private void Awake()
        {
            InitializeAmmo();
            objPoolData = ObjectPoolManager.Instance.CreatObjectPool(effect, 7, 140);
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
            yield return SatelliteCommander.waitShoot;
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
            CollisionDetection();
        }

        protected override void CollisionDetection()
        {
            raycastHits = Physics.RaycastAll(pointStarting, myTransform.forward, Vector3.Distance(myTransform.position, pointStarting));
            if (raycastHits.Length > 0)
            {
                KocmocraftMechDroid hull = raycastHits[0].transform.GetComponent<KocmocraftMechDroid>();
                if (hull)
                {
                    if (hull.Number == shooter) return;
                    float basicDamage = myRigidbody.velocity.magnitude * KocmoRocketLauncher.coefficientDamageBasic;
                    hull.Hit(new DamageInfo()
                    {
                        Attacker = owner,
                        Hull = (int)(basicDamage * KocmoRocketLauncher.coefficientDamageHull),
                        Shield = (int)(basicDamage * KocmoRocketLauncher.coefficientDamageShield)
                    });
                }
                objPoolData.Reuse(raycastHits[0].point, Quaternion.identity);
                Recycle(gameObject);
                return;
            }
            pointStarting = myTransform.position;
        }
    }
}