﻿using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Kocmoca
{
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
    public class HangarRanger : MonoBehaviour
    {
        public CanvasGroup hangarCanvas;
        public CanvasGroup mask;
        [Header("Hangar Rail & Camera")]
        public Transform hangarRailMain;
        public Transform hangarRailY;
        public Transform hangarRailX;
        public Transform hangarCamera;
        public Transform[] hangarCenter;
        private TweenerState hangarState = TweenerState.Ready;
        private int hangarIndex;
        [Header("Billboard")]
        public Transform billboard;
        private Vector3 billboardPos = new Vector3(9.289f,0,12.972f);
        private Vector3 billboardHide = new Vector3(0, -100, 0);
        [Header("Panel")]
        public Transform blockInfo;
        public Transform blockHangar;
        public Transform blockKocmocraft;
        public Transform blockData;
        public Button btnOpen;
        public Button btnHide;
        private TweenerState panelState = TweenerState.Hide;
        [Header("Info")]
        public Image imageFrame;
        public Image imageButton;
        public TextMeshProUGUI textInfo;
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
        //private SciFiBar barTurretCount;
        public TextMeshProUGUI textFireRPS;
        //private SciFiBar barFireRPS;
        public TextMeshProUGUI textDamage;
        private SciFiBar barDamage;
        public TextMeshProUGUI textMaxRange;
        private SciFiBar barMaxRange;

        void Awake()
        {
            mask.alpha = 1;
            hangarIndex = PlayerPrefs.GetInt(LobbyInfomation.PREFS_TYPE);
            billboard.localPosition = billboardHide;
            blockInfo.localScale = Vector3.zero;
            blockHangar.localPosition = new Vector3(blockHangar.localPosition.x, -128,0);
            blockKocmocraft.localPosition = new Vector3(blockKocmocraft.localPosition.x, -128, 0);
            blockData.localPosition = new Vector3(blockData.localPosition.x, -128, 0);
            btnOpen.onClick.AddListener(() => OpenPanel());
            btnHide.onClick.AddListener(() => HidePanel());
            barHull.Initialize(textMaxHull.transform.parent.GetComponentsInChildren<Image>(), 4000, 25000);
            barShield.Initialize(textMaxShield.transform.parent.GetComponentsInChildren<Image>(), 3000, 24000);
            barEnergy.Initialize(textMaxEnergy.transform.parent.GetComponentsInChildren<Image>(), 500, 3700);
            barCruise.Initialize(textCruiseSpeed.transform.parent.GetComponentsInChildren<Image>(), 20, 60);
            barAfterburne.Initialize(textAfterburneSpeed.transform.parent.GetComponentsInChildren<Image>(), 70, 133);
            barDamage.Initialize(textDamage.transform.parent.GetComponentsInChildren<Image>(), 1092, 2842);
            barMaxRange.Initialize(textMaxRange.transform.parent.GetComponentsInChildren<Image>(), 955, 1270);
        }

        private void Start()
        {
            mask.DOFade(0, 3.37f);
            hangarState = TweenerState.Moving;
            hangarRailY.DOLocalRotate(hangarIndex < 12 ? new Vector3(0, -117, 0) : new Vector3(0, 117, 0), 3.37f);
            hangarRailX.DOLocalRotate(new Vector3(71, 0, 0), 3.37f);
            hangarCamera.DOLocalMove(new Vector3(0, 0, -13.7f), 3.37f);
            hangarRailMain.DOMove(hangarCenter[hangarIndex].position, 3.37f).OnComplete(() =>
            {
                hangarState = TweenerState.Ready;
                billboard.localPosition = hangarIndex < 12 ? billboardPos : -billboardPos;
                LoadHangarData();
            });
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                SceneManager.LoadScene("Galaxy Lobby");

            if (Input.GetKeyDown(KeyCode.D))
            {
                hangarIndex = (int)Mathf.Repeat(++hangarIndex, hangarCenter.Length);
                MoveHangarRail();
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                hangarIndex = (int)Mathf.Repeat(--hangarIndex, hangarCenter.Length);
                MoveHangarRail();
            }

            billboard.LookAt(hangarCamera);
            billboard.eulerAngles = new Vector3(0, billboard.eulerAngles.y, 0);

            if (hangarState == TweenerState.Ready)
            {
                hangarCanvas.alpha = 1.0f;
                if (Input.GetKeyDown(KeyCode.J))
                    OpenPanel();
                if (Input.GetKeyDown(KeyCode.K))
                    HidePanel();
                if (Input.GetKey(KeyCode.Mouse1))
                {
                    hangarCanvas.alpha = 0;
                    hangarRailY.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * 2, 0);
                    hangarRailX.rotation *= Quaternion.Euler(-Input.GetAxis("Mouse Y") * 2, 0, 0);
                    hangarRailX.eulerAngles = new Vector3(Mathf.Clamp(hangarRailX.rotation.eulerAngles.x, 60, 120), hangarRailX.rotation.eulerAngles.y, hangarRailX.rotation.eulerAngles.z);
                }
                hangarCamera.localPosition = Vector3.Lerp(hangarCamera.localPosition, hangarCamera.localPosition + new Vector3(0, 0, 10 * Input.GetAxis("Mouse ScrollWheel")), 0.5f);
                hangarCamera.localPosition = new Vector3(hangarCamera.localPosition.x, hangarCamera.localPosition.y, Mathf.Clamp(hangarCamera.localPosition.z, -20, -5));
            }
        }

        void MoveHangarRail()
        {
            billboard.localPosition = billboardHide;
            hangarCanvas.alpha = 0.0f;
            hangarRailMain.DOKill();
            hangarState = TweenerState.Moving;
            hangarRailMain.DOMove(hangarCenter[hangarIndex].position, 0.73f).OnComplete(() =>
            {
                hangarState = TweenerState.Ready;
                if (hangarIndex < 20)
                    PlayerPrefs.SetInt(LobbyInfomation.PREFS_TYPE, hangarIndex);
                billboard.localPosition = hangarIndex < 12 ? billboardPos : -billboardPos;
                LoadHangarData();
            });
        }

        void LoadHangarData()
        {
            imageFrame.color = HangarData.FrameColor[hangarIndex];
            imageButton.color = HangarData.ButtonColor[hangarIndex];
            textInfo.color = HangarData.TextColor[hangarIndex];
            textInfo.text = HangarData.Info[hangarIndex];

            textOKB.text = "" + HangarData.OKB[hangarIndex];
            textKocmocraft.text = "" + HangarData.Kocmocraft[hangarIndex];
            textCode.text = "" + HangarData.Code[hangarIndex];
            textDubi.text = "" + HangarData.Dubi[hangarIndex];
            textEngine.text = "" + HangarData.Engine[hangarIndex];

            textMaxHull.text = "" + KocmocraftData.MaxHull[hangarIndex];
            barHull.SetBar(KocmocraftData.MaxHull[hangarIndex]);
            textMaxShield.text = "" + KocmocraftData.MaxShieldl[hangarIndex];
            barShield.SetBar(KocmocraftData.MaxShieldl[hangarIndex]);
            textMaxEnergy.text = "" + KocmocraftData.MaxEnergy[hangarIndex];
            barEnergy.SetBar(KocmocraftData.MaxEnergy[hangarIndex]);
            textCruiseSpeed.text = (KocmocraftData.CruiseSpeed[hangarIndex] * 1.9438445f).ToString("0.00") + " knot";
            barCruise.SetBar(KocmocraftData.CruiseSpeed[hangarIndex]);
            textAfterburneSpeed.text = (KocmocraftData.AfterburnerSpeed[hangarIndex] * 1.9438445f).ToString("0.00") + " knot";
            barAfterburne.SetBar(KocmocraftData.AfterburnerSpeed[hangarIndex]);

            if (hangarIndex < 20)
            {
                WeaponData.GetWeaponData(hangarIndex);
                textTurretCount.text = WeaponData.TurretCount[hangarIndex] + "x 突击激光炮";
                textFireRPS.text = KocmoLaserCannon.fireRoundPerSecond + " rps";
                textDamage.text = string.Format("{0} ~ {1} dmg", WeaponData.MinDamage, WeaponData.MaxDamage);
                barDamage.SetBar(WeaponData.MaxDamage);
                textMaxRange.text = WeaponData.MaxRange + " m";
                barMaxRange.SetBar(WeaponData.MaxRange);
            }
            else
            {
                textTurretCount.text = "---";
                textFireRPS.text = "---";
                textDamage.text = "---";
                barDamage.SetBar(0);
                textMaxRange.text = "--- m";
                barMaxRange.SetBar(0);
            }

        }

        public void OpenPanel()
        {
            if (panelState != TweenerState.Hide) return;
            blockInfo.DOKill();
            blockHangar.DOKill();
            blockKocmocraft.DOKill();
            blockData.DOKill();
            panelState = TweenerState.Moving;

            blockInfo.DOScale(Vector3.one,0.37f).OnComplete(() => 
            {
                StartCoroutine(Animation());
            });
        }

        public void HidePanel()
        {
            if (panelState != TweenerState.Open) return;
            blockInfo.DOKill();
            blockHangar.DOKill();
            blockKocmocraft.DOKill();
            blockData.DOKill();
            panelState = TweenerState.Moving;

            blockInfo.DOScale(Vector3.zero, 0.37f);
            blockHangar.DOLocalMoveY(-128, 0.37f);
            blockKocmocraft.DOLocalMoveY(-128, 0.37f);
            blockData.DOLocalMoveY(-128, 0.37f).OnComplete(() => { panelState = TweenerState.Hide; });
        }
        readonly WaitForSeconds delay = new WaitForSeconds(0.137f);
        IEnumerator Animation()
        {
            blockKocmocraft.DOLocalMoveY(128, 1.0f).SetEase(Ease.OutElastic);
            yield return delay;
            blockHangar.DOLocalMoveY(128, 1.0f).SetEase(Ease.OutElastic);
            yield return delay;
            blockData.DOLocalMoveY(128, 1.0f).SetEase(Ease.OutElastic).OnComplete(() => { panelState = TweenerState.Open; });
        }
    }
}