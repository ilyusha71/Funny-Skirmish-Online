using UnityEngine;

namespace Kocmoca
{
    public class KocmoMissileFlying : Ammo
    {
        private bool targetIsLocalPlayer;
        private float realtimeThrust;
        private float initialMinSpeed;
        private float TargetLockDirection = 0.5f;

        private void Awake()
        {
            InitializeAmmo();
            initialMinSpeed = KocmoMissileLauncher.flightVelocity;;
        }

        private void OnEnable()
        {
            ResetAmmo();
            targetIsLocalPlayer = false;
            realtimeThrust = KocmoMissileLauncher.minThrust;
        }
        public override void InputAmmoData(int numberShooter, int numberTarget, Vector3 initialVelocity, float spread)
        {
            shooter = numberShooter;
            target = null;
            SatelliteCommander.Instance.listKocmonaut.TryGetValue(numberShooter, out owner);
            SatelliteCommander.Instance.listKocmocraft.TryGetValue(numberTarget, out target);
            myRigidbody.velocity = initialVelocity;
            projectileSpread = spread;
            if (target)
            {
                if (target.GetComponent<AvionicsSystem>().controlUnit == ControlUnit.LocalPlayer)
                {
                    targetIsLocalPlayer = true;
                    SatelliteCommander.Instance.MissileLockOnWarning(true, name);
                }
            }
            timeRecovery = Time.time + KocmoMissileLauncher.FlightTime;
        }
        void Update()
        {
            if (Time.time > timeRecovery)
                Recycle(gameObject);

            if (target)
            {
                float realtimeSpeed = myRigidbody.velocity.magnitude;
                if (realtimeSpeed < initialMinSpeed) return;
                float expectedDistance = Vector3.Distance(target.transform.position, myTransform.position);
                float expectedHitTime = expectedDistance / realtimeSpeed;
                Vector3 expectedTargetPos = target.transform.position + target.GetComponent<Rigidbody>().velocity * expectedHitTime;
                Vector3 dir = (expectedTargetPos - myTransform.position).normalized;
                float direction = Vector3.Dot(dir, myTransform.forward);
                float lockLimit = TargetLockDirection;

                if (expectedHitTime < 0.15f)
                    lockLimit = 0.996f;
                if (direction < lockLimit || expectedDistance < 30)
                {
                    target = null;
                    if (targetIsLocalPlayer)
                        SatelliteCommander.Instance.MissileLockOnWarning(false, name);
                }

                if (target)
                {
                    Quaternion rotation = Quaternion.LookRotation(dir);
                    myTransform.rotation = Quaternion.Slerp(myTransform.rotation, rotation, Time.deltaTime * 30);
                }
            }
            else
            {
                if (targetIsLocalPlayer)
                    SatelliteCommander.Instance.MissileLockOnWarning(false, name);
            }
        }

        private void FixedUpdate()
        {
            if (realtimeThrust < KocmoMissileLauncher.maxThrust)
                realtimeThrust += KocmoMissileLauncher.acceleration * Time.fixedDeltaTime;
            myRigidbody.velocity = myTransform.forward * (realtimeThrust * Time.fixedDeltaTime);

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
                    float basicDamage = myRigidbody.velocity.magnitude * KocmoMissileLauncher.coefficientDamageBasic;
                    hull.Hit(new DamagePower()
                    {
                        Attacker = owner,
                        Hull = (int)(basicDamage * KocmoMissileLauncher.coefficientDamageHull),
                        Shield = (int)(basicDamage * KocmoMissileLauncher.coefficientDamageShield)
                    });
                }
                ResourceManager.hitFire.Reuse(raycastHit.point, Quaternion.identity);
                Recycle(gameObject);
                return;
            }
            pointStarting = myTransform.position;
        }

        private void OnDisable()
        {
            if (SatelliteCommander.Instance)
                SatelliteCommander.Instance.MissileLockOnWarning(false, name);
        }
    }
}