/***************************************************************************
 * Ammo
 * 彈藥
 * Last Updated: 2018/10/11
 * Description:
 * 1. 
 ***************************************************************************/
using UnityEngine;

namespace Kocmoca
{
    public class Ammo : ObjectRecycleSystem
    {
        protected Transform myTransform;
        protected Rigidbody myRigidbody;
        [Header("Basic")]
        public Transform target;
        public Kocmonaut owner;
        protected int shooter;
        // Physics Param
        protected Vector3 pointStarting;
        protected float projectileSpread;
        // Raycast
        protected RaycastHit[] raycastHits;
        protected RaycastHit raycastHit;
        protected int countRaycastHits;

        protected void InitializeAmmo()
        {
            myTransform = transform;
            myRigidbody = GetComponent<Rigidbody>();
            ResetAmmo();
        }

        protected void ResetAmmo()
        {
            target = null;
            myRigidbody.Sleep();
            pointStarting = Vector3.zero;
            timeRecovery = Time.time + 100;
            pointStarting = myTransform.position;
        }

        public virtual void InputAmmoData(int numberShooter, int numberTarget, Vector3 initialVelocity, float spread)
        {
            shooter = numberShooter;
            SatelliteCommander.Instance.listKocmonaut.TryGetValue(numberShooter, out owner);
            SatelliteCommander.Instance.listKocmocraft.TryGetValue(numberTarget, out target);
            myRigidbody.velocity = initialVelocity;
            projectileSpread = spread;
        }

        protected virtual void DetectCollisionByLinecast() { }
    }
}
