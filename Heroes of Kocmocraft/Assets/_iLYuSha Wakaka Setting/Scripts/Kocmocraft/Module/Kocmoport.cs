/***************************************************************************
 * Kocmoport
 * 宇航站
 * Last Updated: 2019/05/26
 * 
 * v19.0601
 * 1. 由于Photon的TransformView与RigidbodyView必须与PhotonView在同一个物件上，故前次的想法无法实现
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
 * 6. m_Port為20架宇航機的序號為0~19，可轉換為
 *     陣營代碼 = m_Port%2
 *     成員代碼 = m_Port/2
 *     宇航基地代碼 = m_Port%4
 *     宇航機庫代碼 = m_Port/4
 ***************************************************************************/
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

namespace Kocmoca
{
    [RequireComponent (typeof (PhotonView))]
    public class Kocmoport : MonoBehaviourPun
    {
        [Header ("Kocmoport 宇航站")]
        [Tooltip ("宇航機")]
        public GameObject[] kocmocraft;
        [Tooltip ("控制單元")]
        public ControlUnit m_Core;
        [Tooltip ("宇航站编号00~99")]
        public int m_Port;
        [Tooltip ("阵营")]
        public int m_Faction;
        [Tooltip ("宇航机型号")]
        public int m_Type;
        [Tooltip ("宇航机塗裝")]
        public int m_Skin;
        [Tooltip ("宇航机编号")]
        public int m_Number;
        private Vector3 portalPos;
        private Quaternion portalRot;

        public AvionicsSystem myAvionicsSystem;

        public Transform rootTransform;
        public Rigidbody rootRigidbody;

        // Basic Data

        [Header ("Weapon Manager")]
        public FireControlSystem[] listFCS;
        public int countFCS;
        public int currentFCS;
        private ObjectPoolData[] listAmmoOPD;

        /// <summary>
        /// Reset is called when the user hits the Reset button in the Inspector's
        /// context menu or when adding the component the first time.
        /// </summary>
        void Reset ()
        {
            rootTransform = transform;
            rootRigidbody = GetComponent<Rigidbody> ();
            rootRigidbody.isKinematic = true;
        }
        public void CreatePlayerKocmoport ()
        {
            photonView.RPC ("SynchronizePlayerKocmoport", RpcTarget.Others);
            m_Core = ControlUnit.LocalPlayer;
            m_Port = photonView.Owner.GetPlayerNumber ();
            m_Faction = m_Port % 2;
            m_Type = (int) photonView.Owner.CustomProperties[LobbyInfomation.PROPERTY_TYPE];
            m_Skin = (int) photonView.Owner.CustomProperties[LobbyInfomation.PROPERTY_SKIN];
            m_Number = photonView.Owner.ActorNumber;
            name = "[Player] " + photonView.Owner.NickName;
            portalPos = SatelliteCommander.portalPos[m_Port];
            portalRot = SatelliteCommander.portalRot[m_Port];
            Takeoff ();
        }
        public void CreateBotKocmoport (int port, int type)
        {
            m_Core = ControlUnit.LocalBot;
            m_Port = port;
            m_Faction = m_Port % 2;
            m_Type = type;
            m_Skin = kocmocraft[m_Type].GetComponentInChildren<Prototype> ().GetRandomSkinIndex ();
            photonView.RPC ("SynchronizeBotPlayerKocmoport", RpcTarget.Others, m_Port, m_Type, m_Skin);
            m_Number = port * 1000;
            name = KocmocraftData.GetBotName (m_Port);
            portalPos = SatelliteCommander.portalPos[m_Port];
            portalRot = SatelliteCommander.portalRot[m_Port];
        }

        [PunRPC]
        public void SynchronizePlayerKocmoport (PhotonMessageInfo info)
        {
            m_Core = ControlUnit.RemotePlayer;
            m_Port = photonView.Owner.GetPlayerNumber ();
            m_Faction = m_Port % 2;
            m_Type = (int) photonView.Owner.CustomProperties[LobbyInfomation.PROPERTY_TYPE];
            m_Skin = (int) photonView.Owner.CustomProperties[LobbyInfomation.PROPERTY_SKIN];
            m_Number = photonView.Owner.ActorNumber;
            name = "[Remote] " + photonView.Owner.NickName;
            portalPos = SatelliteCommander.portalPos[m_Port];
            portalRot = SatelliteCommander.portalRot[m_Port];
        }

