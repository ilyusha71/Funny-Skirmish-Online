﻿/***************************************************************************
 * Head Up Display Manager
 * 觀察者攝影機
 * Last Updated: 2018/09/22
 * Description:
 * 1. 管理HUD各類UI切換與賦值
 * 2. 管理HUD相關音效
 ***************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

namespace Kocmoca
{
    public class HeadUpDisplayManager : MonoBehaviour
    {
        public static HeadUpDisplayManager Instance { get; private set; }
        private AudioSource myAudioSource;
        private static float barOffset = 1.0f / 12.0f; // 計量條的零點位於逆時針第2格的位置（24等分）
        public struct BarData
        {
            public float Value;
            public float Max;
            public float Bar { get { return (Value / Max) / 3.0f + barOffset; } } // 計量條共8格（24等分）
        }
        [Header("UI - Global")]
        public CanvasGroup uiMask;
        public CanvasGroup uiGlobal;
        public Image iconFaction;
        public Sprite[] spriteFaction;
        public TextMeshProUGUI textObserverFaction;
        public TextMeshProUGUI textObserverName;
        [Header("UI - Block - Respawn")]
        public TextMeshProUGUI textRespawn;
        [Header("UI - Block - Destroyed Info")]
        public TextMeshProUGUI[] textDestroyed;
        public Queue<TextMeshProUGUI> listDestroyedText = new Queue<TextMeshProUGUI>();
        [Header("UI - Block - WhoAttackU")]
        public CanvasGroup blockWhoAttackU;
        public TextMeshProUGUI textAttackerInfo;
        public TextMeshProUGUI textDamageInfo;
        public TextMeshProUGUI textKillStealer;
        private int countList;
        [Header("UI - HUD")]
        public CanvasGroup uiHUD;
        public TextMeshProUGUI textPING;
        public TextMeshProUGUI textFPS;
        private float deltaTime;

        [Header("UI - Kocmocraft Realtime Status")]
        public Image barEnergy;
        public Image barSpeed;
        public Image barHull;
        public Image barShield;
        public TextMeshProUGUI textValueEnergy;
        public TextMeshProUGUI textValueSpeed;
        public TextMeshProUGUI textValueHull;
        public TextMeshProUGUI textValueShield;
        public BarData dataEnergy = new BarData();
        public BarData dataSpeed = new BarData();
        public BarData dataHull = new BarData();
        public BarData dataShield = new BarData();
        [Header("UI - Onboard Radar Lock On")]
        public Transform markerLock; // 射控雷達開火提示 only one
        public TextMeshProUGUI textTargetName;
        public TextMeshProUGUI textTargetDistance;
        public Image targetHull;
        public Image targetShield;
        public TextMeshProUGUI textHitDamage;
        private float fadeHit;
        private Vector3 invisiblePos = new Vector3(999, 10000, -1);
        [Header("UI - Onboard Radar Warning Alarm")]
        public Transform iconWarningAlarm;
        public CanvasGroup iconRadarAlarm;
        public CanvasGroup iconMissileAlarm;
        public TextMeshProUGUI textValueRadarLocked;
        public TextMeshProUGUI textValueMissileLocked;
        private Vector3 defaultPos;
        public bool isRadarLocked;
        public bool isMissileLocked;
        [Header("UI - FCS")]
        public Toggle[] toggleWeaponOption;
        public TextMeshProUGUI textRocketCount;
        public TextMeshProUGUI textRocketReloadPecentage;
        public TextMeshProUGUI textMissileCount;
        public TextMeshProUGUI textMissileReloadPecentage;
        [Header("UI - Beacon")]
        public Vector3[] posBeacon = new Vector3[4];
        public Image[] markerBeacon;
        private TextMeshProUGUI[] textBeaconFaction = new TextMeshProUGUI[4];
        private TextMeshProUGUI[] textBeaconDistance = new TextMeshProUGUI[4];

        [Header("【機載雷達】")]
        public Transform onboardRadar;
        public AudioSource lockOnAudio;
        public GameObject iconFriend;
        public GameObject iconFoe;
        public GameObject iconLock;

        private Transform[] markFriend; // 機載雷達敵我辨識
   //     private int orderFriend = 0;
        private Transform[] markFoe; // 機載雷達敵我辨識
     //   private int orderFoe = 0;
        public Transform[] markFireControl; // 射控雷達掃描
    //    private int orderFireControl = 0;
        public Transform markTracking;
        private int markSize;
        private Transform targetLockOn;
        private Transform targetTracking;

        [Header("Player Kocmocraft Module")]
        public Transform myKocmocraft;
        public Rigidbody myRigidbody;
        //private KocmocraftManager myKocmocraftManager;
        private AvionicsSystem myAvionicsSystem;
        private KocmocraftMechDroid myMechDroid;
        private FireControlSystem myRocketFCS;
        private FireControlSystem myMissileFCS;
        private int myNumber;
        private Faction myFaction;
        //public WeaponManager myWeaponManager;
        //public WeaponSystem myWeaponSystem;
        //public WeaponSystem myRocketSystem;
        //public WeaponSystem myMissileSystem;

        private void Awake()
        {
            Instance = this;
            myAudioSource = GetComponent<AudioSource>();
            uiGlobal.alpha = 0;
            uiHUD.alpha = 0;
        }
        public void InitializeHUD(Transform myKocmocraft,int kocmonautNumber)
        {
            this.myKocmocraft = myKocmocraft;
            myRigidbody = myKocmocraft.GetComponent<Rigidbody>();
            //myKocmocraftManager = myKocmocraft.GetComponent<KocmocraftManager>();
            myAvionicsSystem = myKocmocraft.GetComponent<AvionicsSystem>();
            myMechDroid = myKocmocraft.GetComponent<KocmocraftMechDroid>();
            myRocketFCS = myKocmocraft.GetComponentsInChildren<FireControlSystem>()[1];
            myMissileFCS = myKocmocraft.GetComponentsInChildren<FireControlSystem>()[2];
            myNumber = kocmonautNumber;
            myFaction = SatelliteCommander.Instance.listKocmonaut[myNumber].Faction;

            uiMask.alpha = 1;
            uiMask.DOFade(0, 3.0f);
            uiGlobal.alpha = 1;
            textRespawn.GetComponent<CanvasGroup>().alpha = 0;
            for (int i = 0; i < textDestroyed.Length; i++)
            {
                textDestroyed[i].alpha = 0;
                listDestroyedText.Enqueue(textDestroyed[i]);
            }
            blockWhoAttackU.alpha = 0;
            textAttackerInfo.text = "";
            textDamageInfo.text = "";
            countList = 0;

            uiHUD.alpha = 1;
            dataEnergy.Max = myAvionicsSystem.dataEnergy.Max;
            dataSpeed.Max = myAvionicsSystem.dataSpeed.Max;
            dataHull.Max = myMechDroid.dataHull.Max;
            dataShield.Max = myMechDroid.dataShield.Max;

            defaultPos = iconWarningAlarm.localPosition;
            iconWarningAlarm.localPosition = invisiblePos;
            iconRadarAlarm.alpha = 0;
            iconMissileAlarm.alpha = 0;
        }
        private void Update()
        {
            textPING.text = PhotonNetwork.GetPing() + " ms";
            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
            textFPS.text = Mathf.Ceil(1.0f / deltaTime) + " fps"; ;
            if (Input.GetKeyDown(KeyCode.F9))
            {
                uiMask.alpha = 1;
                PhotonNetwork.LeaveRoom();
            }

            if (Time.time > fadeHit)
                textHitDamage.transform.localPosition = invisiblePos;

            if (myKocmocraft)
            {
                ShowBeaconMarker();
                ShowKocmocraftRealtimeStatus();
                ShowFCS();
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.F3))
                    blockWhoAttackU.alpha = Mathf.Repeat(blockWhoAttackU.alpha + 1, 2);
                if (Input.GetKeyDown(KeyCode.F2)) // 暂时 只能按一次
                {
                    SatelliteCommander.Instance.SpawnPlayerKocmocraft();
                }
            }
        }

        // General
        Color32 colorTextFriend = new Color32(0, 255, 30, 255);
        Color32 colorTextFoe = new Color32(255, 0, 32, 255);
        string PlayerText(string text)
        {
            return "<color=#45CBFF>" + text + "</color>";
        }
        string FriendText(string text)
        {
            return "<color=#00FF1E>" + text + "</color>";
        }
        string FoeText(string text)
        {
            return "<color=#FF0020>" + text + "</color>";  //"<color=#A90015>" + text + "</color>";
        }
        string ApovakaText(string text)
        {
            return "<color=#004309>" + text + "</color>";
        }
        string PerivakaText(string text)
        {
            return "<color=#6C2300>" + text + "</color>";
        }
        public void ClearData()
        {

            uiHUD.alpha = 0;
            isRadarLocked = false;
            isMissileLocked = false;
            myAudioSource.Stop();
        }

        // Satellite Commander UI Method
        public void CheckRadarLocked(int count)
        {
            if (count == 0)
            {
                iconRadarAlarm.alpha = 0;
                isRadarLocked = false;
            }
            else
            {
                iconRadarAlarm.alpha = 1;
                textValueRadarLocked.text = count.ToString();
                isRadarLocked = true;
            }
            CheckWarningAlarm();
        }
        public void CheckMissileLocked(int count)
        {
            if (count == 0)
            {
                iconMissileAlarm.alpha = 0;
                isMissileLocked = false;
            }
            else
            {
                iconMissileAlarm.alpha = 1;
                textValueMissileLocked.text = count.ToString();
                isMissileLocked = true;

                //if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.M))
                //{
                //    if (myHullManager.valueEnergy > 5000)
                //    {
                //        myHullManager.valueEnergy -= 5000;
                //        AntiMissile();
                //    }
                //}
            }
            CheckWarningAlarm();
        }
        public void CheckWarningAlarm()
        {
            if (!isActiveAndEnabled)
                return;

            if (isRadarLocked || isMissileLocked)
            {
                iconWarningAlarm.localPosition = defaultPos;
                myAudioSource.clip = (isMissileLocked) ? ResourceManager.instance.soundMissileLocked : ResourceManager.instance.soundRadarLocked;
                if (!myAudioSource.isPlaying)
                    myAudioSource.Play();
            }
            else
            {
                iconWarningAlarm.localPosition = invisiblePos;
                StartCoroutine(RadarAudioBuffer());
            }
        }
        IEnumerator RadarAudioBuffer()
        {
            yield return new WaitForSeconds(0.37f);
            if (!isRadarLocked && !isMissileLocked)
                myAudioSource.Stop();
        }


        // Realtime Display
        void ShowBeaconMarker()
        {
            for (int i = 0; i < posBeacon.Length; i++)
            {
                float distance = Vector3.Distance(myKocmocraft.position, posBeacon[i]);
                if (distance > 1000)
                {
                    markerBeacon[i].transform.position = invisiblePos;
                    continue;
                } 
                Vector3 screenPos = Camera.main.WorldToScreenPoint(posBeacon[i]);
                //screenPos.y += 55;
                if (screenPos.z <= 0)
                    markerBeacon[i].transform.position = invisiblePos;
                else
                {
                    screenPos.z = 999;
                    markerBeacon[i].transform.position = screenPos;
                }
                textBeaconDistance[i].text = Mathf.Floor(distance) + " m";
            }
        }
        void ShowKocmocraftRealtimeStatus()
        {
            dataEnergy.Value = myAvionicsSystem.dataEnergy.Value;
            barEnergy.fillAmount = dataEnergy.Bar;
            textValueEnergy.text = Mathf.Ceil(dataEnergy.Value).ToString();

            dataSpeed.Value = myAvionicsSystem.dataSpeed.Value;
            barSpeed.fillAmount = dataSpeed.Bar;
            textValueSpeed.text = Mathf.Ceil(dataSpeed.Value*3.6f).ToString();

            dataHull.Value = myMechDroid.dataHull.Value;
            barHull.fillAmount = dataHull.Bar;
            textValueHull.text = Mathf.Ceil(dataHull.Value).ToString();

            dataShield.Value = myMechDroid.dataShield.Value;
            barShield.fillAmount = dataShield.Bar;
            textValueShield.text = Mathf.Ceil(dataShield.Value).ToString();
        }
        void ShowFCS()
        {
            textRocketCount.text = "" + myRocketFCS.countAmmo;
            if (myRocketFCS.countAmmo < myRocketFCS.maxAmmoCapacity)
                textRocketReloadPecentage.text = string.Format("{0:0%}", 1 - myRocketFCS.reloadingProcess);
            else
                textRocketReloadPecentage.text = "MAX";

            textMissileCount.text = "" + myMissileFCS.countAmmo;
            if (myMissileFCS.countAmmo < myMissileFCS.maxAmmoCapacity)
                textMissileReloadPecentage.text = string.Format("{0:0%}", 1 - myMissileFCS.reloadingProcess);
            else
                textMissileReloadPecentage.text = "MAX";
        }

        // Observer UI Method
        public void ShowObserver(Faction faction, string name)
        {
            string info = name.Split('-')[0] + "\n<size=37>" + name.Split('-')[1] + "</size>";
            iconFaction.sprite = spriteFaction[(int)faction];
            textObserverFaction.text = faction == Faction.Apovaka ? ApovakaText("Apovaka") : PerivakaText("Perivaka");
            textObserverName.text = faction == Faction.Apovaka ? ApovakaText(info) : PerivakaText(info);
        }

        public void ShowDestroyed(int destroyedNumber)
        {
            StartCoroutine(FadeDestroyed(destroyedNumber));
        }
        IEnumerator FadeDestroyed(int destroyedNumber)
        {
            Kocmonaut destroyed = SatelliteCommander.Instance.listKocmonaut[destroyedNumber];
            string info = SatelliteCommander.Instance.listKocmonaut[destroyedNumber].Name.Split('-')[0];
            TextMeshProUGUI show = listDestroyedText.Dequeue();
            show.DOFade(1,0.73f);
            show.text = (destroyed.Faction == myFaction ? FriendText(info) : FoeText(info)) + " Destroyed";
            yield return new WaitForSeconds(3.37f);
            show.DOFade(0, 0.73f);
            yield return new WaitForSeconds(1.00f);
            listDestroyedText.Enqueue(show);
        }
        public void ShowWhoAttackU(int attackerNumber, int damage)
        {
            countList++;
            blockWhoAttackU.GetComponent<RectTransform>().sizeDelta = new Vector2(700, 137 + countList * 47);
            blockWhoAttackU.alpha = 1;
            Kocmonaut attacker = SatelliteCommander.Instance.listKocmonaut[attackerNumber];
            string infoAttacker = attacker.Name.Split('-')[0] + "\n";
            string infoDamage = damage + "\n";
            if (myNumber == attackerNumber)
            {
                textAttackerInfo.text += PlayerText(infoAttacker);
                textDamageInfo.text += PlayerText(infoDamage);
            }
            else
            {
                textAttackerInfo.text += (attacker.Faction == myFaction ? FriendText(infoAttacker) : FoeText(infoAttacker));
                textDamageInfo.text += (attacker.Faction == myFaction ? FriendText(infoDamage) : FoeText(infoDamage));
            }
        }
        public void ShowKillStealer(int stealerNumber)
        {
            textKillStealer.text = SatelliteCommander.Instance.listKocmonaut[stealerNumber].Name.Split('-')[0];
        }

        // Kocmocraft Mech Droid UI Method
        public void ShowHitDamage(Vector3 posHit, int damage)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(posHit);
            screenPos.z = 999;
            textHitDamage.transform.position = (screenPos + new Vector3(97, 0, 0));
            textHitDamage.text = "" + damage;
            fadeHit = Time.time + 1.37f;
        }

        // Onboard Radar UI Method
        public void NewTargetTone()
        {

        }
        public void MarkTarget(Transform target, float distance)
        {
            if (target != targetLockOn)
                myAudioSource.PlayOneShot(ResourceManager.instance.soundLockOn, 3.0f);
            targetLockOn = target;

            Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position);
            screenPos.z = 999;

            markerLock.position = screenPos;
            textTargetName.text = target.name.Split('-')[0] + "\n<size=23>" + target.name.Split('-')[1] + "</size>";
            textTargetDistance.text = Mathf.Floor(distance) + " m";

            KocmocraftMechDroid targetInfo = target.GetComponent<KocmocraftMechDroid>();
            targetHull.fillAmount = targetInfo.dataHull.Percent;
            targetShield.fillAmount = targetInfo.dataShield.Percent;
        }

        // Beacon UI Method
        public void InitializeBeaconUI(int index, Faction faction,Vector3 position )
        {
            // Initialize TMP UI
            textBeaconFaction[index] = markerBeacon[index].GetComponentsInChildren<TextMeshProUGUI>()[0];
            textBeaconDistance[index] = markerBeacon[index].GetComponentsInChildren<TextMeshProUGUI>()[1];
            // Load Data
            posBeacon[index] = position;
            ChangeBeaconFaction(index, faction);
        }
        public void ChangeBeaconFaction(int index, Faction faction)
        {
            markerBeacon[index].color = faction == myFaction ? new Color32(0, 255, 0, 255) : new Color32(255, 64, 65, 255);
            textBeaconFaction[index].color = faction == myFaction ? colorTextFriend : colorTextFoe;
            textBeaconFaction[index].text = faction.ToString();
            textBeaconDistance[index].color = faction == myFaction ? colorTextFriend : colorTextFoe;
        }
    }
}