/***************************************************************************
 * Kocmocraft Mech Droid
 * 宇航技工機器人
 * Last Updated: 2018/09/22
 * Description:
 * 1. Damage Manager -> Hull Manager -> Kocmocraft Mech Droid
 * 2. 管理能量、機甲、護盾與傷害
 ***************************************************************************/
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kocmoca
{
    public struct Data
    {
        public float Max;
        public float Value;
        public float Percent { get { return (Value / Max); } }
    }
    public struct DamageInfo
    {
        public Kocmonaut Attacker;
        public int Hull;
        public int Shield;
    }
    public class KocmocraftMechDroid : MonoBehaviour, IPunObservable
    {
        [Header("Dependent Components")]
        private Rigidbody myRigidbody;
        private AudioSource myAudioSource;
        private PhotonView myPhotonView;
        private AvionicsSystem myAvionicsSystem;
        //private ParticleSystem myOnFireParticle;
        private Transform myCockpitViewpoint;
        // Basic Data
        public Core Core { get; protected set; }
        public int Number { get; protected set; }
        [Header("Modular Parameter")]
        public Data dataHull;
        public Data dataShield;
        [Header("Crash Info")]
        public Dictionary<int, int> listAttacker = new Dictionary<int, int>(); // 損傷記錄索引
        private Kocmonaut lastAttacker = new Kocmonaut { Number = -1 };
        private Vector3 lastHitVelocity;
        //public ObjectPoolData ammoRemnantOPD;
        private GameObject wreckage;

        public void Initialize(Core core, int type, int number, GameObject pilot, GameObject wreckage)
        {
            // Dependent Components
            myRigidbody = GetComponent<Rigidbody>();
            myAudioSource = GetComponent<AudioSource>();
            myPhotonView = GetComponent<PhotonView>();
            myPhotonView.ObservedComponents.Add(this);
            myAvionicsSystem = GetComponent<AvionicsSystem>();
            //myOnFireParticle = transform.Find("OnFire").GetComponent<ParticleSystem>();
            //if (!myOnFireParticle) Debug.LogError("No OnFire VFX");
            myCockpitViewpoint = transform.Find("Cockpit Viewpoint");
            // Kocmonaut Data
            Core = core;
            Number = number;
            if (core == Core.LocalPlayer)
                SatelliteCommander.Instance.Observer.InitializeView(myCockpitViewpoint, pilot, Number);
            else
                SatelliteCommander.Instance.Observer.listOthers.Add(Number);
            // Modular Parameter
            dataHull = new Data { Max = KocmocraftData.MaxHull[type] ,Value = KocmocraftData.MaxHull[type] };
            dataShield = new Data { Max = KocmocraftData.MaxShieldl[type], Value = KocmocraftData.MaxShieldl[type] };
            // Crash
            //ammoRemnantOPD = ObjectPoolManager.Instance.CreatObjectPool(remnant, 1,5);
            this.wreckage = wreckage;
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.U) && Core == Core.LocalPlayer)
                dataHull.Value = 1000000;

            //if (dataHull.Value < (int)(dataHull.Max * 0.237f))
            //    myOnFireParticle.Play();
            //else
            //    myOnFireParticle.Stop();

            if (Core == Core.RemotePlayer || Core == Core.RemoteBot) return;
            dataShield.Value = Mathf.Clamp(dataShield.Value + Time.deltaTime * myAvionicsSystem.dataEnergy.Value * 0.1f, 0, dataShield.Max);
        }
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(dataHull.Value);
                stream.SendNext(dataShield.Value);
            }
            else
            {
                dataHull.Value = (float)stream.ReceiveNext();
                dataShield.Value = (float)stream.ReceiveNext();
            }
        }
        public void Hit(DamageInfo damageInfo)
        {
            if (Core == Core.RemotePlayer || Core == Core.RemoteBot) return;
            if (dataHull.Value <= 0) return;
            int damageSourceNumber = damageInfo.Attacker.Number;
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
                lastAttacker = damageInfo.Attacker;
            else
            {
                if (damageInfo.Attacker.Number != Number)
                    lastAttacker = damageInfo.Attacker;
            }
            //if (lastHitKocmonaut.Number == -1)
            //{
            //    if (damageInfo.pilot.Number == Number)
            //        damageInfo.pilot.Number = -777;
            //    lastHitKocmonaut = damageInfo.pilot;
            //}
            //else
            //{
            //    // 刷新
            //    if (damageInfo.pilot.Number != -777)
            //    {
            //        if (damageInfo.pilot.Number != Number)
            //            lastHitKocmonaut = damageInfo.pilot;
            //    }
            //}

            if (dataShield.Value > 300 && damageInfo.Shield != 0)
            {
                // 損傷記錄
                if (listAttacker.ContainsKey(damageSourceNumber))
                    listAttacker[damageSourceNumber] += damageInfo.Shield;
                else
                    listAttacker.Add(damageInfo.Attacker.Number, damageInfo.Shield);
                dataShield.Value = Mathf.Clamp(dataShield.Value - damageInfo.Shield, 0, dataShield.Max);

                // 本地玩家擊中本地AI
                if (damageInfo.Attacker.Core == Core.LocalPlayer)
                    HeadUpDisplayManager.Instance.ShowHitDamage(myRigidbody.position, damageInfo.Shield);
                // 遠端玩家擊中本地玩家或本地AI
                else if (damageInfo.Attacker.Core == Core.RemotePlayer)
                    myPhotonView.RPC("ShowHitDamage", RpcTarget.AllViaServer, damageInfo.Attacker.Number, damageInfo.Shield);
            }
            else
            {
                // 損傷記錄
                if (listAttacker.ContainsKey(damageSourceNumber))
                    listAttacker[damageSourceNumber] += damageInfo.Hull;
                else
                    listAttacker.Add(damageInfo.Attacker.Number, damageInfo.Hull);
                dataHull.Value = Mathf.Clamp(dataHull.Value - damageInfo.Hull, 0, dataHull.Max);

                // 本地玩家擊中本地AI
                if (damageInfo.Attacker.Core == Core.LocalPlayer)
                    HeadUpDisplayManager.Instance.ShowHitDamage(myRigidbody.position, damageInfo.Hull);
                // 遠端玩家擊中本地玩家或本地AI
                else if (damageInfo.Attacker.Core == Core.RemotePlayer)
                    myPhotonView.RPC("ShowHitDamage", RpcTarget.AllViaServer, damageInfo.Attacker.Number, damageInfo.Hull);

                //if (Core == Core.LocalPlayer)
                //    myAudioSource.PlayOneShot(ResourceManager.instance.soundHitSelf, 1.0f);
            }

            if (dataHull.Value <= 0)
            {
                if (Core == Core.LocalPlayer)
                {
                    GetComponent<LocalPlayerController>().enabled = false;
                    SatelliteCommander.Instance.PlayerCrash();
                    foreach (int key in listAttacker.Keys)
                    {
                        HeadUpDisplayManager.Instance.ShowWhoAttackU(key, listAttacker[key]);
                    }
                    HeadUpDisplayManager.Instance.ShowKillStealer(lastAttacker.Number);
                }
                else if (Core == Core.LocalBot)
                {
                    GetComponent<LocalBotController>().enabled = false;
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
        [PunRPC]
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
                    Destroy(GetComponent<LocalPlayerController>());
                    GetComponent<OnboardRadar>().Stop();
                    //Destroy(GetComponent<OnboardRadar>());
                    break;
                case Core.LocalBot:
                    Destroy(GetComponent<LocalBotController>());
                    GetComponent<OnboardRadar>().Stop();
                    //Destroy(GetComponent<OnboardRadar>());
                    break;
            }
            ShowRemnant();
            if (myPhotonView.IsMine)
                PhotonNetwork.Destroy(this.gameObject);
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
            wreckage.transform.SetParent(null);
            wreckage.tag = "Untagged";
            Rigidbody rigid = wreckage.AddComponent<Rigidbody>();
            rigid.mass = 100;
            rigid.velocity = myRigidbody.velocity;
            rigid.AddForce(Random.rotation.eulerAngles * Random.Range(100, 500));
            rigid.AddTorque(Random.rotation.eulerAngles * Random.Range(100, 2000));
            Destroy(wreckage, 10.0f);
            //remnant
            //myRigidbody.useGravity = true;

            //GameObject obj = ammoRemnantOPD.Reuse(transform.position, transform.rotation);
            //obj.GetComponent<Rigidbody>().velocity = (myRigidbody.velocity == Vector3.zero) ? lastHitVelocity : myRigidbody.velocity;
            //obj.GetComponent<Rigidbody>().AddForce(Random.rotation.eulerAngles * Random.Range(100, 500));
            //obj.GetComponent<Rigidbody>().AddTorque(Random.rotation.eulerAngles * Random.Range(100, 2000));
        }

        //private void OnTriggerStay(Collider other)
        //{
        //    if (other.tag == "SupremacyRange")
        //    {
        //        AirEarlyWarning.instance.BFM[(int)pilot.BattlefieldFaction].AirPower[(int)pilot.FormationFlyingOrder] += Time.deltaTime;
        //        AirEarlyWarning.instance.BFM[(int)pilot.BattlefieldFaction].valueAirSupremacy += Time.deltaTime;
        //    }
        //}

        private void OnCollisionEnter(Collision collision)
        {
            // 消除撞擊造成的力矩
            myRigidbody.isKinematic = true;
            myRigidbody.isKinematic = false;

            if (Core == Core.RemotePlayer || Core == Core.RemoteBot) return;
            if (dataHull.Value <= 0) return;
            lastHitVelocity = myRigidbody.velocity;

            // 計算傷害
            DamageInfo damageInfo = new DamageInfo();
            if (collision.gameObject.CompareTag("Scene"))
            {
                damageInfo.Hull = 3000;
                Vector3 inDirection = -collision.relativeVelocity;
                Vector3 normal = collision.contacts[0].normal;
                Vector3 outDirection = Vector3.Reflect(inDirection, normal);
                transform.forward = outDirection;
                GetComponent<AvionicsSystem>().mainRot = transform.rotation;
                myRigidbody.AddForce(outDirection * collision.impulse.magnitude);
            }
            else
                damageInfo.Hull = Mathf.Clamp((int)Mathf.Abs(collision.impulse.magnitude), 0, 1000);
            damageInfo.Shield = 0;

            // 記錄攻擊者
            damageInfo.Attacker = new Kocmonaut { Number = -1 };
            KocmocraftMechDroid collisionKocmocraft = collision.gameObject.GetComponent<KocmocraftMechDroid>();
            if (collisionKocmocraft)
            {
                damageInfo.Attacker.Core = collisionKocmocraft.Core;
                damageInfo.Attacker.Number = collisionKocmocraft.Number;
            }
            else
            {
                damageInfo.Attacker.Core = Core;
                damageInfo.Attacker.Number = Number;
            }
            Hit(damageInfo);
        }

        private void OnCollisionExit(Collision collision)
        {
            // 確保撞擊後恢復一般剛體
            myRigidbody.isKinematic = true;
            myRigidbody.isKinematic = false;
        }
    }
}