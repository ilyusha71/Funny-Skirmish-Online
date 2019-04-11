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

        private void Awake()
        {
            InitializeAmmo();
            vfx = GetComponent<TrailRenderer>();
        }

        private void OnEnable()
        {
            vfx.enabled = false;
            ResetAmmo();
            StartCoroutine(FlyingInitialize());
        }
        // 使用協程是因為在PRC呼叫Ammo生成之後，AddForce的力道大於一定值會產生向前的位移偏差
        // 來自FCS的賦值InputAmmoData()會在OnEnable()之後，必須放在等待時間之後，不然owner來不及更新
        IEnumerator FlyingInitialize()
        {
            yield return SatelliteCommander.waitShoot;
            float coefficient = WeaponData.GetCoefficient(owner.Type);
            if (target)
            {
                targetRigid = target.GetComponent<Rigidbody>();
                float expectedTime = Mathf.Sqrt(Vector3.SqrMagnitude(target.position - myTransform.position) / (coefficient * coefficient * KocmoLaserCannon.flightVelocity * KocmoLaserCannon.flightVelocity - targetRigid.velocity.sqrMagnitude));

                Vector3 expectedTargetPosition = target.position + targetRigid.velocity * expectedTime;
                Vector3 expectedTargetDirection = (expectedTargetPosition - myTransform.position).normalized;
                myTransform.forward = expectedTargetDirection;
            }
            myTransform.localRotation *= Quaternion.Euler(0, projectileSpread, 0);
            myRigidbody.AddForce(myTransform.forward * (KocmoLaserCannon.propulsion * coefficient));
            vfx.enabled = true;

            yield return SatelliteCommander.waitLaserRecovery;
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
                KocmocraftMechDroid hull = raycastHit.transform.GetComponent<KocmocraftMechDroid>();
                if (hull)
                {
                    float basicDamage = myRigidbody.velocity.sqrMagnitude * 0.000066f;
                    hull.Hit(new DamageInfo()
                    {
                        Attacker = owner,
                        Hull = (int)(basicDamage * KocmoLaserCannon.coefficientMinDamage),
                        Shield = (int)(basicDamage * KocmoLaserCannon.coefficientMaxDamage)
                    });
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

        protected override void CollisionDetection()
        {
            raycastHits = Physics.RaycastAll(pointStarting, myTransform.forward, Vector3.Distance(myTransform.position, pointStarting));
            if (raycastHits.Length > 0)
            {
                KocmocraftMechDroid hull = raycastHits[0].transform.GetComponent<KocmocraftMechDroid>();
                if (hull)
                {
                    float basicDamage = myRigidbody.velocity.sqrMagnitude * 0.000066f;
                    hull.Hit(new DamageInfo()
                    {
                        Attacker = owner,
                        Hull = (int)(basicDamage * KocmoLaserCannon.coefficientMinDamage),
                        Shield = (int)(basicDamage * KocmoLaserCannon.coefficientMaxDamage)
                    });
                    ResourceManager.hitSpark.Reuse(raycastHits[0].point, Quaternion.identity);
                }
                else
                {
                    switch (raycastHits[0].transform.tag)
                    {
                        case "Water": ResourceManager.hitWater.Reuse(raycastHits[0].point, Quaternion.identity); break;
                        default: ResourceManager.hitGround.Reuse(raycastHits[0].point, Quaternion.identity); break;
                    }
                }
                Recycle(gameObject);
                return;
            }
            pointStarting = myTransform.position;
        }
    }
}