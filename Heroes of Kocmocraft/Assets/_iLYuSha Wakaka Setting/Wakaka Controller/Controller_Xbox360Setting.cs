/***************************************************************************
 * Wakaka Controller - Xbox 360 Setting
 * Xbox 360 搖桿設定
 * Last Updated: 2018/11/07
 * Description:
 * 1. Reference: http://wiki.unity3d.com/index.php/Xbox360Controller
 ***************************************************************************/
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public partial class Controller : MonoBehaviour
{
    private static readonly float XBox360AxisSensitivity = 0.7f;

    public static void UseXbox360()
    {
        controllerType = ControllerType.Xbox360;
        PlayerPrefs.SetInt(PREFS_CONTROLLER_TYPE, (int)controllerType);
    }

    private void Xbox360AxisInput()
    {
        controlValue[0] = Input.GetAxis("Xbox360 Horizontal"); // 右搖桿水平軸，需要額外在Input Manager設定
        controlValue[1] = Input.GetAxis("Xbox360 Vertical"); // 右搖桿垂直軸，需要額外在Input Manager設定
        controlValue[2] = Input.GetAxis("Horizontal"); // 左搖桿水平軸，直接映射左右方向鍵（Xbox360原始設定）
        controlValue[3] = Input.GetAxis("Vertical"); // 左搖桿垂直軸，直接映射上下方向鍵（Xbox360原始設定）

        // 飛行輸入值
        roll = Mathf.Clamp(controlValue[indexRoll] * XBox360AxisSensitivity, -1, 1);
        pitch = Mathf.Clamp(controlValue[indexPitch] * XBox360AxisSensitivity, -1, 1);
        yaw = Mathf.Clamp(controlValue[indexYaw] * XBox360AxisSensitivity, -1, 1);
        throttle = Mathf.Clamp(controlValue[indexThrotte] * XBox360AxisSensitivity, -1, 1);

        axisLevel = 0.7f;
    }























    private int[] values;
    private int[] keysXbox360 = new int[10];
    [Header("Xbox 360 Setting")]
    public Toggle[] toggleXboxButton;
    public TextMeshProUGUI[] textKeyName;
    // Input
    private int indexXbox360Button;
    private int inputKeyCode;
    private string inputKeyName;

    void InitializeXbox360Setting()
    {
        for (int i = 0; i < 10; i++)
        {
            indexXbox360Button = i;
            Xbox360KeyboardMapping(PlayerPrefs.GetInt(Xbox360_UnityKeyCode[indexXbox360Button]));
        }
    }
    public void SetKey(int index)
    {
        indexXbox360Button = index;
        //state = toggleXboxButton[indexXbox360Button].isOn ? PanelState.Xbox360Setting : PanelState.Ready;
    }

    void Xbox360KeyboardMapping(int unityCode)
    {
        if (GetKeyboardKeycode(unityCode, out inputKeyCode, out inputKeyName))
        {
            PlayerPrefs.SetInt(Xbox360_UnityKeyCode[indexXbox360Button], unityCode);
            PlayerPrefs.SetInt(Xbox360_Keyboard[indexXbox360Button], inputKeyCode);
            keysXbox360[indexXbox360Button] = PlayerPrefs.GetInt(Xbox360_Keyboard[indexXbox360Button]);
            textKeyName[indexXbox360Button].text = inputKeyName;
            toggleXboxButton[indexXbox360Button].isOn = false;
            //state = PanelState.Ready;
        }
    }

    void Xbox360JoystickInput()
    {
        controlValue[0] = Input.GetAxis("Joystick Horizontal"); // 右搖桿水平軸，需要額外在Input Manager設定
        controlValue[1] = Input.GetAxis("Joystick Vertical"); // 右搖桿垂直軸，需要額外在Input Manager設定
        controlValue[2] = Input.GetAxis("Horizontal"); // 左搖桿水平軸，直接映射左右方向鍵（Xbox360原始設定）
        controlValue[3] = Input.GetAxis("Vertical"); // 左搖桿垂直軸，直接映射上下方向鍵（Xbox360原始設定）
        // 右搖桿映射WSAD鍵
        sensitivityWSAD = 0.7f; // 靈敏度
        //xAxisValue = controlValue[0];
        //yAxisValue = controlValue[1];
    }

    void Xbox360ButtonMapping()
    {
        // A Button (0)
        if (Input.GetKey(KeyCode.Joystick1Button0))
            CustomMapping(keysXbox360[0], 1);
        if (Input.GetKeyDown(KeyCode.Joystick1Button0))
            CustomMapping(keysXbox360[0], 0);
        else if (Input.GetKeyUp(KeyCode.Joystick1Button0))
            CustomMapping(keysXbox360[0], 2);
        // B Button (1)
        if (Input.GetKey(KeyCode.Joystick1Button1))
            CustomMapping(keysXbox360[1], 1);
        if (Input.GetKeyDown(KeyCode.Joystick1Button1))
            CustomMapping(keysXbox360[1], 0);
        else if (Input.GetKeyUp(KeyCode.Joystick1Button1))
            CustomMapping(keysXbox360[1], 2);
        // X Button (2)
        if (Input.GetKey(KeyCode.Joystick1Button2))
            CustomMapping(keysXbox360[2], 1);
        if (Input.GetKeyDown(KeyCode.Joystick1Button2))
            CustomMapping(keysXbox360[2], 0);
        else if (Input.GetKeyUp(KeyCode.Joystick1Button2))
            CustomMapping(keysXbox360[2], 2);
        // Y Button (3)
        if (Input.GetKey(KeyCode.Joystick1Button3))
            CustomMapping(keysXbox360[3], 1);
        if (Input.GetKeyDown(KeyCode.Joystick1Button3))
            CustomMapping(keysXbox360[3], 0);
        else if (Input.GetKeyUp(KeyCode.Joystick1Button3))
            CustomMapping(keysXbox360[3], 2);
        // Left Bumper (4)
        if (Input.GetKey(KeyCode.Joystick1Button4))
            CustomMapping(keysXbox360[4], 1);
        if (Input.GetKeyDown(KeyCode.Joystick1Button4))
            CustomMapping(keysXbox360[4], 0);
        else if (Input.GetKeyUp(KeyCode.Joystick1Button4))
            CustomMapping(keysXbox360[4], 2);
        // Right Bumper (5)
        if (Input.GetKey(KeyCode.Joystick1Button5))
            CustomMapping(keysXbox360[5], 1);
        if (Input.GetKeyDown(KeyCode.Joystick1Button5))
            CustomMapping(keysXbox360[5], 0);
        else if (Input.GetKeyUp(KeyCode.Joystick1Button5))
            CustomMapping(keysXbox360[5], 2);
        // Back Button (6)
        if (Input.GetKey(KeyCode.Joystick1Button6))
            CustomMapping(keysXbox360[6], 1);
        if (Input.GetKeyDown(KeyCode.Joystick1Button6))
            CustomMapping(keysXbox360[6], 0);
        else if (Input.GetKeyUp(KeyCode.Joystick1Button6))
            CustomMapping(keysXbox360[6], 2);
        // Start Button (7)
        if (Input.GetKey(KeyCode.Joystick1Button7))
            CustomMapping(keysXbox360[7], 1);
        if (Input.GetKeyDown(KeyCode.Joystick1Button7))
            CustomMapping(keysXbox360[7], 0);
        else if (Input.GetKeyUp(KeyCode.Joystick1Button7))
            CustomMapping(keysXbox360[7], 2);
        // Left Stick Click (8)
        if (Input.GetKey(KeyCode.Joystick1Button8))
            CustomMapping(keysXbox360[8], 1);
        if (Input.GetKeyDown(KeyCode.Joystick1Button8))
            CustomMapping(keysXbox360[8], 0);
        else if (Input.GetKeyUp(KeyCode.Joystick1Button8))
            CustomMapping(keysXbox360[8], 2);
        // Right Stick Click (9)
        if (Input.GetKey(KeyCode.Joystick1Button9))
            CustomMapping(keysXbox360[9], 1);
        if (Input.GetKeyDown(KeyCode.Joystick1Button9))
            CustomMapping(keysXbox360[9], 0);
        else if (Input.GetKeyUp(KeyCode.Joystick1Button9))
            CustomMapping(keysXbox360[9], 2);
    }

    public static readonly string[] Xbox360_UnityKeyCode = new string[]
{
        "Xbox360_UnityKeyCode - A Button",
        "Xbox360_UnityKeyCode - B Button",
        "Xbox360_UnityKeyCode - X Buttonr",
        "Xbox360_UnityKeyCode - Y Button",
        "Xbox360_UnityKeyCode - Left Bumper",
        "Xbox360_UnityKeyCode - Right Bumper",
        "Xbox360_UnityKeyCode - Back Button",
        "Xbox360_UnityKeyCode - Start Button",
        "Xbox360_UnityKeyCode - Left Stick Click",
        "Xbox360_UnityKeyCode - Right Stick Click"
};
    public static readonly string[] Xbox360_Keyboard = new string[]
{
        "Xbox360_Keyboard - A Button",
        "Xbox360_Keyboard - B Button",
        "Xbox360_Keyboard - X Buttonr",
        "Xbox360_Keyboard - Y Button",
        "Xbox360_Keyboard - Left Bumper",
        "Xbox360_Keyboard - Right Bumper",
        "Xbox360_Keyboard - Back Button",
        "Xbox360_Keyboard - Start Button",
        "Xbox360_Keyboard - Left Stick Click",
        "Xbox360_Keyboard - Right Stick Click"
};
}
