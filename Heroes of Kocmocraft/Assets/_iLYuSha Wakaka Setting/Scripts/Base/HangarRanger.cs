using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

namespace Kocmoca
{
    public enum HangarState
    {
        Portal = 0,
        Ready,
        Moving,
        Open,
        Hide,
    }
    public enum TweenerState
    {
        Ready,
        Moving,
        Open,
        Hide,
    }
    public struct SciFiBar
    {
        public Image[] bar;
        private int Denominator;
        private int Minimum;

        public void Initialize(Image[] shell, int min, int max)
        {
            bar = new Image[7];
            for (int i = 0; i < 7; i++)
            {
                bar[i] = shell[i + 1];
            }
            Denominator = max - min;
            Minimum = min;
        }

        public void SetBar(int value)
        {
            int Numerator = value - Minimum;
            float level = Numerator / (float)Denominator;
            for (int i = 0; i < 7; i++)
            {
                bar[i].enabled = true;
            }
            if (level < 0.8571428) bar[6].enabled = false;
            if (level < 0.7142857f) bar[5].enabled = false;
            if (level < 0.5714285f) bar[4].enabled = false;
            if (level < 0.4285714f) bar[3].enabled = false;
            if (level < 0.2857142f) bar[2].enabled = false;
            if (level < 0.1428571f) bar[1].enabled = false;
            if (level < 0.0100001f) bar[0].enabled = false;
        }
    }
    public partial class HangarRanger : CameraTrackingSystem
    {




        [Header("External Scripts")]
        public PortalController Portal;
        [Header("UI")]
        private AudioSource SFX;
        [Header("Hangar Rail & Camera")]
        public Transform[] hangarCenter;
        private HangarState hangarState = HangarState.Portal;

        [Header("Billboard")]
        public Transform billboard;
        private Vector3 billboardPos = new Vector3(-12.972f, 0, 9.289f);
        private Vector3 billboardHide = new Vector3(0, 1000, 0);

        [Header("Hangar Data")]
        public TextMeshProUGUI textEngine;
        [Header("Kocmocraft Data")]
        public TextMeshProUGUI textMaxHull;
        public TextMeshProUGUI textMaxShield;
        public TextMeshProUGUI textMaxEnergy;
        public TextMeshProUGUI textCruiseSpeed;
        public TextMeshProUGUI textAfterburneSpeed;
        private SciFiBar barHull;
        private SciFiBar barShield;
        private SciFiBar barEnergy;
        private SciFiBar barCruise;
        private SciFiBar barAfterburne;
        [Header("Weapon Data")]
        public TextMeshProUGUI textTurretCount;
        public TextMeshProUGUI textFireRPS;
        public TextMeshProUGUI textDamage;
        private SciFiBar barDamage;
        public TextMeshProUGUI textMaxRange;
        private SciFiBar barMaxRange;


        public class PanelBlock
        {
            public TextMeshProUGUI textTitle;
            public TextMeshProUGUI[] item;
        }
        public class DataBlock: PanelBlock
        {
            internal ModuleData moduleData;
        }

        [Serializable]
        public class AstromechDroid : DataBlock
        {
            public TextMeshProUGUI textInfomation;
            public TextMeshProUGUI textShieldRecharge;
            public TextMeshProUGUI textCollisionResistance;
            public TextMeshProUGUI textEnginePower;
            public TextMeshProUGUI textLockTime;

            public void SetData(int index)
            {
                moduleData = KocmocaData.KocmocraftData[index];
                textTitle.text = moduleData.DroidName;
                textInfomation.text = moduleData.DroidDetail;
                textShieldRecharge.text = moduleData.ShieldRecharge.ToString();
                textCollisionResistance.text = moduleData.CollisionResistance.ToString();
                textEnginePower.text = moduleData.EnginePower.ToString();
                textLockTime.text = moduleData.LockTime.ToString() + " sec";

                textInfomation.color = HangarData.TextColor[index];
                textShieldRecharge.color = HangarData.TextColor[index];
                textCollisionResistance.color = HangarData.TextColor[index];
                textEnginePower.color = HangarData.TextColor[index];
                textLockTime.color = HangarData.TextColor[index];

                for (int i = 0; i < item.Length; i++)
                {
                    item[i].color = HangarData.TextColor[index];
                }
            }
        }
        [Serializable]
        public class Radar : DataBlock
        {
            public TextMeshProUGUI textInfomation;
            public TextMeshProUGUI textMaxSearchRadius;
            public TextMeshProUGUI textMinSearchRadius;
            public TextMeshProUGUI textMaxSearchAngle;
            public TextMeshProUGUI textLockDistance;
            public TextMeshProUGUI textLockAngle;

