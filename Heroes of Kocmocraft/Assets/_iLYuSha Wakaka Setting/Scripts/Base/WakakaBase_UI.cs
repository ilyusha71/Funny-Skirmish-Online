﻿using DG.Tweening;
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
        [Header("UI - Panel")]
        public AudioClip sfxHidePanel;
        public Transform panel;
        public Button btnHidePanel;
        public AudioSource audioSource;
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
        public AudioSource talkSource;
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
        [Header("UI - Panel - Dubi")]
        public DubiPanel dubi;
        [Header("UI - Panel - Performance")]
        public PerformancePanel performance;
        [Header("UI - Panel - Turret")]
        public TurretPanel turret;

        [Header("Astromech Droid Data")]
        public AstromechDroid astromechDroid;
        [Header("Radar Data")]
        public Radar radar;

        void Awake()
        {
            //Portal.OnShutterPressedUp += EnterHangar;

            ViewSetting();
            hangarIndex = PlayerPrefs.GetInt(LobbyInfomation.PREFS_TYPE);
            InitializePanel();
            hangarState = HangarState.Ready;
        }

        void InitializePanel()
        {
            //btnOption.onClick.AddListener(() => OpenPanel());
            panel.localScale = Vector3.zero;
            btnHidePanel.onClick.AddListener(() => ClosePanel());

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
            btnTalk.onClick.AddListener(() => 
            {
                if (!talkSource.isPlaying && hangarIndex < hangarCount)
                    talkSource.PlayOneShot(sfxTalk[hangarIndex]);
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
            turret.SetData(hangarIndex, database.kocmocraft[hangarIndex].turret);
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
            public TextMeshProUGUI textChiefPilot;
            public TextMeshProUGUI textChiefResume;
            public RectTransform scaleReserveHeight;
            public TextMeshProUGUI textReserveHeight;
            public TextMeshProUGUI textReservePilot;
            public TextMeshProUGUI textReserveResume;

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
                        case "Dubi - Chief Name": textChiefPilot = child.GetComponent<TextMeshProUGUI>(); break;
                        case "Dubi - Chief Resume": textChiefResume = child.GetComponent<TextMeshProUGUI>(); break;
                        case "View Scale - Reserve Height": scaleReserveHeight = child.GetComponent<RectTransform>(); break;
                        case "View Text - Reserve Height": textReserveHeight = child.GetComponent<TextMeshProUGUI>(); break;
                        case "Dubi - Reserve Name": textReservePilot = child.GetComponent<TextMeshProUGUI>(); break;
                        case "Dubi - Reserve Resume": textReserveResume = child.GetComponent<TextMeshProUGUI>(); break;
                    }
                }
                panel.SetActive(false);
            }
            public void SetData(int index, KocmocraftModule dubi)
            {
                scaleChiefHeight.sizeDelta = new Vector2(274 * dubi.chief.height / 200, 37);
                textChiefHeight.text = dubi.chief.height.ToString() + " cm";
                textChiefPilot.text = dubi.chief.pilot;
                textChiefResume.text = dubi.chief.resume;

                scaleReserveHeight.sizeDelta = new Vector2(274 * dubi.reserve.height / 200, 37);
                textReserveHeight.text = dubi.reserve.height.ToString() + " cm";
                textReservePilot.text = dubi.reserve.pilot;
                textReserveResume.text = dubi.reserve.resume;
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
            public TextMeshProUGUI textShieldCrystal, textEMField, textEMFieldLevel, textAirframe, textArmor, textArmorLevel, textEngine, textAfterburner, textAfterburnerLevel;
            public void Initialize(GameObject panel)
            {
                panel.SetActive(true);
                Transform[] obj = panel.GetComponentsInChildren<Transform>();
                foreach (Transform child in obj)
                {
                    switch (child.name)
                    {
                        case "Pilot Tier": textLicense = child.GetComponent<TextMeshProUGUI>(); break;
                        case "Performance - Shield": textShield = child.GetComponent<TextMeshProUGUI>(); imgShieldBar = child.GetComponentsInChildren<Image>(); break;
                        case "Performance - Hull": textHull = child.GetComponent<TextMeshProUGUI>(); imgHullBar = child.GetComponentsInChildren<Image>(); break;
                        case "Performance - Speed": textSpeed = child.GetComponent<TextMeshProUGUI>(); imgSpeedBar = child.GetComponentsInChildren<Image>(); break;
                        case "Performance - Shield Level": textShieldLevel = child.GetComponentInChildren<TextMeshProUGUI>(); break;
                        case "Performance - Hull Level": textHullLevel = child.GetComponentInChildren<TextMeshProUGUI>(); break;
                        case "Performance - Speed Level": textSpeedLevel = child.GetComponentInChildren<TextMeshProUGUI>(); break;
                        case "Performance - Shield Crystal": textShieldCrystal = child.GetComponent<TextMeshProUGUI>(); break;
                        case "Performance - EM Field": textEMField = child.GetComponent<TextMeshProUGUI>(); break;
                        case "Performance - EM Field Level": textEMFieldLevel = child.GetComponent<TextMeshProUGUI>(); break;
                        case "Performance - Airframe": textAirframe = child.GetComponent<TextMeshProUGUI>(); break;
                        case "Performance - Armor": textArmor = child.GetComponent<TextMeshProUGUI>(); break;
                        case "Performance - Armor Level": textArmorLevel = child.GetComponent<TextMeshProUGUI>(); break;
                        case "Performance - Engine": textEngine = child.GetComponent<TextMeshProUGUI>(); break;
                        case "Performance - Afterburner": textAfterburner = child.GetComponent<TextMeshProUGUI>(); break;
                        case "Performance - Afterburner Level": textAfterburnerLevel = child.GetComponent<TextMeshProUGUI>(); break;
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
                textShieldLevel.text = m_ShieldLevel.ToString();
                textHullLevel.text = m_HullLevel.ToString();
                textSpeedLevel.text = m_SpeedLevel.ToString();
                textShieldCrystal.text = "+ " + performance.shield.shieldCrystal.ToString();
                textEMField.text = "+ " + performance.shield.emField.ToString();
                textEMFieldLevel.text = performance.shield.emFieldLevel.ToString();
                textAirframe.text = "+ " + performance.hull.airframe.ToString();
                textArmor.text = "+ " + performance.hull.armor.ToString();
                textArmorLevel.text = performance.hull.armorLevel.ToString();
                textEngine.text = "+ "+((int)(performance.speed.engine * 3.6f)).ToString();
                textAfterburner.text = "+ " + ((int)(performance.speed.afterburner * 3.6f)).ToString();
                textAfterburnerLevel.text = performance.speed.afterburnerLevel.ToString();

                for (int i = 1; i < 8; i++)
                {
                    imgShieldBar[i].enabled = i <= m_ShieldLevel ? true : false;
                    imgHullBar[i].enabled = i <= m_HullLevel ? true : false;
                    imgSpeedBar[i].enabled = i <= m_SpeedLevel ? true : false;
                }
            }
        }

        [System.Serializable]
        public class TurretPanel
        {
            public TextMeshProUGUI textCannonName;
            public TextMeshProUGUI textCannonCount;
            public TextMeshProUGUI textMaxAutoAimAngle;
            public TextMeshProUGUI textRoundsPerMinute;
            public TextMeshProUGUI textMaxProjectileSpread;
            public TextMeshProUGUI textAmmoName;
            public TextMeshProUGUI textAmmoVelocity;
            public TextMeshProUGUI textOperationalRange;
            public TextMeshProUGUI textDamage;
            public TextMeshProUGUI textDPS;
            public TextMeshProUGUI textPenetration;
            public TextMeshProUGUI textPiercing;

            public void Initialize(GameObject panel)
            {
                panel.SetActive(true);
                Transform[] obj = panel.GetComponentsInChildren<Transform>();
                foreach (Transform child in obj)
                {
                    switch (child.name)
                    {
                        case "Turret - Cannon Name": textCannonName = child.GetComponent<TextMeshProUGUI>(); break;
                        case "Turret - Cannon Count": textCannonCount = child.GetComponent<TextMeshProUGUI>(); break;
                        case "Turret - Auto Aim": textMaxAutoAimAngle = child.GetComponent<TextMeshProUGUI>(); break;
                        case "Turret - RPM": textRoundsPerMinute = child.GetComponent<TextMeshProUGUI>(); break;
                        case "Turret - Spread": textMaxProjectileSpread = child.GetComponent<TextMeshProUGUI>(); break;
                        case "Turret - Ammo Name": textAmmoName = child.GetComponent<TextMeshProUGUI>(); break;
                        case "Turret - Ammo Velocity": textAmmoVelocity = child.GetComponent<TextMeshProUGUI>(); break;
                        case "Turret - Operational Range": textOperationalRange = child.GetComponent<TextMeshProUGUI>(); break;
                        case "Turret - Damage": textDamage = child.GetComponent<TextMeshProUGUI>(); break;
                        case "Turret - DPS": textDPS = child.GetComponent<TextMeshProUGUI>(); break;
                        case "Turret - Penetration": textPenetration = child.GetComponent<TextMeshProUGUI>(); break;
                        case "Turret - Piercing": textPiercing = child.GetComponent<TextMeshProUGUI>(); break;
                    }
                }
                panel.SetActive(false);
            }

            public void SetData(int index, Turret turret)
            {
                textCannonName.text = turret.cannonName;
                textCannonCount.text = turret.cannonCount.ToString() + " Cannon";
                textMaxAutoAimAngle.text = turret.maxAutoAimAngle.ToString() + "°";
                textRoundsPerMinute.text = turret.roundsPerMinute.ToString() + " rpm";
                textMaxProjectileSpread.text = turret.maxProjectileSpread.ToString() + "°";
                textAmmoName.text = turret.ammoName;
                textAmmoVelocity.text = Mathf.RoundToInt(turret.ammoVelocity*3.6f).ToString() + " Kph";
                textOperationalRange.text = Mathf.RoundToInt(turret.operationalRange).ToString() + " m";
                textDamage.text = turret.damage.ToString() + " Pts";
                textDPS.text = turret.dPS.ToString() + " Pts";
                textPenetration.text = turret.penetration.ToString() + " %";
                textPiercing.text = turret.piercing.ToString() + " %";
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
