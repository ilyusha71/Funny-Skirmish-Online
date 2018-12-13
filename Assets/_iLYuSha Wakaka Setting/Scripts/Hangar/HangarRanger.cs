using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

namespace Kocmoca
{
    public enum HangarState
    {
        Ready,
        Moving,
        Login,
        Lobby,
        Hangar,
        Show,
        newEvent,
    }
    public struct SciFiBar
    {
        public Image[] bar;
        private int Denominator;
        private int Minimum;

        public void Initialize(Image[] shell, int min,int max)
        {
            bar =new Image[7];
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
        }
    }
    public class HangarRanger : MonoBehaviour
    {
        [Header("Hangar Camera")]
        public Transform hangarRailY;
        public Transform hangarRailX;
        public Transform hangarCamera;
        public Transform[] hangarCenter;
        private int now = 0;
        public HangarState hangarState = HangarState.Ready;

        public Text textHangarInfo;
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
        private SciFiBar barTurretCount;
        public TextMeshProUGUI textFireRPS;
        private SciFiBar barFireRPS;
        public TextMeshProUGUI textDamage;
        private SciFiBar barDamage;
        public TextMeshProUGUI textMaxRange;
        private SciFiBar barMaxRange;

        void Awake()
        {
            hangarState = HangarState.Moving;
            hangarRailY.DOMove(hangarCenter[now].position, 0.73f).OnComplete(() =>
            {
                hangarState = HangarState.Ready;
                LoadHangarData();
            });
            //hangarRailY.DORotateQuaternion(hangarCenter[now].rotation, 0.73f);
            barHull.Initialize(textMaxHull.transform.parent.GetComponentsInChildren<Image>(), 3000, 30000);
            barShield.Initialize(textMaxShield.transform.parent.GetComponentsInChildren<Image>(), 3000, 20000);
            barEnergy.Initialize(textMaxEnergy.transform.parent.GetComponentsInChildren<Image>(), 1000, 3000);
            barCruise.Initialize(textCruiseSpeed.transform.parent.GetComponentsInChildren<Image>(), 20, 50);
            barAfterburne.Initialize(textAfterburneSpeed.transform.parent.GetComponentsInChildren<Image>(), 70, 110);

            barDamage.Initialize(textDamage.transform.parent.GetComponentsInChildren<Image>(), 1200, 1600);

        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.D))
                NextHanger();
            else if (Input.GetKeyDown(KeyCode.A))
                PreviousHanger();

            if (hangarState == HangarState.Ready)
            {
                if (Input.GetKey(KeyCode.Mouse1))
                {
                    hangarRailY.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * 2, 0);
                    hangarRailX.rotation *= Quaternion.Euler(-Input.GetAxis("Mouse Y") * 2, 0, 0);
                    hangarRailX.eulerAngles = new Vector3(Mathf.Clamp(hangarRailX.rotation.eulerAngles.x, 60, 120), hangarRailX.rotation.eulerAngles.y, hangarRailX.rotation.eulerAngles.z);
                }
                hangarCamera.localPosition = Vector3.Lerp(hangarCamera.localPosition, hangarCamera.localPosition + new Vector3(0, 0, 10 * Input.GetAxis("Mouse ScrollWheel")), 0.5f);
                hangarCamera.localPosition = new Vector3(hangarCamera.localPosition.x, hangarCamera.localPosition.y, Mathf.Clamp(hangarCamera.localPosition.z, -20, -5));
            }
        }

        void NextHanger()
        {
            now = (int)Mathf.Repeat(++now, hangarCenter.Length);
            hangarRailY.DOKill();
            hangarState = HangarState.Moving;
            hangarRailY.DOMove(hangarCenter[now].position, 0.73f).OnComplete(() =>
            {
                hangarState = HangarState.Ready;
                LoadHangarData();
            });
            //hangarRailY.DORotateQuaternion(hangarCenter[now].rotation, 0.73f);
        }

        void PreviousHanger()
        {
            now = (int)Mathf.Repeat(--now, hangarCenter.Length);
            hangarRailY.DOKill();
            hangarState = HangarState.Moving;
            hangarRailY.DOMove(hangarCenter[now].position, 0.73f).OnComplete(() =>
            {
                hangarState = HangarState.Ready;
                LoadHangarData();
            });
            //hangarRailY.DORotateQuaternion(hangarCenter[now].rotation, 0.73f);
        }

        void LoadHangarData()
        {
            //// General
            //textHangarInfo.text = HangarData.OKB[now] + "\n" + HangarData.Kocmocraft[now] + "\n" + HangarData.Code[now] + "\n" + HangarData.Dubi[now] + "\n拦截";
            //// 
            //textKocmocraft.text =
            //    KocmocraftData.MaxHull[now] + "\n" + 
            //    KocmocraftData.MaxShieldl[now] + "\n" + 
            //    KocmocraftData.MaxEnergy[now] + "\n" + 
            //    KocmocraftData.CruiseSpeed[now] * 3.6f + " km/h\n" + 
            //    KocmocraftData.AfterburnerSpeed[now] * 3.6f + " km/h";

            textOKB.text = "" + HangarData.OKB[now];
            textKocmocraft.text = "" + HangarData.Kocmocraft[now];
            textCode.text = "" + HangarData.Code[now];
            textDubi.text = "" + HangarData.Dubi[now];
            textEngine.text = "" + HangarData.Engine[now];

            textMaxHull.text = "" + KocmocraftData.MaxHull[now];
            barHull.SetBar(KocmocraftData.MaxHull[now]);
            textMaxShield.text = "" + KocmocraftData.MaxShieldl[now];
            barShield.SetBar(KocmocraftData.MaxShieldl[now]);
            textMaxEnergy.text = "" + KocmocraftData.MaxEnergy[now];
            barEnergy.SetBar(KocmocraftData.MaxEnergy[now]);
            textCruiseSpeed.text = "" + (KocmocraftData.CruiseSpeed[now] * 1.9438445f).ToString(".00") + " knot";
            barCruise.SetBar(KocmocraftData.CruiseSpeed[now]);
            textAfterburneSpeed.text = "" + (KocmocraftData.AfterburnerSpeed[now] * 1.9438445f).ToString(".00") + " knot";
            barAfterburne.SetBar(KocmocraftData.AfterburnerSpeed[now]);


            textTurretCount.text = "" + KocmocraftData.GetTurretCount((Type)now) + "x 突击激光炮";
            textFireRPS.text = "" + KocmoLaserCannon.fireRoundPerSecond + " rps";
            textDamage.text = "" + KocmocraftData.GetPowerData((Type)now) + " dmg";
            barDamage.SetBar(KocmocraftData.GetMaxDamage((Type)now));
            textMaxRange.text = "" + KocmocraftData.GetMaxRangeData((Type)now) + " m";
        }
    }
}