            public void SetData(int index)
            {
                moduleData = KocmocaData.KocmocraftData[index];
                textTitle.text = moduleData.RadarName;
                textInfomation.text = moduleData.RadarDetail;
                textMaxSearchRadius.text = moduleData.MaxSearchRadius.ToString() + " m";
                textMinSearchRadius.text = moduleData.MinSearchRadius.ToString() + " m";
                textMaxSearchAngle.text = moduleData.MaxSearchAngle.ToString() + " 度";
                textLockDistance.text = moduleData.MaxLockDistance.ToString() + " m";
                textLockAngle.text = moduleData.MaxLockAngle.ToString() + " 度";

                textInfomation.color = HangarData.TextColor[index];
                textMaxSearchRadius.color = HangarData.TextColor[index];
                textMinSearchRadius.color = HangarData.TextColor[index];
                textMaxSearchAngle.color = HangarData.TextColor[index];
                textLockDistance.color = HangarData.TextColor[index];
                textLockAngle.color = HangarData.TextColor[index];

                for (int i = 0; i < item.Length; i++)
                {
                    item[i].color = HangarData.TextColor[index];
                }
            }
        }

        [Header("Kocmomech Droid Data")]
        public AstromechDroid astromechDroid;
        [Header("Radar Data")]
        public Radar radar;
        [Header("Turret Data")]
        public Turret turret;

        void Awake()
        {
            InitializeTrackingSystem();
            //Portal.OnShutterPressedUp += EnterHangar;
            SFX = GetComponent<AudioSource>();


            //ViewSetting();
            //hangarIndex = PlayerPrefs.GetInt(LobbyInfomation.PREFS_TYPE);
            //billboard.localPosition = billboardHide;
            //InitializePanel();
            //blockData.localPosition = new Vector3(blockData.localPosition.x, -500, 0);
            // Button & Toggle Event


            // Bar
            barHull.Initialize(textMaxHull.transform.parent.GetComponentsInChildren<Image>(), 4000, 25000);
            barShield.Initialize(textMaxShield.transform.parent.GetComponentsInChildren<Image>(), 3000, 24000);
            barEnergy.Initialize(textMaxEnergy.transform.parent.GetComponentsInChildren<Image>(), 500, 3700);
            barCruise.Initialize(textCruiseSpeed.transform.parent.GetComponentsInChildren<Image>(), 20, 60);
            barAfterburne.Initialize(textAfterburneSpeed.transform.parent.GetComponentsInChildren<Image>(), 70, 133);
            barDamage.Initialize(textDamage.transform.parent.GetComponentsInChildren<Image>(), 1092, 2842);
            barMaxRange.Initialize(textMaxRange.transform.parent.GetComponentsInChildren<Image>(), 955, 1270);

            hangarState = HangarState.Ready;
            //billboard.localPosition = billboardPos;
            //MoveHangarRail();
        }

        //private void EnterHangar()
        //{
        //    valueRotY = hangarIndex < hangarHalfCount ? valueRotY : -valueRotY;
        //    Controller.controlMode = ControlMode.General;
        //    hangarState = HangarState.Moving;
        //    axisY.DOLocalRotate(new Vector3(0, valueRotY, 0), 3.37f);
        //    axisX.DOLocalRotate(new Vector3(valueRotX, 0, 0), 3.37f);
        //    slider.DOLocalMove(new Vector3(0, 0, valuePosZ), 3.37f);
        //    pivot.DORotateQuaternion(kocmocraftSize[hangarIndex].transform.rotation, 3.37f);
        //    pivot.DOMove(kocmocraftSize[hangarIndex].transform.position, 3.37f).OnComplete(() =>
        //    {
        //        hangarState = HangarState.Ready;
        //        billboard.localPosition = billboardPos;
        //        LoadHangarData();
        //    });
        //}


        private void Start()
        {
            
        }


        void Loading()
        {
            SceneManager.LoadScene(LobbyInfomation.SCENE_LOADING);
        }

        //void ChangeSkin()
        //{
        //    if(hangarIndex< hangarCount)
        //        PlayerPrefs.SetInt(LobbyInfomation.PREFS_SKIN + hangarIndex, kocmocraftSkin[hangarIndex].ChangeSkin());
        //}
    }
}