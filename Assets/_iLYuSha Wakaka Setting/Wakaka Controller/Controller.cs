/***************************************************************************
 * Wakaka Controller
 * 哇咔咔控制器（輸入控制管理）
 * Last Updated: 2018/10/23
 * Description:
 * 1. 修正映射WSAD鍵控制（2018/10/23）
 *     Input.GetAxis("Horizontal") 的輸入值可以來自【左右方向鍵】與【AD鍵】
 *     但是 Input.GetAxis("Horizontal") 不能用來判斷 Input.GetKey(KeyCode.D)
 *     【主要】方向鍵映射WSAD鍵點擊事件
 * 2. 使用MouseLock類進行設定視窗與鼠標顯示控制
 * 3. 顯示FPS控制功能
 * 4. 擴增Xbox 360搖桿對鍵盤映射
 ***************************************************************************/
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;



public partial class Controller : MonoBehaviour
{
    public static Controller Instance { get; private set; }
    public bool Testing;

    // General Setting
    [SerializeField] public static ControllerType controllerType = ControllerType.MouseKeyboard;
    private static readonly string PREFS_CONTROLLER_TYPE = "ControllerType";
    [SerializeField] public static ControlMode controlMode = ControlMode.General;
    [SerializeField] public static ControlHands controlHands = ControlHands.AmericanHands;
    private static readonly string PREFS_CONTROLLER_HAND = "ControllerHand";

    // General Control
    private float[] controlValue = new float[4];
    private AxisData axisHorizontal = new AxisData { Plus = new AxisButton { Keycode = 102 }, Minus = new AxisButton { Keycode = 100 } };
    private AxisData axisVertical = new AxisData { Plus = new AxisButton { Keycode = 104 }, Minus = new AxisButton { Keycode = 98 } };
    private static float axisLevel; // 軸映射點擊事件的臨界值

    private void LoadPrefs()
    {
        controllerType = (ControllerType)PlayerPrefs.GetInt(PREFS_CONTROLLER_TYPE);
        controlHands = (ControlHands)PlayerPrefs.GetInt(PREFS_CONTROLLER_HAND);
    }

    private void Awake()
    {
        LoadPrefs();
    }





