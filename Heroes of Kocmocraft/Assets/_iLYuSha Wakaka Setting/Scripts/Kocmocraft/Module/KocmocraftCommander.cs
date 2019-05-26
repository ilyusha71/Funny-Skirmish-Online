﻿/***************************************************************************
 * Kocmocraft Commander
 * 宇航機指揮官
 * Last Updated: 2019/05/18
 * 
 * v19.0518
 * 1. KocmocraftManager => KocmocraftCommander
 * 2. 簡化AddComponent與賦值等繁瑣工作，
 *     一些Data的初始化將交由下層直接使用Reset進行Inspector初始化
 * 3. 主要用於Photon網路連線控制與作戰相關組件的初始化（AvionicsSystem、OnboardRadar.cs）
 * 
 * v18.0922
 * 1. 原版Aircraft由AEM生成Instance再AddComponent
 * 2. 新版Kocmocraft由此腳本進行AddComponent動作
 * 3. Kocmocraft Manager相當於本地端Photon View功能
 * 4. 整合WeaponManager
 * 5. 原FCS所發射的各類彈藥將通過RPC由本腳本調用下列方法
 *     激光：[PunRPC]LaserShoot()
 *     火箭：[PunRPC]RockeLaunch()
 *     飛彈：[PunRPC]MissileLaunch()
 * 6. portNumber為20架宇航機的序號為0~19，可轉換為
 *     陣營代碼 = portNumber%2
 *     成員代碼 = portNumber/2
 *     宇航基地代碼 = portNumber%4
 *     宇航機庫代碼 = portNumber/4
 ***************************************************************************/
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

namespace Kocmoca
{
    [RequireComponent(typeof(PhotonView))]
    [RequireComponent(typeof(PhotonTransformView))]
    //[RequireComponent(typeof(PhotonRigidbodyView))]
    public class KocmocraftCommander : MonoBehaviourPun
    {
        public KocmocraftDatabase module;

        // Dependent Components
        private Transform myTransform;
        private Rigidbody myRigidbody;
        // Basic Data
        private int portNumber;
        public Faction Faction { get; protected set; }
        public Type Type { get; protected set; }
        public int Number { get; protected set; }
        [Header("Weapon Manager")]
        public FireControlSystem[] listFCS;
        public int countFCS;
        public int currentFCS;
        private ObjectPoolData[] listAmmoOPD;
        [Header("Child Node")]
        public GameObject pilot;
        public GameObject wreckage;

        private void Awake()
        {
            myTransform = transform;
            myRigidbody = GetComponent<Rigidbody>();
        }

