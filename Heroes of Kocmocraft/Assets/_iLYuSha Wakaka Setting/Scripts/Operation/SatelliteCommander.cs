/***************************************************************************
 * Satellite Commander
 * 衛星指揮官
 * Last Updated: 2018/09/22
 * Description:
 * 1. AirEarylyWarnint -> Satellite Commander
 * 2. 管理宇航機生成、銷毀
 ***************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

namespace Kocmoca
{
    public struct FactionData
    {
        // 陣營數據
        public float valueAirSupremacy;
        public float valueAirSupremacyPercentage;
        public int countPilot;
        //public List<Transform> listFriend; // 友機列表
        //public List<Transform> listFoe; // 敵機列表

        // 用於for迴圈取值，Array效能優於List約2~10%
        public Transform[] arrayFriend; // 友機列表
        public Transform[] arrayFoe; // 敵機列表
        public float nextTimeTakeOff; // 下一架次起飛時間
        // 成員數據
        public bool[] isPlayer;
        public bool[] hasTakenOff;
        // 記錄
        public Type[] Type;
        public int[] Number;
        public string[] TypeName;
        public string[] Scheme;
        public float[] AirPower;
        public int[] ShootDown;
        public int[] TakeOff;
        public int[] Damage;
        public int[] Rank;

        public Image[] IconAircraft;
        public TextMeshProUGUI textAirSupremacy;
        public TextMeshProUGUI textAirSupremacyPercentage;
        public TextMeshProUGUI textNumberPilots;
        public TextMeshProUGUI[] textAircraftScheme;
        public TextMeshProUGUI[] textAirPower;
        public TextMeshProUGUI[] textDamage;
        public TextMeshProUGUI[] textShootDown;
        public TextMeshProUGUI[] textTakeOff;
    }

    public class SatelliteCommander : MonoBehaviour
    {
        public static WaitForSeconds waitRocketRecovery = new WaitForSeconds(KocmoRocketLauncher.FlightTime);
        public static WaitForSeconds waitMissileRecovery = new WaitForSeconds(KocmoMissileLauncher.FlightTime);

        //
        public static SatelliteCommander Instance { get; private set; }
        private AudioSource myAudioSource;
        [Header("Satellite Components")]
        public ObserverCamera Observer;
        [Header("Satellite Data")]
        public Dictionary<int,Transform> listKocmocraft = new Dictionary<int, Transform>(); // 宇航機列表
        public Dictionary<int, Kocmonaut> listKocmonaut = new Dictionary<int, Kocmonaut>(); // 宇航員列表

        //Onboard常用資訊使用static比透過instance呼叫高效
        // 效能與放在全域Static 差不多
        public static FactionData[] factionData = new FactionData[2]; // 陣營數據
        private readonly int ountFactionKocmocraftCounter = 50;

        const int countMissionPilot = 50;
        const int countTarget = 100;
        [Header("Formation Data")]
        public Transform[] kocmoWing; // 宇航聯隊 = 10 宇航團（10名玩家）
        public Transform[] kocmoGroup; // 宇航團 = 5 宇航中隊（玩家為長機，其餘僚機為Bot）
        public Transform[] kocmoSquadron; // 宇航中隊 = 1架主力宇航機 + 4架迷你特攻機
        public static Vector3[] portalPos; // 中隊傳送點
        public static Quaternion[] portalRot;

        [Header("Commander")]
        public AudioClip[] soundTakeOff;

        private bool isCrash;

        private void Awake()
        {
            Instance = this;
            myAudioSource = GetComponent<AudioSource>();
        }
        void Start()
        {
            LocalPlayerRealtimeData.Status = FlyingStatus.Waiting;
            InitializeRadar();

            portalPos = new Vector3[100];
            portalRot = new Quaternion[100];
            for (int i = 0; i < 100; i++)
            {
                int indexSquadron = i / 20;
                int indexGroup = (int)(i * 0.5f) % 10;
                int indexWing = i % 2;
                portalPos[i] = kocmoGroup[indexGroup].InverseTransformPoint(kocmoSquadron[indexSquadron].position)+ kocmoGroup[indexGroup].localPosition * 2;
                portalPos[i] = kocmoWing[indexWing].TransformPoint(portalPos[i]);
                portalRot[i] = kocmoWing[indexWing].rotation;
            }
        }

        public static OnboardRadar[] radarManager = new OnboardRadar[100];
        //private void Update()
        //{
        //    Radar();
        //}
        void Radar()
        {
            for (int i = 0; i < 100; i++)
            {
                if (radarManager[i])
                radarManager[i].UpdateMe();
            }
        }

        public void InitializeSatellite()
        {
            PhotonNetwork.SendRate = 60;
            PhotonNetwork.SerializationRate = 60;
            factionData[0].arrayFriend = new Transform[ountFactionKocmocraftCounter];
            factionData[0].arrayFoe = new Transform[ountFactionKocmocraftCounter];
            factionData[1].arrayFriend = new Transform[ountFactionKocmocraftCounter];
            factionData[1].arrayFoe = new Transform[ountFactionKocmocraftCounter];
            SpawnPlayerKocmocraft();
            if (PhotonNetwork.IsMasterClient)
            {
                int countPlayer = PhotonNetwork.CurrentRoom.PlayerCount;
                for (int portNumber = countPlayer; portNumber <0; portNumber++)
                {
                    int type = Random.Range(0,20);
                    PhotonNetwork.Instantiate(string.Format("Kocmocraft ({0}) - {1}", type.ToString("00"), DesignData.Code[type]), new Vector3(0, 10000, 0), Quaternion.identity, 0).GetComponent<Kocmoport>().InitializeLocalBot(portNumber);
                }
            }
        }
        public void SpawnPlayerKocmocraft()
        {
            PhotonNetwork.Instantiate("Kocmoport", new Vector3(0, -10000, 0), Quaternion.identity, 0).GetComponent<Kocmoport>().CreatePlayerKocmoport();

            //ClearData();
            //HeadUpDisplayManager.Instance.ClearData();
            //isCrash = false;
            //myAudioSource.PlayOneShot(ResourceManager.instance.soundTakeOff, 0.37f);
            //int type =7;// PlayerPrefs.GetInt(LobbyInfomation.PREFS_TYPE);
            //            //string typeName = "Kocmocraft(" + type.ToString("00")+ ") - " + KocmocraftData.GetKocmocraftName(type);
            //PhotonNetwork.Instantiate(string.Format("Kocmocraft ({0}) - {1}", type.ToString("00"), DesignData.Code[type]), new Vector3(0, 10000, 0), Quaternion.identity, 0).GetComponent<Kocmoport>().InitializeLocalPlayer();
            //LocalPlayerRealtimeData.Status = FlyingStatus.Flying;
        }
        void SpawnBotKocmocraft(int kocmonautNumber,int portNumber)
        {
            //int faction = portNumber % 2;
            //int order = portNumber / 2;
            int type = (int)listKocmonaut[kocmonautNumber].Type;//Random.Range(0,20); // 測試  (int)factionData[faction].Type[order];
            //string typeName = "Kocmocraft " + type.ToString("00") + " - " + KocmocraftData.GetKocmocraftName(type);
            PhotonNetwork.Instantiate(string.Format("Kocmocraft ({0}) - {1}", type.ToString("00"), DesignData.Code[type]), new Vector3(0, 10000, 0), Quaternion.identity, 0).GetComponent<Kocmoport>().InitializeLocalBot(portNumber);
        }
        public void PlayerCrash()
        {
            StartCoroutine(PlayerRespawnCountdown());
        }
        IEnumerator PlayerRespawnCountdown()
        {
            HeadUpDisplayManager.Instance.textRespawn.GetComponent<CanvasGroup>().alpha = 1;
            int countdown = 30;
            while (countdown > 0)
            {
                HeadUpDisplayManager.Instance.textRespawn.text = countdown.ToString();
                yield return new WaitForSeconds(1.0f);
                countdown--;
            }
            HeadUpDisplayManager.Instance.textRespawn.text = "It's time to take off";
        }
        public void BotCrash(int kocmonautNumber)
        {
            StartCoroutine(BotRespawn(kocmonautNumber));
        }
        public IEnumerator BotRespawn(int kocmonautNumber)
        {
            yield return new WaitForSeconds(7.0f);
            SpawnBotKocmocraft(kocmonautNumber,KocmocraftData.GetPortNumber(kocmonautNumber));
        }

        // 新生成宇航機加入搜索列表
        public void AddSearchArray(Transform kocmocraft, int order, int faction, int number)
        {
            listKocmocraft.Add(number, kocmocraft);
            for (int i = 0; i < 2; i++)
            {
                if (i == faction)
                    factionData[i].arrayFriend[order] = kocmocraft;
                else
                    factionData[i].arrayFoe[order] = kocmocraft;
            }
        }

        // 設定宇航機機庫位置（僅本地端）
        public void SetHangar(Transform kocmocraft, int portNumber)
        {
            kocmocraft.SetPositionAndRotation(portalPos[portNumber], portalRot[portNumber]);

            // 重生位置提前計算
            //int index = portNumber % 16;
            //kocmocraft.SetPositionAndRotation(kocmoPortal[index].TransformPoint(kocmoFormation[portNumber / 16].localPosition), kocmoPortal[index].rotation);

            // 效能差異 約8~9倍
            //kocmocraft.SetParent(kocmoPortal[portNumber%16]);
            //kocmocraft.localPosition = kocmoFormation[portNumber/16].localPosition;
            //kocmocraft.localRotation = Quaternion.identity;
            //kocmocraft.SetParent(null);
        }

        // 初始化宇航員數據（僅第一次生成）
        public void NewKocmonautJoin(ControlUnit core, int portNumber, Type type, int number, string name) // 宇航機實例化記錄宇航員資料
        {
            Kocmonaut kocmonaut = new Kocmonaut(
                (Faction)(portNumber%2),
                (Order)(portNumber/
                2),
                type,
                number,
                name,
                core);
            listKocmonaut.Add(number, kocmonaut); // 初始化


            //SatelliteCommander.Instance.factionData[faction].Type[order] = kocmocraftType;
            //SatelliteCommander.Instance.factionData[faction].Number[order] = kocmonautNumber;
            //SatelliteCommander.Instance.factionData[faction].TypeName[order] = KocmocraftData.aircraftDisplayName[(int)type];
            //SatelliteCommander.Instance.factionData[faction].Scheme[order] = "";
            ////        SatelliteCommander.Instance.listKocmonaut.Add(kocmonautNumber, kocmonaut);
            ////        SatelliteCommander.Instance.factionData[faction].Type[order] = kocmocraftType;
            ////        SatelliteCommander.Instance.factionData[faction].Number[order] = kocmonautNumber;
            ////        SatelliteCommander.Instance.factionData[faction].TypeName[order] = KocmocraftData.aircraftDisplayName[(int)kocmocraftType];
            ////        SatelliteCommander.Instance.factionData[faction].Scheme[order] = "";
        }

        public static string Number2Scheme(int number)
        {
            int alphaOrder = number % 26;
            int alphaOrder2 = number % (Random.Range(7, 27));
            return "" + (char)(alphaOrder + 65) + (char)(alphaOrder2 + 65) + "-" + number % 100;
        }

        [Header("【機載雷達】")]
        public Transform onboardRadar;
        public AudioSource lockOnAudio;
        public GameObject iconFriend;
        public GameObject iconFoe;
        public GameObject iconLock;

        private Transform[] markFriend; // 機載雷達敵我辨識
        private int orderFriend = 0;
        private Transform[] markFoe; // 機載雷達敵我辨識
        private int orderFoe = 0;
        public Transform[] markFireControl; // 射控雷達掃描
        private int orderFireControl = 0;
        public Transform markLock; // 射控雷達開火提示 only one
        public Transform markTracking;
        //private int markSize;
        //private Transform targetLockOn;
        //private Transform targetFollow;

        [Header("UI - Onboard Radar")]
        private Vector3 invisiblePos = new Vector3(999, 10000, -1);
        public Transform localPlayer;

        public void RemoveFlight(int flight)
        {
            Transform kocmocraft = listKocmocraft[flight];
            for (int i = 0; i < 2; i++)
            {
                //factionData[i].listFriend.Remove(kocmocraft);
                //factionData[i].listFoe.Remove(kocmocraft);
            }
            listKocmocraft.Remove(flight);
          //  listKocmonaut.Remove(flight);
        }



        // 主動事件（搜索、鎖定、追蹤）
        public void InitializeRadar()
        {
            lockOnAudio = onboardRadar.GetComponent<AudioSource>();
            //markFriend = new Transform[countMissionPilot - 1];
            //for (int i = 0; i < countMissionPilot - 1; i++)
            //{
            //    markFriend[i] = Instantiate(iconFriend).transform;
            //    markFriend[i].SetParent(onboardRadar);
            //    markFriend[i].localScale = new Vector3(1, 1, 1);
            //    markFriend[i].localPosition = invisiblePos;
            //}
            markFoe = new Transform[countTarget];
            markFireControl = new Transform[countTarget];
            for (int i = 0; i < countTarget; i++)
            {
                markFoe[i] = Instantiate(iconFoe).transform;
                markFoe[i].SetParent(onboardRadar);
                markFoe[i].localScale = new Vector3(1, 1, 1);
                markFoe[i].localPosition = invisiblePos;

                markFireControl[i] = Instantiate(iconLock).transform;
                markFireControl[i].SetParent(onboardRadar);
                markFireControl[i].localScale = new Vector3(1, 1, 1);
                markFireControl[i].localPosition = invisiblePos;
            }

            markLock.localPosition = invisiblePos;
            markTracking.localPosition = invisiblePos;
            //markSize = -(int)markTracking.GetComponent<RectTransform>().rect.x;
        }
        /* Onboard Radar 呼叫 AEW */
        public void ResetOnboardRadarRadar()
        {
            orderFriend = -1;
            orderFoe = -1;
            orderFireControl = -1;
            for (int i = 0; i < countMissionPilot - 1; i++)
            {
                //markFriend[i].localPosition = invisiblePos;
            }
            for (int i = 0; i < countTarget; i++)
            {
                markFoe[i].localPosition = invisiblePos;
                markFireControl[i].localPosition = invisiblePos;
            }
            markLock.localPosition = invisiblePos;
            markTracking.localPosition = invisiblePos;
        }
        public void IdentifyFriend(Transform friend)
        {
            Vector3 screenPos = HeadUpDisplayManager.mainCamera.WorldToScreenPoint(friend.position);
            screenPos.z = 999;

            orderFriend++;
            //markFriend[orderFriend].position = screenPos;
        }
        public void IdentifyTarget(Transform target)
        {
            Vector3 screenPos = HeadUpDisplayManager.mainCamera.WorldToScreenPoint(target.position);
            screenPos.z = 999;

            orderFoe++;
            markFoe[orderFoe].position = screenPos;
        }
        public void FireControlLookTarget(Transform target)
        {
            if (orderFireControl > 1)
                return;
            Vector3 screenPos = HeadUpDisplayManager.mainCamera.WorldToScreenPoint(target.position);
            screenPos.z = 999;

            orderFireControl++;
            markFireControl[orderFireControl].position = screenPos;
        }    
        public void MarkTarget()
        {
            //if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Joystick1Button5))
            //{
            //    if (targetLockOn)
            //        targetFollow = targetLockOn;
            //}
            //if (targetFollow)
            //{
            //    Vector3 screenPos = Camera.main.WorldToScreenPoint(targetFollow.position);

            //    if (screenPos.z > 0)
            //    {
            //        if (screenPos.x > Screen.width - markSize * 0.5f)
            //            screenPos.x = Screen.width - markSize * 0.5f;
            //        else if (screenPos.x < markSize * 0.5f)
            //            screenPos.x = markSize * 0.5f;

            //        if (screenPos.y > Screen.height - markSize * 0.5f)
            //            screenPos.y = Screen.height - markSize * 0.5f;
            //        else if (screenPos.y < markSize * 0.5f)
            //            screenPos.y = markSize * 0.5f;

            //        screenPos.z = 100;
            //    }
            //    else
            //    {
            //        screenPos.x = Screen.width - screenPos.x;
            //        screenPos.y = Screen.height - screenPos.y;
            //        screenPos.z = -100;

            //        bool xFix = false;
            //        bool yFix = false;

            //        if (screenPos.x > Screen.width - markSize * 0.5f)
            //            screenPos.x = Screen.width - markSize * 0.5f;
            //        else if (screenPos.x < markSize * 0.5f)
            //            screenPos.x = markSize * 0.5f;
            //        else
            //            xFix = true;

            //        if (screenPos.y > Screen.height - markSize * 0.5f)
            //            screenPos.y = Screen.height - markSize * 0.5f;
            //        else if (screenPos.y < markSize * 0.5f)
            //            screenPos.y = markSize * 0.5f;
            //        else
            //            yFix = true;

            //        if (xFix && yFix)
            //        {
            //            float absX;
            //            float absY;

            //            if (screenPos.x > Screen.width * 0.5f)
            //            {
            //                absX = Screen.width - screenPos.x;
            //                // 第一象限
            //                if (screenPos.y > Screen.height * 0.5f)
            //                {
            //                    absY = Screen.height - screenPos.y;
            //                    if (absX < absY)
            //                        screenPos.x = Screen.width - markSize * 0.5f;
            //                    else
            //                        screenPos.y = Screen.height - markSize * 0.5f;
            //                }
            //                // 第四象限
            //                else
            //                {
            //                    absY = screenPos.y;
            //                    if (absX < absY)
            //                        screenPos.x = Screen.width - markSize * 0.5f;
            //                    else
            //                        screenPos.y = markSize * 0.5f;
            //                }
            //            }
            //            else
            //            {
            //                absX = screenPos.x;
            //                // 第二象限
            //                if (screenPos.y > Screen.height * 0.5f)
            //                {
            //                    absY = Screen.height - screenPos.y;
            //                    if (absX < absY)
            //                        screenPos.x = markSize * 0.5f;
            //                    else
            //                        screenPos.y = Screen.height - markSize * 0.5f;
            //                }
            //                // 第三象限
            //                else
            //                {
            //                    absY = screenPos.y;
            //                    if (absX < absY)
            //                        screenPos.x = markSize * 0.5f;
            //                    else
            //                        screenPos.y = markSize * 0.5f;
            //                }
            //            }
            //        }
            //    }
            //    markTracking.localPosition = invisiblePos;
            //    markTracking.position = screenPos;
            //}
        }

        public List<int> listRadarLocked = new List<int>();
        public List<string> listMissileLocked = new List<string>();
        public void RadarLockOnWarning(bool isLockOn, int sourceRadar)
        {
            //Debug.LogWarning("AFrame: " + Time.frameCount + "/Radar: " + listRadarLocked.Count);
            if (isCrash == true)
                return;
            if (isLockOn)
            {
                if (listRadarLocked.Contains(sourceRadar))
                    return;
                else
                    listRadarLocked.Add(sourceRadar);
            }
            else
            {
                if (listRadarLocked.Contains(sourceRadar))
                    listRadarLocked.Remove(sourceRadar);
            }
            //Debug.LogWarning("BFrame: " + Time.frameCount + "/Radar: " + listRadarLocked.Count);
            HeadUpDisplayManager.Instance.CheckRadarLocked(listRadarLocked.Count);
        }
        public void MissileLockOnWarning(bool isLockOn, string sourceMissile)
        {
            if (isCrash == true)
                return;
            if (isLockOn)
            {
                if (listMissileLocked.Contains(sourceMissile))
                    return;
                else
                    listMissileLocked.Add(sourceMissile);
            }
            else
            {
                if (listMissileLocked.Contains(sourceMissile))
                    listMissileLocked.Remove(sourceMissile);
            }
            HeadUpDisplayManager.Instance.CheckMissileLocked(listMissileLocked.Count);
        }
        public void ClearData() // 本地玩家宇航機Crash呼叫此方法解除預警
        {
            isCrash = true;
            listRadarLocked.Clear();
            listMissileLocked.Clear();
        }



        //void InitializeFaction()
        //{
        //    //// Bot 固定值： 代號
        //    //// Bot 宇航機：隨機 Master


        //    ////countFaction = airport.Length;
        //    ////countMissionPilot = formationFlying.Length;
        //    ////countAircraftType = aircraftType.Length;
        //    ////countTarget = (countFaction - 1) * countMissionPilot;

        //    ////BFM = new BattlefieldFactionManager[countFaction];
        //    //for (int i = 0; i < 2; i++)
        //    //{
        //    //    // 陣營通用數據
        //    //    //factionData[i].valueAirSupremacy = 1;
        //    //    //totalAirSupremacy += BFM[i].valueAirSupremacy;
        //    //    //factionData[i].countPilot = countReadyPilot;
        //    //    //factionData[i].listFriend = new List<Transform>(); // 陣營友機索引（機載雷達專用）
        //    //    //factionData[i].listFoe = new List<Transform>(); // 陣營敵機索引（機載雷達專用）
        //    //    factionData[i].arrayFriend = new Transform[50]; // 陣營友機索引（機載雷達專用）
        //    //    factionData[i].arrayFoe = new Transform[50];  // 陣營敵機索引（機載雷達專用）
        //    //    KocmocaData.factionData[i].arrayFriend = new Transform[50]; // 陣營友機索引（機載雷達專用）
        //    //    KocmocaData.factionData[i].arrayFoe = new Transform[50];  // 陣營敵機索引（機載雷達專用）

        //    //    //factionData[i].Type = new Type[10];
        //    //    //factionData[i].Number = new int[10];
        //    //    //factionData[i].TypeName = new string[10];
        //    //    //factionData[i].Scheme = new string[10];
        //    //    //factionData[i].AirPower = new float[countMissionPilot];
        //    //    //factionData[i].ShootDown = new int[countMissionPilot];
        //    //    //factionData[i].TakeOff = new int[countMissionPilot];
        //    //    //factionData[i].Damage = new int[countMissionPilot];
        //    //    //factionData[i].Rank = new int[countMissionPilot];

        //    //    // 陣營各單位數據
        //    //    //for (int j = 0; j < 10; j++)
        //    //    //{

        //    //    //    //BFM[i].IconAircraft[j].sprite = spriteMiniAircraft[(int)aiAircraftType];
        //    //    //    //BFM[i].textAircraftScheme[j].text = "" + BFM[i].AircraftScheme[j];
        //    //    //    //BFM[i].textAirPower[j].text = "" + BFM[i].AirPower[j];
        //    //    //    //BFM[i].textDamage[j].text = "" + BFM[i].Damage[j];
        //    //    //    //BFM[i].textShootDown[j].text = "" + BFM[i].ShootDown[j];
        //    //    //    //BFM[i].textTakeOff[j].text = "" + BFM[i].TakeOff[j];
        //    //    //}
        //    //}
        //    //Debug.LogWarning("InitializeBattlefieldFaction Completed");
        //}
    }
}
