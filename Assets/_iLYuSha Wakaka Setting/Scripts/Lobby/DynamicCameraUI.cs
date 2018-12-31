using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

namespace Kocmoca
{




    public class DynamicCameraUI : MonoBehaviour
    {
        public LobbyState state = LobbyState.Login;
        //public CanvasGroup OpeningCanvas;
        [Header("Camera")]
        public Transform pointLogin;
        public Transform pointLobby;
        public Transform pointHangarRail;
        private Transform mainCamera;
        private float timeIntoHangar;
        ////private float timeCameraMoving = 1.0f;
        // Dependent Components
        private AudioSource myAudioSource;
        [Header("Overlay Canvas")]
        public GalaxyLobbyPanel mainPanel;
        [Header("Dorara Hand")]
        public Transform doraraHand;
        private Animation xbox360;
        //private Movable posDoraraHand = new Movable();
        [Header("Exhibition")]
        //public GameObject[] model;
        //public GameObject[] scan;
        [Header("Display")]
        public Transform display;
        public Transform displayLeft;
        public Transform displayRight;
        public Transform displayLeftReady;
        public Transform displayRightReady;
        [Header("Display Control")]
        //private float timeDisplayDropDown = 0.137f;
        //private float timeBlockTypeDown = 0.37f;
        //private float timeSwitchBlock = 7.93f;
        //private float timeOpenData = 0.137f;
        private bool isMovingCamera;
        private bool allowShowData;
        //private int indexAvailable = 16; // 可使用的宇航機編號
        private float nextBlockTime;

        public DynamicUIState uiState = DynamicUIState.Hangar;
        public DisplayContent currentContent = DisplayContent.Kocmocraft;
        int now;
        public AudioClip BGMTakeOff;

        void Awake()
        {
            ////OpeningCanvas.alpha = 1;
            ////OpeningCanvas.DOFade(0, 2.0f).OnComplete(() => OpeningCanvas.blocksRaycasts = false);
            //mainCamera = transform;
            //mainCamera.position = new Vector3(0, 5000, 0);
            //mainCamera.rotation = Quaternion.Euler(0,77,0);
            //myAudioSource = GetComponent<AudioSource>();
            //Initialize();
        }
        //void Initialize()
        //{
        //    初始化哆啦手
        //   posDoraraHand = new Movable
        //   {
        //       Show = doraraHand.localPosition,
        //       Ready = doraraHand.localPosition - new Vector3(0, 0.7f, 0),
        //       Interval = 0.37f
        //   };
        //    doraraHand.localPosition = posDoraraHand.Ready;
        //    doraraHand.gameObject.SetActive(true);
        //    xbox360 = doraraHand.GetComponent<Animation>();
        //    xbox360["Xbox 360"].speed = 0;
        //}

        //public void VisitLogin()
        //{
        //    OnCameraMove();
        //    MouseLock.MouseLocked = false;
        //    mainCamera.DOMove(pointLogin.position, timeCameraMoving * 2);
        //    mainCamera.DOLookAt(pointLogin.TransformPoint(new Vector3(0, 0, 1)), timeCameraMoving * 2);
        //    mainCamera.DORotate(pointLogin.rotation.eulerAngles, timeCameraMoving * 2).OnComplete(() =>
        //    {
        //        state = LobbyState.Login;
        //        mainPanel.lobbyState = LobbyState.Login;
        //        mainPanel.SetActivePanel("LoginPanel");
        //        StartCoroutine(FindObjectOfType<AnimationTween>().Play());
        //    });
        //}

        //public void VisitLobby()
        //{
        //    OnCameraMove();
        //    MouseLock.MouseLocked = false;
        //    //now = PlayerPrefs.GetInt(LobbyInfomation.PREFS_TYPE);
        //    //Exhibition();
        //    mainCamera.DOMove(pointLobby.position, timeCameraMoving).OnComplete(() =>
        //    {
        //        state = LobbyState.Lobby;
        //        mainPanel.lobbyState = LobbyState.Lobby;
        //        mainPanel.SetActivePanel("SelectionPanel");
        //        doraraHand.DOLocalMove(posDoraraHand.Show, posDoraraHand.Interval);
        //    });
        //    mainCamera.DOLookAt(pointLobby.TransformPoint(new Vector3(0, 0, 1)), timeCameraMoving);
        //    mainCamera.DORotate(pointLobby.rotation.eulerAngles, timeCameraMoving);
        //}

        //public void VisitHangar()
        //{
        //    SceneManager.LoadScene("New Airport 3");
        //}
        //void Exhibition()
        //{
        //    for (int i = 0; i < model.Length; i++)
        //    {
        //        model[i].SetActive(i == now);
        //        scan[i].SetActive(i == now);
        //    }
        //}
        //public void OnHoverXbox360()
        //{
        //    xbox360["Xbox 360"].speed = 1;
        //}
        //public void OnExitXbox360()
        //{
        //    xbox360["Xbox 360"].speed = 0;
        //}

        //    void OnCameraMove()
        //    {
        //        state = LobbyState.Moving;
        //        doraraHand.DOLocalMove(posDoraraHand.Ready, posDoraraHand.Interval);
        //    }
        //    private void MoveCamera(Transform destination)
        //    {
        //        mainCamera.DOMove(destination.position, timeCameraMoving);
        //        mainCamera.DOLookAt(destination.TransformPoint(new Vector3(0, 0, 1)), timeCameraMoving);
        //        mainCamera.DORotate(destination.rotation.eulerAngles, timeCameraMoving);
        //    }
        //}
    }
}