        public void InitializeLocalPlayer()
        {
            //Controller.ChangeMode(ControlMode.Flying);
            // Load Data
            portNumber = photonView.Owner.GetPlayerNumber();
            Faction = (Faction)(portNumber % 2);
            Type = (Type)(int.Parse(myTransform.name.Split(new char[2] { '(', ')' })[1]));
            Number = photonView.Owner.ActorNumber;
            object indexSkin;
            if (photonView.Owner.CustomProperties.TryGetValue(LobbyInfomation.PLAYER_SKIN_OPTION, out indexSkin))
            {
                GetComponentInChildren<Prototype>().LoadSkin((int)indexSkin);
            }

            myTransform.name = photonView.Owner.NickName + "-" +
                DesignData.Code[(int)Type] + "-" +
                DesignData.Project[(int)Type];
            // Add Search List
            SatelliteCommander.Instance.AddSearchArray(myTransform, portNumber / 2, (int)Faction, Number);
            // Kocmonaut Info ( first time only )
            if (!SatelliteCommander.Instance.listKocmonaut.ContainsKey(Number))
                SatelliteCommander.Instance.NewKocmonautJoin(Core.LocalPlayer, portNumber, Type, Number, myTransform.name);
            // Hangar Info ( local only )
            SatelliteCommander.Instance.SetHangar(myTransform, portNumber);
            // Upagrader
            Destroy(gameObject.GetComponent<HarmonicMotion>());
            gameObject.AddComponent<AvionicsSystem>().Initialize(module.kocmocraft[(int)Type], (int)Type); // local only
            gameObject.AddComponent<KocmocraftMechDroid>().Initialize(module.kocmocraft[(int)Type],Core.LocalPlayer, (int)Type, Number, pilot,wreckage);
            SatelliteCommander.radarManager[portNumber] = gameObject.AddComponent<OnboardRadar>();
            SatelliteCommander.radarManager[portNumber].Initialize(Core.LocalPlayer, (int)Faction, (int)Type, Number); // local only
            gameObject.AddComponent<LocalPlayerController>();
            ActiveFCS(true);
            // Synchronize
            photonView.RPC("SynchronizePlayerKocmocraft", RpcTarget.Others);
            // Local Data - Initialize
            FindObjectOfType<HeadUpDisplayManager>().InitializeHUD(myTransform);
            LocalPlayerRealtimeData.Number = Number;
            LocalPlayerRealtimeData.Faction = SatelliteCommander.Instance.listKocmonaut[Number].Faction;
        }
        [PunRPC]
        public void SynchronizePlayerKocmocraft(PhotonMessageInfo info)
        {
            // Load Data
            portNumber = photonView.Owner.GetPlayerNumber();
            Faction = (Faction)(portNumber % 2);
            Type = (Type)(int.Parse(myTransform.name.Split(new char[2] { '(', ')' })[1]));
            Number = photonView.Owner.ActorNumber;

            object indexSkin;
            if (photonView.Owner.CustomProperties.TryGetValue(LobbyInfomation.PLAYER_SKIN_OPTION, out indexSkin))
            {
                GetComponentInChildren<Prototype>().LoadSkin((int)indexSkin);
            }

            myTransform.name = photonView.Owner.NickName + "-" +
                DesignData.Code[(int)Type] + "-" +
                DesignData.Project[(int)Type];
            // Add Search List
            SatelliteCommander.Instance.AddSearchArray(myTransform, portNumber / 2, (int)Faction, Number);
            // Kocmonaut Info ( first time only )
            if (!SatelliteCommander.Instance.listKocmonaut.ContainsKey(Number))
                SatelliteCommander.Instance.NewKocmonautJoin(Core.RemotePlayer, portNumber, Type, Number, myTransform.name);
            // Upagrader
            Destroy(gameObject.GetComponent<HarmonicMotion>());
            gameObject.AddComponent<KocmocraftMechDroid>().Initialize(module.kocmocraft[(int)Type], Core.RemotePlayer, (int)Type, Number, pilot, wreckage);
            ActiveFCS(false);
        }
        public void InitializeLocalBot(int portNumber)
        {
            // Load Data
            Faction = (Faction)(portNumber % 2);
            Type = (Type)(int.Parse(myTransform.name.Split(new char[2] { '(', ')' })[1]));
            Number = KocmocraftData.GetKocmonautNumber(portNumber);
            GetComponentInChildren<Prototype>().RandomSkin();
            myTransform.name = KocmocraftData.GetBotName(portNumber) + "-" +
                DesignData.Code[(int)Type] + "-" +
                DesignData.Project[(int)Type];
            // Add Search List
            SatelliteCommander.Instance.AddSearchArray(myTransform, portNumber / 2, (int)Faction, Number);
            // Kocmonaut Info ( first time only )
            if (!SatelliteCommander.Instance.listKocmonaut.ContainsKey(Number))
                SatelliteCommander.Instance.NewKocmonautJoin(Core.LocalBot, portNumber, Type, Number, myTransform.name);
            // Hangar Info ( local only )
            SatelliteCommander.Instance.SetHangar(myTransform, portNumber);
            // Upagrader
            Destroy(gameObject.GetComponent<HarmonicMotion>());
            gameObject.AddComponent<AvionicsSystem>().Initialize(module.kocmocraft[(int)Type], (int)Type); // local only
            gameObject.AddComponent<KocmocraftMechDroid>().Initialize(module.kocmocraft[(int)Type], Core.LocalBot, (int)Type, Number, pilot, wreckage);
            SatelliteCommander.radarManager[portNumber] = gameObject.AddComponent<OnboardRadar>();
            SatelliteCommander.radarManager[portNumber].Initialize(Core.LocalBot, (int)Faction, (int)Type, Number); // local only
            gameObject.AddComponent<LocalBotController>();
            if (Type == Type.VladimirPutin || Type == Type.Cuckoo || Type == Type.PapoyUnicorn || Type == Type.PumpkinGhost)
                GetComponent<LocalBotController>().mission = Mission.Snipper;
            else if (Type == Type.MinionArmor || Type == Type.PaperAeroplane || Type == Type.BulletBill || Type == Type.TimeMachine || Type == Type.nWidia)
                GetComponent<LocalBotController>().mission = Mission.Battle;
            ActiveFCS(true);
            photonView.RPC("SynchronizeBotKocmocraft", RpcTarget.Others, portNumber);
        }
        [PunRPC]
        public void SynchronizeBotKocmocraft(int portNumber, PhotonMessageInfo info)
        {
            // Load Data
            Faction = (Faction)(portNumber % 2);
            Type = (Type)(int.Parse(myTransform.name.Split(new char[2] { '(', ')' })[1]));
            Number = KocmocraftData.GetKocmonautNumber(portNumber);
            myTransform.name = KocmocraftData.GetBotName(portNumber) + "-" +
                DesignData.Code[(int)Type] + "-" +
                DesignData.Project[(int)Type];
            // Add Search List
            SatelliteCommander.Instance.AddSearchArray(myTransform, portNumber / 2, (int)Faction, Number);
            // Kocmonaut Info ( first time only )
            if (!SatelliteCommander.Instance.listKocmonaut.ContainsKey(Number))
                SatelliteCommander.Instance.NewKocmonautJoin(Core.RemoteBot, portNumber, Type, Number, myTransform.name);
            // Upagrader
            Destroy(gameObject.GetComponent<HarmonicMotion>());
            gameObject.AddComponent<KocmocraftMechDroid>().Initialize(module.kocmocraft[(int)Type], Core.RemoteBot, (int)Type, Number, pilot, wreckage);
            ActiveFCS(false);
        }

