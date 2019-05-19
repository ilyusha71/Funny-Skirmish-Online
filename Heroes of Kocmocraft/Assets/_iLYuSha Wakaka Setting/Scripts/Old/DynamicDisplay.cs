using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

namespace Kocmoca
{
    public enum DisplayContent
    {
        None = 0,
        Kocmocraft = 1,
        Weapon = 2,
        Radar = 3,
    }
    public enum DynamicUIState
    {
        Ready,
        Hangar,
        Show,
        newEvent,
    }
    public enum DataShowState
    {
        Ready,
        Wait,
        Moving,
        Show,
    }

    public struct DisplayData
    {
        public int count;
        public Vector3[] posReady;
        public Vector3[] posShow;

        public void Initialize(int count)
        {
            this.count = count;
            posReady = new Vector3[count];
            posShow = new Vector3[count];
        }
    }

    //public enum DisplayState
    //{
    //    Ready,
    //    Wait,
    //    Moving,
    //    Show,
    //}

    public class DynamicDisplay : MonoBehaviour
    {
        public LobbyState state = LobbyState.Login;
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
                state = LobbyState.Login;
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
                state = LobbyState.Lobby;
                mainPanel.SetActivePanel("SelectionPanel");
                doraraHand.DOLocalMove(posDoraraHand.Show, posDoraraHand.Interval);
            });
            mainCamera.DOLookAt(pointLobby.TransformPoint(new Vector3(0, 0, 1)), timeCameraMoving);
            mainCamera.DORotate(pointLobby.rotation.eulerAngles, timeCameraMoving);
        }
        public void VisitHangar()
        {
            Invoke("LoadHangarScene", 2.0f);
        }
        private void LoadHangarScene()
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
            state = LobbyState.Moving;
            doraraHand.DOLocalMove(posDoraraHand.Ready, posDoraraHand.Interval);
            ResetDisplay();
        }
        private void MoveCamera(Transform destination)
        {
            mainCamera.DOMove(destination.position, timeCameraMoving);
            mainCamera.DOLookAt(destination.TransformPoint(new Vector3(0, 0, 1)), timeCameraMoving);
            mainCamera.DORotate(destination.rotation.eulerAngles, timeCameraMoving);
        }

        void Update()
        {
            //if (Input.GetKeyDown(KeyCode.V))
            //    SceneManager.LoadScene(LobbyInfomation.SCENE_LOBBY);
            //if (Input.GetKeyDown(KeyCode.N))
            //    mainPanel.SetActivePanel("CreateRoomPanel");
            if (state == LobbyState.Lobby)
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
            else if (state == LobbyState.Hangar)
            {
                //if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button6))
                //    VisitLobby();
                //if (Input.GetKeyDown(Controller.KEY_PreviousHangar_0) ||
                //    Input.GetKeyDown(Controller.KEY_PreviousHangar_1))
                //    PreviousHangar();
                //else if (Input.GetKeyDown(Controller.KEY_NextHangar_0) ||
                //    Input.GetKeyDown(Controller.KEY_NextHangar_1))
                //    NextHangar();
                //if (Input.GetKeyDown(Controller.KEY_PreviousData_0) ||
                //    Input.GetKeyDown(Controller.KEY_PreviousData_1))
                //    PreviousData();
                //else if (Input.GetKeyDown(Controller.KEY_NextData_0) ||
                //    Input.GetKeyDown(Controller.KEY_NextData_1))
                //    NextData();
                //if (Input.GetKeyDown(Controller.Vocal.KeyCode) ||
                //    Input.GetKeyDown(Controller.XBOX360_Vocal))
                //    myAudioSource.PlayOneShot(voiceTakeOff[now]);

                if (isMovingCamera)
                {
                    if (Time.time > timeIntoHangar && isMovingCamera)
                    {
                        isMovingCamera = false;
                        CameraInPlaceCallback();
                    }
                }
                if (allowShowData)
                {
                    if (Time.time > nextBlockTime)
                        NextData();
                }
            }
        }

        void NextHangar()
        {
            now++;
            now = (int)Mathf.Repeat(now, max);
            TransitCamera();
        }
        void PreviousHangar()
        {
            now--;
            now = (int)Mathf.Repeat(now, max);
            TransitCamera();
        }
        void TransitCamera()
        {
            if (now <= indexAvailable) PlayerPrefs.SetInt(LobbyInfomation.PREFS_TYPE, now);
            // 攝影機移動過程
            isMovingCamera = true;
            timeIntoHangar = Time.time + timeCameraMoving;
            pointHangarRail.DOMove(point[now].position, timeCameraMoving);
            pointHangarRail.DOLookAt(point[now].TransformPoint(new Vector3(0, 0, 1)), timeCameraMoving);
            pointHangarRail.DORotate(point[now].rotation.eulerAngles, timeCameraMoving);

            ResetDisplay();
        }
        //顯示器恢復預設
        void ResetDisplay()
        {
            allowShowData = false; // 限制顯示器畫面輸出
            display.DOKill();
            if (now < 8)
                display.DOLocalMove(displayLeftReady.localPosition, 0.73f);
            else
                display.DOLocalMove(displayRightReady.localPosition, 0.73f);
            blockType.DOKill();
            blockType.position = display.TransformPoint(readyPosType);
            HideData();
            currentContent = DisplayContent.None;
        }
        // 攝影機就位
        void CameraInPlaceCallback()
        {
            display.DOKill();
            if (now < 8)
            {
                display.localPosition = displayLeftReady.localPosition;
                display.localRotation = displayLeftReady.localRotation;
                display.DOLocalMove(displayLeft.localPosition, timeDisplayDropDown).SetEase(Ease.OutFlash).OnComplete(DisplayInPlaceCallback);
            }
            else
            {
                display.localPosition = displayRightReady.localPosition;
                display.localRotation = displayRightReady.localRotation;
                display.DOLocalMove(displayRight.localPosition, timeDisplayDropDown).SetEase(Ease.OutFlash).OnComplete(DisplayInPlaceCallback);
            }
        }
        // 顯示器就位
        void DisplayInPlaceCallback()
        {
            blockType.DOKill();
            blockType.position = display.TransformPoint(readyPosType);
            blockType.DOMove(display.TransformPoint(relativePosType), timeBlockTypeDown).OnComplete(ShowData); ;
            textType.text = DesignData.Project[now];
        }
        void ShowData()
        {
            allowShowData = true;
            nextBlockTime = 0;
            if (currentContent != DisplayContent.None)
                Debug.LogError("Fucku");
        }
        void HideData()
        {
            switch (currentContent)
            {
                case DisplayContent.Kocmocraft:
                    for (int i = 0; i < dataKocmocraft.count; i++)
                    {
                        blockKocmocraft[i].DOKill();
                        blockKocmocraft[i].position = display.TransformPoint(dataKocmocraft.posReady[i]);
                    }
                    break;
                case DisplayContent.Weapon:
                    for (int i = 0; i < dataWeapon.count; i++)
                    {
                        blockWeapon[i].DOKill();
                        blockWeapon[i].position = display.TransformPoint(dataWeapon.posReady[i]);
                    }
                    break;
                case DisplayContent.Radar:
                    for (int i = 0; i < datatRadar.count; i++)
                    {
                        blockRadar[i].DOKill();
                        blockRadar[i].position = display.TransformPoint(datatRadar.posReady[i]);
                    }
                    break;
            }
        }
        void NextData()
        {
            if (!allowShowData)
                return;
            nextBlockTime = Time.time + timeSwitchBlock;
            HideData();
            switch (currentContent)
            {
                case DisplayContent.None: ShowKocmocraftData(0); break;
                case DisplayContent.Kocmocraft: ShowWeaponData(0); break;
                case DisplayContent.Weapon: ShowRadarData(0); break;
                case DisplayContent.Radar: ShowKocmocraftData(0); break;
            }
        }
        void PreviousData()
        {
            if (!allowShowData)
                return;
            nextBlockTime = Time.time + timeSwitchBlock;
            HideData();
            switch (currentContent)
            {
                case DisplayContent.Kocmocraft: ShowRadarData(0); break;
                case DisplayContent.Weapon: ShowKocmocraftData(0); break;
                case DisplayContent.Radar: ShowWeaponData(0); break;
            }
        }
        void ShowKocmocraftData(int index)
        {
            if (!allowShowData)
                return;
            currentContent = DisplayContent.Kocmocraft;
            if (now > indexAvailable)
            {
                textKocmocraft[0].text = "" + DesignData.Project[now];
                textKocmocraft[1].text = "---";
                textKocmocraft[2].text = "---";
                textKocmocraft[3].text = "---";
                textKocmocraft[4].text = "---";
                textKocmocraft[5].text = "---";
            }
            else
            {
                switch (index)
                {
                    case 0: textKocmocraft[0].text = "" + DesignData.Project[now]; break;
                    case 1: textKocmocraft[1].text = "" + KocmocraftData.Hull[now]; break;
                    case 2: textKocmocraft[2].text = "" + KocmocraftData.Shield[now]; break;
                    case 3: textKocmocraft[3].text = "" + KocmocraftData.Energy[now]; break;
                    case 4: textKocmocraft[4].text = "" + KocmocraftData.CruiseSpeed[now] * 3.6f + " km/h"; break;
                    case 5: textKocmocraft[5].text = "" + KocmocraftData.AfterburnerSpeed[now] * 3.6f + " km/h"; break;
                }
            }
            if (index == dataKocmocraft.count - 1)
                blockKocmocraft[index].DOMove(display.TransformPoint(dataKocmocraft.posShow[index]), timeOpenData);
            else
                blockKocmocraft[index].DOMove(display.TransformPoint(dataKocmocraft.posShow[index]), timeOpenData).OnComplete(() => ShowKocmocraftData(index + 1));
        }
        void ShowWeaponData(int index)
        {
            if (!allowShowData)
                return;
            currentContent = DisplayContent.Weapon;
            if (now > indexAvailable)
            {
                textWeapon[1].text = "---";
                textWeapon[2].text = "---";
                textWeapon[3].text = "---";
                textWeapon[4].text = "---";
            }
            else
            {
                switch (index)
                {
                    //case 1: textWeapon[1].text = "" + WeaponData.TurretCount[now] + "x Assault Laser"; break;
                    //case 2: textWeapon[2].text = "" + KocmoLaserCannon.FireRoundPerSecond + " rps"; break;
                    case 3: textWeapon[3].text = "update" + " dmg"; break;
                    case 4: textWeapon[4].text = "update" + " m"; break;
                }
            }
            if (index == dataWeapon.count - 1)
                blockWeapon[index].DOMove(display.TransformPoint(dataWeapon.posShow[index]), timeOpenData);
            else
                blockWeapon[index].DOMove(display.TransformPoint(dataWeapon.posShow[index]), timeOpenData).OnComplete(() => ShowWeaponData(index + 1));
        }
        void ShowRadarData(int index)
        {
            if (!allowShowData)
                return;
            currentContent = DisplayContent.Radar;
            if (index == datatRadar.count - 1)
                blockRadar[index].DOMove(display.TransformPoint(datatRadar.posShow[index]), timeOpenData);
            else
                blockRadar[index].DOMove(display.TransformPoint(datatRadar.posShow[index]), timeOpenData).OnComplete(() => ShowRadarData(index + 1));
        }

        IEnumerator TakeOff()
        {
            display.DOKill();
            if (now < 8)
                display.DOLocalMove(displayLeftReady.localPosition, 0.73f);
            else
                display.DOLocalMove(displayRightReady.localPosition, 0.73f);
            blockType.position = display.TransformPoint(readyPosType);

            //for (int i = 0; i < countDisplayData; i++)
            //{
            //    blockAircraft[i].position = display.TransformPoint(dataKocmocraft[i]);
            //    if (i < countRadarData)
            //}
            yield return new WaitForSeconds(1.73f);
            isMovingCamera = true;
            mainCamera.DOMove(camPoint[0].position, 5.0f);
            mainCamera.DOLookAt(camPoint[0].TransformPoint(new Vector3(0, 0, 1)), 5.0f);
            mainCamera.DORotateQuaternion(camPoint[0].rotation, 5.0f);
            yield return new WaitForSeconds(4.0f);
            myAudioSource.clip = BGMTakeOff;
            myAudioSource.Play();
            for (int i = 0; i < takeOffAircraft.Length; i++)
            {
                takeOffAircraft[i].GetComponent<HarmonicMotion>().enabled = false;
                takeOffAircraft[i].GetComponent<AudioSource>().DOFade(0.3f, 4.0f);
            }
            yield return new WaitForSeconds(2.0f);
            for (int i = 0; i < takeOffAircraft.Length; i++)
            {
                takeOffAircraft[i].transform.DOLocalMoveX(takeOffAircraft[i].transform.localPosition.z - 1000, 25.0f).SetEase(Ease.InCubic);
                if (i % 2 == 0)
                    yield return new WaitForSeconds(0.3f);
                else
                    yield return new WaitForSeconds(0.7f);
            }
            mainCamera.DOMove(camPoint[1].position, 5.0f);
            mainCamera.DOLookAt(camPoint[2].TransformPoint(new Vector3(0, 0, 1)), 5.0f);
            mainCamera.DORotateQuaternion(camPoint[1].rotation, 5.0f);
            yield return new WaitForSeconds(3.0f);
            mainCamera.DOMove(camPoint[2].position, 5.0f);
            mainCamera.DOLookAt(camPoint[2].TransformPoint(new Vector3(0, 0, 1)), 5.0f);
            mainCamera.DORotateQuaternion(camPoint[2].rotation, 5.0f);
            yield return new WaitForSeconds(5.0f);
            for (int i = 0; i < takeOffAircraft.Length; i++)
            {
                takeOffAircraft[i].GetComponent<HarmonicMotion>().enabled = false;
                takeOffAircraft[i].GetComponent<AudioSource>().DOFade(0.05f, 2.0f);
            }
            mask.DOFade(1.0f, 1.5f);
            yield return new WaitForSeconds(1.5f);
            SceneManager.LoadSceneAsync("Airport2");
        }
    }
}