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
        [Header("Weapon Data")]
        public Text textTurretCount;
        public Text textFireRPS;
        public Text textDamage;
        public Text textMaxRange;

        void Awake()
        {
            hangarState = HangarState.Moving;
            hangarRailY.DOMove(hangarCenter[now].position, 0.73f).OnComplete(() =>
            {
                hangarState = HangarState.Ready;
                LoadHangarData();
            });
            //hangarRailY.DORotateQuaternion(hangarCenter[now].rotation, 0.73f);
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
            textMaxShield.text = "" + KocmocraftData.MaxShieldl[now];
            textMaxEnergy.text = "" + KocmocraftData.MaxEnergy[now];
            textCruiseSpeed.text = "" + KocmocraftData.CruiseSpeed[now] * 194.38445f + " knot";
            textAfterburneSpeed.text = "" + KocmocraftData.AfterburnerSpeed[now] * 194.38445f + " knot";
            //textTurretCount.text = "" + KocmocraftData.GetTurretCount((Type)now) + "x Assault Laser";
            //textFireRPS.text = "" + KocmoLaserCannon.fireRoundPerSecond + " rps";
            //textDamage.text = "" + KocmocraftData.GetPowerData((Type)now) + " dmg";
            //textMaxRange.text = "" + KocmocraftData.GetMaxRangeData((Type)now) + " m";
        }
    }
}
