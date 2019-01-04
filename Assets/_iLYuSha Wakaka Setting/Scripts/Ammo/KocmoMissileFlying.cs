using UnityEngine;

namespace Kocmoca
{
    public class KocmoMissileFlying : Ammo
    {
        private bool targetIsLocalPlayer;
        private float realtimeThrust;
        private float initialMinSpeed;
        private float TargetLockDirection = 0.5f;
        public GameObject effect;
        public ObjectPoolData objPoolData;

        private void Awake()
        {
            InitializeAmmo();
            initialMinSpeed = KocmoMissileLauncher.flightVelocity;;
            objPoolData = ObjectPoolManager.Instance.CreatObjectPool(effect, 5,100);
        }

        private void OnEnable()
        {
            ResetAmmo();
            targetIsLocalPlayer = false;
            realtimeThrust = KocmoMissileLauncher.minThrust;
        }
        public override void InputAmmoData(int numberShooter, int numberTarget, Vector3 initialVelocity, float spread)
        {
            target = null;
            SatelliteCommander.Instance.listKocmonaut.TryGetValue(numberShooter, out owner);
            SatelliteCommander.Instance.listKocmocraft.TryGetValue(numberTarget, out target);
            myRigidbody.velocity = initialVelocity;
            projectileSpread = spread;
            if (target)
            {
                if (target.GetComponent<KocmocraftMechDroid>().Core == Core.LocalPlayer)
                {
                    targetIsLocalPlayer = true;
                    SatelliteCommander.Instance.MissileLockOnWarning(true, name);
                }
            }
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
            myRigidbody.velocity = myTransform.forward * realtimeThrust * Time.fixedDeltaTime;

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
                    float basicDamage = myRigidbody.velocity.magnitude * KocmoMissileLauncher.coefficientDamageBasic;
                    hull.Hit(new DamageInfo()
                    {
                        Attacker = owner,
                        Hull = (int)(basicDamage * KocmoMissileLauncher.coefficientDamageHull),
                        Shield = (int)(basicDamage * KocmoMissileLauncher.coefficientDamageShield)
                    });
                }
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