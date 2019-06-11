/***************************************************************************
 * Kocmocraft Mech Droid
 * 宇航機技工機器人
 * Last Updated: 2018/09/22
 * Description:
 * 1. Damage Manager -> Hull Manager -> Kocmocraft Mech Droid
 * 2. 管理能量、機甲、護盾與傷害
 ***************************************************************************/
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace Kocmoca
{
    public class AvionicsSystem : MonoBehaviour
    {
        [Header ("Preset Module Data")]
        public Type kocmocraftType;
        public Shield shield;
        public Hull hull;
        public Speed speed;
        public Kocmomech kocmomech;
        [Header ("Preset Module System")]
        public Prototype myPrototype;
        public Transform myWreckage;
        public PilotManager myDubis;
        public PowerController myPowerController;
        public TurretFireControlSystem myTurretFCS;
        public RocketFireControlSystem myRocketFCS;
        public MissileFireControlSystem myMissileFCS;
        public Cinemachine.CinemachineVirtualCamera cockpitView;
        public Cinemachine.CinemachineVirtualCamera followView;
        [Header ("Constant")]
        public float attitudeLimit = 0.2792527f; // 16度
        public float autoLevelAngle = 0.1570796f; // 9度
        public float autoLevelPeriod = 0.07853982f; // equal half auto level angle
        public float inverseAngle = 0.1178097f; // 3/4 auto level angle
        public float inversePeriod = 0.2356195f; // equal half auto level angle
        [Header ("Active System")]
        public ControlUnit controlUnit;
        public int kocmoNumber;
        private Transform portTransform;
        private Rigidbody portRigidbody;
        private PhotonView portPhotonView;
        [Header ("Variable")]
        [Tooltip ("The position of target relative to own position.")]
        public Vector3 localTargetPos;
        public float realtimeShield, realtimeHull, realtimeSpeed;
        public float shieldPercent { get { return realtimeShield / shield.maximum; } }
        public float hullPercent { get { return realtimeHull / hull.maximum; } }
        public float speedPercent { get { return realtimeSpeed / speed.maximum; } }

        [Tooltip ("About Engine sounds and effects level")]
        public float afterburnerPower;

        public Dictionary<int, int> listAttacker = new Dictionary<int, int> (); // 損傷記錄索引
        public Kocmonaut lastAttacker = new Kocmonaut { Number = -1 };
        private int damage; // 确认是否宣告？

        private void Reset ()
        {
            int type = int.Parse (name.Split (new char[2] { '(', ')' }) [1]);
            KocmocraftDatabase index = UnityEditor.AssetDatabase.LoadAssetAtPath<KocmocraftDatabase> ("Assets/_iLYuSha Wakaka Setting/ScriptableObject/Kocmocraft Database.asset");
            KocmocraftModule module = index.kocmocraft[type];
            kocmocraftType = (Type) type;
            shield = module.shield;
            hull = module.hull;
            speed = module.speed;
            kocmomech = module.kocmomech;
            myPrototype = GetComponentInChildren<Prototype> ();
            myPrototype.CreatePrototypeDatabase ();
            myWreckage = myPrototype.transform;
            myDubis = GetComponentInChildren<PilotManager> ();
            myPowerController = GetComponentInChildren<PowerController> ();
            myTurretFCS = GetComponentInChildren<TurretFireControlSystem> ();
            myRocketFCS = GetComponentInChildren<RocketFireControlSystem> ();
            myMissileFCS = GetComponentInChildren<MissileFireControlSystem> ();
            GetComponent<OnboardRadar> ().Preset (module);
            myTurretFCS.Preset (module);
            myRocketFCS.Preset (module);
            myMissileFCS.Preset (module);

            // Cockpit View setting
            cockpitView = GetComponentsInChildren<Cinemachine.CinemachineVirtualCamera> () [3];
            cockpitView.enabled = true; // if disable, cant get tansposer.
            cockpitView.m_Follow = myDubis.transform;
            cockpitView.m_LookAt = myDubis.transform;
            cockpitView.m_Lens.FieldOfView = 60;
            cockpitView.m_Lens.NearClipPlane = 0.5f;
            cockpitView.m_Lens.FarClipPlane = 15000;
            Cinemachine.CinemachineTransposer transposer = cockpitView.GetCinemachineComponent<Cinemachine.CinemachineTransposer> ();
            transposer.m_XDamping = 0;
            transposer.m_YDamping = 0;
            transposer.m_ZDamping = 0;
            transposer.m_PitchDamping = 1;
            transposer.m_YawDamping = 1;
            cockpitView.GetCinemachineComponent<Cinemachine.CinemachineComposer> ().m_TrackedObjectOffset = new Vector3 (0, 0, 10);
            cockpitView.enabled = false;

            // Follow View setting
            followView = GetComponentsInChildren<Cinemachine.CinemachineVirtualCamera> () [4];
            followView.enabled = true;
            followView.m_Follow = myPrototype.transform;
            followView.m_LookAt = myPrototype.transform;
            followView.m_Lens.FieldOfView = 60;
            followView.m_Lens.NearClipPlane = 0.1f;
            followView.m_Lens.FarClipPlane = 15000;
            transposer = followView.GetCinemachineComponent<Cinemachine.CinemachineTransposer> ();
            float offsetZ = -module.design.size.wingspan * 3.5f - module.design.size.length * 0.5f;
            float offsetY = -offsetZ * Mathf.Tan (17 * Mathf.Deg2Rad);
            transposer.m_FollowOffset = new Vector3 (0, offsetY, offsetZ);
            transposer.m_XDamping = 0;
            transposer.m_YDamping = 0;
            transposer.m_ZDamping = 0;
            transposer.m_PitchDamping = 1;
            transposer.m_YawDamping = 1;
            // Aim 瞄准机身前缘方向
            followView.GetCinemachineComponent<Cinemachine.CinemachineComposer> ().m_TrackedObjectOffset = new Vector3 (0, module.design.size.height * 0.5f, module.design.size.length * 7);
            followView.enabled = false;

            controlUnit = ControlUnit.None;
            // Initial value
            realtimeShield = shield.maximum;
            realtimeHull = hull.maximum;
            realtimeSpeed = speed.powerSystem;
            enabled = false;
            Debug.Log ("<color=Yellow>" + name + " data has been preset.</color>");
        }
        public void Active (Transform rootTransform, Rigidbody rootRigidbody, PhotonView rootPhotonView, ControlUnit core, int rootNumber)
        {
            portTransform = rootTransform;
            portRigidbody = rootRigidbody;
            portPhotonView = rootPhotonView;
            portPhotonView.ObservedComponents.Add (this);
            controlUnit = core;
            kocmoNumber = rootNumber;
            enabled = true;
            if (controlUnit == ControlUnit.LocalPlayer)
                SatelliteCommander.Instance.Observer.AssignPilotView (cockpitView, followView);
        }
        private void FixedUpdate ()
        {
            //if (portRigidbody.angularVelocity.magnitude > 3)
            //    portRigidbody.Sleep();
            switch (controlUnit)
            {
                case ControlUnit.LocalBot:
                    localTargetPos = portTransform.InverseTransformPoint (LocalBotController.targetPos);
                    break;
                case ControlUnit.LocalPlayer:
                    localTargetPos = portTransform.InverseTransformPoint (LocalPlayerController.targetPos);
                    break;
                default:
                    return;
            }

            var pitchChange = Mathf.Clamp (-Mathf.Atan2 (localTargetPos.y, localTargetPos.z), -attitudeLimit, attitudeLimit);
            var yawChange = Mathf.Clamp (-Mathf.Atan2 (localTargetPos.x, localTargetPos.z), -attitudeLimit, attitudeLimit);
            var yawAbs = Mathf.Abs (yawChange);

            if (yawAbs <= autoLevelAngle && Mathf.Abs (pitchChange) <= autoLevelAngle)
            {
                if (yawAbs > inverseAngle)
                {
                    var rollChange = autoLevelAngle * Mathf.Cos (yawChange * Mathf.PI / autoLevelPeriod) * Mathf.Sign (yawChange);
                    portRigidbody.rotation *= Quaternion.Euler (pitchChange * kocmomech.pitch, -yawChange * kocmomech.yaw, rollChange * kocmomech.roll);
                }
                else
                {
                    var fwd = portTransform.forward;
                    fwd.y = 0;
                    //fwd *= Mathf.Sign(portTransform.up.y); // 投影面法向量修正，roll的范围将会变成-90~90
                    fwd.Normalize (); // 先进行Normalize再Cross会比较快
                    var right = Vector3.Cross (Vector3.up, fwd);
                    // 计算 InverseTransformDirection + 反正切 (Atan) 速度更快
                    var localFlatRight = portTransform.InverseTransformDirection (right);
                    var rollAngle = Mathf.Atan2 (localFlatRight.y, localFlatRight.x);
                    var rollChange = autoLevelAngle * Mathf.Cos (yawChange * Mathf.PI / inversePeriod + Mathf.PI) * -Mathf.Sign (rollAngle); // 正确    
                    portRigidbody.rotation *= Quaternion.Euler (pitchChange * kocmomech.pitch, -yawChange * kocmomech.yaw, rollChange * kocmomech.roll);
                }
            }
            else
            {
                var rollChange = yawChange;
                portRigidbody.rotation *= Quaternion.Euler (pitchChange * kocmomech.pitch, -yawChange * kocmomech.yaw, rollChange * kocmomech.roll);
            }
            portRigidbody.velocity = (portRigidbody.rotation * Vector3.forward) * realtimeSpeed;
        }
        public void ControlThrottle (float throttle)
        {
            if (realtimeSpeed >= speed.powerSystem)
            {
                realtimeSpeed += (throttle == 0 ? -3.0f : Mathf.Sign (throttle) * kocmomech.acceleration) * Time.deltaTime;
                afterburnerPower += (throttle == 0 ? -0.1f : Mathf.Sign (throttle) * 0.2f) * Time.deltaTime;
                afterburnerPower = Mathf.Clamp (afterburnerPower, 0.4f, 1.0f);
            }
            else
            {
                realtimeSpeed += (throttle == 0 ? +3.0f : Mathf.Sign (throttle) * kocmomech.deceleration) * Time.deltaTime;
                // 當速度低於 engine 提供的巡航速度時，AB power 與當前速度成線性關係
                afterburnerPower = 0.4f * Mathf.InverseLerp (0, speed.powerSystem, realtimeSpeed);
            }
            realtimeSpeed = Mathf.Clamp (realtimeSpeed, 0, speed.maximum);
            myPowerController.Power (afterburnerPower);
        }
        private void OnCollisionEnter (Collision collision)
        {
            // 消除撞擊造成的力矩
            portRigidbody.isKinematic = true;
            portRigidbody.isKinematic = false;

            if (controlUnit == ControlUnit.RemotePlayer || controlUnit == ControlUnit.RemoteBot) return;
            if (realtimeHull <= 0) return;
            //lastHitVelocity = portRigidbody.velocity;

            // 計算傷害
            DamagePower damagePower = new DamagePower ();
            if (collision.gameObject.CompareTag ("Untagged") || collision.gameObject.CompareTag ("Water"))
            {
                damagePower.Penetration = 50000;
                // 下次更新要做的
                // 入射角越大伤害越大，避免卡住
                //Vector3 inDirection = -collision.relativeVelocity;
                //Vector3 normal = collision.contacts[0].normal;
                //Vector3 outDirection = Vector3.Reflect(inDirection, normal);
                //transform.forward = outDirection;
                //GetComponent<AvionicsSystem>().mainRot = transform.rotation;
                //portRigidbody.AddForce(outDirection * collision.impulse.magnitude);
            }
            else
                damagePower.Penetration = Mathf.Clamp ((int) Mathf.Abs (collision.impulse.magnitude), 0, 5000);
            //damagePower.Shield = -999;

            // 記錄攻擊者
            damagePower.Attacker = new Kocmonaut { Number = -1 };
            AvionicsSystem collisionKocmocraft = collision.gameObject.GetComponent<AvionicsSystem> ();
            if (collisionKocmocraft)
            {
                damagePower.Attacker.ControlUnit = collisionKocmocraft.controlUnit;
                damagePower.Attacker.Number = collisionKocmocraft.kocmoNumber;
            }
            else
            {
                damagePower.Attacker.ControlUnit = controlUnit;
                damagePower.Attacker.Number = kocmoNumber;
            }
            Hit (damagePower);
        }
        private void OnCollisionExit (Collision collision)
        {
            // 確保撞擊後恢復一般剛體
            portRigidbody.isKinematic = true;
            portRigidbody.isKinematic = false;
        }
        public void Hit (DamagePower damagePower)
        {
            if (controlUnit == ControlUnit.RemotePlayer || controlUnit == ControlUnit.RemoteBot) return;
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
                if (damagePower.Attacker.Number != kocmoNumber)
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
                    realtimeHull = Mathf.Clamp (realtimeHull - damagePower.Penetration, 0, hull.maximum);
                else
                {
                    realtimeHull = Mathf.Clamp (realtimeHull + realtimeShield - damagePower.Penetration, 0, hull.maximum);
                    realtimeShield = 0;
                }
                damage = (int) (damagePower.Absorb + damagePower.Penetration);
            }
            else
            {
                realtimeHull = Mathf.Clamp (realtimeHull - damagePower.Damage, 0, hull.maximum);
                damage = (int) damagePower.Damage;
            }
            /*****************************************************************************
             * 損傷記錄
             *****************************************************************************/
            //damage = (int)(damageShield + damageHull);
            if (listAttacker.ContainsKey (damageSourceNumber))
                listAttacker[damageSourceNumber] += damage;
            else
                listAttacker.Add (damagePower.Attacker.Number, damage);

            // 本地玩家擊中本地AI
            if (damagePower.Attacker.ControlUnit == ControlUnit.LocalPlayer)
                HeadUpDisplayManager.Instance.ShowHitDamage (portRigidbody.position, damage);
            // 遠端玩家擊中本地玩家或本地AI
            else if (damagePower.Attacker.ControlUnit == ControlUnit.RemotePlayer)
                portPhotonView.RPC ("ShowHitDamage", RpcTarget.AllViaServer, damagePower.Attacker.Number, damage);

            /************************************* Crash *************************************/

            if (realtimeHull <= 0)
            {
                if (controlUnit == ControlUnit.LocalPlayer)
                {
                    transform.root.GetComponent<LocalPlayerController> ().enabled = false;
                    SatelliteCommander.Instance.PlayerCrash ();
                    //foreach (int key in listAttacker.Keys)
                    //{
                    //    HeadUpDisplayManager.Instance.ShowWhoAttackU(key, listAttacker[key]);
                    //}
                    HeadUpDisplayManager.Instance.ShowKillStealer (lastAttacker.Number);
                }
                else if (controlUnit == ControlUnit.LocalBot)
                {
                    transform.root.GetComponent<LocalBotController> ().enabled = false;
                    SatelliteCommander.Instance.BotCrash (kocmoNumber);
                }
                portPhotonView.RPC ("Crash", RpcTarget.AllViaServer, lastAttacker.Number);
            }
        }
        public void Crash (int stealerNumber, PhotonMessageInfo info)
        {
            // 若宇航機是本地玩家擊落，顯示擊落訊息
            if (stealerNumber == PhotonNetwork.LocalPlayer.ActorNumber)
                HeadUpDisplayManager.Instance.ShowDestroyed (kocmoNumber);

            SatelliteCommander.Instance.Observer.listOthers.Remove (kocmoNumber);
            SatelliteCommander.Instance.RemoveFlight (kocmoNumber);
            switch (controlUnit)
            {
                case ControlUnit.LocalPlayer:
                    LocalPlayerRealtimeData.Status = FlyingStatus.Crash;
                    //SatelliteCommander.Instance.Observer.TransferCamera();
                    SatelliteCommander.Instance.ClearData ();
                    HeadUpDisplayManager.Instance.ClearData ();
                    Destroy (transform.root.GetComponent<LocalPlayerController> ());
                    transform.root.GetComponentInChildren<OnboardRadar> ().Stop ();
                    //Destroy(GetComponent<OnboardRadar>());
                    break;
                case ControlUnit.LocalBot:
                    Destroy (transform.root.GetComponent<LocalBotController> ());
                    transform.root.GetComponentInChildren<OnboardRadar> ().Stop ();
                    //Destroy(GetComponent<OnboardRadar>());
                    break;
            }
            ShowRemnant ();
            if (portPhotonView.IsMine)
                PhotonNetwork.Destroy (transform.root.gameObject); // 改变PhotoView Fixed
            //StartCoroutine(Crash());
        }
        private void ShowRemnant ()
        {
            // 重写
            //if (portRigidbody.velocity == Vector3.zero) // 撞击可能触发修正而变成0向量
            //    portRigidbody.velocity = Vector3.one;
            //wreckage.transform.SetParent(null);
            //wreckage.tag = "Untagged";

            //ResourceManager.hitDown.Reuse(portRigidbody.position, Quaternion.identity);

            //Rigidbody rigid = wreckage.AddComponent<Rigidbody>();
            //rigid.mass = 100;
            //rigid.velocity = portRigidbody.velocity;
            //rigid.AddForce(Random.rotation.eulerAngles * Random.Range(100, 500));
            //rigid.AddTorque(Random.rotation.eulerAngles * Random.Range(100, 2000));
            //Destroy(wreckage, 15.0f);
        }

        // 传统控制
        private float rollAxis, pitchAxis, yawAxis;
        public void ControlRollAndPitch (Vector2 axis)
        {
            rollAxis = Mathf.Lerp (rollAxis, Mathf.Clamp (axis.x, -1, 1) * kocmomech.roll, Time.deltaTime);
            pitchAxis = Mathf.Lerp (pitchAxis, Mathf.Clamp (axis.y, -1, 1) * kocmomech.pitch, Time.deltaTime);
        }
        public void ControlYaw (float yaw)
        {
            yawAxis += yaw * Time.deltaTime * 0.2f;
        }

        //[Header("Dependent Components")]
        //private Rigidbody portRigidbody;
        //private PowerController myEngine;
        //[Header("Modular Parameter")]
        ////public Data dataEnergy;
        //public Data dataSpeed;
        //private float valueSpeedCruise;
        //private float valueSpeedHigh;
        //private bool isCharge;

        //[Header("Constant")]
        //public float RotationSpeed = 50.0f;// Turn Speed
        //public float SpeedPitch = 2;// rotation X
        //public float SpeedRoll = 3;// rotation Z
        //public float SpeedYaw = 1;// rotation Y
        //public float DampingTarget = 10.0f;// rotation speed to facing to a target
        //public bool AutoPilot = false;// if True this plane will follow a target automatically
        //[HideInInspector]
        //public bool SimpleControl = true;// set true is enabled casual controling
        //[HideInInspector]
        //public bool FollowTarget = false;
        //[HideInInspector]
        //public Vector3 PositionTarget = Vector3.zero;// current target position
        //[HideInInspector]
        //private Vector3 positionTarget = Vector3.zero;
        //public Quaternion mainRot = Quaternion.identity;
        ////[HideInInspector]
        //public float roll = 0;
        ////[HideInInspector]
        //public float pitch = 0;
        ////[HideInInspector]
        //public float yaw = 0;
        //public Vector2 LimitAxisControl = new Vector2(2, 1);// limited of axis rotation magnitude
        //public bool FixedX;
        //public bool FixedY;
        //public bool FixedZ;
        //public float Mess = 30;
        //public bool DirectVelocity = true;// if true this riggidbody will not receive effect by other force.
        //public float DampingVelocity = 5;

        //public void Initialize(KocmocraftModule module, int type)
        //{
        //    // Dependent Components
        //    portRigidbody = GetComponent<Rigidbody>();
        //    myEngine = GetComponentInChildren<PowerController>();
        //    // Modular Parameter
        //    //dataEnergy = new Data { Max = KocmocraftData.Energy[type], Value = KocmocraftData.Energy[type] };
        //    dataSpeed = new Data { Max = KocmocraftData.AfterburnerSpeed[type], Value = 0 };
        //    valueSpeedCruise = KocmocraftData.CruiseSpeed[type];
        //    valueSpeedHigh = valueSpeedCruise * 1.1f;        
        //}

        //void Start()
        //{
        //    mainRot = this.transform.rotation;
        //}

        //void FixedUpdate()
        //{
        //    if (!portRigidbody)
        //        return;

        //    Quaternion AddRot = Quaternion.identity;
        //    Vector3 velocityTarget = Vector3.zero;

        //    // 转向控制

        //    if (AutoPilot)
        //    {
        //        if (portRigidbody.angularVelocity.magnitude > 3)
        //            portRigidbody.Sleep();

        //        // if auto pilot
        //        if (FollowTarget)
        //        {
        //            // rotation facing to the positionTarget
        //            positionTarget = Vector3.Lerp(positionTarget, PositionTarget, Time.fixedDeltaTime * DampingTarget);
        //            Vector3 relativePoint = this.transform.InverseTransformPoint(positionTarget).normalized; // 计算相对位置的单位向量
        //            mainRot = Quaternion.LookRotation(positionTarget - this.transform.position);
        //            portRigidbody.rotation = Quaternion.Lerp(portRigidbody.rotation, mainRot, Time.fixedDeltaTime * (RotationSpeed * 0.005f) * SpeedYaw);
        //            portRigidbody.rotation *= Quaternion.Euler(-relativePoint.y * SpeedPitch * 0.5f, 0, -relativePoint.x * SpeedRoll * 0.5f);
        //            // 根据单位向量分配 Pitch与 Roll的转动量
        //        }
        //        velocityTarget = (portRigidbody.rotation * Vector3.forward) * (dataSpeed.Value);
        //    }
        //    else
        //    {
        //        // axis control by input
        //        AddRot.eulerAngles = new Vector3(pitch, yaw, -roll);
        //        mainRot *= AddRot;

        //        if (SimpleControl)
        //        {
        //            Quaternion saveQ = mainRot;

        //            Vector3 fixedAngles = new Vector3(mainRot.eulerAngles.x, mainRot.eulerAngles.y, mainRot.eulerAngles.z);

        //            if (FixedX)
        //                fixedAngles.x = 1;
        //            if (FixedY)
        //                fixedAngles.y = 1;
        //            if (FixedZ)
        //                fixedAngles.z = 1;

        //            saveQ.eulerAngles = fixedAngles;

        //            mainRot = Quaternion.Lerp(mainRot, saveQ, Time.fixedDeltaTime * 2);
        //        }

        //        portRigidbody.rotation = Quaternion.Lerp(portRigidbody.rotation, mainRot, Time.fixedDeltaTime * RotationSpeed);
        //        //Debug.Log(portRigidbody.angularVelocity.ToString("f4"));
        //        //if (remote)
        //        //    Debug.Log("NxtRot2: " + Time.frameCount + " / " + portRigidbody.rotation.eulerAngles);

        //        velocityTarget = (portRigidbody.rotation * Vector3.forward) * (dataSpeed.Value);
        //    }
        //    // add velocity to the riggidbody

        //    if (DirectVelocity)
        //    {
        //        portRigidbody.velocity = velocityTarget;
        //    }
        //    else
        //    {
        //        portRigidbody.velocity = Vector3.Lerp(portRigidbody.velocity, velocityTarget, Time.fixedDeltaTime * DampingVelocity);
        //    }
        //    yaw = Mathf.Lerp(yaw, 0, Time.deltaTime);

        //    //dataEnergy.Value = Mathf.Clamp(dataEnergy.Value + Time.deltaTime * 71, 0, dataEnergy.Max);
        //    //if (dataEnergy.Value <= 10)
        //    //    isCharge = true;
        //    //else if (dataEnergy.Value > 300)
        //    //    isCharge = false;
        //}

        //// Input function. ( roll and pitch)
        //public void AxisControl(Vector2 axis)
        //{
        //    if (SimpleControl)
        //    {
        //        LimitAxisControl.y = LimitAxisControl.x;
        //    }
        //    // Debug.Log(axis.x);
        //    roll = Mathf.Lerp(roll, Mathf.Clamp(axis.x, -LimitAxisControl.x, LimitAxisControl.x) * SpeedRoll, Time.deltaTime);
        //    pitch = Mathf.Lerp(pitch, Mathf.Clamp(axis.y, -LimitAxisControl.y, LimitAxisControl.y) * SpeedPitch, Time.deltaTime);
        //}
        //// Input function ( yaw) 
        //public void TurnControl(float turn)
        //{
        //    yaw += turn * Time.deltaTime * 0.2f;
        //}
        //public void SpeedControl(float throttle, bool useAfterBurner)
        //{
        //    if (isCharge)
        //        useAfterBurner = false;
        //    if (throttle > 0)
        //    {
        //        if (useAfterBurner)
        //        {
        //            dataSpeed.Value = Mathf.Lerp(dataSpeed.Value, dataSpeed.Max, Time.deltaTime * (0.73f * throttle));
        //            //dataEnergy.Value = Mathf.Clamp(dataEnergy.Value - Time.deltaTime * 0.163f, 0, dataEnergy.Max);
        //        }
        //        else
        //            dataSpeed.Value = Mathf.Lerp(dataSpeed.Value, valueSpeedHigh, Time.deltaTime * (0.73f * throttle));
        //    }
        //    else if (throttle < 0)
        //        dataSpeed.Value = Mathf.Lerp(dataSpeed.Value, 0, Time.deltaTime * (0.73f * -throttle));
        //    else
        //        dataSpeed.Value = Mathf.Lerp(dataSpeed.Value, valueSpeedCruise, Time.deltaTime);

        //    myEngine.Power(dataSpeed.Percent);
        //}
    }

}