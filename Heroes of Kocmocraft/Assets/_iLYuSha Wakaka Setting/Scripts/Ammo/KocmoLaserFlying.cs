/***************************************************************************
 * KocmoLaser Flying
 * 卡斯摩激光飛行
 * Last Updated: 2018/10/11
 * Description:
 * 1. 繼承Ammo彈藥腳本，包含初始化與物件池管理
 * 2. 在PUN生成實例後，myRigidbody.AddForce在OnEnable執行會出現激光生成位置過前的問題
 *     透過Flying Initialize飛行初始化協程進行延後觸發
 ***************************************************************************/
using System.Collections;
using UnityEngine;

namespace Kocmoca
{
    public class KocmoLaserFlying : Ammo
    {
        private TrailRenderer vfx;

        // Target param.
        private Rigidbody targetRigid;

        // Ammo Parameter
        //private int ammoVelocity;
        //private int propulsion;
        //private WaitForSeconds waitRecovery;
        //private float hullPenetration;
        //private float shieldPenetration;

        private Vector3 testOri;

        ModuleData moduleData;
        private void Awake()
        {
            InitializeAmmo();
            vfx = GetComponent<TrailRenderer>();
        }


        public Type type;
        public bool test;
        public string lastOwner;
        public string nowOwner;
        public override void InputAmmoData(int numberShooter, int numberTarget, Vector3 initialVelocity, float spread)
        {
            lastOwner = owner.Name;
            // ResetAmmo
            vfx.enabled = false;
            target = null;
            myRigidbody.Sleep();

            pointStarting = Vector3.zero;
            //timeRecovery = Time.time + 100;
            pointStarting = myTransform.position;

            // InputAmmoData
            shooter = numberShooter;
            SatelliteCommander.Instance.listKocmonaut.TryGetValue(numberShooter, out owner);
            moduleData = KocmocaData.KocmocraftData[(int)owner.Type];
            vfx.startWidth = moduleData.RaySize;
            nowOwner = owner.Name;

            SatelliteCommander.Instance.listKocmocraft.TryGetValue(numberTarget, out target);
            myRigidbody.velocity = initialVelocity;
            projectileSpread = spread;
            type = owner.Type;
            gameObject.SetActive(true);
            //Flying();
        }
        private void OnEnable()
        {
            StartCoroutine(FlyingInitialize());
        }
        // 使用協程是因為在PRC呼叫Ammo生成之後，AddForce的力道大於一定值會產生向前的位移偏差
        // 來自FCS的賦值InputAmmoData()會在OnEnable()之後，必須放在等待時間之後，不然owner來不及更新
        IEnumerator FlyingInitialize()
        {
            yield return null;
            if (target)
            {
                targetRigid = target.GetComponent<Rigidbody>();
                float expectedTime = Mathf.Sqrt(Vector3.SqrMagnitude(target.position - myTransform.position) / (moduleData.AmmoVelocity * moduleData.AmmoVelocity - targetRigid.velocity.sqrMagnitude));

                Vector3 expectedTargetPosition = target.position + targetRigid.velocity * expectedTime;
                Vector3 expectedTargetDirection = (expectedTargetPosition - myTransform.position).normalized;
                myTransform.forward = expectedTargetDirection;
            }
            myTransform.localRotation *= Quaternion.Euler(0, projectileSpread, 0);
            testOri = myTransform.position;
            myRigidbody.AddForce(myTransform.forward * moduleData.propulsion);
            vfx.enabled = true;

            yield return moduleData.waitRecovery;
            //if (type == Type.Cuckoo || type == Type.PapoyUnicorn)
            //    Debug.Log(owner.Name + " / MISS");
            Recycle(gameObject);
        }

        void Flying()
        {
            if (target)
            {
                targetRigid = target.GetComponent<Rigidbody>();
                float expectedTime = Mathf.Sqrt(Vector3.SqrMagnitude(target.position - myTransform.position) / (moduleData.AmmoVelocity * moduleData.AmmoVelocity - targetRigid.velocity.sqrMagnitude));

                Vector3 expectedTargetPosition = target.position + targetRigid.velocity * expectedTime;
                Vector3 expectedTargetDirection = (expectedTargetPosition - myTransform.position).normalized;
                myTransform.forward = expectedTargetDirection;
            }
            myTransform.localRotation *= Quaternion.Euler(0, projectileSpread, 0);
            testOri = myTransform.position;
            myRigidbody.AddForce(myTransform.forward * moduleData.propulsion);
            vfx.enabled = true;

            StartCoroutine(Re());
        }




        IEnumerator Re()
        {
            yield return moduleData.waitRecovery;
            //if (type == Type.Cuckoo || type == Type.PapoyUnicorn)
            //    Debug.Log(owner.Name + " / MISS");
            Recycle(gameObject);
        }


        private void FixedUpdate()
        {
            DetectCollisionByLinecast();
        }

        protected override void DetectCollisionByLinecast()
        {
            if (Physics.Linecast(pointStarting, myTransform.position, out raycastHit, ~(1 << 10)))
            {
                AvionicsSystem hull = raycastHit.transform.GetComponent<AvionicsSystem>();
                if (hull)
                {
                    float basicDamage = myRigidbody.velocity.sqrMagnitude * WeaponData.fixDamage;
                    hull.Hit(new DamagePower()
                    {
                        Attacker = owner,
                        Hull = (int)(basicDamage),
                        Shield = (int)(basicDamage),
                        Absorb = basicDamage * (100 - moduleData.ShieldPenetration) * 0.01f,
                        Penetration = basicDamage * moduleData.ShieldPenetration * 0.01f,
                        Damage = Random.Range(0, 100) < moduleData.HullPenetration ? basicDamage * 3 : basicDamage
                    });
                    if (type == Type.Cuckoo || type == Type.PapoyUnicorn || type == Type.VladimirPutin || type == Type.PumpkinGhost)
                    {
                        //Debug.LogWarning(owner.Name + " / " + (int)(basicDamage * moduleData.PenetrationHull) + " / " + (int)(basicDamage * moduleData.PenetrationShield));
                        ResourceManager.hitFire.Reuse(raycastHit.point, Quaternion.identity);
                    }
                    else
                        ResourceManager.hitSpark.Reuse(raycastHit.point, Quaternion.identity);
                }
                else
                {
                    switch (raycastHit.transform.tag)
                    {
                        case "Water": ResourceManager.hitWater.Reuse(raycastHit.point, Quaternion.identity); break;
                        default: ResourceManager.hitGround.Reuse(raycastHit.point, Quaternion.identity); break;
                    }
                }
                Recycle(gameObject);
                return;
            }
            pointStarting = myTransform.position;
        }
    }
}