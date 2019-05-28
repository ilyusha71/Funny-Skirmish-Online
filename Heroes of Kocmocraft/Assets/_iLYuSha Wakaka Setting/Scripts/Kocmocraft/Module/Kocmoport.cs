/***************************************************************************
 * Kocmoport
 * 宇航站
 * Last Updated: 2019/05/26
 * 
 * v19.0526
 * 1. Kocmocraft Commander => Kocmoport
 * 2. 本脚本升级为宇航站（Kocmoport），由卫星指挥官（SatelliteCommander）控管所有宇航站，目前为100座。
 * 3. 原宇航机所有模组脚本（Module）移至下层，宇航机层（Kocmocraft），目前为Hangar层。
 * 4. Kocmoport只包含PhotonView，负责该Port的宇航机管理
 * 5. 所有宇航机的RPC接收端移至Kocmoport，负责宇航机重生与销毁。
 * 6. 以上为预计实现功能
 * 
 * v19.0518
 * 1. KocmocraftManager => Kocmocraft Commander
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
 * 6. port為20架宇航機的序號為0~19，可轉換為
 *     陣營代碼 = port%2
 *     成員代碼 = port/2
 *     宇航基地代碼 = port%4
 *     宇航機庫代碼 = port/4
 ***************************************************************************/
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

namespace Kocmoca
{
    [RequireComponent(typeof(PhotonView))]
    [RequireComponent(typeof(PhotonTransformView))]
    //[RequireComponent(typeof(PhotonRigidbodyView))]
    public class Kocmoport : MonoBehaviourPun
    {
        [Header("Kocmoport 宇航站")]
        [Tooltip("宇航站编号00~99")]
        public int port;
        [Tooltip("阵营")]
        public Faction Faction;
        [Tooltip("宇航机型号")]
        public Type Type;
        [Tooltip("宇航机编号")]
        public int Number;




        // 暂时
        public KocmocraftDatabase module;

        // Dependent Components
        private Transform kocmocraft;
        private Rigidbody rigidbody;

        // Basic Data

        [Header("Weapon Manager")]
        public FireControlSystem[] listFCS;
        public int countFCS;
        public int currentFCS;
        private ObjectPoolData[] listAmmoOPD;
        [Header("Child Node")]
        public GameObject pilot;
        public GameObject wreckage;