        [PunRPC]
        public void SynchronizeBotPlayerKocmoport (int port, int type, int skin, PhotonMessageInfo info)
        {
            m_Core = ControlUnit.RemoteBot;
            m_Port = port;
            m_Faction = m_Port % 2;
            m_Type = type;
            m_Skin = skin;
            m_Number = m_Port * 1000;
            name = KocmocraftData.GetBotName (m_Port);
            portalPos = SatelliteCommander.portalPos[m_Port];
            portalRot = SatelliteCommander.portalRot[m_Port];
            Takeoff ();
        }

        [PunRPC]
        public void Takeoff ()
        {
            GameObject go = Instantiate (kocmocraft[m_Type], rootTransform);
            go.GetComponentInChildren<Prototype> ().LoadSkin (m_Skin);
            // ObserverCamera
            SatelliteCommander.Instance.AddSearchArray (rootTransform, m_Port / 2, (int) m_Faction, m_Number);
            // Kocmonaut Info ( first time only )
            if (!SatelliteCommander.Instance.listKocmonaut.ContainsKey (m_Number))
                SatelliteCommander.Instance.NewKocmonautJoin (ControlUnit.LocalPlayer, m_Port, (Type) m_Type, m_Number, name);

            GetComponentInChildren<AvionicsSystem> ().Active (rootTransform, rootRigidbody, photonView, m_Core, m_Number);
            bool isLocal;
            if (m_Core == ControlUnit.LocalPlayer || m_Core == ControlUnit.LocalBot)
            {
                isLocal = true;
                rootTransform.SetPositionAndRotation (portalPos, portalRot);
                if (m_Core == ControlUnit.LocalPlayer)
                {
                    go.AddComponent<LocalPlayerController> ();
                    GetComponentInChildren<OnboardRadar> ().Active (rootTransform, photonView, m_Faction, m_Number, true);
                    FindObjectOfType<HeadUpDisplayManager> ().InitializeHUD (rootTransform);
                    LocalPlayerRealtimeData.Number = m_Number;
                    LocalPlayerRealtimeData.Faction = SatelliteCommander.Instance.listKocmonaut[m_Number].Faction;
                }
                else
                {
                    go.AddComponent<LocalBotController> ();
                    GetComponentInChildren<OnboardRadar> ().Active (rootTransform, photonView, m_Faction, m_Number, false);
                }
            }
            else
            {
                isLocal = false;
                Destroy (GetComponentInChildren<OnboardRadar> ());
            }
            myAvionicsSystem = GetComponentInChildren<AvionicsSystem> ();
            turretPos = myAvionicsSystem.myTurretFCS.ActiveTurret (photonView, m_Number, isLocal);
            rocketHardpointPos = myAvionicsSystem.myRocketFCS.ActiveHardpoint (photonView, m_Number, isLocal);
            missileHardpointPos = myAvionicsSystem.myRocketFCS.ActiveHardpoint (photonView, m_Number, isLocal);
            listAmmoOPD = ResourceManager.instance.listAmmoOPD;
            // 記錄子彈 也記錄音效
            rootRigidbody.isKinematic = false;
        }

        private Vector3[] turretPos;
        private Vector3 rocketHardpointPos;
        private Vector3 missileHardpointPos;
        private void ActiveFCS (bool isLocal)
        {

        }
        public void LaunchWeapon ()
        {
            listFCS[currentFCS].Shoot ();
        }
        public int SwitchWeapon ()
        {
            currentFCS++;
            currentFCS = (int) Mathf.Repeat (currentFCS, countFCS);
            return currentFCS;
        }
        public int SwitchWeapon (int index)
        {
            currentFCS = index;
            return currentFCS;
        }

        [PunRPC]
        public void RadarLockOn (int targetNumber, int radarNumber, bool isLockOn, PhotonMessageInfo info)
        {
            if (targetNumber == PhotonNetwork.LocalPlayer.ActorNumber) // Radar warning receiver RWR 是否為本地端玩家
            {
                //Debug.LogWarning(myTransform.name + "/" + radarNumber + "/" + Time.frameCount + "/"+ isLockOn + "/");
                SatelliteCommander.Instance.RadarLockOnWarning (isLockOn, radarNumber);
            }
        }

        [PunRPC]
        public void TurretFire (int index, int shooterNumber, int targetNumber, float spread, PhotonMessageInfo info)
        {
            listAmmoOPD[0].ReuseAmmo (rootTransform.TransformPoint (turretPos[index]), rootTransform.rotation).GetComponent<Ammo> ().InputAmmoData (shooterNumber, targetNumber, rootRigidbody.velocity, spread);
        }

