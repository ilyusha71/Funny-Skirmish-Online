using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AirSupremacy
{
    public class Ammo : ObjectRecycleSystem
    {
        public DamageType damageType;
        [Header("Basic")]
        public Transform target;
        public Pilot pilot;
        protected Transform myTransform;
        protected Rigidbody myRigidbody;
        public float maxFlyingTime;
        [Header("Raycast")]
        protected Vector3 pointStarting;
        protected float distanceRay;
        protected RaycastHit[] raycastHits;
        protected int countRaycastHits;
        //[Header("Damage")]
        ////public int ammoDamage;

        protected void InitializeAmmo()
        {
            myTransform = transform;
            myRigidbody = GetComponent<Rigidbody>();
            ResetAmmo();
        }
        protected void ResetAmmo()
        {
            pointStarting = Vector3.zero;
            timeRecovery = Time.time + maxFlyingTime;
            try { GetComponent<TrailRenderer>().enabled = false; } catch { }
        }
        // Rocket & Missile
        public virtual void LoadAmmo(Pilot shooter, Transform targetLocked, Vector3 direction, Vector3 aircraftVelocity)
        {
            pilot = shooter;
            target = targetLocked;
            myTransform.forward = direction;
            myRigidbody.velocity = aircraftVelocity;
        }
        // Gun & Cannon
        public virtual void LoadAmmo(Pilot shooter, Transform targetLocked, DamageType damageType, Vector3 direction, float spread, Vector3 aircraftVelocity, float propulsion, float flightTime)
        {
            pilot = shooter;
            target = targetLocked;
            this.damageType = damageType;
            myTransform.forward = direction;
            myTransform.localRotation *= Quaternion.Euler(0, Random.Range(-spread, spread), 0);
            myRigidbody.velocity = aircraftVelocity;
            myRigidbody.AddForce(myTransform.forward * propulsion);
            timeRecovery = Time.time + flightTime;
            GetComponent<TrailRenderer>().enabled = true;
        }
        protected virtual void CollisionDetection()
        {
            raycastHits = Physics.RaycastAll(pointStarting, transform.forward, Vector3.Distance(myTransform.position, pointStarting));
            countRaycastHits = raycastHits.Length;
            for (int i = 0; i < countRaycastHits; i++)
            {
                if (pointStarting != Vector3.zero &&
                    raycastHits[i].transform.name != pilot.AircraftName &&
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
