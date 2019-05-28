/***************************************************************************
 * Kocmocraft Manager
 * 宇航機管理員
 * Last Updated: 2019/05/18
 * 
 * v19.0518
 * 1. 原KocmocraftManager已升級為Kocmoport，主要用於Photon網路連線控制
 * 2. 主要負責宇航機於機庫時的基本設定，以及相關模組的初始化
 ***************************************************************************/
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kocmoca
{
    public class KocmocraftManager : MonoBehaviour
    {
        public bool active;
        [Header("Preset")]
        public Shield shield;
        public Hull hull;
        public Speed speed;
        public Kocmomech kocmomech;
        public Rigidbody myRigidbody;
        public GameObject myWreckage;
        public Transform myCockpitViewpoint;
        public float realtimeShield, realtimeHull, realSpeed;
        public float shieldPercent { get { return realtimeShield / shield.maximum; } }
        public float hullPercent { get { return realtimeHull / hull.maximum; } }
        public float speedPercent { get { return realSpeed / speed.maximum; } }

        [Header("Mech")]
        private PhotonView myPhotonView;
        public Core Core { get; protected set; }
        public int Number { get; protected set; }
        private int damage;
        [Header("Crash Info")]
        public Dictionary<int, int> listAttacker = new Dictionary<int, int>(); // 損傷記錄索引
        private Kocmonaut lastAttacker = new Kocmonaut { Number = -1 };
        private Vector3 lastHitVelocity;

        Camera followCam;

        //private int type;
        private void Reset()
        {
            int type = int.Parse(name.Split(new char[2] { '(', ')' })[1]);
            KocmocraftDatabase index = UnityEditor.AssetDatabase.LoadAssetAtPath<KocmocraftDatabase>("Assets/_iLYuSha Wakaka Setting/ScriptableObject/Kocmocraft Database.asset");
            shield = index.kocmocraft[type].shield;
            realtimeShield = shield.maximum;
            hull = index.kocmocraft[type].hull;
            realtimeHull = hull.maximum;
            speed = index.kocmocraft[type].speed;
            realSpeed = speed.engine;
            kocmomech = index.kocmocraft[type].kocmomech;

            myRigidbody = GetComponent<Rigidbody>();
            myRigidbody.mass = 100;
            myRigidbody.angularDrag = 0;
            myRigidbody.useGravity = false;
            myRigidbody.isKinematic = true;
            myCockpitViewpoint = transform.Find("Cockpit Viewpoint");
            myWreckage = GetComponentInChildren<Prototype>().gameObject;

            FireControlSystem[] fcs = GetComponentsInChildren<FireControlSystem>();
            for (int i = 0; i < fcs.Length; i++)
            {
                fcs[i].Preset(type);
            }

            GetComponentInChildren<EngineController>().Preset(index.kocmocraft[type].engine);

            enabled = false;
        }

        public void Initialize(KocmocraftModule module, Core core, int type, int number, GameObject pilot, GameObject wreckage)
        {
            // Dependent Components
            myPhotonView = transform.root.GetComponent<PhotonView>();
            myPhotonView.ObservedComponents.Add(this);
            // Kocmonaut Data
            Core = core;
            Number = number;
            if (core == Core.LocalPlayer)
                SatelliteCommander.Instance.Observer.InitializeView(myCockpitViewpoint, pilot, Number);
            else
                SatelliteCommander.Instance.Observer.listOthers.Add(Number);






            if (Core == Core.RemotePlayer || Core == Core.RemoteBot) return;

            myEngine = GetComponentInChildren<EngineController>();
            // Modular Parameter
            //dataEnergy = new Data { Max = KocmocraftData.Energy[type], Value = KocmocraftData.Energy[type] };
            dataSpeed = new Data { Max = KocmocraftData.AfterburnerSpeed[type], Value = 0 };
            valueSpeedCruise = KocmocraftData.CruiseSpeed[type];
            valueSpeedHigh = valueSpeedCruise * 1.1f;

            followCam = Camera.main;
            myRigidbody.isKinematic = false;
            mainRot = transform.rotation;
            enabled = true;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(realtimeShield);
                stream.SendNext(realtimeHull);
            }
            else
            {
                realtimeShield = (float)stream.ReceiveNext();
                realtimeHull = (float)stream.ReceiveNext();
            }
        }
        public void Hit(DamagePower damagePower)
        {
            if (Core == Core.RemotePlayer || Core == Core.RemoteBot) return;
            if (realtimeHull <= 0) return;
            int damageSourceNumber = damagePower.Attacker.Number;
            // 狀況1
            // 第一次造成傷害來源：自己 123
            // 最後造成傷害來源：自己 123
            // 擊落者：自己墜機 123
            // 狀況2
            // 第一次造成傷害來源：自己 123
            // 最後造成傷害來源：敵人 789
            // 擊落者：敵人 789
            // 狀況3
            // 第一次造成傷害來源：敵人 789
            // 最後造成傷害來源：自己 123
            // 擊落者：敵人

            // 第一次受到損傷
            if (lastAttacker.Number == -1)
                lastAttacker = damagePower.Attacker;
            else
            {
                if (damagePower.Attacker.Number != Number)
                    lastAttacker = damagePower.Attacker;
            }



            /*****************************************************************************
             * 傷害計算方法
             * Last Updated: 2019-04-18
             * 
             * 護盾值 realtimeShield
             * 機甲值 realtimeHull
             * 護盾傷害威力值 damagePower.Shield
             * 機甲傷害威力值 damagePower.Hull
             * 穿透係數 coefficientPenetration
             * 護盾傷害值 damageShield
             * 機甲傷害值 damageHull
             * 總傷害值 damageTotal
             * 
             * 1. 計算護盾傷害與穿透係數
             *      剩餘護盾值 = 原始護盾值 - 護盾傷害威力值
             *      護盾值足夠
             *          => 護盾傷害值 = 護盾傷害威力值
             *          => 護盾穿透係數 = 0
             *      護盾值不足
             *          => 護盾傷害值 = 原始護盾值 = 剩餘護盾值 + 護盾傷害威力值
             *          => 護盾穿透係數 = -1 * 剩餘護盾值 / 護盾傷害威力值
             *          => 剩餘護盾值 = 0
             *          
             * 2. 計算機甲傷害
             *      機甲傷害值 = 機甲傷害威力值 * 穿透係數
             *      剩餘機甲值 = 原始機甲值 - 機甲傷害值
             *      
             *****************************************************************************/
            //if (damagePower.Shield == -999)
            //    coefficientPenetration = 1;
            //else
            //{
            //    realtimeShield -= damagePower.Shield;
            //    if (realtimeShield >= 0)
            //    {
            //        damageShield = damagePower.Shield;
            //        coefficientPenetration = 0;
            //    }
            //    else
            //    {
            //        damageShield = realtimeShield + damagePower.Shield;
            //        coefficientPenetration = -realtimeShield / damagePower.Shield;
            //        realtimeShield = 0;
            //    }
            //}
            //damageHull = damagePower.Hull * coefficientPenetration;
            //realtimeHull = Mathf.Clamp(realtimeHull - damageHull, 0, dataHull.Max);

            /*****************************************************************************
             * 傷害計算方法
             * Last Updated: 2019-04-18
             * 
             * 護盾值 realtimeShield
             * 機甲值 realtimeHull
             * 護盾傷害威力值 damagePower.Shield
             * 機甲傷害威力值 damagePower.Hull
             * 穿透係數 coefficientPenetration
             * 護盾傷害值 damageShield
             * 機甲傷害值 damageHull
             * 總傷害值 damageTotal
             * 
             * 吸收伤害值 Absorb
             * 穿透伤害值 Penetration
             * 
             * 1. 計算護盾傷害與穿透係數
             *      剩餘護盾值 = 原始護盾值 - 吸收伤害值
             *      護盾值足夠
             *          => 剩餘機甲值 = 原始機甲值 - 穿透伤害值
             *      護盾值不足
             *          => 剩餘機甲值 = 原始機甲值 + 剩餘護盾值（负的不足护盾值） - 穿透伤害值
             *          => 剩餘護盾值 = 0
             *          
             * 2. 計算機甲傷害
             *      機甲傷害值 = 機甲傷害威力值 * 穿透係數
             *      剩餘機甲值 = 原始機甲值 - 機甲傷害值
             *      
             *****************************************************************************/
            if (realtimeShield > 0)
            {
                realtimeShield -= damagePower.Absorb;
                if (realtimeShield >= 0)
                    realtimeHull = Mathf.Clamp(realtimeHull - damagePower.Penetration, 0, hull.maximum);
                else
                {
                    realtimeHull = Mathf.Clamp(realtimeHull + realtimeShield - damagePower.Penetration, 0, hull.maximum);
                    realtimeShield = 0;
                }
                damage = (int)(damagePower.Absorb + damagePower.Penetration);
            }
            else
            {
                realtimeHull = Mathf.Clamp(realtimeHull - damagePower.Damage, 0, hull.maximum);
                damage = (int)damagePower.Damage;
            }
            /*****************************************************************************
             * 損傷記錄
             *****************************************************************************/
            //damage = (int)(damageShield + damageHull);
            if (listAttacker.ContainsKey(damageSourceNumber))
                listAttacker[damageSourceNumber] += damage;
            else
                listAttacker.Add(damagePower.Attacker.Number, damage);

            // 本地玩家擊中本地AI
            if (damagePower.Attacker.Core == Core.LocalPlayer)
                HeadUpDisplayManager.Instance.ShowHitDamage(myRigidbody.position, damage);
            // 遠端玩家擊中本地玩家或本地AI
            else if (damagePower.Attacker.Core == Core.RemotePlayer)
                myPhotonView.RPC("ShowHitDamage", RpcTarget.AllViaServer, damagePower.Attacker.Number, damage);

            /************************************* Crash *************************************/

            if (realtimeHull <= 0)
            {
                if (Core == Core.LocalPlayer)
                {
                    transform.root.GetComponent<LocalPlayerController>().enabled = false;
                    SatelliteCommander.Instance.PlayerCrash();
                    //foreach (int key in listAttacker.Keys)
                    //{
                    //    HeadUpDisplayManager.Instance.ShowWhoAttackU(key, listAttacker[key]);
                    //}
                    HeadUpDisplayManager.Instance.ShowKillStealer(lastAttacker.Number);
                }
                else if (Core == Core.LocalBot)
                {
                  transform.root.  GetComponent<LocalBotController>().enabled = false;
                    SatelliteCommander.Instance.BotCrash(Number);
                }
                myPhotonView.RPC("Crash", RpcTarget.AllViaServer, lastAttacker.Number);
            }
        }

        [PunRPC]
        public void ShowHitDamage(int numberShooter, int damage, PhotonMessageInfo info)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == numberShooter)
                HeadUpDisplayManager.Instance.ShowHitDamage(myRigidbody.position, damage);
        }

        //[PunRPC]
        public void Crash(int stealerNumber, PhotonMessageInfo info)
        {
            // 若宇航機是本地玩家擊落，顯示擊落訊息
            if (stealerNumber == PhotonNetwork.LocalPlayer.ActorNumber)
                HeadUpDisplayManager.Instance.ShowDestroyed(Number);

            SatelliteCommander.Instance.Observer.listOthers.Remove(Number);
            SatelliteCommander.Instance.RemoveFlight(Number);
            switch (Core)
            {
                case Core.LocalPlayer:
                    LocalPlayerRealtimeData.Status = FlyingStatus.Crash;
                    SatelliteCommander.Instance.Observer.TransferCamera();
                    SatelliteCommander.Instance.ClearData();
                    HeadUpDisplayManager.Instance.ClearData();
                    Destroy(transform.root.GetComponent<LocalPlayerController>());
                    transform.root.GetComponentInChildren<OnboardRadar>().Stop();
                    //Destroy(GetComponent<OnboardRadar>());
                    break;
                case Core.LocalBot:
                    Destroy(transform.root.GetComponent<LocalBotController>());
                    transform.root.GetComponentInChildren<OnboardRadar>().Stop();
                    //Destroy(GetComponent<OnboardRadar>());
                    break;
            }
            ShowRemnant();
            if (myPhotonView.IsMine)
                PhotonNetwork.Destroy(transform.root.gameObject); // 改变PhotoView Fixed
            //StartCoroutine(Crash());
        }
        IEnumerator Crash()
        {
            yield return new WaitForSeconds(0.01f);
            //ShowRemnant();
            //if(myPhotonView.IsMine)
            //    PhotonNetwork.Destroy(this.gameObject);
        }
        private void ShowRemnant()
        {
            if (myRigidbody.velocity == Vector3.zero) // 撞击可能触发修正而变成0向量
                myRigidbody.velocity = lastHitVelocity;
            myWreckage.transform.SetParent(null);
            myWreckage.tag = "Untagged";

            ResourceManager.hitDown.Reuse(myRigidbody.position, Quaternion.identity);

            Rigidbody rigid = myWreckage.AddComponent<Rigidbody>();
            rigid.mass = 100;
            rigid.velocity = myRigidbody.velocity;
            rigid.AddForce(Random.rotation.eulerAngles * Random.Range(100, 500));
            rigid.AddTorque(Random.rotation.eulerAngles * Random.Range(100, 2000));
            Destroy(myWreckage, 15.0f);
        }

        private void OnCollisionEnter(Collision collision)
        {
            // 消除撞擊造成的力矩
            myRigidbody.isKinematic = true;
            myRigidbody.isKinematic = false;

            if (Core == Core.RemotePlayer || Core == Core.RemoteBot) return;
            if (realtimeHull <= 0) return;
            lastHitVelocity = myRigidbody.velocity;

            // 計算傷害
            DamagePower damagePower = new DamagePower();
            if (collision.gameObject.CompareTag("Untagged") || collision.gameObject.CompareTag("Water"))
            {
                damagePower.Penetration = 50000;
                // 下次更新要做的
                // 入射角越大伤害越大，避免卡住
                //Vector3 inDirection = -collision.relativeVelocity;
                //Vector3 normal = collision.contacts[0].normal;
                //Vector3 outDirection = Vector3.Reflect(inDirection, normal);
                //transform.forward = outDirection;
                //GetComponent<AvionicsSystem>().mainRot = transform.rotation;
                //myRigidbody.AddForce(outDirection * collision.impulse.magnitude);
            }
            else
                damagePower.Penetration = Mathf.Clamp((int)Mathf.Abs(collision.impulse.magnitude), 0, 5000);
            //damagePower.Shield = -999;

            // 記錄攻擊者
            damagePower.Attacker = new Kocmonaut { Number = -1 };
            KocmocraftManager collisionKocmocraft = collision.gameObject.GetComponent<KocmocraftManager>();
            if (collisionKocmocraft)
            {
                damagePower.Attacker.Core = collisionKocmocraft.Core;
                damagePower.Attacker.Number = collisionKocmocraft.Number;
            }
            else
            {
                damagePower.Attacker.Core = Core;
                damagePower.Attacker.Number = Number;
            }
            Hit(damagePower);
        }

        private void OnCollisionExit(Collision collision)
        {
            // 確保撞擊後恢復一般剛體
            myRigidbody.isKinematic = true;
            myRigidbody.isKinematic = false;
        }








        [Header("Calculate")]
        public Vector3 mousePos;





        [Header("Dependent Components")]
        private EngineController myEngine;
        [Header("Modular Parameter")]
        //public Data dataEnergy;
        public Data dataSpeed;
        private float valueSpeedCruise;
        private float valueSpeedHigh;
        private bool isCharge;

        [Header("Constant")]
        public float RotationSpeed = 50.0f;// Turn Speed

        public float DampingTarget = 10.0f;// rotation speed to facing to a target
        public bool AutoPilot = false;// if True this plane will follow a target automatically
        [HideInInspector]
        public bool FollowTarget = false;
        [HideInInspector]
        public Vector3 PositionTarget = Vector3.zero;// current target position
        [HideInInspector]
        private Vector3 positionTarget = Vector3.zero;
        public Quaternion mainRot = Quaternion.identity;

        void FixedUpdate()
        {
            if (Core == Core.RemotePlayer || Core == Core.RemoteBot) return;
            if (!myRigidbody)
                return;
            if (AutoPilot)
            {
                if (myRigidbody.angularVelocity.magnitude > 3)
                    myRigidbody.Sleep();

                // if auto pilot
                if (FollowTarget)
                {
                    // rotation facing to the positionTarget
                    positionTarget = Vector3.Lerp(positionTarget, PositionTarget, Time.fixedDeltaTime * DampingTarget);
                    Vector3 relativePoint = this.transform.InverseTransformPoint(positionTarget).normalized; // 计算相对位置的单位向量
                    mainRot = Quaternion.LookRotation(positionTarget - this.transform.position);
                    myRigidbody.rotation = Quaternion.Lerp(myRigidbody.rotation, mainRot, Time.fixedDeltaTime * (RotationSpeed * 0.005f) * kocmomech.yaw);
                    myRigidbody.rotation *= Quaternion.Euler(-relativePoint.y * kocmomech.pitch * 0.5f, 0, -relativePoint.x * kocmomech.roll * 0.5f);
                    // 根据单位向量分配 Pitch与 Roll的转动量
                }
                myRigidbody.velocity = (myRigidbody.rotation * Vector3.forward) * realSpeed;
            }
            else
            {
                Vector3 mousePos = followCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.normalized.x, Input.mousePosition.normalized.y, followCam.farClipPlane));
                Vector3 relativePoint = this.transform.InverseTransformPoint(mousePos).normalized; // 计算相对位置的单位向量
                myRigidbody.rotation *= Quaternion.Euler(-relativePoint.y * kocmomech.pitch, relativePoint.x * kocmomech.yaw, -relativePoint.x * kocmomech.roll- myRigidbody.rotation.z);
                myRigidbody.velocity = (myRigidbody.rotation * Vector3.forward) * realSpeed;

                //// axis control by input
                //AddRot.eulerAngles = new Vector3(pitchAxis, yawAxis, -rollAxis);
                //mainRot *= AddRot;
                //myRigidbody.rotation = Quaternion.Lerp(myRigidbody.rotation, mainRot, Time.fixedDeltaTime * RotationSpeed);
                //myRigidbody.velocity = (myRigidbody.rotation * Vector3.forward) * realSpeed);
            }
        }
        private float afterburnerPower;
        public void ControlThrottle(float throttle)
        {
            realSpeed += Time.deltaTime * (realSpeed >= speed.engine ?
                (throttle > 0 ? kocmomech.acceleration : -kocmomech.acceleration) :
                (throttle < 0 ? -kocmomech.deceleration : +kocmomech.deceleration));
            realSpeed = Mathf.Clamp(realSpeed, 0, speed.maximum);

            afterburnerPower += 0.37f * (throttle > 0 ? Time.deltaTime : -Time.deltaTime);
            afterburnerPower = Mathf.Clamp01(afterburnerPower);
            myEngine.Power(afterburnerPower);
        }
        // 传统控制
        private float rollAxis, pitchAxis, yawAxis;
        public void ControlRollAndPitch(Vector2 axis)
        {
            rollAxis = Mathf.Lerp(rollAxis, Mathf.Clamp(axis.x,-1,1) * kocmomech.roll, Time.deltaTime);
            pitchAxis = Mathf.Lerp(pitchAxis, Mathf.Clamp(axis.y, -1,1) * kocmomech.pitch, Time.deltaTime);
        }
        public void ControlYaw(float yaw)
        {
            yawAxis += yaw * Time.deltaTime * 0.2f;
        }
    }
}