        [PunRPC]
        public void RockeLaunch (int shooterNumber, int targetNumber, PhotonMessageInfo info)
        {
            listAmmoOPD[1].Reuse (rootTransform.TransformPoint (rocketHardpointPos), rootTransform.rotation).GetComponent<Ammo> ().InputAmmoData (shooterNumber, targetNumber, rootRigidbody.velocity, 0);
        }

        [PunRPC]
        public void MissileLaunch (int shooterNumber, int targetNumber, PhotonMessageInfo info)
        {
            listAmmoOPD[2].Reuse (rootTransform.TransformPoint (missileHardpointPos), rootTransform.rotation).GetComponent<Ammo> ().InputAmmoData (shooterNumber, targetNumber, rootRigidbody.velocity, 0);
        }

        [PunRPC]
        public void Crash (int stealerNumber, PhotonMessageInfo info)
        {
            GetComponentInChildren<AvionicsSystem> ().Crash (stealerNumber, info);
        }

        [PunRPC]
        public void ShowHitDamage (int shooterNumber, int damage, PhotonMessageInfo info)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == shooterNumber)
                HeadUpDisplayManager.Instance.ShowHitDamage (rootRigidbody.position, damage);
        }

        public void InitializeLocalPlayer ()
        {
            //Controller.ChangeMode(ControlMode.Flying);
            // Load Data
            m_Port = photonView.Owner.GetPlayerNumber ();
            m_Faction = m_Port % 2;
            m_Type = (int.Parse (name.Split (new char[2]
            {
                '(',
                ')'
            }) [1]));
            m_Number = photonView.Owner.ActorNumber;
            object indexSkin;
            if (photonView.Owner.CustomProperties.TryGetValue (LobbyInfomation.PLAYER_SKIN_OPTION, out indexSkin))
            {
                GetComponentInChildren<Prototype> ().LoadSkin ((int) indexSkin);
            }

            name = photonView.Owner.NickName + "-" +
                DesignData.Code[(int) m_Type] + "-" +
                DesignData.Project[(int) m_Type];
            // Add Search List
            SatelliteCommander.Instance.AddSearchArray (rootTransform, m_Port / 2, (int) m_Faction, m_Number);
            // Kocmonaut Info ( first time only )
            if (!SatelliteCommander.Instance.listKocmonaut.ContainsKey (m_Number))
                SatelliteCommander.Instance.NewKocmonautJoin (ControlUnit.LocalPlayer, m_Port, (Type) m_Type, m_Number, name);
            // Hangar Info ( local only )
            SatelliteCommander.Instance.SetHangar (rootTransform, m_Port);
            // Upagrader
            //Destroy(gameObject.GetComponent<HarmonicMotion>());
            //gameObject.AddComponent<AvionicsSystem>().Initialize(module.rootTransform[(int)Type], (int)Type); // local only
            //GetComponentInChildren<AvionicsSystem>().Active(ControlUnit.LocalPlayer, rootTransform, rootRigidbody, photonView, m_Number, pilot);
            SatelliteCommander.radarManager[m_Port] = GetComponentInChildren<OnboardRadar> ();
            //SatelliteCommander.radarManager[m_Port].Initialize(ControlUnit.LocalPlayer, (int)m_Faction, (int)m_Type, m_Number); // local only
            gameObject.AddComponent<LocalPlayerController> ();
            ActiveFCS (true);
            // Synchronize
            photonView.RPC ("SynchronizePlayerKocmocraft", RpcTarget.Others);
            // Local Data - Initialize
            FindObjectOfType<HeadUpDisplayManager> ().InitializeHUD (rootTransform);
            LocalPlayerRealtimeData.Number = m_Number;
            LocalPlayerRealtimeData.Faction = SatelliteCommander.Instance.listKocmonaut[m_Number].Faction;
        }

        [PunRPC]
        public void SynchronizePlayerKocmocraft (PhotonMessageInfo info)
        {
            // Load Data
            m_Port = photonView.Owner.GetPlayerNumber ();
            m_Faction = m_Port % 2;
            m_Type = (int.Parse (name.Split (new char[2]
            {
                '(',
                ')'
            }) [1]));
            m_Number = photonView.Owner.ActorNumber;

            object indexSkin;
            if (photonView.Owner.CustomProperties.TryGetValue (LobbyInfomation.PLAYER_SKIN_OPTION, out indexSkin))
            {
                GetComponentInChildren<Prototype> ().LoadSkin ((int) indexSkin);
            }

            name = photonView.Owner.NickName + "-" +
                DesignData.Code[(int) m_Type] + "-" +
                DesignData.Project[(int) m_Type];
            // Add Search List
            SatelliteCommander.Instance.AddSearchArray (rootTransform, m_Port / 2, (int) m_Faction, m_Number);
            // Kocmonaut Info ( first time only )
            if (!SatelliteCommander.Instance.listKocmonaut.ContainsKey (m_Number))
                SatelliteCommander.Instance.NewKocmonautJoin (ControlUnit.RemotePlayer, m_Port, (Type) m_Type, m_Number, name);
            // Upagrader
            Destroy (gameObject.GetComponent<HarmonicMotion> ());
            //GetComponentInChildren<AvionicsSystem>().Active(ControlUnit.RemotePlayer, rootTransform, rootRigidbody, photonView, m_Number, pilot);
            ActiveFCS (false);
        }
        public void InitializeLocalBot (int port)
        {
            m_Port = port;
            // Load Data
            m_Faction = m_Port % 2;
            m_Type = (int.Parse (name.Split (new char[2]
            {
                '(',
                ')'
            }) [1]));
            m_Number = KocmocraftData.GetKocmonautNumber (m_Port);
            GetComponentInChildren<Prototype> ().RandomSkin ();
            name = KocmocraftData.GetBotName (m_Port) + "-" +
                DesignData.Code[(int) m_Type] + "-" +
                DesignData.Project[(int) m_Type];
            // Add Search List
            SatelliteCommander.Instance.AddSearchArray (rootTransform, m_Port / 2, (int) m_Faction, m_Number);
            // Kocmonaut Info ( first time only )
            if (!SatelliteCommander.Instance.listKocmonaut.ContainsKey (m_Number))
                SatelliteCommander.Instance.NewKocmonautJoin (ControlUnit.LocalBot, m_Port, (Type) m_Type, m_Number, name);
            // Hangar Info ( local only )
            SatelliteCommander.Instance.SetHangar (rootTransform, m_Port);
            // Upagrader
            Destroy (gameObject.GetComponent<HarmonicMotion> ());
            //gameObject.AddComponent<AvionicsSystem>().Initialize(module.rootTransform[(int)Type], (int)Type); // local only
            //GetComponentInChildren<AvionicsSystem>().Active(ControlUnit.LocalBot, rootTransform, rootRigidbody, photonView, m_Number, pilot);
            SatelliteCommander.radarManager[m_Port] = GetComponentInChildren<OnboardRadar> ();
            //SatelliteCommander.radarManager[m_Port].Initialize(ControlUnit.LocalBot, (int)m_Faction, (int)m_Type, m_Number); // local only
            gameObject.AddComponent<LocalBotController> ();
            //if (m_Type == Type.VladimirPutin || m_Type == Type.Cuckoo || m_Type == Type.PapoyUnicorn || m_Type == Type.PumpkinGhost)
            //    GetComponent<LocalBotController>().mission = Mission.Snipper;
            //else if (m_Type == Type.MinionArmor || m_Type == Type.PaperAeroplane || m_Type == Type.BulletBill || m_Type == Type.TimeMachine || m_Type == Type.nWidia)
            //    GetComponent<LocalBotController>().mission = Mission.Battle;
            ActiveFCS (true);
            photonView.RPC ("SynchronizeBotKocmocraft", RpcTarget.Others, m_Port);
        }

        [PunRPC]
        public void SynchronizeBotKocmocraft (int port, PhotonMessageInfo info)
        {
            // Load Data
            m_Port = port;
            m_Faction = m_Port % 2;
            m_Type = (int.Parse (name.Split (new char[2]
            {
                '(',
                ')'
            }) [1]));
            m_Number = KocmocraftData.GetKocmonautNumber (m_Port);
            name = KocmocraftData.GetBotName (m_Port) + "-" +
                DesignData.Code[(int) m_Type] + "-" +
                DesignData.Project[(int) m_Type];
            // Add Search List
            SatelliteCommander.Instance.AddSearchArray (rootTransform, m_Port / 2, (int) m_Faction, m_Number);
            // Kocmonaut Info ( first time only )
            if (!SatelliteCommander.Instance.listKocmonaut.ContainsKey (m_Number))
                SatelliteCommander.Instance.NewKocmonautJoin (ControlUnit.RemoteBot, m_Port, (Type) m_Type, m_Number, name);
            // Upagrader
            Destroy (gameObject.GetComponent<HarmonicMotion> ());
            //GetComponentInChildren<AvionicsSystem>().Active(ControlUnit.RemoteBot, rootTransform,rootRigidbody, photonView,m_Number, pilot );
            ActiveFCS (false);
        }

    }
}