        public void InitializeLocalPlayer()
        {
            kocmocraft = GetComponentInChildren<KocmocraftManager>().transform;
            rigidbody = GetComponentsInChildren<Rigidbody>()[1];
            //Controller.ChangeMode(ControlMode.Flying);
            // Load Data
            port = photonView.Owner.GetPlayerNumber();
            Faction = (Faction)(port % 2);
            Type = (Type)(int.Parse(name.Split(new char[2] { '(', ')' })[1]));
            Number = photonView.Owner.ActorNumber;
            object indexSkin;
            if (photonView.Owner.CustomProperties.TryGetValue(LobbyInfomation.PLAYER_SKIN_OPTION, out indexSkin))
            {
                GetComponentInChildren<Prototype>().LoadSkin((int)indexSkin);
            }

            name = photonView.Owner.NickName + "-" +
                DesignData.Code[(int)Type] + "-" +
                DesignData.Project[(int)Type];
            // Add Search List
            SatelliteCommander.Instance.AddSearchArray(kocmocraft, port / 2, (int)Faction, Number);
            // Kocmonaut Info ( first time only )
            if (!SatelliteCommander.Instance.listKocmonaut.ContainsKey(Number))
                SatelliteCommander.Instance.NewKocmonautJoin(Core.LocalPlayer, port, Type, Number, name);
            // Hangar Info ( local only )
            SatelliteCommander.Instance.SetHangar(kocmocraft, port);
            // Upagrader
            Destroy(gameObject.GetComponent<HarmonicMotion>());
            //gameObject.AddComponent<AvionicsSystem>().Initialize(module.kocmocraft[(int)Type], (int)Type); // local only
            GetComponentInChildren<KocmocraftManager>().Initialize(module.kocmocraft[(int)Type],Core.LocalPlayer, (int)Type, Number, pilot,wreckage);
            SatelliteCommander.radarManager[port] = GetComponentInChildren<OnboardRadar>();
            SatelliteCommander.radarManager[port].Initialize(Core.LocalPlayer, (int)Faction, (int)Type, Number); // local only
            gameObject.AddComponent<LocalPlayerController>();
            ActiveFCS(true);
            // Synchronize
            photonView.RPC("SynchronizePlayerKocmocraft", RpcTarget.Others);
            // Local Data - Initialize
            FindObjectOfType<HeadUpDisplayManager>().InitializeHUD(kocmocraft);
            LocalPlayerRealtimeData.Number = Number;
            LocalPlayerRealtimeData.Faction = SatelliteCommander.Instance.listKocmonaut[Number].Faction;
        }
        [PunRPC]
        public void SynchronizePlayerKocmocraft(PhotonMessageInfo info)
        {
            kocmocraft = GetComponentInChildren<KocmocraftManager>().transform;
            rigidbody = GetComponentsInChildren<Rigidbody>()[1];
            // Load Data
            port = photonView.Owner.GetPlayerNumber();
            Faction = (Faction)(port % 2);
            Type = (Type)(int.Parse(name.Split(new char[2] { '(', ')' })[1]));
            Number = photonView.Owner.ActorNumber;

            object indexSkin;
            if (photonView.Owner.CustomProperties.TryGetValue(LobbyInfomation.PLAYER_SKIN_OPTION, out indexSkin))
            {
                GetComponentInChildren<Prototype>().LoadSkin((int)indexSkin);
            }

            name = photonView.Owner.NickName + "-" +
                DesignData.Code[(int)Type] + "-" +
                DesignData.Project[(int)Type];
            // Add Search List
            SatelliteCommander.Instance.AddSearchArray(kocmocraft, port / 2, (int)Faction, Number);
            // Kocmonaut Info ( first time only )
            if (!SatelliteCommander.Instance.listKocmonaut.ContainsKey(Number))
                SatelliteCommander.Instance.NewKocmonautJoin(Core.RemotePlayer, port, Type, Number, name);
            // Upagrader
            Destroy(gameObject.GetComponent<HarmonicMotion>());
            GetComponentInChildren<KocmocraftManager>().Initialize(module.kocmocraft[(int)Type], Core.RemotePlayer, (int)Type, Number, pilot, wreckage);
            ActiveFCS(false);
        }
        public void InitializeLocalBot(int port)
        {
            kocmocraft = GetComponentInChildren<KocmocraftManager>().transform;
            rigidbody = GetComponentsInChildren<Rigidbody>()[1];
            // Load Data
            Faction = (Faction)(port % 2);
            Type = (Type)(int.Parse(name.Split(new char[2] { '(', ')' })[1]));
            Number = KocmocraftData.GetKocmonautNumber(port);
            GetComponentInChildren<Prototype>().RandomSkin();
            name = KocmocraftData.GetBotName(port) + "-" +
                DesignData.Code[(int)Type] + "-" +
                DesignData.Project[(int)Type];
            // Add Search List
            SatelliteCommander.Instance.AddSearchArray(kocmocraft, port / 2, (int)Faction, Number);
            // Kocmonaut Info ( first time only )
            if (!SatelliteCommander.Instance.listKocmonaut.ContainsKey(Number))
                SatelliteCommander.Instance.NewKocmonautJoin(Core.LocalBot, port, Type, Number, name);
            // Hangar Info ( local only )
            SatelliteCommander.Instance.SetHangar(kocmocraft, port);
            // Upagrader
            Destroy(gameObject.GetComponent<HarmonicMotion>());
            //gameObject.AddComponent<AvionicsSystem>().Initialize(module.kocmocraft[(int)Type], (int)Type); // local only
            GetComponentInChildren<KocmocraftManager>().Initialize(module.kocmocraft[(int)Type], Core.LocalBot, (int)Type, Number, pilot, wreckage);
            SatelliteCommander.radarManager[port] = GetComponentInChildren<OnboardRadar>();
            SatelliteCommander.radarManager[port].Initialize(Core.LocalBot, (int)Faction, (int)Type, Number); // local only
            gameObject.AddComponent<LocalBotController>();
            if (Type == Type.VladimirPutin || Type == Type.Cuckoo || Type == Type.PapoyUnicorn || Type == Type.PumpkinGhost)
                GetComponent<LocalBotController>().mission = Mission.Snipper;
            else if (Type == Type.MinionArmor || Type == Type.PaperAeroplane || Type == Type.BulletBill || Type == Type.TimeMachine || Type == Type.nWidia)
                GetComponent<LocalBotController>().mission = Mission.Battle;
            ActiveFCS(true);
            photonView.RPC("SynchronizeBotKocmocraft", RpcTarget.Others, port);
        }
        [PunRPC]
        public void SynchronizeBotKocmocraft(int port, PhotonMessageInfo info)
        {
            kocmocraft = GetComponentInChildren<KocmocraftManager>().transform;
            rigidbody = GetComponentsInChildren<Rigidbody>()[1];
            // Load Data
            Faction = (Faction)(port % 2);
            Type = (Type)(int.Parse(name.Split(new char[2] { '(', ')' })[1]));
            Number = KocmocraftData.GetKocmonautNumber(port);
            name = KocmocraftData.GetBotName(port) + "-" +
                DesignData.Code[(int)Type] + "-" +
                DesignData.Project[(int)Type];
            // Add Search List
            SatelliteCommander.Instance.AddSearchArray(kocmocraft, port / 2, (int)Faction, Number);
            // Kocmonaut Info ( first time only )
            if (!SatelliteCommander.Instance.listKocmonaut.ContainsKey(Number))
                SatelliteCommander.Instance.NewKocmonautJoin(Core.RemoteBot, port, Type, Number, name);
            // Upagrader
            Destroy(gameObject.GetComponent<HarmonicMotion>());
            GetComponentInChildren<KocmocraftManager>().Initialize(module.kocmocraft[(int)Type], Core.RemoteBot, (int)Type, Number, pilot, wreckage);
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
            listAmmoOPD[0].ReuseAmmo(kocmocraft.TransformPoint(listFCS[0].launcher[muzzle]), kocmocraft.rotation).GetComponent<Ammo>().InputAmmoData(numberShooter, numberTarget, rigidbody.velocity, spread);
        }
        [PunRPC]
        public void RockeLaunch(int muzzle, int numberShooter, int numberTarget, PhotonMessageInfo info)
        {
            listAmmoOPD[1].Reuse(kocmocraft.TransformPoint(listFCS[1].launcher[muzzle]), kocmocraft.rotation).GetComponent<Ammo>().InputAmmoData(numberShooter, numberTarget, rigidbody.velocity, 0);
        }
        [PunRPC]
        public void MissileLaunch(int muzzle, int numberShooter, int numberTarget, PhotonMessageInfo info)
        {
            listAmmoOPD[2].Reuse(kocmocraft.TransformPoint(listFCS[2].launcher[muzzle]), kocmocraft.rotation).GetComponent<Ammo>().InputAmmoData(numberShooter, numberTarget, rigidbody.velocity, 0);
        }
        [PunRPC]
        public void Crash(int stealerNumber, PhotonMessageInfo info)
        {
            GetComponentInChildren<KocmocraftManager>().Crash(stealerNumber, info);
        }
    }
}
