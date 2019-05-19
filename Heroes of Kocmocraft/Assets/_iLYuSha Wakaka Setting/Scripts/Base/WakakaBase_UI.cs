using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Kocmoca
{
    public partial class WakakaBase : MonoBehaviour
    {
        [Header("UI - Main")]
        public Button btnOption; // Hotkey: ESC
        public Button btnOpenPanel; // Hotkey: P
        [Header("UI - Panel")]
        public AudioClip sfxOpenPanel;
        public AudioClip sfxHidePanel;
        public Transform panel;
        public Button btnHidePanel;
        private Image imgPanel;
        private Image imgHideButton;
        private AudioSource audioSource;
        private TweenerState panelState = TweenerState.Hide;
        [Header("UI - Navigation Bar - Toggle")]
        public TextMeshProUGUI textMain;
        public AudioClip sfxTabPress;
        public Toggle togDesign; // Hotkey: F1
        public Toggle togDubi; // Hotkey: F2
        public Toggle togPerformance; // Hotkey: F3
        public Toggle togAstromech; // Hotkey: F4
        public Toggle togRadar; // Hotkey: F5
        public Toggle togTurret; // Hotkey: F6
        public Toggle togMissile; // Hotkey: F7
        [Header("UI - Navigation Bar - Button")]
        public AudioClip sfxChangeSkin;
        public AudioClip sfxSwitchWireframe;
        public AudioClip sfxChangePilot; // Hotkey: K
        public AudioClip[] sfxTalk; // Hotkey: T
        public Button btnChangeSkin; // Hotkey: C
        public Button btnSwitchWireframe; // Hotkey: W
        public Button btnChangePilot;
        public Button btnTalk;
        private AudioSource speech;
        [Header("UI - Panel")]
        public GameObject panelDesign;
        public GameObject panelDubi;
        public GameObject panelPerformance;
        public GameObject panelAstromech;
        public GameObject panelRadar;
        public GameObject panelTurret;
        public GameObject panelMissile;
        [Header("UI - Panel - Design")]
        public DesignPanel design;
        [Header("UI - Block - Dubi")]
        public DubiPanel dubi;
        [Header("UI - Block - Performance")]
        public PerformancePanel performance;


        [Header("Astromech Droid Data")]
        public AstromechDroid astromechDroid;
        [Header("Radar Data")]
        public Radar radar;
        [Header("Turret Data")]
        public Turret turret;

        void Awake()
        {
            //InitializeTrackingSystem();
            //Portal.OnShutterPressedUp += EnterHangar;
            //SFX = GetComponent<AudioSource>();


            ViewSetting();
            hangarIndex = PlayerPrefs.GetInt(LobbyInfomation.PREFS_TYPE);
            //billboard.localPosition = billboardHide;
            InitializePanel();
            //blockData.localPosition = new Vector3(blockData.localPosition.x, -500, 0);
            // Button & Toggle Event


            // Bar
            //barHull.Initialize(textMaxHull.transform.parent.GetComponentsInChildren<Image>(), 4000, 25000);
            //barShield.Initialize(textMaxShield.transform.parent.GetComponentsInChildren<Image>(), 3000, 24000);
            //barEnergy.Initialize(textMaxEnergy.transform.parent.GetComponentsInChildren<Image>(), 500, 3700);
            //barCruise.Initialize(textCruiseSpeed.transform.parent.GetComponentsInChildren<Image>(), 20, 60);
            //barAfterburne.Initialize(textAfterburneSpeed.transform.parent.GetComponentsInChildren<Image>(), 70, 133);
            //barDamage.Initialize(textDamage.transform.parent.GetComponentsInChildren<Image>(), 1092, 2842);
            //barMaxRange.Initialize(textMaxRange.transform.parent.GetComponentsInChildren<Image>(), 955, 1270);

            hangarState = HangarState.Ready;
            //billboard.localPosition = billboardPos;
            //MoveHangarRail();
        }


      

        [System.Serializable]
        public class Turret : DataBlock
        {
            public TextMeshProUGUI textTurretCount;
            public TextMeshProUGUI textMaxAutoAimAngle;
            public TextMeshProUGUI textRoundsPerMinute;
            public TextMeshProUGUI textMaxProjectileSpread;
            public TextMeshProUGUI textAmmoVelocity;
            public TextMeshProUGUI textOperationalRange;
            public TextMeshProUGUI textDamage;
            public TextMeshProUGUI textDPS;
            public TextMeshProUGUI textShieldPenetration;
            public TextMeshProUGUI textHullPenetration;

            public void SetData(int index)
            {
                //moduleData = KocmocaData.KocmocraftData[index];
                //textTitle.text = moduleData.TurretName;
                //textTurretCount.text = moduleData.TurretCount.ToString() + "管"  ;
                //textMaxAutoAimAngle.text = moduleData.MaxAutoAimAngle.ToString() + " 度";
                //textRoundsPerMinute.text = moduleData.RoundsPerMinute.ToString() + " rpm";
                //textMaxProjectileSpread.text = moduleData.MaxProjectileSpread.ToString() + " 度";
                //textAmmoVelocity.text = Mathf.RoundToInt(moduleData.AmmoVelocity).ToString() + " mps" ;
                //textOperationalRange.text = Mathf.RoundToInt(moduleData.operationalRange).ToString() + " m";
                //textDamage.text = moduleData.Damage.ToString();
                //textDPS.text = moduleData.DPS.ToString();
                //textShieldPenetration.text = moduleData.ShieldPenetration.ToString() + "%";
                //textHullPenetration.text = moduleData.HullPenetration.ToString() + "%";

                //textTurretCount.color = HangarData.TextColor[index];
                //textMaxAutoAimAngle.color = HangarData.TextColor[index];
                //textRoundsPerMinute.color = HangarData.TextColor[index];
                //textMaxProjectileSpread.color = HangarData.TextColor[index];
                //textAmmoVelocity.color = HangarData.TextColor[index];
                //textOperationalRange.color = HangarData.TextColor[index];
                //textDamage.color = HangarData.TextColor[index];
                //textDPS.color = HangarData.TextColor[index];

                //for (int i = 0; i < item.Length; i++)
                //{
                //    item[i].color = HangarData.TextColor[index];
                //}
            }
        }

        void InitializePanel()
        {
            //btnOption.onClick.AddListener(() => OpenPanel());
            //btnOpenPanel.onClick.AddListener(() => OpenPanel());
            panel.localScale = Vector3.zero;
            btnHidePanel.onClick.AddListener(() => ClosePanel());
            imgPanel = panel.GetComponent<Image>();
            imgHideButton = btnHidePanel.image;
            audioSource = panel.GetComponent<AudioSource>();

            // Tab
            togDesign.onValueChanged.AddListener(isOn =>
            {
                audioSource.PlayOneShot(sfxTabPress, 0.73f);
                panelDesign.SetActive(isOn);
                if (isOn) OpenPanel(); else HidePanel();
            });
            togDubi.onValueChanged.AddListener(isOn =>
            {
                audioSource.PlayOneShot(sfxTabPress, 0.73f);
                panelDubi.SetActive(isOn);
                if (isOn) OpenPanel(); else HidePanel();
            });
            togPerformance.onValueChanged.AddListener(isOn =>
            {
                audioSource.PlayOneShot(sfxTabPress, 0.73f);
                panelPerformance.SetActive(isOn);
                if (isOn) OpenPanel(); else HidePanel();

            });
            togAstromech.onValueChanged.AddListener(isOn =>
            {
                audioSource.PlayOneShot(sfxTabPress, 0.73f);
                panelAstromech.SetActive(isOn);
                if (isOn) OpenPanel(); else HidePanel();
            });
            togRadar.onValueChanged.AddListener(isOn =>
            {
                audioSource.PlayOneShot(sfxTabPress, 0.73f);
                panelRadar.SetActive(isOn);
                if (isOn) OpenPanel(); else HidePanel();
            });
            togTurret.onValueChanged.AddListener(isOn =>
            {
                audioSource.PlayOneShot(sfxTabPress, 0.73f);
                panelTurret.SetActive(isOn);
                if (isOn) OpenPanel(); else HidePanel();
            });
            togMissile.onValueChanged.AddListener(isOn =>
            {
                audioSource.PlayOneShot(sfxTabPress, 0.73f);
                panelMissile.SetActive(isOn);
                if (isOn) OpenPanel(); else HidePanel();
            });

            btnChangeSkin.onClick.AddListener(() =>
            {
                audioSource.PlayOneShot(sfxChangeSkin);
                PlayerPrefs.SetInt(LobbyInfomation.PREFS_SKIN + hangarIndex, prototype[hangarIndex].ChangeSkin());
            });
            btnSwitchWireframe.onClick.AddListener(() =>
            {
                audioSource.PlayOneShot(sfxSwitchWireframe);
                prototype[hangarIndex].SwitchWireframe();
            });
            btnChangePilot.onClick.AddListener(() =>
            {
                audioSource.PlayOneShot(sfxChangePilot);
                pilot[hangarIndex].ChangeSkin();
            });
            speech = btnTalk.GetComponent<AudioSource>();
            btnTalk.onClick.AddListener(() => 
            {
                if (!speech.isPlaying && hangarIndex < hangarCount)
                    speech.PlayOneShot(sfxTalk[hangarIndex], 3.7f);
            });
        }

        public void OpenPanel()
        {
            panel.DOKill();
            panel.DOScale(Vector3.one, 0.37f);
            panelState = TweenerState.Open;
        }

        public void HidePanel()
        {
            panel.DOKill();
            panel.DOScale(Vector3.zero, 0.37f);
            panelState = TweenerState.Hide;
        }

        public void ClosePanel()
        {
            panel.DOKill();
            panel.DOScale(Vector3.zero, 0.37f);
            panelState = TweenerState.Hide;

            togDesign.isOn = false;
            togDubi.isOn = false;
            togPerformance.isOn = false;
            togAstromech.isOn = false;
            togRadar.isOn = false;
            togTurret.isOn = false;
            togMissile.isOn = false;
        }



        void LoadHangarData()
        {
            textMain.text = DesignData.Code[hangarIndex];
            design.SetData(hangarIndex, database.kocmocraft[hangarIndex].size);
            dubi.SetData(hangarIndex, database.kocmocraft[hangarIndex]);
            performance.SetData(hangarIndex, database.kocmocraft[hangarIndex]);
            astromechDroid.SetData(hangarIndex);
            radar.SetData(hangarIndex);
            turret.SetData(hangarIndex);
            // 三视图
            topCamera.orthographicSize = database.kocmocraft[hangarIndex].view.orthoSize;
            sideCamera.orthographicSize = database.kocmocraft[hangarIndex].view.orthoSize;
            frontCamera.orthographicSize = database.kocmocraft[hangarIndex].view.orthoSize;
        }


        [System.Serializable]
        public class DesignPanel
        {
            public TextMeshProUGUI textSeason;
            public TextMeshProUGUI textMission;
            public TextMeshProUGUI textOKB;
            public TextMeshProUGUI textProject;
            public TextMeshProUGUI textCode;
            public TextMeshProUGUI textDescription;
            public RectTransform scaleWingspan;
            public RectTransform scaleLength;
            public RectTransform scaleHeight;
            public TextMeshProUGUI textWingspan;
            public TextMeshProUGUI textLength;
            public TextMeshProUGUI textHeight;

            public void Initialize(GameObject panel)
            {
                panel.SetActive(true);
                Transform[] obj = panel.GetComponentsInChildren<Transform>();
                foreach (Transform child in obj)
                {
                    switch (child.name)
                    {
                        case "Design - Season": textSeason = child.GetComponent<TextMeshProUGUI>(); break;
                        case "Design - Mission": textMission = child.GetComponent<TextMeshProUGUI>(); break;
                        case "Design - OKB": textOKB = child.GetComponent<TextMeshProUGUI>(); break;
                        case "Design - Project": textProject = child.GetComponent<TextMeshProUGUI>(); break;
                        case "Design - Code": textCode = child.GetComponent<TextMeshProUGUI>(); break;
                        case "Design - Description": textDescription = child.GetComponent<TextMeshProUGUI>(); break;
                        case "View Scale - Wingspan": scaleWingspan = child.GetComponent<RectTransform>(); break;
                        case "View Text - Wingspan": textWingspan = child.GetComponent<TextMeshProUGUI>(); break;
                        case "View Scale - Length": scaleLength = child.GetComponent<RectTransform>(); break;
                        case "View Text - Length": textLength = child.GetComponent<TextMeshProUGUI>(); break;
                        case "View Scale - Height": scaleHeight = child.GetComponent<RectTransform>(); break;
                        case "View Text - Height": textHeight = child.GetComponent<TextMeshProUGUI>(); break;
                    }
                }
                panel.SetActive(false);
            }
            public void SetData(int index, Size size)
            {
                textSeason.text = DesignData.Season[index];
                textMission.text = DesignData.Mission[index];
                textOKB.text = DesignData.OKB[index];
                textProject.text = DesignData.Project[index];
                textCode.text = DesignData.Code[index];
                textDescription.text = DesignData.Description[index];
                scaleWingspan.sizeDelta = new Vector2(274 * size.wingspanScale, 37);
                scaleLength.sizeDelta = new Vector2(274 * size.lengthScale, 37);
                scaleHeight.sizeDelta = new Vector2(274 * size.heightScale, 37);
                textWingspan.text = size.wingspan.ToString() + " m";
                textLength.text = size.length.ToString() + " m";
                textHeight.text = size.height.ToString() + " m";
            }
        }
        [System.Serializable]
        public class DubiPanel
        {
            public RectTransform scaleChiefHeight;
            public TextMeshProUGUI textChiefHeight;
            public TextMeshProUGUI textChiefName;
            public TextMeshProUGUI textChiefResume;
            public RectTransform scaleSecondHeight;
            public TextMeshProUGUI textSecondHeight;
            public TextMeshProUGUI textSecondName;
            public TextMeshProUGUI textSecondResume;

            public void Initialize(GameObject panel)
            {
                panel.SetActive(true);
                Transform[] obj = panel.GetComponentsInChildren<Transform>();
                foreach (Transform child in obj)
                {
                    switch (child.name)
                    {
                        case "View Scale - Chief Height": scaleChiefHeight = child.GetComponent<RectTransform>(); break;
                        case "View Text - Chief Height": textChiefHeight = child.GetComponent<TextMeshProUGUI>(); break;
                        case "Dubi - Chief Name": textChiefName = child.GetComponent<TextMeshProUGUI>(); break;
                        case "Dubi - Chief Resume": textChiefResume = child.GetComponent<TextMeshProUGUI>(); break;
                        case "View Scale - Second Height": scaleSecondHeight = child.GetComponent<RectTransform>(); break;
                        case "View Text - Second Height": textSecondHeight = child.GetComponent<TextMeshProUGUI>(); break;
                        case "Dubi - Second Name": textSecondName = child.GetComponent<TextMeshProUGUI>(); break;
                        case "Dubi - Second Resume": textSecondResume = child.GetComponent<TextMeshProUGUI>(); break;
                    }
                }
                panel.SetActive(false);
            }
            public void SetData(int index, KocmocraftModule dubi)
            {
                dubi.X = 1007;
                dubi.Z = 2950;
                textChiefName.text = dubi.chief.pilot;
                textChiefResume.text = dubi.chief.resume;
                scaleChiefHeight.sizeDelta = new Vector2(274 * dubi.chief.height / 200, 37);
                textChiefHeight.text = dubi.chief.height.ToString() + " cm";

                textSecondName.text = dubi.reserve.pilot;
                textSecondResume.text = dubi.reserve.resume;
                scaleSecondHeight.sizeDelta = new Vector2(274 * dubi.reserve.height / 200, 37);
                textSecondHeight.text = dubi.reserve.height.ToString() + " cm";
            }
        }
        [System.Serializable]
        public class PerformancePanel
        {
            public RawImage raw;
            public Texture2D[] textures;
            public TextMeshProUGUI textShield, textHull, textSpeed, textLicense, textShieldLevel, textHullLevel, textSpeedLevel;
            public Image[] imgShieldBar, imgHullBar, imgSpeedBar;
            private int m_ShieldLevel, m_HullLevel, m_SpeedLevel;

            public void Initialize(GameObject panel)
            {
                panel.SetActive(true);
                Transform[] obj = panel.GetComponentsInChildren<Transform>();
                foreach (Transform child in obj)
                {
                    switch (child.name)
                    {
                        case "Performance - Shield": textShield = child.GetComponent<TextMeshProUGUI>(); break;
                        case "Performance - Hull": textHull = child.GetComponent<TextMeshProUGUI>(); break;
                        case "Performance - Speed": textSpeed = child.GetComponent<TextMeshProUGUI>(); break;
                        case "Pilot Tier": textLicense = child.GetComponent<TextMeshProUGUI>(); break;
                        case "Block - Shield": textShieldLevel = child.GetComponentInChildren<TextMeshProUGUI>(); imgShieldBar = child.GetComponentsInChildren<Image>(); break;
                        case "Block - Hull": textHullLevel = child.GetComponentInChildren<TextMeshProUGUI>(); imgHullBar = child.GetComponentsInChildren<Image>(); break;
                        case "Block - Speed": textSpeedLevel = child.GetComponentInChildren<TextMeshProUGUI>(); imgSpeedBar = child.GetComponentsInChildren<Image>(); break;
                    }
                }
                panel.SetActive(false);
            }
            public void SetData(int index,  KocmocraftModule performance)
            {
                raw.texture = textures[index];
                textShield.text = performance.shield.value.ToString();
                textHull.text = performance.hull.value.ToString();
                textSpeed.text = ((int)(performance.speed.value * 3.6f)).ToString();
                textLicense.text = "T" + PerformanceData.PilotTier[index].ToString();
                m_ShieldLevel = performance.shield.level;
                m_HullLevel = performance.hull.level;
                m_SpeedLevel = performance.speed.level;
                textShieldLevel.text = "Lv." + m_ShieldLevel + " Shield";
                textHullLevel.text = "Lv." + m_HullLevel + " Hull";
                textSpeedLevel.text = "Lv." + m_SpeedLevel + " Speed";

                for (int i = 1; i < 8; i++)
                {
                    imgShieldBar[i].enabled = i <= m_ShieldLevel ? true : false;
                    imgHullBar[i].enabled = i <= m_HullLevel ? true : false;
                    imgSpeedBar[i].enabled = i <= m_SpeedLevel ? true : false;
                }
            }
        }

        public class PanelBlock
        {
            public TextMeshProUGUI textTitle;
            public TextMeshProUGUI[] item;
        }
        public class DataBlock : PanelBlock
        {
            internal ModuleData moduleData;
        }

        [System.Serializable]
        public class AstromechDroid : DataBlock
        {
            public TextMeshProUGUI textInfomation;
            public TextMeshProUGUI textShieldRecharge;
            public TextMeshProUGUI textCollisionResistance;
            public TextMeshProUGUI textEnginePower;
            public TextMeshProUGUI textLockTime;

            public void SetData(int index)
            {
                //moduleData = KocmocaData.KocmocraftData[index];
                //textTitle.text = moduleData.DroidName;
                //textInfomation.text = moduleData.DroidDetail;
                //textShieldRecharge.text = moduleData.ShieldRecharge.ToString();
                //textCollisionResistance.text = moduleData.CollisionResistance.ToString();
                //textEnginePower.text = moduleData.EnginePower.ToString();
                //textLockTime.text = moduleData.LockTime.ToString() + " sec";

                //textInfomation.color = HangarData.TextColor[index];
                //textShieldRecharge.color = HangarData.TextColor[index];
                //textCollisionResistance.color = HangarData.TextColor[index];
                //textEnginePower.color = HangarData.TextColor[index];
                //textLockTime.color = HangarData.TextColor[index];

                //for (int i = 0; i < item.Length; i++)
                //{
                //    item[i].color = HangarData.TextColor[index];
                //}
            }
        }
        [System.Serializable]
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
                //moduleData = KocmocaData.KocmocraftData[index];
                //textTitle.text = moduleData.RadarName;
                //textInfomation.text = moduleData.RadarDetail;
                //textMaxSearchRadius.text = moduleData.MaxSearchRadius.ToString() + " m";
                //textMinSearchRadius.text = moduleData.MinSearchRadius.ToString() + " m";
                //textMaxSearchAngle.text = moduleData.MaxSearchAngle.ToString() + " 度";
                //textLockDistance.text = moduleData.MaxLockDistance.ToString() + " m";
                //textLockAngle.text = moduleData.MaxLockAngle.ToString() + " 度";

                //textInfomation.color = HangarData.TextColor[index];
                //textMaxSearchRadius.color = HangarData.TextColor[index];
                //textMinSearchRadius.color = HangarData.TextColor[index];
                //textMaxSearchAngle.color = HangarData.TextColor[index];
                //textLockDistance.color = HangarData.TextColor[index];
                //textLockAngle.color = HangarData.TextColor[index];

                //for (int i = 0; i < item.Length; i++)
                //{
                //    item[i].color = HangarData.TextColor[index];
                //}
            }
        }
    }
}
