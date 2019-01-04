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
        public GameObject effect;
        public ObjectPoolData objPoolData;
        private TrailRenderer vfx;

        private void Awake()
        {
            InitializeAmmo();
            objPoolData = ObjectPoolManager.Instance.CreatObjectPool(effect, 40, 800);
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
            yield return new WaitForSeconds(0.0001f);
            float coefficient = WeaponData.GetCoefficient(owner.Type); 
            if (target)
            {
                float nowDistance = Vector3.Distance(target.transform.position, myTransform.position);
                float expectedTime = Mathf.Sqrt(nowDistance * nowDistance / (coefficient * coefficient * KocmoLaserCannon.flightVelocity * KocmoLaserCannon.flightVelocity - target.GetComponent<Rigidbody>().velocity.sqrMagnitude));

                Vector3 expectedTargetPosition =
                    target.transform.position +
                    target.GetComponent<Rigidbody>().velocity * expectedTime;
                Vector3 expectedTargetDirection = (expectedTargetPosition - myTransform.position).normalized;
                myTransform.forward = expectedTargetDirection;
            }
            myTransform.localRotation *= Quaternion.Euler(0, projectileSpread, 0);
            timeRecovery = Time.time + KocmoLaserCannon.flightTime;
            myRigidbody.AddForce(myTransform.forward * KocmoLaserCannon.propulsion* coefficient);
            vfx.enabled = true;
        }

        void Update()
        {
            if (Time.time > timeRecovery)
                Recycle(gameObject);
        }

        private void FixedUpdate()
        {
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
                    float basicDamage = myRigidbody.velocity.sqrMagnitude * 0.000066f; ;
                    hull.Hit(new DamageInfo()
                    {
                        Attacker = owner,
                        Hull = (int)(basicDamage * KocmoLaserCannon.coefficientMinDamage),
                        Shield = (int)(basicDamage * KocmoLaserCannon.coefficientMaxDamage)
                    });
                }
                Recycle(gameObject);
                return;
            }
            pointStarting = myTransform.position;
        }
    }
}