        private void ActiveFCS(bool isLocal)
        {

            listFCS = GetComponentsInChildren<FireControlSystem>();
            countFCS = listFCS.Length;
            for (int i = 0; i < countFCS; i++)
            {
                listFCS[i].Active((int)Type, Number, isLocal);
            }
            listAmmoOPD = ResourceManager.instance.listAmmoOPD;
        }
        public void LaunchWeapon()
        {
            listFCS[currentFCS].Shoot();
        }
        public int SwitchWeapon()
        {
            currentFCS++;
            currentFCS = (int)Mathf.Repeat(currentFCS, countFCS);
            return currentFCS;
        }
        public int SwitchWeapon(int index)
        {
            currentFCS = index;
            return currentFCS;
        }
        [PunRPC]
        public void RadarLockOn(int targetNumber, int radarNumber, bool isLockOn, PhotonMessageInfo info)
        {
            if (targetNumber == PhotonNetwork.LocalPlayer.ActorNumber) // Radar warning receiver RWR 是否為本地端玩家
            {
                //Debug.LogWarning(myTransform.name + "/" + radarNumber + "/" + Time.frameCount + "/"+ isLockOn + "/");
                SatelliteCommander.Instance.RadarLockOnWarning(isLockOn, radarNumber);
            }
        }
        [PunRPC]
        public void LaserShoot(int muzzle, int numberShooter, int numberTarget, float spread, PhotonMessageInfo info)
        {
            listAmmoOPD[0].ReuseAmmo(myTransform.TransformPoint(listFCS[0].launcher[muzzle]), myTransform.rotation).GetComponent<Ammo>().InputAmmoData(numberShooter, numberTarget, myRigidbody.velocity, spread);
        }
        [PunRPC]
        public void RockeLaunch(int muzzle, int numberShooter, int numberTarget, PhotonMessageInfo info)
        {
            listAmmoOPD[1].Reuse(myTransform.TransformPoint(listFCS[1].launcher[muzzle]), myTransform.rotation).GetComponent<Ammo>().InputAmmoData(numberShooter, numberTarget, myRigidbody.velocity, 0);
        }
        [PunRPC]
        public void MissileLaunch(int muzzle, int numberShooter, int numberTarget, PhotonMessageInfo info)
        {
            listAmmoOPD[2].Reuse(myTransform.TransformPoint(listFCS[2].launcher[muzzle]), myTransform.rotation).GetComponent<Ammo>().InputAmmoData(numberShooter, numberTarget, myRigidbody.velocity, 0);
        }
    }
}
