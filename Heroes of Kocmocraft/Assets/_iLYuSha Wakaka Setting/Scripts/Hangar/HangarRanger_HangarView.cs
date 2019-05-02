using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

namespace Kocmoca
{
    public partial class HangarRanger : CameraTrackingSystem
    {


        [Header("Hangar")]
        public Transform viewCamera;
        public Camera cameraTop;
        public Camera cameraSide;
        public Camera cameraFront;
        public Transform hangar;
        private BoxCollider[] kocmocraftSize;
        private CinemachineFreeLook[] kocmocraftCamera;
        private SkinManager[] kocmocraftSkin;
        private Transform[] hangarApron;
        private ViewData[] viewData;
        private float radius;
        private int hangarCount; // 改名
        private int hangarIndex;
        private readonly int hangarMaxCount = 24;
        private readonly int hangarHalfCount = 12;




        void ViewSetting()
        {
            kocmocraftSize = hangar.GetComponentsInChildren<BoxCollider>();
            kocmocraftCamera = hangar.GetComponentsInChildren<CinemachineFreeLook>();
            kocmocraftSkin = hangar.GetComponentsInChildren<SkinManager>();

            hangarCount = kocmocraftSize.Length;
            hangarApron = new Transform[hangarCount];
            viewData = new ViewData[hangarCount];

            for (int i = 0; i < hangarCount; i++)
            {
                kocmocraftSkin[i].LoadSkin(PlayerPrefs.GetInt(LobbyInfomation.PREFS_SKIN + i));
                for (int k = 0; k < 3; k++)
                {
                    kocmocraftCamera[i].m_Orbits[k].m_Radius = 11;
                }
                hangarApron[i] = kocmocraftSize[i].transform;
                viewData[i] = kocmocraftSkin[i].viewData;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            MoveHangarRail();

        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                hangarState = HangarState.Portal;
                //hangarCanvas.alpha = 0;
                PlayerPrefs.SetString(LobbyInfomation.PREFS_LOAD_SCENE, LobbyInfomation.SCENE_LOBBY);
                Portal.Ending();
                Invoke("Loading", 2.0f);
            }

            if (hangarState == HangarState.Portal) return;

            if (Input.GetKeyDown(Controller.KEY_NextHangar))
            {
                hangarIndex = (int)Mathf.Repeat(++hangarIndex, hangarCount);
                MoveHangarRail();
            }
            else if (Input.GetKeyDown(Controller.KEY_PreviousHangar))
            {
                hangarIndex = (int)Mathf.Repeat(--hangarIndex, hangarCount);
                MoveHangarRail();
            }

            billboard.LookAt(slider);
            billboard.eulerAngles = new Vector3(0, billboard.eulerAngles.y, 0);

            if (hangarState == HangarState.Ready)
            {
                panel.localPosition = new Vector3(0, -120, 0);
                if (Input.GetKeyDown(Controller.KEYBOARD_Panel) || Input.GetKeyDown(Controller.XBOX360_Panel))
                {
                    if (panelState == TweenerState.Hide)
                        OpenPanel();
                    else if (panelState == TweenerState.Open)
                        HidePanel();
                    else
                        return;
                }
                if (Input.GetKey(KeyCode.Mouse1))
                    panel.localPosition = KocmocaData.invisible;

                if (Input.GetKey(KeyCode.Mouse1))
                {
                    kocmocraftCamera[hangarIndex].m_XAxis.m_InputAxisValue = Input.GetAxis("Mouse X");
                    kocmocraftCamera[hangarIndex].m_YAxis.m_InputAxisValue = Input.GetAxis("Mouse Y");
                }
                else
                {
                    kocmocraftCamera[hangarIndex].m_XAxis.m_InputAxisValue = 0;
                    kocmocraftCamera[hangarIndex].m_YAxis.m_InputAxisValue = 0;
                }
                if (Input.GetAxis("Mouse ScrollWheel") != 0)
                {
                   radius =  Mathf.Clamp(radius -= Input.GetAxis("Mouse ScrollWheel") * 20, viewData[hangarIndex].NearView, 17.3f);
                }
                for (int i = 0; i < 3; i++)
                {
                    kocmocraftCamera[hangarIndex].m_Orbits[i].m_Radius = Mathf.Lerp(kocmocraftCamera[hangarIndex].m_Orbits[i].m_Radius,radius, 0.37f);
                }
            }
        }

        void MoveHangarRail()
        {
            viewCamera.SetPositionAndRotation(hangarApron[hangarIndex].position, hangarApron[hangarIndex].rotation);
            radius = kocmocraftCamera[hangarIndex].m_Orbits[0].m_Radius;
            for (int i = 0; i < hangarCount; i++)
            {
                kocmocraftCamera[i].enabled = false;
            }
            kocmocraftCamera[hangarIndex].enabled = true;

            if (hangarIndex < hangarCount)
                kocmocraftSkin[hangarIndex].LoadSkin(PlayerPrefs.GetInt(LobbyInfomation.PREFS_SKIN + hangarIndex));
            billboard.localPosition = billboardHide;
            panel.localPosition = KocmocaData.invisible;
            axisX.DOKill();
            hangarState = HangarState.Moving;
            axisX.DORotateQuaternion(kocmocraftSize[hangarIndex].transform.rotation, 0.73f);
            axisX.DOMove(kocmocraftSize[hangarIndex].transform.position, 0.73f).OnComplete(() =>
            {
                hangarState = HangarState.Ready;
                if (hangarIndex < hangarCount)
                    PlayerPrefs.SetInt(LobbyInfomation.PREFS_TYPE, hangarIndex);
                billboard.localPosition = billboardPos;
                LoadHangarData();
            });
        }

    }
}
