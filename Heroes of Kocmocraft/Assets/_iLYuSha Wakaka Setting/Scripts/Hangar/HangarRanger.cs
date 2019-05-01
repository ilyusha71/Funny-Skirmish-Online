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
        public CanvasGroup hangarCanvas;
        private AudioSource SFX;
        [Header("Hangar Rail & Camera")]
        //public Transform hangarRailMain;
        //public Transform hangarRailY;
        //public Transform hangarRailX;
        //public Transform hangarCamera;
        public Transform[] hangarCenter;
        public Camera cameraTop;
        public Camera cameraSide;
        public Camera cameraFront;

        private HangarState hangarState = HangarState.Portal;

        [Header("Billboard")]
        public Transform billboard;
        private Vector3 billboardPos = new Vector3(-12.972f, 0, 9.289f);
        private Vector3 billboardHide = new Vector3(0, 1000, 0);
        [Header("Panel")]
        public Transform blockInfo;
        //public Transform blockHangar;
        //public Transform blockKocmocraft;
        public Transform blockData;
        public Button btnOpen;
        public Button btnHide;
        public AudioClip sfxOpen;
        public AudioClip sfxHide;
        private TweenerState panelState = TweenerState.Hide;
        private readonly WaitForSeconds delay = new WaitForSeconds(0.137f);


        private AudioSource panelSFX;
        [Header("UI - Info Block")]
        public Image frameMain;
        public Image frameClose;
        [Header("UI - Tab")]
        public AudioClip sfxTabDown;
        public Toggle toggleDesign;
        public Toggle toggleDubi;
        public Toggle togglePerformance;
        public Toggle toggleAstromech;
        public Toggle toggleRadar;
        public Toggle toggleTurret;
        public Toggle toggleMissile;
        public Image tabDesign;
        public Image tabDubi;
        public Image tabPerformance;
        public Image tabAstromech;
        public Image tabRadar;
        public Image tabTurret;
        public Image tabMissile;
        public TextMeshProUGUI tabTextDesign;
        public TextMeshProUGUI tabTextDubi;
        public TextMeshProUGUI tabTextPerformance;
        public TextMeshProUGUI tabTextAstromech;
        public TextMeshProUGUI tabTextRadar;
        public TextMeshProUGUI tabTextTurret;
        public TextMeshProUGUI tabTextMissile;
        // Block
        public GameObject blockDesign;
        public GameObject blockDubi;
        public GameObject blockPerformance;
        public GameObject blockAstromech;
        public GameObject blockRadar;
        public GameObject blockTurret;
        public GameObject blockMissile;
        public TextMeshProUGUI textKocmocraftName;
        public TextMeshProUGUI textDubiName;

        [Header("Info Block - Kocmocraft")]
        public TextMeshProUGUI textInfo;
        public RectTransform scaleWingspan;
        public RectTransform scaleLength;
        public RectTransform scaleHeight;
        public TextMeshProUGUI textWingspan;
        public TextMeshProUGUI textLength;
        public TextMeshProUGUI textHeight;
        [Header("Skin")]
        public AudioClip sfxSkin;
        public Button btnSkin;
        public Button btnWireframe;
        [Header("Info Block - Dubi")]
        public Button btnTalk;
        public AudioClip [] sfxTalk;
        [Header("Data Block")]
        //public Toggle btnDesign;
        //public Toggle btnPerformance;
        //public Toggle btnWeapon;
        //public GameObject blockDesign;
        //public GameObject blockPerformance;
        //public GameObject blockWeapon;
        [Header("Hangar Data")]
        public TextMeshProUGUI textOKB;
        public TextMeshProUGUI textKocmocraft;
        public TextMeshProUGUI textCode;
        public TextMeshProUGUI textDubi;
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



        public class DataBlock
        {
            internal ModuleData moduleData;
            public TextMeshProUGUI textTitle;
        }

        [Serializable]
        public class AstromechDroid : DataBlock
        {
            public TextMeshProUGUI textInfomation;
            public TextMeshProUGUI[] item;
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
            public TextMeshProUGUI[] item;
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

        [Header("Astromech Droid Data")]
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


            ViewSetting();
            hangarIndex = PlayerPrefs.GetInt(LobbyInfomation.PREFS_TYPE);
            billboard.localPosition = billboardHide;
            blockInfo.localScale = Vector3.zero;
            blockData.localPosition = new Vector3(blockData.localPosition.x, -500, 0);
            // Button & Toggle Event
            panelSFX = frameMain.GetComponent<AudioSource>();


            btnOpen.onClick.AddListener(() => OpenPanel());


            // Tab Toggle
            toggleDesign.onValueChanged.AddListener(isOn =>
            {
                panelSFX.PlayOneShot(sfxTabDown, 0.73f);
                blockDesign.SetActive(isOn);
            });
            toggleDubi.onValueChanged.AddListener(isOn =>
            {
                panelSFX.PlayOneShot(sfxTabDown, 0.73f);
                blockDubi.SetActive(isOn);
            });
            togglePerformance.onValueChanged.AddListener(isOn =>
            {
                panelSFX.PlayOneShot(sfxTabDown, 0.73f);
                blockPerformance.SetActive(isOn);
            });
            toggleAstromech.onValueChanged.AddListener(isOn =>
            {
                panelSFX.PlayOneShot(sfxTabDown, 0.73f);
                blockAstromech.SetActive(isOn);
            });
            toggleRadar.onValueChanged.AddListener(isOn =>
            {
                panelSFX.PlayOneShot(sfxTabDown, 0.73f);
                blockRadar.SetActive(isOn);
            });
            toggleTurret.onValueChanged.AddListener(isOn =>
            {
                panelSFX.PlayOneShot(sfxTabDown, 0.73f);
                blockTurret.SetActive(isOn);
            });
            toggleMissile.onValueChanged.AddListener(isOn =>
            {
                panelSFX.PlayOneShot(sfxTabDown, 0.73f);
                blockMissile.SetActive(isOn);
            });
            btnHide.onClick.AddListener(() => HidePanel());


            // Switch Skin
            btnSkin.onClick.AddListener(() =>
            {
                panelSFX.PlayOneShot(sfxSkin);
                ChangeSkin();
            });
            btnWireframe.onClick.AddListener(() =>
            {
                panelSFX.PlayOneShot(sfxSkin);
                kocmocraftSkin[hangarIndex].SwitchWireframe();
            });


            //btnDesign.onValueChanged.AddListener(isOn => blockDesign.SetActive(isOn));   
            btnTalk.onClick.AddListener(() => { if (!panelSFX.isPlaying && hangarIndex < hangarCount) panelSFX.PlayOneShot(sfxTalk[hangarIndex],1.5f); });
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

        private void EnterHangar()
        {
            valueRotY = hangarIndex < hangarHalfCount ? valueRotY : -valueRotY;
            Controller.controlMode = ControlMode.General;
            hangarState = HangarState.Moving;
            axisY.DOLocalRotate(new Vector3(0, valueRotY, 0), 3.37f);
            axisX.DOLocalRotate(new Vector3(valueRotX, 0, 0), 3.37f);
            slider.DOLocalMove(new Vector3(0, 0, valuePosZ), 3.37f);
            pivot.DORotateQuaternion(kocmocraftSize[hangarIndex].transform.rotation, 3.37f);
            pivot.DOMove(kocmocraftSize[hangarIndex].transform.position, 3.37f).OnComplete(() =>
            {
                hangarState = HangarState.Ready;
                billboard.localPosition = billboardPos;
                LoadHangarData();
            });
        }




        public void OpenPanel()
        {
            blockInfo.DOKill();
            //blockHangar.DOKill();
            //blockKocmocraft.DOKill();
            blockData.DOKill();
            panelState = TweenerState.Moving;
            SFX.PlayOneShot(sfxOpen);
            blockInfo.DOScale(Vector3.one,0.37f).OnComplete(() => 
            {
                blockData.DOLocalMoveY(0, 1.0f).SetEase(Ease.OutElastic).OnComplete(() => { panelState = TweenerState.Open; });
            });
        }

        public void HidePanel()
        {
            blockInfo.DOKill();
            blockData.DOKill();
            panelState = TweenerState.Moving;
            SFX.PlayOneShot(sfxHide);
            blockInfo.DOScale(Vector3.zero, 0.37f);
            blockData.DOLocalMoveY(-500, 0.37f).OnComplete(() => { panelState = TweenerState.Hide; });
        }

        void Loading()
        {
            SceneManager.LoadScene(LobbyInfomation.SCENE_LOADING);
        }

        void ChangeSkin()
        {
            if(hangarIndex< hangarCount)
                PlayerPrefs.SetInt(LobbyInfomation.PREFS_SKIN + hangarIndex, kocmocraftSkin[hangarIndex].ChangeSkin());
        }
    }
}