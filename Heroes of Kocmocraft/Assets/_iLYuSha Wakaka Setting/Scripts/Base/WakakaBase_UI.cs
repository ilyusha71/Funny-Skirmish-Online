﻿using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
        private AudioSource panelAudio;
        private TweenerState panelState = TweenerState.Hide;
        [Header("UI - Tab")]
        public AudioClip sfxTabDown;
        public Toggle togDesign;
        public Toggle togDubi;
        public Toggle togPerformance;
        public Toggle togAstromech;
        public Toggle togRadar;
        public Toggle togTurret;
        public Toggle togMissile;
        public Image[] imgTab;
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
        public class DesignBlock: PanelBlock
        {
            public TextMeshProUGUI textSeason;
            public TextMeshProUGUI textOKB;
            public TextMeshProUGUI textCode;
            public TextMeshProUGUI textDescription;

            public void SetData(int index)
            {
                textTitle.text = DesignData.Kocmocraft[index];
                textSeason.text = DesignData.Season[index];
                textOKB.text = DesignData.OKB[index];
                textCode.text = DesignData.Code[index];
                textDescription.text = DesignData.Design[index];

                textSeason.color = HangarData.TextColor[index];
                textOKB.color = HangarData.TextColor[index];
                textCode.color = HangarData.TextColor[index];
                textDescription.color = HangarData.TextColor[index];

                for (int i = 0; i < item.Length; i++)
                {
                    item[i].color = HangarData.TextColor[index];
                }
            }
        }
        [Header("UI - Hangar Panel - Design")]
        public DesignBlock design;
        public AudioClip sfxChangeSkin;
        public AudioClip sfxSwitchWireframe;
        public Button btnChangeSkin; // Hotkey: C
        public Button btnSwitchWireframe; // Hotkey: W
        public RectTransform scaleWingspan;
        public RectTransform scaleLength;
        public RectTransform scaleHeight;
        public TextMeshProUGUI textWingspan;
        public TextMeshProUGUI textLength;
        public TextMeshProUGUI textHeight;
        [System.Serializable]
        public class DubiBlock : PanelBlock
        {
            public TextMeshProUGUI textChiefName;
            public TextMeshProUGUI textChiefResume;
            public RectTransform scaleChiefHeight;
            public TextMeshProUGUI textChiefHeight;
            public TextMeshProUGUI textSecondName;
            public TextMeshProUGUI textSecondResume;
            public RectTransform scaleSecondHeight;
            public TextMeshProUGUI textSecondHeight;

            public void SetData(int index)
            {
                textTitle.text = DubiData.Dubi[index];

                textChiefName.text = DubiData.ChiefPilot[index];
                textChiefResume.text = DubiData.ChiefResume[index];
                scaleChiefHeight.sizeDelta = new Vector2(274 * DubiData.ChiefHeight[index] / 200, 37);
                textChiefHeight.text = DubiData.ChiefHeight[index].ToString() + " cm";

                textSecondName.text = DubiData.SecondPilot[index];
                textSecondResume.text = DubiData.SecondResume[index];
                scaleSecondHeight.sizeDelta = new Vector2(274 * DubiData.SecondHeight[index] / 200, 37);
                textSecondHeight.text = DubiData.SecondHeight[index].ToString() + " cm";

                textChiefName.color = HangarData.TextColor[index];
                textChiefResume.color = HangarData.TextColor[index];
                textSecondName.color = HangarData.TextColor[index];
                textSecondResume.color = HangarData.TextColor[index];

                for (int i = 0; i < item.Length; i++)
                {
                    item[i].color = HangarData.TextColor[index];
                }
            }
        }
        [Header("UI - Hangar Panel - Dubi")]
        public DubiBlock dubi;
        public AudioClip sfxChangePilot; // Hotkey: K
        public AudioClip[] sfxTalk; // Hotkey: T
        public Button btnChangePilot;
        public Button btnTalk;
        private AudioSource speech;
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
            btnOpenPanel.onClick.AddListener(() => OpenPanel());
            panel.localScale = Vector3.zero;
            btnHidePanel.onClick.AddListener(() => HidePanel());
            imgPanel = panel.GetComponent<Image>();
            imgHideButton = btnHidePanel.image;
            panelAudio = panel.GetComponent<AudioSource>();


            // Tab
            togDesign.onValueChanged.AddListener(isOn =>
            {
                panelAudio.PlayOneShot(sfxTabDown, 0.73f);
                blockDesign.SetActive(isOn);
                if (isOn) OpenPanel(); else HidePanel();
            });
            togDubi.onValueChanged.AddListener(isOn =>
            {
                panelAudio.PlayOneShot(sfxTabDown, 0.73f);
                blockDubi.SetActive(isOn);
                if (isOn) OpenPanel(); else HidePanel();

            });
            togPerformance.onValueChanged.AddListener(isOn =>
            {
                panelAudio.PlayOneShot(sfxTabDown, 0.73f);
                blockPerformance.SetActive(isOn);
                if (isOn) OpenPanel(); else HidePanel();

            });
            togAstromech.onValueChanged.AddListener(isOn =>
            {
                panelAudio.PlayOneShot(sfxTabDown, 0.73f);
                blockAstromech.SetActive(isOn);
                if (isOn) OpenPanel(); else HidePanel();
            });
            togRadar.onValueChanged.AddListener(isOn =>
            {
                panelAudio.PlayOneShot(sfxTabDown, 0.73f);
                blockRadar.SetActive(isOn);
                if (isOn) OpenPanel(); else HidePanel();
            });
            togTurret.onValueChanged.AddListener(isOn =>
            {
                panelAudio.PlayOneShot(sfxTabDown, 0.73f);
                blockTurret.SetActive(isOn);
                if (isOn) OpenPanel(); else HidePanel();
            });
            togMissile.onValueChanged.AddListener(isOn =>
            {
                panelAudio.PlayOneShot(sfxTabDown, 0.73f);
                blockMissile.SetActive(isOn);
                if (isOn) OpenPanel(); else HidePanel();
            });

            btnChangeSkin.onClick.AddListener(() =>
            {
                panelAudio.PlayOneShot(sfxChangeSkin);
                PlayerPrefs.SetInt(LobbyInfomation.PREFS_SKIN + hangarIndex, prototype[hangarIndex].ChangeSkin());
            });
            btnSwitchWireframe.onClick.AddListener(() =>
            {
                panelAudio.PlayOneShot(sfxSwitchWireframe);
                prototype[hangarIndex].SwitchWireframe();
            });
            btnChangePilot.onClick.AddListener(() =>
            {
                panelAudio.PlayOneShot(sfxChangePilot);
                pilot[hangarIndex].ChangeSkin();
            });
            speech = btnTalk.GetComponent<AudioSource>();
            btnTalk.onClick.AddListener(() => 
            {
                if (!speech.isPlaying && hangarIndex < hangarCount)
                    speech.PlayOneShot(sfxTalk[hangarIndex], 1.5f);
            });
        }

        public void OpenPanel()
        {
            panel.DOKill();
            panel.DOScale(Vector3.one, 0.37f);
            //panelAudio.PlayOneShot(sfxOpenPanel);
            panelState = TweenerState.Open;
            //panelState = TweenerState.Moving;
            //.OnComplete(() => { panelState = TweenerState.Open; });
        }

        public void HidePanel()
        {
            panel.DOKill();
            panel.DOScale(Vector3.zero, 0.37f);
            //panelAudio.PlayOneShot(sfxHidePanel);
            panelState = TweenerState.Hide;
            //panelState = TweenerState.Moving;

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
            design.SetData(hangarIndex);
            dubi.SetData(hangarIndex);
            astromechDroid.SetData(hangarIndex);
            radar.SetData(hangarIndex);
            turret.SetData(hangarIndex);




            // Panel
            imgPanel.color = HangarData.FrameColor[hangarIndex];
            imgHideButton.color = HangarData.ButtonColor[hangarIndex];

            // Tab
            for (int i = 0; i < imgTab.Length; i++)
            {
                imgTab[i].color = HangarData.TabColor[hangarIndex];
            }




            //textEngine.text = "" + DesignData.Engine[hangarIndex];

            //textMaxHull.text = "" + KocmocraftData.Hull[hangarIndex];
            //barHull.SetBar(KocmocraftData.Hull[hangarIndex]);
            //textMaxShield.text = "" + KocmocraftData.Shield[hangarIndex];
            //barShield.SetBar(KocmocraftData.Shield[hangarIndex]);
            //textMaxEnergy.text = "" + KocmocraftData.Energy[hangarIndex];
            //barEnergy.SetBar(KocmocraftData.Energy[hangarIndex]);
            //textCruiseSpeed.text = (KocmocraftData.CruiseSpeed[hangarIndex] * 1.9438445f).ToString("0.00") + " knot";
            //barCruise.SetBar(KocmocraftData.CruiseSpeed[hangarIndex]);
            //textAfterburneSpeed.text = (KocmocraftData.AfterburnerSpeed[hangarIndex] * 1.9438445f).ToString("0.00") + " knot";
            //barAfterburne.SetBar(KocmocraftData.AfterburnerSpeed[hangarIndex]);

            if (hangarIndex < hangarCount)
            {
                //WeaponData.GetWeaponData(hangarIndex);
                //textTurretCount.text = WeaponData.TurretCount[hangarIndex] + "x 突击激光炮";
                //textFireRPS.text = KocmoLaserCannon.FireRoundPerSecond + " rps";
                //textDamage.text = string.Format("{0} ~ {1} dmg", WeaponData.MinDamage, WeaponData.MaxDamage);
                //barDamage.SetBar(WeaponData.MaxDamage);
                //textMaxRange.text = WeaponData.MaxRange + " m";
                //barMaxRange.SetBar(WeaponData.MaxRange);
            }
            else
            {
                //textTurretCount.text = "---";
                //textFireRPS.text = "---";
                //textDamage.text = "---";
                //barDamage.SetBar(0);
                //textMaxRange.text = "--- m";
                //barMaxRange.SetBar(0);
            }

            // 三视图
            topCamera.orthographicSize = prototype[hangarIndex].orthoSize;
            sideCamera.orthographicSize = prototype[hangarIndex].orthoSize;
            frontCamera.orthographicSize = prototype[hangarIndex].orthoSize;
            scaleWingspan.sizeDelta = new Vector2(274 * prototype[hangarIndex].wingspanScale, 37);
            scaleLength.sizeDelta = new Vector2(274 * prototype[hangarIndex].lengthScale, 37);
            scaleHeight.sizeDelta = new Vector2(274 * prototype[hangarIndex].heightScale, 37);
            textWingspan.text = prototype[hangarIndex].wingspan.ToString() + " m";
            textLength.text = prototype[hangarIndex].length.ToString() + " m";
            textHeight.text = prototype[hangarIndex].height.ToString() + " m";
        }

    }
}
