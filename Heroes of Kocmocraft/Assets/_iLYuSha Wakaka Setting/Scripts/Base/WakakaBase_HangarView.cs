using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

namespace Kocmoca
{
    public partial class WakakaBase : MonoBehaviour
    {
        public HangarState hangarState = HangarState.Portal;
        public Transform axisX;
        [Header ("Hangar")]

        //public Transform hangar;

        //private Transform[] hangarApron;
        public float radius;
        //private int hangarCount; // /*改名*/
        private int hangarIndex;

        void SetHangarData ()
        {

        }

        void ViewSetting ()
        {
            for (int i = 0; i < hangarCount; i++)
            {
                //kocmocraftSkin[i].LoadSkin(PlayerPrefs.GetInt(LobbyInfomation.PREFS_SKIN + i));
            }
        }

        // Start is called before the first frame update
        void Start ()
        {
            MoveHangarRail ();

        }

        public void Test01 ()
        {
            for (int i = 0; i < 8; i++)
            {
                if (Input.GetKeyDown ((KeyCode) (i + 282)))
                {
                    togTabs[i].isOn = !togTabs[i].isOn;
                }
            }
        }
        void Update ()
        {
            // Toggle Tab Hotkey
            for (int i = 0; i < 8; i++)
            {
                if (Input.GetKeyDown ((KeyCode) (i + 282)))
                {
                    togTabs[i].isOn = !togTabs[i].isOn;
                }
            }

            if (Input.GetKeyDown (KeyCode.C))
                SwitchCockpit ();

            if (Input.GetKeyDown (KeyCode.Escape))
            {
                hangarState = HangarState.Portal;
                //hangarCanvas.alpha = 0;
                PlayerPrefs.SetString (LobbyInfomation.PREFS_LOAD_SCENE, LobbyInfomation.SCENE_LOBBY);
                //Portal.Ending();
                Invoke ("Loading", 2.0f);
            }

            if (hangarState == HangarState.Portal) return;

            if (Input.GetKeyDown (Controller.KEY_NextHangar))
            {
                hangarIndex = (int) Mathf.Repeat (++hangarIndex, hangarCount);
                MoveHangarRail ();
            }
            else if (Input.GetKeyDown (Controller.KEY_PreviousHangar))
            {
                hangarIndex = (int) Mathf.Repeat (--hangarIndex, hangarCount);
                MoveHangarRail ();
            }

            //billboard.LookAt(slider);
            //billboard.eulerAngles = new Vector3(0, billboard.eulerAngles.y, 0);

            if (hangarState == HangarState.Ready)
            {
                panel.localPosition = Vector3.zero;
                if (Input.GetKeyDown (Controller.KEYBOARD_Panel) || Input.GetKeyDown (Controller.XBOX360_Panel))
                {
                    if (panelState == TweenerState.Hide)
                        OpenPanel ();
                    else if (panelState == TweenerState.Open)
                        HidePanel ();
                    else
                        return;
                }

                if (Input.GetKey (KeyCode.Mouse1))
                {
                    panel.localPosition = KocmocaData.invisible;
                    cmFreeLook[hangarIndex].m_XAxis.m_InputAxisValue = Input.GetAxis ("Mouse X");
                    cmFreeLook[hangarIndex].m_YAxis.m_InputAxisValue = Input.GetAxis ("Mouse Y");
                }
                else
                {
                    cmFreeLook[hangarIndex].m_XAxis.m_InputAxisValue = 0;
                    cmFreeLook[hangarIndex].m_YAxis.m_InputAxisValue = 0;
                }
                if (Input.GetAxis ("Mouse ScrollWheel") != 0)
                {
                    radius = Mathf.Clamp (radius -= Input.GetAxis ("Mouse ScrollWheel") * 37, database.kocmocraft[hangarIndex].design.view.near, 18.2f);
                }
                for (int i = 0; i < 2; i++)
                {
                    cmFreeLook[hangarIndex].m_Orbits[i].m_Radius = Mathf.Lerp (cmFreeLook[hangarIndex].m_Orbits[i].m_Radius, radius, 0.073f);
                }
            }
        }
        bool isCockpitView = false;
        public void SwitchCockpit ()
        {
            isCockpitView = !isCockpitView;
            for (int i = 0; i < hangarCount; i++)
            {
                cmFreeLook[i].enabled = false;
                cmCockpit[i].enabled = false;
            }
            if (isCockpitView)
            {
                // hangar[hangarIndex].GetComponentInChildren<PilotManager> ().HidePilot();
                cmCockpit[hangarIndex].enabled = true;
            }
            else
                cmFreeLook[hangarIndex].enabled = true;
        }
        void MoveHangarRail ()
        {
            apronView.SetPositionAndRotation (apron[hangarIndex].position, apron[hangarIndex].rotation);
            hangarView.SetPositionAndRotation (hangar[hangarIndex].position, hangar[hangarIndex].rotation);

            radius = cmFreeLook[hangarIndex].m_Orbits[0].m_Radius;
            for (int i = 0; i < hangarCount; i++)
            {
                cmFreeLook[i].enabled = false;
                cmCockpit[i].enabled = false;
            }
            if (!isCockpitView)
                cmFreeLook[hangarIndex].enabled = true;
            else
                cmCockpit[hangarIndex].enabled = true;

            //if (hangarIndex < hangarCount)
            //    kocmocraftSkin[hangarIndex].LoadSkin(PlayerPrefs.GetInt(LobbyInfomation.PREFS_SKIN + hangarIndex));
            //billboard.localPosition = billboardHide;
            panel.localPosition = KocmocaData.invisible;
            axisX.DOKill ();
            hangarState = HangarState.Moving;
            axisX.DORotateQuaternion (hangar[hangarIndex].rotation, 0.73f);
            axisX.DOMove (hangar[hangarIndex].position, 0.73f).OnComplete (() =>
            {
                hangarState = HangarState.Ready;
                if (hangarIndex < hangarCount)
                    PlayerPrefs.SetInt (LobbyInfomation.PREFS_TYPE, hangarIndex);
                //billboard.localPosition = billboardPos;
                LoadHangarData ();
            });
        }

    }
}