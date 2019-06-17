/***************************************************************************
 * Wakaka Base UI
 * 基地UI
 * Last Updated: 2019/06/07
 * 
 * v19.0607
 * 1. Panel
 * 
 ***************************************************************************/

using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Kocmoca
{
    public partial class WakakaBase : MonoBehaviour
    {
        [Header ("UI - Panel")]
        public AudioClip sfxHidePanel;
        public Transform panel;
        public Button btnHidePanel;
        private TweenerState panelState = TweenerState.Hide;
        public UITextConverter display;
        [Header ("UI - Navigation Bar - Toggle Tab")]
        public AudioSource audioTabPress;
        public Toggle[] togTabs; // Hotkey: F1~F8
        public RectTransform[] panels;
        public int panelCounts;
        [Header ("UI - Navigation Bar - Button")]
        public AudioSource audioButtonPress;
        public AudioClip sfxChangeSkin;
        public AudioClip sfxChangePainting;
        public AudioClip sfxSwitchWireframe;
        public AudioClip sfxChangePilot;
        public AudioSource radioAudio;
        public AudioClip[] radioClips;
        public Button btnChangePainting;
        public Button btnSwitchWireframe;
        public Button btnChangePilot;
        public Button btnSwitchCockpitView; // Hotkey: C
        public Button btnRadio; // Hotkey: R
        public Button btnOption; // Hotkey: ESC
        [Header ("UI - Panel")]
        public DesignPanel designPanel;
        public PilotPanel pilotPanel;
        public PerformancePanel performancePanel;
        public PowerSystemPanel powerSystemPanel;
        public TurretPanel turretPanel;
        public KocmomechPanel kocmomechPanel;
        public Radar radar;

        void Awake ()
        {
            //Portal.OnShutterPressedUp += EnterHangar;

            ViewSetting ();
            hangarIndex = PlayerPrefs.GetInt (LobbyInfomation.PREFS_TYPE);
            InitializePanel ();
            hangarState = HangarState.Ready;
        }
        void InitializePanel ()
        {
            // Panel setting
            panel.localScale = Vector3.zero;
            btnHidePanel.onClick.AddListener (() => ClosePanel ());

            // Toggle tabs & panels
            for (int i = 0; i < panelCounts; i++)
            {
                int index = i;
                togTabs[index].onValueChanged.AddListener (isOn =>
                {
                    audioTabPress.Play ();
                    // panels[index].SetActive (isOn);
                    if (isOn)
                    {
                        OpenPanel ();
                        panels[index].anchoredPosition = Vector3.zero;
                    }
                    else
                    {
                        panels[index].anchoredPosition = KocmocaData.invisible;
                        HidePanel ();
                    }
                });
                panels[index].gameObject.SetActive (true);
                panels[index].anchoredPosition = KocmocaData.invisible;
            }

            // Button
            btnChangePainting.onClick.AddListener (() =>
            {
                audioButtonPress.clip = sfxChangePainting;
                audioButtonPress.Play ();
                PlayerPrefs.SetInt (LobbyInfomation.PREFS_SKIN + hangarIndex, prototype[hangarIndex].ChangeSkin ());
            });
            btnSwitchWireframe.onClick.AddListener (() =>
            {
                audioButtonPress.clip = sfxSwitchWireframe;
                audioButtonPress.Play ();
                prototype[hangarIndex].SwitchWireframe ();
            });
            btnChangePilot.onClick.AddListener (() =>
            {
                audioButtonPress.clip = sfxChangePilot;
                audioButtonPress.Play ();
                pilot[hangarIndex].ChangePilot ();
            });
            btnSwitchCockpitView.onClick.AddListener (() =>
            {
                SwitchCockpit ();
            });
            btnRadio.onClick.AddListener (() =>
            {
                radioAudio.clip = radioClips[hangarIndex];
                radioAudio.Play ();
            });
        }
        public void OpenPanel ()
        {
            panel.DOKill ();
            panel.DOScale (Vector3.one, 0.37f);
            panelState = TweenerState.Open;
        }
        public void HidePanel ()
        {
            panel.DOKill ();
            panel.DOScale (Vector3.zero, 0.37f);
            panelState = TweenerState.Hide;
        }
        public void ClosePanel ()
        {
            panel.DOKill ();
            panel.DOScale (Vector3.zero, 0.37f);
            panelState = TweenerState.Hide;

            for (int i = 0; i < panelCounts; i++)
            {
                togTabs[i].isOn = false;
            }
        }
        void LoadHangarData ()
        {
            KocmocraftModule module = database.kocmocraft[hangarIndex];
            designPanel.SetData (module.design);
            pilotPanel.SetData (hangarIndex, module);
            performancePanel.SetData (hangarIndex, module);
            powerSystemPanel.SetData (hangarIndex, module.powerSystem);
            radar.SetData (hangarIndex);
            turretPanel.SetData (hangarIndex, module.turret);
            kocmomechPanel.SetData (hangarIndex, module.kocmomech);
        }

        [System.Serializable]
        public class DesignPanel
        {
            public UITextConverter display;
            public TextMeshProUGUI textCode, textProject, textOKB, textMission, textDebut, textDevelopment;
            public Camera topCamera, sideCamera, frontCamera;
            public RectTransform scaleWingspan, scaleLength, scaleHeight;
            public TextMeshProUGUI textWingspan, textLength, textHeight;
            public TextMeshProUGUI textWeight;
#if UNITY_EDITOR
            public void Initialize (RectTransform panel, Transform hangarView)
            {
                topCamera = hangarView.GetComponentsInChildren<Camera> () [0];
                sideCamera = hangarView.GetComponentsInChildren<Camera> () [1];
                frontCamera = hangarView.GetComponentsInChildren<Camera> () [2];
                Transform[] obj = panel.GetComponentsInChildren<Transform> ();
                foreach (Transform child in obj)
                {
                    switch (child.name)
                    {
                        case "Design - Code":
                            textCode = child.GetComponent<TextMeshProUGUI> ();
                            break;
                        case "Design - Project":
                            textProject = child.GetComponent<TextMeshProUGUI> ();
                            break;
                        case "Design - OKB":
                            textOKB = child.GetComponent<TextMeshProUGUI> ();
                            break;
                        case "Design - Mission":
                            textMission = child.GetComponent<TextMeshProUGUI> ();
                            break;
                        case "Design - Debut":
                            textDebut = child.GetComponent<TextMeshProUGUI> ();
                            break;
                        case "Design - Development":
                            textDevelopment = child.GetComponent<TextMeshProUGUI> ();
                            break;
                        case "View Scale - Wingspan":
                            scaleWingspan = child.GetComponent<RectTransform> ();
                            break;
                        case "View Scale - Length":
                            scaleLength = child.GetComponent<RectTransform> ();
                            break;
                        case "View Scale - Height":
                            scaleHeight = child.GetComponent<RectTransform> ();
                            break;
                        case "View Text - Wingspan":
                            textWingspan = child.GetComponent<TextMeshProUGUI> ();
                            break;
                        case "View Text - Length":
                            textLength = child.GetComponent<TextMeshProUGUI> ();
                            break;
                        case "View Text - Height":
                            textHeight = child.GetComponent<TextMeshProUGUI> ();
                            break;
                        case "Design - Weight":
                            textWeight = child.GetComponent<TextMeshProUGUI> ();
                            break;
                    }
                }
                panel.anchoredPosition = KocmocaData.invisible;
            }
#endif
            public void SetData (Design design)
            {
                display.en = design.code;
                display.cn = design.project;
                display.textTitle.text = display.en;
                textCode.text = design.code;
                textProject.text = design.project;
                textOKB.text = design.OKB;
                textDebut.text = design.debut;
                textMission.text = design.mission;
                textDevelopment.text = design.development;
                topCamera.orthographicSize = design.view.orthoSize;
                sideCamera.orthographicSize = design.view.orthoSize;
                frontCamera.orthographicSize = design.view.orthoSize;
                scaleWingspan.sizeDelta = new Vector2 (274 * design.size.wingspanScale, 37);
                scaleLength.sizeDelta = new Vector2 (274 * design.size.lengthScale, 37);
                scaleHeight.sizeDelta = new Vector2 (274 * design.size.heightScale, 37);
                textWingspan.text = design.size.wingspan.ToString () + " m";
                textLength.text = design.size.length.ToString () + " m";
                textHeight.text = design.size.height.ToString () + " m";
                textWeight.text = design.size.weight.ToString ();
            }
        }

        [System.Serializable]
        public class PilotPanel
        {
            [Header ("Panel")]
            public RectTransform scaleChiefHeight;
            public TextMeshProUGUI textChiefHeight;
            public TextMeshProUGUI textChiefPilot;
            public RectTransform scaleReserveHeight;
            public TextMeshProUGUI textReserveHeight;
            public TextMeshProUGUI textReservePilot;
            [Header ("Tips")]
            public TextMeshProUGUI tipChief;
            public TextMeshProUGUI tipReserve;
#if UNITY_EDITOR
            public void Initialize (RectTransform panel, GameObject tip)
            {
                Transform[] obj = panel.GetComponentsInChildren<Transform> ();
                foreach (Transform child in obj)
                {
                    switch (child.name)
                    {
                        case "View Scale - Chief Height":
                            scaleChiefHeight = child.GetComponent<RectTransform> ();
                            break;
                        case "View Text - Chief Height":
                            textChiefHeight = child.GetComponent<TextMeshProUGUI> ();
                            break;
                        case "Dubi - Chief Pilot":
                            textChiefPilot = child.GetComponent<TextMeshProUGUI> ();
                            break;
                        case "View Scale - Reserve Height":
                            scaleReserveHeight = child.GetComponent<RectTransform> ();
                            break;
                        case "View Text - Reserve Height":
                            textReserveHeight = child.GetComponent<TextMeshProUGUI> ();
                            break;
                        case "Dubi - Reserve Pilot":
                            textReservePilot = child.GetComponent<TextMeshProUGUI> ();
                            break;
                    }
                }
                obj = tip.GetComponentsInChildren<Transform> ();
                foreach (Transform child in obj)
                {
                    switch (child.name)
                    {
                        case "Tip - Chief":
                            tipChief = child.GetComponent<TextMeshProUGUI> ();
                            break;
                        case "Tip - Reserve":
                            tipReserve = child.GetComponent<TextMeshProUGUI> ();
                            break;
                    }
                }
                panel.anchoredPosition = KocmocaData.invisible;
            }
#endif
            public void SetData (int index, KocmocraftModule dubi)
            {
                scaleChiefHeight.sizeDelta = new Vector2 (274 * dubi.chief.height / 200, 37);
                textChiefHeight.text = dubi.chief.height.ToString () + " cm";
                textChiefPilot.text = dubi.chief.pilot;

                scaleReserveHeight.sizeDelta = new Vector2 (274 * dubi.reserve.height / 200, 37);
                textReserveHeight.text = dubi.reserve.height.ToString () + " cm";
                textReservePilot.text = dubi.reserve.pilot;

                tipChief.text = dubi.chief.resume;
                tipReserve.text = dubi.reserve.resume;
            }
        }

        [System.Serializable]
        public class PerformancePanel
        {
            [Header ("Panel")]
            public RawImage raw;
            public Texture2D[] textures;
            public TextMeshProUGUI textShield, textHull, textSpeed, textLicense, textShieldLevel, textHullLevel, textSpeedLevel;
            public Image[] imgShieldBar, imgHullBar, imgSpeedBar;
            private int m_ShieldLevel, m_HullLevel, m_SpeedLevel;
            [Header ("Tips")]
            public TextMeshProUGUI tipEMCrystal, tipEMBoosterl, tipEMBoosterlLevel;
            public TextMeshProUGUI tipAirframe, tipArmor, tipArmorLevel;
            public TextMeshProUGUI tipPowerSystem, tipAfterburner, tipAfterburnerLevel;
#if UNITY_EDITOR
            public void Initialize (RectTransform panel, GameObject tip)
            {
                Transform[] obj = panel.GetComponentsInChildren<Transform> ();
                foreach (Transform child in obj)
                {
                    switch (child.name)
                    {
                        case "Pilot Tier":
                            textLicense = child.GetComponent<TextMeshProUGUI> ();
                            break;
                        case "Performance - Shield":
                            textShield = child.GetComponent<TextMeshProUGUI> ();
                            imgShieldBar = child.GetComponentsInChildren<Image> ();
                            break;
                        case "Performance - Hull":
                            textHull = child.GetComponent<TextMeshProUGUI> ();
                            imgHullBar = child.GetComponentsInChildren<Image> ();
                            break;
                        case "Performance - Speed":
                            textSpeed = child.GetComponent<TextMeshProUGUI> ();
                            imgSpeedBar = child.GetComponentsInChildren<Image> ();
                            break;
                        case "Performance - Shield Level":
                            textShieldLevel = child.GetComponentInChildren<TextMeshProUGUI> ();
                            break;
                        case "Performance - Hull Level":
                            textHullLevel = child.GetComponentInChildren<TextMeshProUGUI> ();
                            break;
                        case "Performance - Speed Level":
                            textSpeedLevel = child.GetComponentInChildren<TextMeshProUGUI> ();
                            break;
                    }
                }
                obj = tip.GetComponentsInChildren<Transform> ();
                foreach (Transform child in obj)
                {
                    switch (child.name)
                    {
                        case "Performance - EM Crystal":
                            tipEMCrystal = child.GetComponent<TextMeshProUGUI> ();
                            break;
                        case "Performance - EM Booster":
                            tipEMBoosterl = child.GetComponent<TextMeshProUGUI> ();
                            break;
                        case "Performance - EM Booster Level":
                            tipEMBoosterlLevel = child.GetComponent<TextMeshProUGUI> ();
                            break;
                        case "Performance - Airframe":
                            tipAirframe = child.GetComponent<TextMeshProUGUI> ();
                            break;
                        case "Performance - Armor":
                            tipArmor = child.GetComponent<TextMeshProUGUI> ();
                            break;
                        case "Performance - Armor Level":
                            tipArmorLevel = child.GetComponent<TextMeshProUGUI> ();
                            break;
                        case "Performance - Power System":
                            tipPowerSystem = child.GetComponent<TextMeshProUGUI> ();
                            break;
                        case "Performance - Afterburner":
                            tipAfterburner = child.GetComponent<TextMeshProUGUI> ();
                            break;
                        case "Performance - Afterburner Level":
                            tipAfterburnerLevel = child.GetComponent<TextMeshProUGUI> ();
                            break;
                    }
                }
                panel.anchoredPosition = KocmocaData.invisible;
            }
#endif
            public void SetData (int index, KocmocraftModule performance)
            {
                raw.texture = textures[index];
                textShield.text = performance.shield.maximum.ToString ();
                textHull.text = performance.hull.maximum.ToString ();
                textSpeed.text = Mathf.RoundToInt (performance.speed.maximum * 3.6f).ToString ();
                textLicense.text = "T" + PerformanceData.PilotTier[index].ToString ();
                m_ShieldLevel = performance.shield.level;
                m_HullLevel = performance.hull.level;
                m_SpeedLevel = performance.speed.level;
                textShieldLevel.text = m_ShieldLevel.ToString ();
                textHullLevel.text = m_HullLevel.ToString ();
                textSpeedLevel.text = m_SpeedLevel.ToString ();

                tipEMCrystal.text = "+ " + performance.shield.emCrystal.ToString ();
                tipEMBoosterl.text = "+ " + performance.shield.emBooster.ToString ();
                tipEMBoosterlLevel.text = performance.shield.emBoosterLevel.ToString ();
                tipAirframe.text = "+ " + performance.hull.airframe.ToString ();
                tipArmor.text = "+ " + performance.hull.armor.ToString ();
                tipArmorLevel.text = performance.hull.armorLevel.ToString ();
                tipPowerSystem.text = "+ " + Mathf.RoundToInt (performance.speed.powerSystem * 3.6f).ToString ();
                tipAfterburner.text = "+ " + Mathf.RoundToInt (performance.speed.afterburner * 3.6f).ToString ();
                tipAfterburnerLevel.text = performance.speed.afterburnerLevel.ToString ();

                for (int i = 1; i < 8; i++)
                {
                    imgShieldBar[i].enabled = i <= m_ShieldLevel ? true : false;
                    imgHullBar[i].enabled = i <= m_HullLevel ? true : false;
                    imgSpeedBar[i].enabled = i <= m_SpeedLevel ? true : false;
                }
            }
        }

        [System.Serializable]
        public class PowerSystemPanel
        {
            [System.Serializable]
            public class PowerUnitText
            {
                public TextMeshProUGUI textThrust, textSpeed, textAcceleration;
            }
            /* abbr. */
            // MPU: Main Power Unit
            // APU: Auxiliary Power Unit
            // MP: Main Power
            // AP: Auxiliary Power
            [Header ("Panel")]
            public Image iconMPU;
            public Image iconAPU;
            public UITextConverter textMPU, textAPU;
            public PowerUnitText mainPower, auxiliaryPower, total;
            [Header ("Tips")]
            public TextMeshProUGUI tipMPUCount;
            public TextMeshProUGUI tipAPUCount;
            public PowerUnitText mpu;
            public PowerUnitText apu;
#if UNITY_EDITOR
            public void Initialize (RectTransform panel, GameObject tip)
            {
                Transform[] obj = panel.GetComponentsInChildren<Transform> ();
                foreach (Transform child in obj)
                {
                    switch (child.name)
                    {
                        case "Icon - MPU":
                            iconMPU = child.GetComponent<Image> ();
                            break;
                        case "Icon - APU":
                            iconAPU = child.GetComponent<Image> ();
                            break;
                        case "Item Text - MPU Type":
                            textMPU = child.GetComponent<UITextConverter> ();
                            break;
                        case "Item Text - APU Type":
                            textAPU = child.GetComponent<UITextConverter> ();
                            break;
                        case "Main Power - Thrust":
                            mainPower.textThrust = child.GetComponent<TextMeshProUGUI> ();
                            break;
                        case "Main Power - Speed":
                            mainPower.textSpeed = child.GetComponent<TextMeshProUGUI> ();
                            break;
                        case "Main Power - Acceleration":
                            mainPower.textAcceleration = child.GetComponent<TextMeshProUGUI> ();
                            break;
                        case "Auxiliary Power - Thrust":
                            auxiliaryPower.textThrust = child.GetComponent<TextMeshProUGUI> ();
                            break;
                        case "Auxiliary Power - Speed":
                            auxiliaryPower.textSpeed = child.GetComponent<TextMeshProUGUI> ();
                            break;
                        case "Auxiliary Power - Acceleration":
                            auxiliaryPower.textAcceleration = child.GetComponent<TextMeshProUGUI> ();
                            break;
                        case "Total - Thrust":
                            total.textThrust = child.GetComponent<TextMeshProUGUI> ();
                            break;
                        case "Total - Speed":
                            total.textSpeed = child.GetComponent<TextMeshProUGUI> ();
                            break;
                        case "Total - Acceleration":
                            total.textAcceleration = child.GetComponent<TextMeshProUGUI> ();
                            break;
                    }
                }
                obj = tip.GetComponentsInChildren<Transform> ();
                foreach (Transform child in obj)
                {
                    switch (child.name)
                    {
                        case "MPU - Count":
                            tipMPUCount = child.GetComponent<TextMeshProUGUI> ();
                            break;
                        case "MPU - Thrust":
                            mpu.textThrust = child.GetComponent<TextMeshProUGUI> ();
                            break;
                        case "MPU - Speed":
                            mpu.textSpeed = child.GetComponent<TextMeshProUGUI> ();
                            break;
                        case "MPU - Acceleration":
                            mpu.textAcceleration = child.GetComponent<TextMeshProUGUI> ();
                            break;
                        case "APU - Count":
                            tipAPUCount = child.GetComponent<TextMeshProUGUI> ();
                            break;
                        case "APU - Thrust":
                            apu.textThrust = child.GetComponent<TextMeshProUGUI> ();
                            break;
                        case "APU - Speed":
                            apu.textSpeed = child.GetComponent<TextMeshProUGUI> ();
                            break;
                        case "APU - Acceleration":
                            apu.textAcceleration = child.GetComponent<TextMeshProUGUI> ();
                            break;
                    }
                }
                panel.anchoredPosition = KocmocaData.invisible;
            }
#endif
            public void SetData (int index, PowerSystem powerSystem)
            {
                iconMPU.sprite = powerSystem.listMPU[0].icon;
                textMPU.en = powerSystem.listMPU[0].typeEN;
                textMPU.cn = powerSystem.listMPU[0].typeCN;
                textMPU.textTitle.text = textMPU.en;
                mainPower.textThrust.text = powerSystem.mainPower.thrust.ToString ();
                mainPower.textSpeed.text = powerSystem.mainPower.speed.ToString ();
                mainPower.textAcceleration.text = powerSystem.mainPower.acceleration.ToString ();
                total.textThrust.text = powerSystem.total.thrust.ToString ();
                total.textSpeed.text = powerSystem.total.speed.ToString ();
                total.textAcceleration.text = powerSystem.total.acceleration.ToString ();

                tipMPUCount.text = "x " + powerSystem.mpuCount.ToString ();
                mpu.textThrust.text = "+ " + powerSystem.mpu.thrust.ToString ();
                mpu.textSpeed.text = "+ " + powerSystem.mpu.speed.ToString ();
                mpu.textAcceleration.text = "+ " + powerSystem.mpu.acceleration.ToString ();

                if (powerSystem.apuCount > 0)
                {
                    iconAPU.enabled = true;
                    iconAPU.sprite = powerSystem.listAPU[0].icon;
                    textAPU.en = powerSystem.listAPU[0].typeEN;
                    textAPU.cn = powerSystem.listAPU[0].typeCN;
                    textAPU.textTitle.text = textAPU.en;
                    auxiliaryPower.textThrust.text = powerSystem.auxiliaryPower.thrust.ToString ();
                    auxiliaryPower.textSpeed.text = powerSystem.auxiliaryPower.speed.ToString ();
                    auxiliaryPower.textAcceleration.text = powerSystem.auxiliaryPower.acceleration.ToString ();

                    tipAPUCount.text = "x " + powerSystem.apuCount.ToString ();
                    apu.textThrust.text = "+ " + powerSystem.apu.thrust.ToString ();
                    apu.textSpeed.text = "+ " + powerSystem.apu.speed.ToString ();
                    apu.textAcceleration.text = "+ " + powerSystem.apu.acceleration.ToString ();
                }
                else
                {
                    iconAPU.enabled = false;
                    textAPU.en = "Not configured";
                    textAPU.cn = "未配置";
                    textAPU.textTitle.text = textAPU.en;
                    auxiliaryPower.textThrust.text = "0";
                    auxiliaryPower.textSpeed.text = "0";
                    auxiliaryPower.textAcceleration.text = "0";

                    tipAPUCount.text = "x 0";
                    apu.textThrust.text = "+ 0";
                    apu.textSpeed.text = "+ 0";
                    apu.textAcceleration.text = "+ 0";
                }
            }
        }

        [System.Serializable]
        public class TurretPanel
        {
            [Header ("Cannon")]
            public UITextConverter textCannonType;
            public TextMeshProUGUI textCount, textAutoAim, textRPM, textSpread, textDPS;
            public TextMeshProUGUI tipCannon;
            [Header ("Ammo")]
            public UITextConverter textAmmoType;
            public TextMeshProUGUI textVelocity, textRange, textDamage, textPenetration, textPiercing;
#if UNITY_EDITOR
            public void Initialize (RectTransform panel, GameObject tip)
            {
                Transform[] obj = panel.GetComponentsInChildren<Transform> ();
                foreach (Transform child in obj)
                {
                    switch (child.name)
                    {
                        case "Item Text - Cannon Type":
                            textCannonType = child.GetComponent<UITextConverter> ();
                            break;
                        case "Cannon - Count":
                            textCount = child.GetComponent<TextMeshProUGUI> ();
                            break;
                        case "Cannon - Auto Aim":
                            textAutoAim = child.GetComponent<TextMeshProUGUI> ();
                            break;
                        case "Cannon - RPM":
                            textRPM = child.GetComponent<TextMeshProUGUI> ();
                            break;
                        case "Cannon - Spread":
                            textSpread = child.GetComponent<TextMeshProUGUI> ();
                            break;
                        case "Cannon - DPS":
                            textDPS = child.GetComponent<TextMeshProUGUI> ();
                            break;
                        case "Item Text - Ammo Type":
                            textAmmoType = child.GetComponent<UITextConverter> ();
                            break;
                        case "Ammo - Velocity":
                            textVelocity = child.GetComponent<TextMeshProUGUI> ();
                            break;
                        case "Ammo - Range":
                            textRange = child.GetComponent<TextMeshProUGUI> ();
                            break;
                        case "Ammo - Damage":
                            textDamage = child.GetComponent<TextMeshProUGUI> ();
                            break;
                        case "Ammo - Penetration":
                            textPenetration = child.GetComponent<TextMeshProUGUI> ();
                            break;
                        case "Ammo - Piercing":
                            textPiercing = child.GetComponent<TextMeshProUGUI> ();
                            break;
                    }
                }
                  obj = tip.GetComponentsInChildren<Transform> ();
                foreach (Transform child in obj)
                {
                    switch (child.name)
                    {
                        case "Tip - Cannon":
                            tipCannon = child.GetComponent<TextMeshProUGUI> ();
                            break;
                    }
                }
                panel.anchoredPosition = KocmocaData.invisible;
            }
#endif
            public void SetData (int index, Turret turret)
            {
                textCannonType.en = turret.cannonTypeEN;
                textCannonType.cn = turret.cannonTypeCN;
                textCannonType.textTitle.text = textCannonType.en;
                textCount.text = turret.cannonCount.ToString ();
                textAutoAim.text = turret.maxAutoAimAngle.ToString ();
                textRPM.text = turret.roundsPerMinute.ToString ();
                textSpread.text = turret.maxProjectileSpread.ToString ();
                textDPS.text = turret.dPS.ToString ();
                tipCannon.text = turret.develoment;

                textAmmoType.en = turret.ammoTypeEN;
                textAmmoType.cn = turret.ammoTypeCN;
                textAmmoType.textTitle.text = textCannonType.en;
                textVelocity.text = Mathf.RoundToInt (turret.ammoVelocity * 3.6f).ToString ();
                textRange.text = Mathf.RoundToInt (turret.operationalRange).ToString ();
                textDamage.text = turret.damage.ToString ();
                textPenetration.text = turret.penetration.ToString ();
                textPiercing.text = turret.piercing.ToString ();
            }
        }

        [System.Serializable]
        public class KocmomechPanel
        {
            [Header ("Panel")]
            public TextMeshProUGUI textType;
            public UIEventTriggerAnimation animLockTime;
            [Header ("Tips")]
            public TextMeshProUGUI tipType;
            public TextMeshProUGUI tipRegeneration;
            public TextMeshProUGUI tipRoll;
            public TextMeshProUGUI tipPitch;
            public TextMeshProUGUI tipYaw;
            public TextMeshProUGUI tipAcceleration;
            public TextMeshProUGUI tipDeceleration;
            public TextMeshProUGUI tipLockTime;
            public TextMeshProUGUI tipMissileCount;
            public TextMeshProUGUI tipMissileReloadTime;
            public TextMeshProUGUI tipRocketCount;
            public TextMeshProUGUI tipRocketReloadTime;
#if UNITY_EDITOR
            public void Initialize (RectTransform panel, GameObject tip)
            {
                Transform[] obj = panel.GetComponentsInChildren<Transform> ();
                foreach (Transform child in obj)
                {
                    switch (child.name)
                    {
                        case "Kocmomech - Type":
                            textType = child.GetComponent<TextMeshProUGUI> ();
                            break;
                        case "UI Event - Block - Lock Time":
                            animLockTime = child.GetComponent<UIEventTriggerAnimation> ();
                            break;
                    }
                }
                obj = tip.GetComponentsInChildren<Transform> ();
                foreach (Transform child in obj)
                {
                    switch (child.name)
                    {
                        case "Tip - Kocmomech":
                            tipType = child.GetComponent<TextMeshProUGUI> ();
                            break;
                        case "Kocmomech - Regeneration":
                            tipRegeneration = child.GetComponent<TextMeshProUGUI> ();
                            break;
                        case "Kocmomech - Roll":
                            tipRoll = child.GetComponent<TextMeshProUGUI> ();
                            break;
                        case "Kocmomech - Pitch":
                            tipPitch = child.GetComponent<TextMeshProUGUI> ();
                            break;
                        case "Kocmomech - Yaw":
                            tipYaw = child.GetComponent<TextMeshProUGUI> ();
                            break;
                        case "Kocmomech - Acceleration":
                            tipAcceleration = child.GetComponent<TextMeshProUGUI> ();
                            break;
                        case "Kocmomech - Deceleration":
                            tipDeceleration = child.GetComponent<TextMeshProUGUI> ();
                            break;
                        case "Kocmomech - Lock Time":
                            tipLockTime = child.GetComponent<TextMeshProUGUI> ();
                            break;
                            //case "Kocmomech - Missile Count": tipMissileCount = child.GetComponent<TextMeshProUGUI>(); break;
                            //case "Kocmomech - Missile Reload Time": tipMissileReloadTime = child.GetComponent<TextMeshProUGUI>(); break;
                            //case "Kocmomech - Rocket Count": tipRocketCount = child.GetComponent<TextMeshProUGUI>(); break;
                            //case "Kocmomech - Rocket Reload Time": tipRocketReloadTime = child.GetComponent<TextMeshProUGUI>(); break;
                    }
                }
                panel.anchoredPosition = KocmocaData.invisible;
            }
#endif
            public void SetData (int index, Kocmomech kocmomech)
            {
                textType.text = kocmomech.type;
                animLockTime.period = kocmomech.lockTime;
                tipType.text = kocmomech.type;
                tipRegeneration.text = "+ " + kocmomech.regeneration.ToString () + " Pts/s";
                tipRoll.text = "+ " + kocmomech.roll.ToString () + "°/s";
                tipPitch.text = "+ " + kocmomech.pitch.ToString () + "°/s";
                tipYaw.text = "+ " + kocmomech.yaw.ToString () + "°/s";
                tipAcceleration.text = "+ " + kocmomech.acceleration.ToString () + " Kph/s";
                tipDeceleration.text = "- " + kocmomech.deceleration.ToString () + " Kph/s";
                tipLockTime.text = "+ " + (kocmomech.lockTime * 1000).ToString () + " ms";
                //tipMissileCount.text = kocmomech.damage.ToString() + " Pts";
                //tipMissileReloadTime.text = kocmomech.dPS.ToString() + " Pts";
                //tipRocketCount.text = kocmomech.penetration.ToString() + " %";
                //tipRocketReloadTime.text = kocmomech.piercing.ToString() + " %";
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
        public class Radar : DataBlock
        {
            public TextMeshProUGUI textInfomation;
            public TextMeshProUGUI textMaxSearchRadius;
            public TextMeshProUGUI textMinSearchRadius;
            public TextMeshProUGUI textMaxSearchAngle;
            public TextMeshProUGUI textLockDistance;
            public TextMeshProUGUI textLockAngle;

            public void SetData (int index)
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