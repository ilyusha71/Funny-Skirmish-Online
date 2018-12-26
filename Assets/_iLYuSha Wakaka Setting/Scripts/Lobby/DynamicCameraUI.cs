using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

namespace Kocmoca
{
    public enum MainState
    {
        Moving,
        Login,
        Lobby,
        Hangar,
        Show,
        newEvent,
    }

    public struct Movable
    {
        internal Vector3 Ready;
        internal Vector3 Show;
        internal float Interval;
    }

    public class DynamicCameraUI : MonoBehaviour
    {
        public MainState state = MainState.Login;
        public CanvasGroup OpeningCanvas;
        [Header("Camera")]
        public Transform pointLogin;
        public Transform pointLobby;
        public Transform pointHangarRail;
        private Transform mainCamera;
        private float timeIntoHangar;
        private float timeCameraMoving = 1.0f;
        // Dependent Components
        private AudioSource myAudioSource;
        [Header("Overlay Canvas")]
        public GalaxyLobbyPanel mainPanel;
        [Header("Dorara Hand")]
        public Transform doraraHand;
        private Animation xbox360;
        private Movable posDoraraHand = new Movable();
        [Header("Exhibition")]
        public GameObject[] exhibitionPlatform;
        public GameObject[] model;
        public GameObject[] scan;
        [Header("Display")]
        public Transform display;
        public Transform displayLeft;
        public Transform displayRight;
        public Transform displayLeftReady;
        public Transform displayRightReady;
        [Header("Display Control")]
        private float timeDisplayDropDown = 0.137f;
        private float timeBlockTypeDown = 0.37f;
        private float timeSwitchBlock = 7.93f;
        private float timeOpenData = 0.137f;
        private bool isMovingCamera;
        private bool allowShowData;
        private int indexAvailable = 16; // 可使用的宇航機編號
        private float nextBlockTime;

        public DynamicUIState uiState = DynamicUIState.Hangar;
        public DisplayContent currentContent = DisplayContent.Kocmocraft;



        [Header("Aircraft Type")]
        public Transform blockType;
        private TextMeshProUGUI textType;
        private Vector3 relativePosType;
        private Vector3 readyPosType;
        [Header("Data")]
        public Transform[] blockKocmocraft;
        public Transform[] blockWeapon;
        public Transform[] blockRadar;
        private TextMeshProUGUI[] textKocmocraft;
        private TextMeshProUGUI[] textWeapon;
        private TextMeshProUGUI[] textRadar;
        private DisplayData dataKocmocraft = new DisplayData();
        private DisplayData dataWeapon = new DisplayData();
        private DisplayData datatRadar = new DisplayData();
        [Header("Control")]


        public GameObject[] apron;
        public Transform[] point;
        int max;
        int now;

        [Header("起飛語音")]
        public AudioClip[] voiceTakeOff;
        //bool enterFlag;
        [Header("起飛鏡頭")]
        public Transform[] camPoint;
        public Transform[] takeOffAircraft;
        public CanvasGroup mask;
        public AudioClip BGMTakeOff;

        void Awake()
        {
            OpeningCanvas.alpha = 1;
            OpeningCanvas.DOFade(0, 2.0f).OnComplete(() => OpeningCanvas.blocksRaycasts = false);
            mainCamera = transform;
            mainCamera.position = new Vector3(0, 5000, 0);
            mainCamera.rotation = Quaternion.identity;
            myAudioSource = GetComponent<AudioSource>();
            Initialize();
        }
        void Initialize()
        {
            // 初始化哆啦手
            posDoraraHand = new Movable
            {
                Show = doraraHand.localPosition,
                Ready = doraraHand.localPosition - new Vector3(0, 0.7f, 0),
                Interval = 0.37f
            };
            doraraHand.localPosition = posDoraraHand.Ready;
            doraraHand.gameObject.SetActive(true);
            xbox360 = doraraHand.GetComponent<Animation>();
            xbox360["Xbox 360"].speed = 0;
        }
        void Display()
        {
            // 初始化顯示器
            display.localPosition = displayLeftReady.localPosition;
            display.gameObject.SetActive(true);
            // 初始化機型顯示器
            textType = blockType.GetComponent<TextMeshProUGUI>();
            relativePosType = display.InverseTransformPoint(blockType.position); // 轉換為顯示器相對座標
            blockType.localPosition += new Vector3(0, 100, 0);
            readyPosType = display.InverseTransformPoint(blockType.position);
            blockType.localPosition -= new Vector3(0, 100, 0);

            // 初始化資料顯示器
            dataKocmocraft.Initialize(blockKocmocraft.Length);
            textKocmocraft = new TextMeshProUGUI[dataKocmocraft.count];
            dataWeapon.Initialize(blockWeapon.Length);
            textWeapon = new TextMeshProUGUI[dataWeapon.count];
            datatRadar.Initialize(blockRadar.Length);
            textRadar = new TextMeshProUGUI[datatRadar.count];
            for (int i = 0; i < 6; i++)
            {
                if (i < dataKocmocraft.count)
                {
                    dataKocmocraft.posShow[i] = display.InverseTransformPoint(blockKocmocraft[i].position);
                    blockKocmocraft[i].localPosition += i == 0 ? new Vector3(0, 500, 0) : new Vector3(-1000, 0, 0);
                    dataKocmocraft.posReady[i] = display.InverseTransformPoint(blockKocmocraft[i].position);
                    textKocmocraft[i] = i == 0 ?
                        blockKocmocraft[i].GetComponent<TextMeshProUGUI>() :
                        blockKocmocraft[i].GetComponentsInChildren<TextMeshProUGUI>()[1];
                }
                if (i < dataWeapon.count)
                {
                    dataWeapon.posShow[i] = display.InverseTransformPoint(blockWeapon[i].position);
                    blockWeapon[i].localPosition += i == 0 ? new Vector3(0, 500, 0) : new Vector3(-1000, 0, 0);
                    dataWeapon.posReady[i] = display.InverseTransformPoint(blockWeapon[i].position);
                    textWeapon[i] = i == 0 ?
                        blockWeapon[i].GetComponent<TextMeshProUGUI>() :
                        blockWeapon[i].GetComponentsInChildren<TextMeshProUGUI>()[1];
                }
                if (i < datatRadar.count)
                {
                    datatRadar.posShow[i] = display.InverseTransformPoint(blockRadar[i].position);
                    blockRadar[i].localPosition += i == 0 ? new Vector3(0, 500, 0) : new Vector3(-1000, 0, 0);
                    datatRadar.posReady[i] = display.InverseTransformPoint(blockRadar[i].position);
                    textRadar[i] = i == 0 ?
                        blockRadar[i].GetComponent<TextMeshProUGUI>() :
                        blockRadar[i].GetComponentsInChildren<TextMeshProUGUI>()[1];
                }
            }

            max = apron.Length;
            point = new Transform[max];
            for (int i = 0; i < max; i++)
            {
                point[i] = apron[i].transform.Find("Camera Point");
            }
        }

