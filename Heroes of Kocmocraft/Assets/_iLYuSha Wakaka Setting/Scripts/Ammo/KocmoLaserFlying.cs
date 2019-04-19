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
        private int ammoVelocity;
        private int propulsion;
        private WaitForSeconds waitRecovery;
        private float hullPenetration;
        private float shieldPenetration;

        private Vector3 testOri;

        private void Awake()
        {
            InitializeAmmo();
            vfx = GetComponent<TrailRenderer>();
        }

        private void OnEnable()
        {
            StartCoroutine(FlyingInitialize());

        }
        public Type type = Type.Unknown;
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
            //ResetAmmo();
            //StartCoroutine(FlyingInitialize());
            //test = false;
            //if (type != Type.Unknown)
            //{
            //    if (owner.Type == Type.PaperAeroplane)
            //    {
            //        Debug.Log("Frame: " + Time.frameCount + " / " + owner.Name);
            //        test = true;
            //        lastType = owner.Name;
            //    }
            //}
            pointStarting = Vector3.zero;
            //timeRecovery = Time.time + 100;
            pointStarting = myTransform.position;

            // InputAmmoData
            shooter = numberShooter;
            SatelliteCommander.Instance.listKocmonaut.TryGetValue(numberShooter, out owner);
            nowOwner = owner.Name;

            SatelliteCommander.Instance.listKocmocraft.TryGetValue(numberTarget, out target);
            myRigidbody.velocity = initialVelocity;
            projectileSpread = spread;

            // Shoot
            vfx.startWidth = 0.5f;
            type = owner.Type;
            //if(test)
            //    Debug.Log("Frame: " + Time.frameCount + " / " + owner.Name + "/LAST/" + lastType);
            //switch (type)
            //{
            //    case Type.MinionArmor:
            //        ammoVelocity = KocmoUltraPowerPlasma.ammoVelocity;
            //        propulsion = KocmoUltraPowerPlasma.propulsion;
            //        waitRecovery = KocmoUltraPowerPlasma.waitRecovery;
            //        hullPenetration = KocmoUltraPowerPlasma.hullPenetration;
            //        shieldPenetration = KocmoUltraPowerPlasma.shieldPenetration;
            //        break;
            //    case Type.RedBullEnergy:
            //        ammoVelocity = KocmoUltraPowerPlasma.ammoVelocity;
            //        propulsion = KocmoUltraPowerPlasma.propulsion;
            //        waitRecovery = KocmoUltraPowerPlasma.waitRecovery;
            //        hullPenetration = KocmoUltraPowerPlasma.hullPenetration;
            //        shieldPenetration = KocmoUltraPowerPlasma.shieldPenetration;
            //        break;
            //    case Type.VladimirPutin:
            //        ammoVelocity = KocmoMegaRailgun.ammoVelocity;
            //        propulsion = KocmoMegaRailgun.propulsion;
            //        waitRecovery = KocmoMegaRailgun.waitRecovery;
            //        hullPenetration = KocmoMegaRailgun.hullPenetration;
            //        shieldPenetration = KocmoMegaRailgun.shieldPenetration;
            //        vfx.startWidth = 5;
            //        break;
            //    //case Type.PaperAeroplane:
            //    //    ammoVelocity = DevilTenderGazer.ammoVelocity;
            //    //    propulsion = DevilTenderGazer.propulsion;
            //    //    waitRecovery = DevilTenderGazer.waitRecovery;
            //    //    hullPenetration = DevilTenderGazer.hullPenetration;
            //    //    shieldPenetration = DevilTenderGazer.shieldPenetration;
            //    //    vfx.startWidth = 7;
            //    //    break;
            //    case Type.Cuckoo:
            //        ammoVelocity = DevilTenderGazer.ammoVelocity;
            //        propulsion = DevilTenderGazer.propulsion;
            //        waitRecovery = DevilTenderGazer.waitRecovery;
            //        hullPenetration = DevilTenderGazer.hullPenetration;
            //        shieldPenetration = DevilTenderGazer.shieldPenetration;
            //        vfx.startWidth = 7;
            //        break;
            //    case Type.BulletBill:
            //        ammoVelocity = KocmoUltraPowerPlasma.ammoVelocity;
            //        propulsion = KocmoUltraPowerPlasma.propulsion;
            //        waitRecovery = KocmoUltraPowerPlasma.waitRecovery;
            //        hullPenetration = KocmoUltraPowerPlasma.hullPenetration;
            //        shieldPenetration = KocmoUltraPowerPlasma.shieldPenetration;
            //        break;
            //    case Type.TimeMachine:
            //        ammoVelocity = KocmoHighspeedIonBlaster.ammoVelocity;
            //        propulsion = KocmoHighspeedIonBlaster.propulsion;
            //        waitRecovery = KocmoHighspeedIonBlaster.waitRecovery;
            //        hullPenetration = KocmoHighspeedIonBlaster.hullPenetration;
            //        shieldPenetration = KocmoHighspeedIonBlaster.shieldPenetration;
            //        break;
            //    case Type.AceKennel:
            //        ammoVelocity = KocmoUltraPowerPlasma.ammoVelocity;
            //        propulsion = KocmoUltraPowerPlasma.propulsion;
            //        waitRecovery = KocmoUltraPowerPlasma.waitRecovery;
            //        hullPenetration = KocmoUltraPowerPlasma.hullPenetration;
            //        shieldPenetration = KocmoUltraPowerPlasma.shieldPenetration;
            //        break;
            //    case Type.KirbyStar:
            //        ammoVelocity = KocmoUltraPowerPlasma.ammoVelocity;
            //        propulsion = KocmoUltraPowerPlasma.propulsion;
            //        waitRecovery = KocmoUltraPowerPlasma.waitRecovery;
            //        hullPenetration = KocmoUltraPowerPlasma.hullPenetration;
            //        shieldPenetration = KocmoUltraPowerPlasma.shieldPenetration;
            //        break;
            //    case Type.FastFoodMan:
            //        ammoVelocity = KocmoUltraPowerPlasma.ammoVelocity;
            //        propulsion = KocmoUltraPowerPlasma.propulsion;
            //        waitRecovery = KocmoUltraPowerPlasma.waitRecovery;
            //        hullPenetration = KocmoUltraPowerPlasma.hullPenetration;
            //        shieldPenetration = KocmoUltraPowerPlasma.shieldPenetration;
            //        break;
            //    case Type.nWidia:
            //        ammoVelocity = KocmoHighspeedIonBlaster.ammoVelocity;
            //        propulsion = KocmoHighspeedIonBlaster.propulsion;
            //        waitRecovery = KocmoHighspeedIonBlaster.waitRecovery;
            //        hullPenetration = KocmoHighspeedIonBlaster.hullPenetration;
            //        shieldPenetration = KocmoHighspeedIonBlaster.shieldPenetration;
            //        break;
            //    case Type.PolarisExpress:
            //        ammoVelocity = KocmoMegaRailgun.ammoVelocity;
            //        propulsion = KocmoMegaRailgun.propulsion;
            //        waitRecovery = KocmoMegaRailgun.waitRecovery;
            //        hullPenetration = KocmoMegaRailgun.hullPenetration;
            //        shieldPenetration = KocmoMegaRailgun.shieldPenetration;
            //        vfx.startWidth = 5;
            //        break;
            //    case Type.PapoyUnicorn:
            //        ammoVelocity = DevilTenderGazer.ammoVelocity;
            //        propulsion = DevilTenderGazer.propulsion;
            //        waitRecovery = DevilTenderGazer.waitRecovery;
            //        hullPenetration = DevilTenderGazer.hullPenetration;
            //        shieldPenetration = DevilTenderGazer.shieldPenetration;
            //        vfx.startWidth = 7;
            //        break;
            //    case Type.PumpkinGhost:
            //        ammoVelocity = KocmoMegaRailgun.ammoVelocity;
            //        propulsion = KocmoMegaRailgun.propulsion;
            //        waitRecovery = KocmoMegaRailgun.waitRecovery;
            //        hullPenetration = KocmoMegaRailgun.hullPenetration;
            //        shieldPenetration = KocmoMegaRailgun.shieldPenetration;
            //        vfx.startWidth = 5;
            //        break;
            //    case Type.GrandLisboa:
            //        ammoVelocity = KocmoHighspeedIonBlaster.ammoVelocity;
            //        propulsion = KocmoHighspeedIonBlaster.propulsion;
            //        waitRecovery = KocmoHighspeedIonBlaster.waitRecovery;
            //        hullPenetration = KocmoHighspeedIonBlaster.hullPenetration;
            //        shieldPenetration = KocmoHighspeedIonBlaster.shieldPenetration;
            //        break;
            //    default:
            //        ammoVelocity = KocmoLaserCannon.flightVelocity;
            //        propulsion = KocmoLaserCannon.propulsion;
            //        waitRecovery = KocmoLaserCannon.waitRecovery;
            //        hullPenetration = KocmoLaserCannon.coefficientMinDamage;
            //        shieldPenetration = KocmoLaserCannon.coefficientMaxDamage;
            //        break;
            //}
      gameObject.SetActive(true);
        }

        // 使用協程是因為在PRC呼叫Ammo生成之後，AddForce的力道大於一定值會產生向前的位移偏差
        // 來自FCS的賦值InputAmmoData()會在OnEnable()之後，必須放在等待時間之後，不然owner來不及更新
        IEnumerator FlyingInitialize()
        {
            yield return null;
            float coefficient = WeaponData.GetCoefficient(type);
            if (target)
            {
                targetRigid = target.GetComponent<Rigidbody>();
                float expectedTime = Mathf.Sqrt(Vector3.SqrMagnitude(target.position - myTransform.position) / (coefficient * coefficient * ammoVelocity * ammoVelocity - targetRigid.velocity.sqrMagnitude));

                Vector3 expectedTargetPosition = target.position + targetRigid.velocity * expectedTime;
                Vector3 expectedTargetDirection = (expectedTargetPosition - myTransform.position).normalized;
                myTransform.forward = expectedTargetDirection;
            }
            myTransform.localRotation *= Quaternion.Euler(0, projectileSpread, 0);
            testOri = myTransform.position;
            myRigidbody.AddForce(myTransform.forward * (propulsion * coefficient));
            vfx.enabled = true;

            yield return waitRecovery;
            if (type == Type.Cuckoo || type == Type.PapoyUnicorn)
                Debug.Log(owner.Name + " / MISS");
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
                    float basicDamage = myRigidbody.velocity.sqrMagnitude * KocmoCannon.fixDamage;
                    hull.Hit(new DamagePower()
                    {
                        Attacker = owner,
                        Hull = basicDamage * hullPenetration,
                        Shield = basicDamage * shieldPenetration
                    });
                    if (type == Type.Cuckoo || type == Type.PapoyUnicorn)
                    {
                        Debug.LogWarning(owner.Name + " / " + (int)(basicDamage * hullPenetration) + " / " + (int)(basicDamage * shieldPenetration));
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

        // Raycast 已棄用
        protected override void CollisionDetection()
        {
            raycastHits = Physics.RaycastAll(pointStarting, myTransform.forward, Vector3.Distance(myTransform.position, pointStarting));
            if (raycastHits.Length > 0)
            {
                KocmocraftMechDroid hull = raycastHits[0].transform.GetComponent<KocmocraftMechDroid>();
                if (hull)
                {
                    float basicDamage = myRigidbody.velocity.sqrMagnitude * 0.000066f;
                    hull.Hit(new DamagePower()
                    {
                        Attacker = owner,
                        Hull = (int)(basicDamage * hullPenetration),
                        Shield = (int)(basicDamage * shieldPenetration)
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