/*****************************************************************************
 * Kocmocraft Mech Droid
 * 宇航技工機器人
 * Last Updated: 2018/09/22
 * Description:
 * 1. Damage Manager -> Hull Manager -> Kocmocraft Mech Droid
 * 2. 管理能量、機甲、護盾與傷害
 *****************************************************************************/
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kocmoca
{
    [System.Serializable]
    public struct Data
    {
        public float Max;
        public float Value;
        public float Percent { get { return (Value / Max); } }
    }
    public struct DamagePower
    {
        public Kocmonaut Attacker;
        public float Hull;
        public float Shield;

        public float Absorb;
        public float Penetration;
        public float Damage;
    }
    //public class KocmocraftMechDroid : MonoBehaviour, IPunObservable
    //{
    //    [Header("Dependent Components")]
    //    private Rigidbody myRigidbody;
    //    private AudioSource myAudioSource;
    //    private PhotonView myPhotonView;
    //    private AvionicsSystem myAvionicsSystem;
    //    //private ParticleSystem myOnFireParticle;
    //    private Transform myCockpitViewpoint;
    //    // Basic Data
    //    public ControlUnit ControlUnit { get; protected set; }
    //    public int Number { get; protected set; }
    //    [Header("Modular Parameter")]
    //    public Data dataHull;
    //    public Data dataShield;

    //    // Damage calculation
    //    private int damage;
    //    [Header("Crash Info")]
    //    public Dictionary<int, int> listAttacker = new Dictionary<int, int>(); // 損傷記錄索引
    //    private Kocmonaut lastAttacker = new Kocmonaut { Number = -1 };
    //    private Vector3 lastHitVelocity;
    //    //public ObjectPoolData ammoRemnantOPD;
    //    private GameObject wreckage;

    //    public void Initialize(KocmocraftModule module, ControlUnit core, int type, int number, GameObject pilot, GameObject wreckage)
    //    {
    //        // Dependent Components
    //        myRigidbody = GetComponent<Rigidbody>();
    //        myAudioSource = GetComponent<AudioSource>();
    //        myPhotonView = GetComponent<PhotonView>();
    //        myPhotonView.ObservedComponents.Add(this);
    //        myAvionicsSystem = GetComponent<AvionicsSystem>();
    //        //myOnFireParticle = transform.Find("OnFire").GetComponent<ParticleSystem>();
    //        //if (!myOnFireParticle) Debug.LogError("No OnFire VFX");
    //        myCockpitViewpoint = transform.Find("Cockpit Viewpoint");
    //        // Kocmonaut Data
    //        ControlUnit = core;
    //        Number = number;
    //        if (core == ControlUnit.LocalPlayer)
    //            SatelliteCommander.Instance.Observer.InitializeView(myCockpitViewpoint, pilot, Number);
    //        else
    //            SatelliteCommander.Instance.Observer.listOthers.Add(Number);
    //        // Modular Parameter
    //        dataHull = new Data { Max = module.hull.value, Value = module.hull.value };
    //        dataShield = new Data { Max = module.shield.value, Value = module.shield.value };
    //        // Crash
    //        //ammoRemnantOPD = ObjectPoolManager.Instance.CreatObjectPool(remnant, 1,5);
    //        this.wreckage = wreckage;
    //    }
    //    //private void Update()
    //    //{
    //    //    if (Input.GetKeyDown(KeyCode.U) && ControlUnit == ControlUnit.LocalPlayer)
    //    //        dataHull.Value = 1000000;

    //    //    //if (dataHull.Value < (int)(dataHull.Max * 0.237f))
    //    //    //    myOnFireParticle.Play();
    //    //    //else
    //    //    //    myOnFireParticle.Stop();

    //    //    if (ControlUnit == ControlUnit.RemotePlayer || ControlUnit == ControlUnit.RemoteBot) return;
    //    //    dataShield.Value = Mathf.Clamp(dataShield.Value + Time.deltaTime * myAvionicsSystem.dataEnergy.Value * 0.1f, 0, dataShield.Max);
    //    //}
    //    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //    {
    //        if (stream.IsWriting)
    //        {
    //            stream.SendNext(dataHull.Value);
    //            stream.SendNext(dataShield.Value);
    //        }
    //        else
    //        {
    //            dataHull.Value = (float)stream.ReceiveNext();
    //            dataShield.Value = (float)stream.ReceiveNext();
    //        }
    //    }
    //    public void Hit(DamagePower damagePower)
    //    {
    //        if (ControlUnit == ControlUnit.RemotePlayer || ControlUnit == ControlUnit.RemoteBot) return;
    //        if (dataHull.Value <= 0) return;
    //        int damageSourceNumber = damagePower.Attacker.Number;
    //        // 狀況1
    //        // 第一次造成傷害來源：自己 123
    //        // 最後造成傷害來源：自己 123
    //        // 擊落者：自己墜機 123
    //        // 狀況2
    //        // 第一次造成傷害來源：自己 123
    //        // 最後造成傷害來源：敵人 789
    //        // 擊落者：敵人 789
    //        // 狀況3
    //        // 第一次造成傷害來源：敵人 789
    //        // 最後造成傷害來源：自己 123
    //        // 擊落者：敵人

    //        // 第一次受到損傷
    //        if (lastAttacker.Number == -1)
    //            lastAttacker = damagePower.Attacker;
    //        else
    //        {
    //            if (damagePower.Attacker.Number != Number)
    //                lastAttacker = damagePower.Attacker;
    //        }



    //        /*****************************************************************************
    //         * 傷害計算方法
    //         * Last Updated: 2019-04-18
    //         * 
    //         * 護盾值 dataShield.Value
    //         * 機甲值 dataHull.Value
    //         * 護盾傷害威力值 damagePower.Shield
    //         * 機甲傷害威力值 damagePower.Hull
    //         * 穿透係數 coefficientPenetration
    //         * 護盾傷害值 damageShield
    //         * 機甲傷害值 damageHull
    //         * 總傷害值 damageTotal
    //         * 
    //         * 1. 計算護盾傷害與穿透係數
    //         *      剩餘護盾值 = 原始護盾值 - 護盾傷害威力值
    //         *      護盾值足夠
    //         *          => 護盾傷害值 = 護盾傷害威力值
    //         *          => 護盾穿透係數 = 0
    //         *      護盾值不足
    //         *          => 護盾傷害值 = 原始護盾值 = 剩餘護盾值 + 護盾傷害威力值
    //         *          => 護盾穿透係數 = -1 * 剩餘護盾值 / 護盾傷害威力值
    //         *          => 剩餘護盾值 = 0
    //         *          
    //         * 2. 計算機甲傷害
    //         *      機甲傷害值 = 機甲傷害威力值 * 穿透係數
    //         *      剩餘機甲值 = 原始機甲值 - 機甲傷害值
    //         *      
    //         *****************************************************************************/
    //        //if (damagePower.Shield == -999)
    //        //    coefficientPenetration = 1;
    //        //else
    //        //{
    //        //    dataShield.Value -= damagePower.Shield;
    //        //    if (dataShield.Value >= 0)
    //        //    {
    //        //        damageShield = damagePower.Shield;
    //        //        coefficientPenetration = 0;
    //        //    }
    //        //    else
    //        //    {
    //        //        damageShield = dataShield.Value + damagePower.Shield;
    //        //        coefficientPenetration = -dataShield.Value / damagePower.Shield;
    //        //        dataShield.Value = 0;
    //        //    }
    //        //}
    //        //damageHull = damagePower.Hull * coefficientPenetration;
    //        //dataHull.Value = Mathf.Clamp(dataHull.Value - damageHull, 0, dataHull.Max);

    //        /*****************************************************************************
    //         * 傷害計算方法
    //         * Last Updated: 2019-04-18
    //         * 
    //         * 護盾值 dataShield.Value
    //         * 機甲值 dataHull.Value
    //         * 護盾傷害威力值 damagePower.Shield
    //         * 機甲傷害威力值 damagePower.Hull
    //         * 穿透係數 coefficientPenetration
    //         * 護盾傷害值 damageShield
    //         * 機甲傷害值 damageHull
    //         * 總傷害值 damageTotal
    //         * 
    //         * 吸收伤害值 Absorb
    //         * 穿透伤害值 Penetration
    //         * 
    //         * 1. 計算護盾傷害與穿透係數
    //         *      剩餘護盾值 = 原始護盾值 - 吸收伤害值
    //         *      護盾值足夠
    //         *          => 剩餘機甲值 = 原始機甲值 - 穿透伤害值
    //         *      護盾值不足
    //         *          => 剩餘機甲值 = 原始機甲值 + 剩餘護盾值（负的不足护盾值） - 穿透伤害值
    //         *          => 剩餘護盾值 = 0
    //         *          
    //         * 2. 計算機甲傷害
    //         *      機甲傷害值 = 機甲傷害威力值 * 穿透係數
    //         *      剩餘機甲值 = 原始機甲值 - 機甲傷害值
    //         *      
    //         *****************************************************************************/
    //        if (dataShield.Value > 0)
    //        {
    //            dataShield.Value -= damagePower.Absorb;
    //            if (dataShield.Value >= 0)
    //                dataHull.Value = Mathf.Clamp(dataHull.Value - damagePower.Penetration, 0, dataHull.Max);
    //            else
    //            {
    //                dataHull.Value = Mathf.Clamp(dataHull.Value + dataShield.Value - damagePower.Penetration, 0, dataHull.Max);
    //                dataShield.Value = 0;
    //            }
    //            damage = (int)(damagePower.Absorb + damagePower.Penetration);
    //        }
    //        else
    //        {
    //            dataHull.Value = Mathf.Clamp(dataHull.Value - damagePower.Damage, 0, dataHull.Max);
    //            damage = (int)damagePower.Damage;
    //        }
    //        /*****************************************************************************
    //         * 損傷記錄
    //         *****************************************************************************/
    //        //damage = (int)(damageShield + damageHull);
    //        if (listAttacker.ContainsKey(damageSourceNumber))
    //            listAttacker[damageSourceNumber] += damage;
    //        else
    //            listAttacker.Add(damagePower.Attacker.Number, damage);

    //        // 本地玩家擊中本地AI
    //        if (damagePower.Attacker.ControlUnit == ControlUnit.LocalPlayer)
    //            HeadUpDisplayManager.Instance.ShowHitDamage(myRigidbody.position, damage);
    //        // 遠端玩家擊中本地玩家或本地AI
    //        else if (damagePower.Attacker.ControlUnit == ControlUnit.RemotePlayer)
    //            myPhotonView.RPC("ShowHitDamage", RpcTarget.AllViaServer, damagePower.Attacker.Number, damage);

    //        /************************************* Crash *************************************/

    //        if (dataHull.Value <= 0)
    //        {
    //            if (ControlUnit == ControlUnit.LocalPlayer)
    //            {
    //                GetComponent<LocalPlayerController>().enabled = false;
    //                SatelliteCommander.Instance.PlayerCrash();
    //                //foreach (int key in listAttacker.Keys)
    //                //{
    //                //    HeadUpDisplayManager.Instance.ShowWhoAttackU(key, listAttacker[key]);
    //                //}
    //                HeadUpDisplayManager.Instance.ShowKillStealer(lastAttacker.Number);
    //            }
    //            else if (ControlUnit == ControlUnit.LocalBot)
    //            {
    //                GetComponent<LocalBotController>().enabled = false;
    //                SatelliteCommander.Instance.BotCrash(Number);
    //            }
    //            myPhotonView.RPC("Crash", RpcTarget.AllViaServer, lastAttacker.Number);
    //        }
    //    }

    //    [PunRPC]
    //    public void ShowHitDamage(int numberShooter, int damage, PhotonMessageInfo info)
    //    {
    //        if (PhotonNetwork.LocalPlayer.ActorNumber == numberShooter)
    //            HeadUpDisplayManager.Instance.ShowHitDamage(myRigidbody.position, damage);
    //    }

    //    [PunRPC]
    //    public void Crash(int stealerNumber, PhotonMessageInfo info)
    //    {
    //        // 若宇航機是本地玩家擊落，顯示擊落訊息
    //        if (stealerNumber == PhotonNetwork.LocalPlayer.ActorNumber)
    //            HeadUpDisplayManager.Instance.ShowDestroyed(Number);

    //        SatelliteCommander.Instance.Observer.listOthers.Remove(Number);
    //        SatelliteCommander.Instance.RemoveFlight(Number);
    //        switch (ControlUnit)
    //        {
    //            case ControlUnit.LocalPlayer:
    //                LocalPlayerRealtimeData.Status = FlyingStatus.Crash;
    //                SatelliteCommander.Instance.Observer.TransferCamera();
    //                SatelliteCommander.Instance.ClearData();
    //                HeadUpDisplayManager.Instance.ClearData();
    //                Destroy(GetComponent<LocalPlayerController>());
    //                GetComponent<OnboardRadar>().Stop();
    //                //Destroy(GetComponent<OnboardRadar>());
    //                break;
    //            case ControlUnit.LocalBot:
    //                Destroy(GetComponent<LocalBotController>());
    //                GetComponent<OnboardRadar>().Stop();
    //                //Destroy(GetComponent<OnboardRadar>());
    //                break;
    //        }
    //        ShowRemnant();
    //        if (myPhotonView.IsMine)
    //            PhotonNetwork.Destroy(this.gameObject); // 改变PhotoView Fixed
    //        //StartCoroutine(Crash());
    //    }
    //    IEnumerator Crash()
    //    {
    //        yield return new WaitForSeconds(0.01f);
    //        //ShowRemnant();
    //        //if(myPhotonView.IsMine)
    //        //    PhotonNetwork.Destroy(this.gameObject);
    //    }
    //    private void ShowRemnant()
    //    {
    //        if (myRigidbody.velocity == Vector3.zero) // 撞击可能触发修正而变成0向量
    //            myRigidbody.velocity = lastHitVelocity;
    //        wreckage.transform.SetParent(null);
    //        wreckage.tag = "Untagged";

    //        ResourceManager.hitDown.Reuse(myRigidbody.position, Quaternion.identity);

    //        Rigidbody rigid = wreckage.AddComponent<Rigidbody>();
    //        rigid.mass = 100;
    //        rigid.velocity = myRigidbody.velocity;
    //        rigid.AddForce(Random.rotation.eulerAngles * Random.Range(100, 500));
    //        rigid.AddTorque(Random.rotation.eulerAngles * Random.Range(100, 2000));
    //        Destroy(wreckage, 15.0f);
    //        //remnant
    //        //myRigidbody.useGravity = true;

    //        //GameObject obj = ammoRemnantOPD.Reuse(transform.position, transform.rotation);
    //        //obj.GetComponent<Rigidbody>().velocity = (myRigidbody.velocity == Vector3.zero) ? lastHitVelocity : myRigidbody.velocity;
    //        //obj.GetComponent<Rigidbody>().AddForce(Random.rotation.eulerAngles * Random.Range(100, 500));
    //        //obj.GetComponent<Rigidbody>().AddTorque(Random.rotation.eulerAngles * Random.Range(100, 2000));
    //    }

    //    //private void OnTriggerStay(Collider other)
    //    //{
    //    //    if (other.tag == "SupremacyRange")
    //    //    {
    //    //        AirEarlyWarning.instance.BFM[(int)pilot.BattlefieldFaction].AirPower[(int)pilot.FormationFlyingOrder] += Time.deltaTime;
    //    //        AirEarlyWarning.instance.BFM[(int)pilot.BattlefieldFaction].valueAirSupremacy += Time.deltaTime;
    //    //    }
    //    //}

    //    private void OnCollisionEnter(Collision collision)
    //    {
    //        // 消除撞擊造成的力矩
    //        myRigidbody.isKinematic = true;
    //        myRigidbody.isKinematic = false;

    //        if (ControlUnit == ControlUnit.RemotePlayer || ControlUnit == ControlUnit.RemoteBot) return;
    //        if (dataHull.Value <= 0) return;
    //        lastHitVelocity = myRigidbody.velocity;

    //        // 計算傷害
    //        DamagePower damagePower = new DamagePower();
    //        if (collision.gameObject.CompareTag("Untagged")|| collision.gameObject.CompareTag("Water"))
    //        {
    //            damagePower.Penetration = 50000;
    //            // 下次更新要做的
    //            // 入射角越大伤害越大，避免卡住
    //            //Vector3 inDirection = -collision.relativeVelocity;
    //            //Vector3 normal = collision.contacts[0].normal;
    //            //Vector3 outDirection = Vector3.Reflect(inDirection, normal);
    //            //transform.forward = outDirection;
    //            //GetComponent<AvionicsSystem>().mainRot = transform.rotation;
    //            //myRigidbody.AddForce(outDirection * collision.impulse.magnitude);
    //        }
    //        else
    //            damagePower.Penetration = Mathf.Clamp((int)Mathf.Abs(collision.impulse.magnitude), 0, 5000);
    //        //damagePower.Shield = -999;

    //        // 記錄攻擊者
    //        damagePower.Attacker = new Kocmonaut { Number = -1 };
    //        KocmocraftMechDroid collisionKocmocraft = collision.gameObject.GetComponent<KocmocraftMechDroid>();
    //        if (collisionKocmocraft)
    //        {
    //            damagePower.Attacker.ControlUnit = collisionKocmocraft.ControlUnit;
    //            damagePower.Attacker.Number = collisionKocmocraft.Number;
    //        }
    //        else
    //        {
    //            damagePower.Attacker.ControlUnit = ControlUnit;
    //            damagePower.Attacker.Number = Number;
    //        }
    //        Hit(damagePower);
    //    }

    //    private void OnCollisionExit(Collision collision)
    //    {
    //        // 確保撞擊後恢復一般剛體
    //        myRigidbody.isKinematic = true;
    //        myRigidbody.isKinematic = false;
    //    }
    //}
}