        public void VisitLogin()
        {
            OnCameraMove();
            MouseLock.MouseLocked = false;
            mainCamera.DOMove(pointLogin.position, timeCameraMoving * 2);
            mainCamera.DOLookAt(pointLogin.TransformPoint(new Vector3(0, 0, 1)), timeCameraMoving * 2);
            mainCamera.DORotate(pointLogin.rotation.eulerAngles, timeCameraMoving * 2).OnComplete(() =>
            {
                state = MainState.Login;
                mainPanel.SetActivePanel("LoginPanel");
                StartCoroutine(FindObjectOfType<AnimationTween>().Play());
            });
        }

        public void VisitLobby()
        {
            OnCameraMove();
            MouseLock.MouseLocked = false;
            now = PlayerPrefs.GetInt(LobbyInfomation.PREFS_TYPE);
            Exhibition();
            mainCamera.DOMove(pointLobby.position, timeCameraMoving).OnComplete(() =>
            {
                state = MainState.Lobby;
                mainPanel.SetActivePanel("SelectionPanel");
                doraraHand.DOLocalMove(posDoraraHand.Show, posDoraraHand.Interval);
            });
            mainCamera.DOLookAt(pointLobby.TransformPoint(new Vector3(0, 0, 1)), timeCameraMoving);
            mainCamera.DORotate(pointLobby.rotation.eulerAngles, timeCameraMoving);
        }

        public void VisitHangar()
        {
            SceneManager.LoadScene("New Airport 3");
        }
        void Exhibition()
        {
            for (int i = 0; i < model.Length; i++)
            {
                model[i].SetActive(i == now);
                scan[i].SetActive(i == now);
            }
        }
        public void OnHoverXbox360()
        {
            xbox360["Xbox 360"].speed = 1;
        }
        public void OnExitXbox360()
        {
            xbox360["Xbox 360"].speed = 0;
        }
        public void OnHoverExhibition()
        {
            exhibitionPlatform[0].SetActive(false);
            exhibitionPlatform[1].SetActive(true);
        }
        public void OnExitExhibition()
        {
            exhibitionPlatform[0].SetActive(true);
            exhibitionPlatform[1].SetActive(false);
        }
        void OnCameraMove()
        {
            state = MainState.Moving;
            doraraHand.DOLocalMove(posDoraraHand.Ready, posDoraraHand.Interval);
        }
        private void MoveCamera(Transform destination)
        {
            mainCamera.DOMove(destination.position, timeCameraMoving);
            mainCamera.DOLookAt(destination.TransformPoint(new Vector3(0, 0, 1)), timeCameraMoving);
            mainCamera.DORotate(destination.rotation.eulerAngles, timeCameraMoving);
        }

        void Update()
        {
            if (state == MainState.Lobby)
            {
                if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button6))
                    mainPanel.OnEscapeButtonClicked();
                if (Input.GetKeyDown(Controller.KEY_Hangar) ||
                    Input.GetKeyDown(Controller.KEYBOARD_Hangar) ||
                    Input.GetKeyDown(Controller.XBOX360_Hangar))
                    VisitHangar();
                else if (Input.GetKeyDown(Controller.KEY_Operation) ||
                    Input.GetKeyDown(Controller.KEYBOARD_Operation) ||
                    Input.GetKeyDown(Controller.XBOX360_Operation))
                    mainPanel.OnJoinRandomRoomButtonClicked();
            }
        }
    }
}