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

            if (Input.GetKeyDown(KeyCode.Backspace))
                Application.Quit();
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
            lobbyState = LobbyState.Portal;
            Portal.Ending();
            PlayerPrefs.SetString(LobbyInfomation.PREFS_LOAD_SCENE, LobbyInfomation.SCENE_HANGAR);
            Invoke("Loading", 2.0f);
        }

        public void OnEscapeButtonClicked()
        {
            PhotonNetwork.Disconnect();
            Lobby.VisitLogin();
        }

        #endregion

        void Loading()
        {
            SceneManager.LoadScene(LobbyInfomation.SCENE_LOADING);
        }
    }
}