    #region FPS Setting
    [Header("FPS")]
    public Toggle toggleFPS;
    public TextMeshProUGUI textFPS;
    private bool showFPS;
    private float deltaTime;
    public void OpenHideFPS()
    {
        showFPS = toggleFPS.isOn;
        PlayerPrefs.SetInt("Open FPS", showFPS ? 1 : 0);
    }
    void ShowFPS()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        textFPS.text = Mathf.Ceil(1.0f / deltaTime) + " fps"; ;
    }
    #endregion

    [Header("Controller Setting")]
    //public static PanelState state = PanelState.Ready;
    //public static ControllerType controllerType;
    //public static ControlMode controlMode;
    //private static ControlHands controlHands;
    [Header("New Setting")]
    private static int indexRoll=0, indexPitch=1, indexYaw=2, indexThrotte=3; // 飛行控制慣用手索引


    //[Header("Old Setting")]
    //public GameObject panelSetting;
    //public GameObject groupType;
    //public GameObject groupMode;
    //public GameObject groupHands;
    //private static Toggle[] type;
    //private static Toggle[] mode;
    //private static Toggle[] hands;

    static Controller()
    {
        GameObject go = new GameObject("Controller");
        DontDestroyOnLoad(go);
        Instance = go.AddComponent<Controller>();
    }

    public static void InitializeController()
    {
        //showFPS = PlayerPrefs.GetInt("Open FPS") == 1 ? true : false;
        //toggleFPS.isOn = showFPS;

        // Initialize
        int loadType = PlayerPrefs.GetInt("saveType");
        int loadMode = PlayerPrefs.GetInt("saveMode");
        int loadHands = PlayerPrefs.GetInt("saveHands");
        controllerType = (ControllerType)loadType;
        //panelHotkey.SetActive(false);

        // Old
        //type = groupType.GetComponentsInChildren<Toggle>();
        //type[loadType].isOn = true;
        //mode = groupMode.GetComponentsInChildren<Toggle>();
        //mode[loadMode].isOn = true;
        //hands = groupHands.GetComponentsInChildren<Toggle>();
        //hands[loadHands].isOn = true;
        //panelSetting.SetActive(false);

        //InitializeHotkey();
        // Xbox 360 Setting
        //InitializeXbox360Setting();
    }



    void Update()
    {
        //if (Input.GetButton("Fire1"))
        //    Debug.Log("FIRE");
        //if (Input.GetButtonDown("Fire1"))
        //    Debug.LogWarning("Down");
        //else if (Input.GetButtonUp("Fire1"))
        //    Debug.LogWarning("Up");

        //if (showFPS) ShowFPS();
        if (ArduinoController.Instance)
        {
            countQueue = ArduinoController.queueCommand.Count;
            if (countQueue > 0)
            {
                for (int i = 0; i < countQueue; i++)
                {
                    msgBox.AddNewMsg(ArduinoController.queueMsg.Dequeue().Replace("Wakaka/", ""));
                    command.Input(ArduinoController.queueCommand.Dequeue());
                }
            }
        }

        // Special Function
        //if (Input.GetKeyDown(KeyCode.B))
        //    toggleFPS.isOn = !toggleFPS.isOn;
        if (Input.GetKeyDown(KeyCode.BackQuote))
            MouseLock.MouseLocked = !MouseLock.MouseLocked;
        //if (Input.GetKeyDown(KeyCode.F11))
        //{
        //    if (!panelSetting.activeSelf)
        //        MouseLock.nowState = MouseLock.MouseLocked;
        //    panelSetting.SetActive(!panelSetting.activeSelf);
        //    MouseLock.MouseLocked = panelSetting.activeSelf ? false : MouseLock.nowState;
        //}

        // Steering wheel mode
        //if (Input.GetKeyDown(KeyCode.H))
        //    UseSteeringWheel();

            RealtimeControl();
    }

    void LateUpdate()
    {
        if (ArduinoController.Instance)
            command.Reset();
    }

    void RealtimeControl()
    {
        InputControllerType();
        if (controlMode == ControlMode.General)
            AxisButtonClickEvent();
    }

    private void AxisButtonClickEvent()
    {
        // 水平軸映射左右方向鍵點擊事件
        if (Math.Abs(axisHorizontal.Value) < Math.Abs(controlValue[2]))
        {
            axisHorizontal.Value = controlValue[2];
            if (axisHorizontal.Plus.Pressed)
            {
                axisHorizontal.Plus.Pressed = false;
                MappingEvent(axisHorizontal.Plus.Keycode, 2);
            }
            if (axisHorizontal.Minus.Pressed)
            {
                axisHorizontal.Minus.Pressed = false;
                MappingEvent(axisHorizontal.Minus.Keycode, 2);
            }
        }
        else
        {
            axisHorizontal.Value = controlValue[2];
            if (axisHorizontal.Value > axisLevel)
            {
                axisHorizontal.Plus.Pressed = true;
                MappingEvent(axisHorizontal.Plus.Keycode, 0);
            }
            else if (axisHorizontal.Value < -axisLevel)
            {
                axisHorizontal.Minus.Pressed = true;
                MappingEvent(axisHorizontal.Minus.Keycode, 0);
            }
        }



        // 垂直軸映射上下方向鍵點擊事件
        axisVertical.Value = controlValue[3];
        if (axisVertical.Value > axisLevel)
        {
            axisVertical.Plus.Pressed = true;
            MappingEvent(axisVertical.Plus.Keycode, 0);
        }
        else if (axisVertical.Value < -axisLevel)
        {
            axisVertical.Minus.Pressed = true;
            MappingEvent(axisVertical.Minus.Keycode, 0);
        }
        else
        {
            if (axisVertical.Plus.Pressed)
            {
                axisVertical.Plus.Pressed = false;
                MappingEvent(axisVertical.Plus.Keycode, 2);
            }
            if (axisVertical.Minus.Pressed)
            {
                axisVertical.Minus.Pressed = false;
                MappingEvent(axisVertical.Minus.Keycode, 2);
            }
        }
    }

    void InputControllerType()
    {
        if (controllerType == ControllerType.MouseKeyboard)
            MouseKeyboardAxisInput();
        else if (controllerType == ControllerType.Xbox360)
        {
            Xbox360AxisInput();
            if (controlMode == ControlMode.Xbox360) // 使用Xbox360搖桿按鍵映射
                Xbox360ButtonMapping();
        }
        else if (controllerType == ControllerType.WakakaFlyingMode)
        {
            WakakaFlyingAxisInput();
            WakakaModeButtonMapping();
        }
    }

    //void InputControllerMode()
    //{
    //    if (controlMode == ControlMode.General)
    //        MapKeyWSAD();
    //    else if (controlMode == ControlMode.Flying)
    //    {

    //    }
    //    else if (controlMode == ControlMode.Driving)
    //    {
    //        if (useSteeringWheel)
    //            DrivingHandleButtonMapping();
    //        else
    //            thruster = Mathf.Clamp(controlValue[indexThrotte] * axisSensasity, -1, 1);
    //        rudder = (useSteeringWheel) ? Mathf.Clamp(xAccelValue * axisSensasity, -1, 1) : Mathf.Clamp(controlValue[indexRoll] * axisSensasity, -1, 1);
    //    }
    //    else if (controlMode == ControlMode.Custom)
    //    {


    //    }
    //}

    //void MapKeyWSAD()
    //{
    //    // 右搖桿X軸映射AD鍵
    //    if (xAxisValue != -999)
    //    {
    //        if (xAxisValue > sensitivityWSAD)
    //        {
    //            if (rightHorizontalState == 1) Keybd_event(68, 0, 1, 0); // 等同於 Input.GetKey(KeyCode.D)
    //            else Keybd_event(68, 0, 0, 0); // 等同於 Input.GetKeyDown(KeyCode.D) 
    //            rightHorizontalState = 1;
    //        }
    //        else if (xAxisValue < -sensitivityWSAD)
    //        {
    //            if (rightHorizontalState == -1) Keybd_event(65, 0, 1, 0); // 等同於 Input.GetKey(KeyCode.A)
    //            else Keybd_event(65, 0, 0, 0); // 等同於 Input.GetKeyDown(KeyCode.A)
    //            rightHorizontalState = -1;
    //        }
    //        else
    //        {
    //            if (rightHorizontalState == 1) Keybd_event(68, 0, 2, 0); // 等同於 Input.GetKeyUp(KeyCode.D)
    //            else if (rightHorizontalState == -1) Keybd_event(65, 0, 2, 0); // 等同於 Input.GetKeyUp(KeyCode.A)
    //            rightHorizontalState = 0;
    //        }
    //    }

    //    // 右搖桿Y軸映射WS鍵
    //    if (yAxisValue != -999)
    //    {
    //        if (yAxisValue > sensitivityWSAD)
    //        {
    //            if (leftVerticalState == 1) Keybd_event(87, 0, 1, 0); // 等同於 Input.GetKey(KeyCode.W)
    //            else Keybd_event(87, 0, 0, 0); // 等同於 Input.GetKeyDown(KeyCode.W)
    //            leftVerticalState = 1;
    //        }
    //        else if (yAxisValue < -sensitivityWSAD)
    //        {
    //            if (leftVerticalState == -1) Keybd_event(83, 0, 1, 0); // 等同於 Input.GetKey(KeyCode.S)
    //            else Keybd_event(83, 0, 0, 0); // 等同於 Input.GetKeyDown(KeyCode.S)
    //            leftVerticalState = -1;
    //        }
    //        else
    //        {
    //            if (leftVerticalState == 1) Keybd_event(87, 0, 2, 0); // 等同於 Input.GetKeyUp(KeyCode.W)
    //            else if (leftVerticalState == -1) Keybd_event(83, 0, 2, 0); // 等同於 Input.GetKeyUp(KeyCode.S)
    //            leftVerticalState = 0;
    //        }
    //    }
    //}

    /* H key to switch */
    void UseSteeringWheel()
    {
        useSteeringWheel = !useSteeringWheel;
        PlayerPrefs.SetInt("saveSteeringWheel", useSteeringWheel ? 1 : 0);
    }
    //void BasicHandleButtonMapping()
    //{
    //    if (command.Button!= "Btn")
    //    {
    //        // Steering wheel mode 方向盤模式（H鍵映射）
    //        if (command.Button.Contains("SteeringWheel") && !stateSteeringWheel)
    //        {
    //            CustomMapping(72, 0);
    //            stateSteeringWheel = true;
    //        }
    //        else if (!command.Button.Contains("SteeringWheel") && stateSteeringWheel)
    //        {
    //            CustomMapping(72, 2);
    //            stateSteeringWheel = false;
    //        }
    //    }
    //}
    void FlyingHandleButtonMapping()
    {
        //if (controllerType == ControllerType.Controller)
        //    WakakaModeButtonMapping();
         if (controllerType == ControllerType.Xbox360)
        {
            // 開火（滑鼠左鍵映射）
            if (Input.GetKey(KeyCode.Joystick1Button5))
                CustomMapping((int)KeyCode.Mouse0, 1);

            // 鎖定（R鍵映射）
            if (Input.GetAxis("Joystick 3axis") > 0.7f)
                CustomMapping(82, 0);
            else
                CustomMapping(82, 2);

            // 後燃器、解除鎖定（M鍵映射）
            if (Input.GetKey(KeyCode.Joystick1Button4))
                CustomMapping(77, 1);
            if (Input.GetKeyDown(KeyCode.Joystick1Button4))
                CustomMapping(77, 0);
            else if (Input.GetKeyUp(KeyCode.Joystick1Button4))
                CustomMapping(77, 2);

            // 更換武器（滑鼠右鍵映射）
            if (Input.GetAxis("Joystick 3axis") < -0.7f)
                CustomMapping((int)KeyCode.Mouse1, 0);
            else
                CustomMapping((int)KeyCode.Mouse1, 2);


        }

    }
    void DrivingHandleButtonMapping()
    {
        //if (command.Button!= "Btn")
        //{
        //    // 推進（W鍵映射）
        //    if (command.Button.Contains("RightFire"))
        //        CustomMapping(87, 1);

        //    // 剎車（S鍵映射）
        //    if (command.Button.Contains("LeftFire"))
        //        CustomMapping(83, 1);

        //    // DRS（Q鍵映射）
        //    if (command.Button.Contains("LeftThumb") && !stateLeftThumb)
        //    {
        //        CustomMapping(81, 0);
        //        stateLeftThumb = true;
        //    }
        //    else if (!command.Button.Contains("LeftThumb") && stateLeftThumb)
        //    {
        //        CustomMapping(81, 2);
        //        stateLeftThumb = false;
        //    }
        //}
    }




    /* Auto Setting */
    //public static void ChangeType(ControllerType newType)
    //{
    //    type[(int)controllerType].isOn = false;
    //    controllerType = newType;
    //    PlayerPrefs.SetInt("saveType", (int)controllerType);
    //    type[(int)controllerType].isOn = true;
    //}
    //public static void ChangeMode(ControlMode newMode)
    //{
    //    mode[(int)controlMode].isOn = false;
    //    controlMode = newMode;
    //    PlayerPrefs.SetInt("saveMode", (int)controlMode);
    //    mode[(int)controlMode].isOn = true;
    //}
    //public static void ChangeHands(ControlHands newHands)
    //{
    //    hands[(int)controlHands].isOn = false;
    //    controlHands = newHands;
    //    PlayerPrefs.SetInt("saveHands", (int)controlHands);
    //    hands[(int)controlHands].isOn = true;
    //    HandsUpdate();
    //}
    /* Manual Setting */
    public void ChangeType(Toggle toggle)
    {
        controllerType = (ControllerType)Enum.Parse(typeof(ControllerType), toggle.name);
        PlayerPrefs.SetInt("saveType", (int)controllerType);
    }
    public void ChangeMode(Toggle toggle)
    {
        controlMode = (ControlMode)Enum.Parse(typeof(ControlMode), toggle.name);
        PlayerPrefs.SetInt("saveMode", (int)controlMode);
    }
    public void ChangeHands(Toggle toggle)
    {
        controlHands = (ControlHands)Enum.Parse(typeof(ControlHands), toggle.name);
        PlayerPrefs.SetInt("saveHands", (int)controlHands);
        HandsUpdate();
    }
    public static void HandsUpdate()
    {
        if (controlHands == ControlHands.AmericanHands)
        {
            indexRoll = 0;
            indexPitch = 1;
            indexYaw = 2;
            indexThrotte = 3;
        }
        else if (controlHands == ControlHands.JapaneseHands)
        {
            indexRoll = 0;
            indexPitch = 3;
            indexYaw = 2;
            indexThrotte = 1;
        }
        else if (controlHands == ControlHands.ChineseHands)
        {
            indexRoll = 2;
            indexPitch = 3;
            indexYaw = 0;
            indexThrotte = 1;
        }
    }

    /* Keys */
    public static bool GetKeyboardKeycode(int unityCode, out int keyCode, out string keyName)
    {
        KeyCode unityKeyCode = (KeyCode)unityCode;
        keyCode = 0;
        keyName = "";

        if (unityCode >= 48 && unityCode <= 57) // Alpha 數字
        {
            keyCode = unityCode;
            keyName = "" + (unityCode - 48);
            return true;
        }
        else if (unityCode >= 97 && unityCode <= 122) // 字母
        {
            keyCode = unityCode - 32;
            keyName = unityKeyCode.ToString();
            return true;
        }
        else if (unityCode >= 256 && unityCode <= 265) // 數字鍵盤
        {
            keyCode = unityCode - 160;
            keyName = "Num " + (unityCode - 256);
            return true;
        }
        else if (unityCode >= 282 && unityCode <= 293) // F1~F12
        {
            keyCode = unityCode - 170;
            keyName = unityKeyCode.ToString();
            return true;
        }
        else
        {
            switch (unityKeyCode)
            {
                case KeyCode.Backspace: keyCode = 8; keyName = "Backspace"; return true;
                case KeyCode.Tab: keyCode = 9; keyName = "Tab"; return true;
                case KeyCode.Clear: keyCode = 12; keyName = "Clear"; return true;
                case KeyCode.Return: keyCode = 13; keyName = "Enter"; return true;
                //
                case KeyCode.RightShift: keyCode = 16; keyName = "R-Shift"; return true;
                case KeyCode.LeftShift: keyCode = 16; keyName = "L-Shift"; return true;
                case KeyCode.RightControl: keyCode = 17; keyName = "R-Ctrl"; return true;
                case KeyCode.LeftControl: keyCode = 17; keyName = "L-Ctrl"; return true;
                case KeyCode.RightAlt: keyCode = 18; keyName = "R-Alt"; return true;
                case KeyCode.LeftAlt: keyCode = 18; keyName = "L-Alt"; return true;
                //
                case KeyCode.CapsLock: keyCode = 20; keyName = "CapsLock"; return true;
                case KeyCode.Pause: keyCode = 19; keyName = "Pause"; return true;
                case KeyCode.Escape: keyCode = 27; keyName = "Esc"; return true;
                case KeyCode.Space: keyCode = 32; keyName = "Space"; return true;
                case KeyCode.PageUp: keyCode = 33; keyName = "PageUp"; return true;
                case KeyCode.PageDown: keyCode = 34; keyName = "PageDown"; return true;
                case KeyCode.End: keyCode = 35; keyName = "End"; return true;
                case KeyCode.Home: keyCode = 36; keyName = "Home"; return true;
                // 方向键
                case KeyCode.LeftArrow: keyCode = 37; keyName = "Left"; return true;
                case KeyCode.UpArrow: keyCode = 38; keyName = "Up"; return true;
                case KeyCode.RightArrow: keyCode = 39; keyName = "Right"; return true;
                case KeyCode.DownArrow: keyCode = 40; keyName = "Down"; return true;
                // 
                case KeyCode.Insert: keyCode = 45; keyName = "Insert"; return true;
                case KeyCode.Delete: keyCode = 46; keyName = "Delete"; return true;
                // 數字 48 ~ 57
                // 字母 65 ~ 90
                // 數字鍵盤 96 ~ 104
                case KeyCode.KeypadMultiply: keyCode = 106; keyName = "Num *"; return true; // *
                case KeyCode.KeypadPlus: keyCode = 107; keyName = "Num +"; return true; // +
                case KeyCode.KeypadEnter: keyCode = 108; keyName = "Num Enter"; return true;
                case KeyCode.KeypadMinus: keyCode = 109; keyName = "Num -"; return true; // -
                case KeyCode.KeypadPeriod: keyCode = 110; keyName = "Num ."; return true; // . Del
                case KeyCode.KeypadDivide: keyCode = 111; keyName = "Num /"; return true; // /
                                                                                          // F1~F12鍵 112 ~ 123
                                                                                          // 符號
                case KeyCode.Semicolon: keyCode = 186; keyName = ";"; return true; // 分號 冒號
                case KeyCode.Equals: keyCode = 187; keyName = "="; return true; // 等於 加號
                case KeyCode.Comma: keyCode = 188; keyName = ","; return true; // 逗號 <
                case KeyCode.Minus: keyCode = 189; keyName = "-"; return true; // 減號 底線
                case KeyCode.Period: keyCode = 190; keyName = "."; return true; // 句號 >
                case KeyCode.Slash: keyCode = 191; keyName = "/"; return true; // 斜線 問號
                case KeyCode.BackQuote: keyCode = 192; keyName = "~"; return true; // 毛毛蟲~
                case KeyCode.LeftBracket: keyCode = 219; keyName = "["; return true; // 前引號
                case KeyCode.Backslash: keyCode = 220; keyName = "\\"; return true; // 反斜線 管線
                case KeyCode.RightBracket: keyCode = 221; keyName = "]"; return true; // 後引號
                case KeyCode.Quote: keyCode = 222; keyName = "'"; return true; // 英文引號
            }
        }
        return false;
    }











    void OLDAwake()
    {

        //if (Instance == null) Instance = this;
        //DontDestroyOnLoad(this);
        //SceneManager.LoadScene(PlayerPrefs.GetInt("MainScene"));
        //if (!Testing)
        //{
        //    if (!ArduinoController.Instance)
        //    {
        //        SceneManager.LoadScene("Arduino Controller");
        //        return;
        //    }
        //    else
        //    {
        //        ArduinoController.commandsCount = commandsCount; // 設定本專案單次接收Unity訊息的數量
        //        ArduinoController.msgQueueCombine = true;
        //        msgBox = ArduinoController.Instance.msgBox;
        //    }

        //    if (Instance == null) Instance = this;
        //    DontDestroyOnLoad(this);
        //    SceneManager.LoadScene(PlayerPrefs.GetInt("MainScene"));
        //}
        //else
        //{
        //    if (!ArduinoController.Instance)
        //    {
        //        PlayerPrefs.SetInt("MainScene", SceneManager.GetActiveScene().buildIndex);
        //        SceneManager.LoadScene("Arduino Controller");
        //        return;
        //    }
        //    else
        //    {
        //        ArduinoController.commandsCount = commandsCount; // 設定本專案單次接收Unity訊息的數量
        //        ArduinoController.msgQueueCombine = true;
        //        msgBox = ArduinoController.Instance.msgBox;
        //    }

        //    if (Instance == null) Instance = this;
        //}

        //showFPS = PlayerPrefs.GetInt("Open FPS") == 1 ? true : false;
        //toggleFPS.isOn = showFPS;

        //// Initialize
        //int loadType = PlayerPrefs.GetInt("saveType");
        //int loadMode = PlayerPrefs.GetInt("saveMode");
        //int loadHands = PlayerPrefs.GetInt("saveHands");
        //controllerType = (ControllerType)loadType;
        //panelHotkey.SetActive(false);

        //// Old
        //type = groupType.GetComponentsInChildren<Toggle>();
        //type[loadType].isOn = true;
        //mode = groupMode.GetComponentsInChildren<Toggle>();
        //mode[loadMode].isOn = true;
        //hands = groupHands.GetComponentsInChildren<Toggle>();
        //hands[loadHands].isOn = true;
        //panelSetting.SetActive(false);

        //values = (int[])System.Enum.GetValues(typeof(KeyCode));
        ////InitializeHotkey();
        //// Xbox 360 Setting
        //InitializeXbox360Setting();
    }


}



public enum ControllerType
{
    MouseKeyboard = 0,
    Xbox360 = 1,
    WakakaFlyingMode = 2,
}
public enum ControlMode
{
    Disable = -1,
    General = 0,
    Flying = 1,
    Driving = 2,
    Custom = 3,
    Xbox360 = 10,
}
public enum ControlHands
{
    AmericanHands = 0,
    JapaneseHands = 1,
    ChineseHands = 2,
}

public class AxisData
{
    public float Value;
    public AxisButton Plus;
    public AxisButton Minus;
}

public class AxisButton
{
    public bool Pressed;
    public int Keycode;
}

