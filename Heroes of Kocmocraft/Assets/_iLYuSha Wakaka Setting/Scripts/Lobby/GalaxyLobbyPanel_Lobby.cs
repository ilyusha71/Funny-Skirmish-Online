using UnityEngine;
using Photon.Pun;
using DG.Tweening;
using UnityEngine.SceneManagement;

namespace Kocmoca
{
    public struct Movable
    {
        internal Vector3 Ready;
        internal Vector3 Show;
        internal float Interval;
    }
    public partial class GalaxyLobbyPanel
    {
        private int localType;
        private int localSkin;
        [Header("Main Camera")]
        public Transform mainCamera;
        public Transform pointLogin;
        public Transform pointLobby;
        private float timeCameraMoving = 1.0f;
        [Header("Dorara Hand & Xbox 360")]
        public Transform doraraHand;
        private Movable posDoraraHand = new Movable();
        public Animation controllerAnimation;
        [Header("Exhibition")]
        public Transform platformAxis;
        public GameObject[] model;
        public GameObject[] scan;

        void Initialize()
        {
            mainCamera.position = new Vector3(0, 5000, 0);
            mainCamera.rotation = Quaternion.Euler(0, 77, 0);

            // 初始化哆啦手
            posDoraraHand = new Movable
            {
                Show = doraraHand.localPosition,
                Ready = doraraHand.localPosition - new Vector3(0, 0.7f, 0),
                Interval = 0.37f
            };
            doraraHand.localPosition = posDoraraHand.Ready;
            doraraHand.gameObject.SetActive(true);
            controllerAnimation = doraraHand.GetComponent<Animation>();
            controllerAnimation["Xbox 360"].speed = 0;
        }

        void Command()
        {
            if (Input.GetKeyDown(Controller.KEY_Hangar) ||
                Input.GetKeyDown(Controller.KEYBOARD_Hangar) ||
                Input.GetKeyDown(Controller.XBOX360_Hangar))
                OnHangarButtonClicked();
            else if (Input.GetKeyDown(Controller.KEY_Operation) ||
                Input.GetKeyDown(Controller.KEYBOARD_Operation) ||
                Input.GetKeyDown(Controller.XBOX360_Operation))
                OnOperationButtonClicked();
            else if (Input.GetKeyDown(Controller.KEY_Controller) ||
                Input.GetKeyDown(Controller.KEYBOARD_Controller) ||
                Input.GetKeyDown(Controller.XBOX360_Controller))
                OnControllerButtonClicked();
            else if (Input.GetKeyDown(Controller.KEY_Escape) ||
                Input.GetKeyDown(Controller.XBOX360_Escape))
                OnEscapeButtonClicked();
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

        public void OnHoverXbox360()
        {
            controllerAnimation["Xbox 360"].speed = 1;
        }
        public void OnExitXbox360()
        {
            controllerAnimation["Xbox 360"].speed = 0;
        }

        public void OnHangarButtonClicked()
        {
            lobbyState = LobbyState.Hangar;
            Portal.Ending();
            PlayerPrefs.SetString(LobbyInfomation.PREFS_LOAD_SCENE, LobbyInfomation.SCENE_HANGAR);
            Invoke("Loading", 2.0f);
        }

        public void OnOperationButtonClicked()
        {
            lobbyState = LobbyState.Operation;
            OnJoinRandomRoomButtonClicked();
        }

        public void OnControllerButtonClicked()
        {
            if(lobbyState == LobbyState.Controller)
                lobbyState = LobbyState.Lobby;
            else if (lobbyState == LobbyState.Lobby)
                lobbyState = LobbyState.Controller;
            FindObjectOfType<ControllerPanel>().SetController();
        }

        public void OnEscapeButtonClicked()
        {
            lobbyState = LobbyState.Escape;
            Escape();
        }

        #endregion

        void SimulateHologram()
        {

        }

        void Loading()
        {
            SceneManager.LoadScene(LobbyInfomation.SCENE_LOADING);
        }

        void MoveToLogin()
        {
            RefreshRegion();
            lobbyState = LobbyState.Moving;
            doraraHand.DOLocalMove(posDoraraHand.Ready, posDoraraHand.Interval);
            MouseLock.MouseLocked = false;
            mainCamera.DOMove(pointLogin.position, timeCameraMoving);
            mainCamera.DOLookAt(pointLogin.TransformPoint(new Vector3(0, 0, 1)), timeCameraMoving);
            mainCamera.DORotate(pointLogin.rotation.eulerAngles, timeCameraMoving).OnComplete(() =>
            {
                lobbyState = LobbyState.Login;
                SetActivePanel("LoginPanel");
                StartCoroutine(FindObjectOfType<AnimationTween>().Play());
            });
        }

        void MoveToLobby()
        {
            lobbyState = LobbyState.Moving;
            MouseLock.MouseLocked = false;
            localType = PlayerPrefs.GetInt(LobbyInfomation.PREFS_TYPE);
            localSkin = PlayerPrefs.GetInt(LobbyInfomation.PREFS_SKIN+ localType);
            model[localType].GetComponentInChildren<Prototype>().LoadSkin(localSkin);
            for (int i = 0; i < model.Length; i++)
            {
                model[i].SetActive(i == localType);
                scan[i].SetActive(i == localType);
            }
            mainCamera.DOMove(pointLobby.position, timeCameraMoving);
            mainCamera.DOLookAt(pointLobby.TransformPoint(new Vector3(0, 0, 1)), timeCameraMoving);
            mainCamera.DORotate(pointLobby.rotation.eulerAngles, timeCameraMoving).OnComplete(() =>
            {
                lobbyState = LobbyState.Lobby;
                SetActivePanel("SelectionPanel");
                doraraHand.DOLocalMove(posDoraraHand.Show, posDoraraHand.Interval);
            });
        }

        void Escape()
        {
            PhotonNetwork.Disconnect();
        }
    }
}