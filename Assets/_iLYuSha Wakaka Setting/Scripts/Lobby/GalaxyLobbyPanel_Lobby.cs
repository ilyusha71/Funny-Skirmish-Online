using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

namespace Kocmoca
{
    public partial class GalaxyLobbyPanel
    {
        [Header("Lobby")]
        public Transform platformAxis;

        void Command()
        {
            if (Input.GetKeyDown(Controller.KEY_Hangar) ||
                Input.GetKeyDown(Controller.KEYBOARD_Hangar) ||
                Input.GetKeyDown(Controller.XBOX360_Hangar))
                OnHangarButtonClicked();
        }

        #region LOBBY CALLBACKS 

        public void OnHoverExhibition()
        {
            platformAxis.localRotation *= Quaternion.Euler(0,180,0);
        }

        public void OnExitExhibition()
        {
            platformAxis.localRotation = Quaternion.identity;
        }

        public void OnHangarButtonClicked()
        {
            State = LobbyState.Portal;
            Portal.Ending();
            Invoke("LoadHangarScene", 2.0f);
        }

        public void OnEscapeButtonClicked()
        {
            PhotonNetwork.Disconnect();
            Lobby.VisitLogin();
        }

        #endregion

        void LoadHangarScene()
        {
            SceneManager.LoadScene(LobbyInfomation.SCENE_HANGAR);
        }
    }
}