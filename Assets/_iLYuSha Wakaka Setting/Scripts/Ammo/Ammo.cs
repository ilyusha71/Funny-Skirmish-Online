/***************************************************************************
 * Ammo
 * 彈藥
 * Last Updated: 2018/10/11
 * Description:
 * 1. 
 ***************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kocmoca
{
    public class Ammo : ObjectRecycleSystem
    {
        [Header("Basic")]
        public Transform target;
        public Kocmonaut owner;
        protected Transform myTransform;
        protected Rigidbody myRigidbody;
        protected float timeRecovery;
        [Header("Raycast")]
        protected Vector3 pointStarting;
        protected float distanceRay;
        protected RaycastHit[] raycastHits;
        protected int countRaycastHits;
        //[Header("Damage")]
        ////public int ammoDamage;
        protected float projectileSpread;
        protected void InitializeAmmo()
        {
            myTransform = transform;
            myRigidbody = GetComponent<Rigidbody>();
            ResetAmmo();
        }
        protected void ResetAmmo()
        {
            myRigidbody.Sleep();
            pointStarting = Vector3.zero;
            timeRecovery = Time.time + 100;
        }
        public virtual void InputAmmoData(int numberShooter, int numberTarget, Vector3 initialVelocity, float spread)
        {
            target = null;
            SatelliteCommander.Instance.listKocmonaut.TryGetValue(numberShooter, out owner);
            SatelliteCommander.Instance.listKocmocraft.TryGetValue(numberTarget, out target);
            myRigidbody.velocity = initialVelocity;
            projectileSpread = spread;
        }
        protected virtual void CollisionDetection()
        {
            raycastHits = Physics.RaycastAll(pointStarting, transform.forward, Vector3.Distance(myTransform.position, pointStarting));
            countRaycastHits = raycastHits.Length;
            for (int i = 0; i < countRaycastHits; i++)
            {
                if (pointStarting != Vector3.zero &&
                    raycastHits[i].transform.name != owner.Name &&
                    raycastHits[i].transform.tag != myTransform.tag &&
                    raycastHits[i].transform.tag != "Particle")
                {
                    Recycle(gameObject);
                    return;
                }
            }
            pointStarting = myTransform.position;
        }
    }
}
