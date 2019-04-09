/***************************************************************************
 * Controller - Wakaka Flying Mode
 * 控制器 - 哇咔咔飛行模式
 * Last Updated: 2018/11/18
 * Description:
 * 1. 
 ***************************************************************************/
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public partial class Controller : MonoBehaviour
{
    // Event
    public delegate void HotKeyChanged();
    public static event HotKeyChanged OnHotKeyChanged;
    public static float FlyingAxisSensitivity, sensitivityFlyingKey = 0.7f;
    // Flying Axis
    public static FlyingAxis[] FlyingAxis = new FlyingAxis[4]
    {
        new FlyingAxis{Plus = new WakakaButton("", "", "WakakaButton-X+"),Minus = new WakakaButton("", "", "WakakaButton-X-")},
        new FlyingAxis{Plus = new WakakaButton("", "", "WakakaButton-Y+"),Minus = new WakakaButton("", "", "WakakaButton-Y-")},
        new FlyingAxis{Plus = new WakakaButton("", "", "WakakaButton-H+"),Minus = new WakakaButton("", "", "WakakaButton-H-")},
        new FlyingAxis{Plus = new WakakaButton("", "", "WakakaButton-V+"),Minus = new WakakaButton("", "", "WakakaButton-V-")}
    };
    // Wakaka Button
    public static WakakaButton JoystickRoot = new WakakaButton("RS", "rs", "WakakaButton-JoystickRoot");
    public static WakakaButton JoystickThumb = new WakakaButton("RB", "rb", "WakakaButton-JoystickThumb");
    public static WakakaButton JoystickIndex = new WakakaButton("RT", "rt", "WakakaButton-JoystickIndex");
    public static WakakaButton Back = new WakakaButton("BACK", "back", "WakakaButton-Back");
    public static WakakaButton Stop = new WakakaButton("STOP", "stop", "WakakaButton-Stop");
    public static WakakaButton Play = new WakakaButton("PLAY", "play", "WakakaButton-Play");
    public static WakakaButton ThrustLeverRoot = new WakakaButton("LS", "ls", "WakakaButton-ThrustLeverRoot");
    public static WakakaButton ThrustLeverThumb = new WakakaButton("LB", "lb", "WakakaButton-ThrustLeverThumb");
    public static WakakaButton ThrustLeverIndex = new WakakaButton("LT", "lt", "WakakaButton-ThrustLeverIndex");
    public static WakakaButton ThrustLeverMiddle = new WakakaButton("LM", "lm", "WakakaButton-ThrustLeverMiddle");
    public static WakakaButton ThrustLeverExtension = new WakakaButton("LE", "le", "WakakaButton-ThrustLeverExtension");
    public static WakakaButton ActiveWakakaButton;
    // Prefs
    private static readonly string PREFS_FLYING_AXIS_SENSITIVITY = "Prefs_FlyingAxis_Sensitivity";

    public static void InitializeWakakaModeData()
    {
        FlyingAxisSensitivity = PlayerPrefs.GetFloat(PREFS_FLYING_AXIS_SENSITIVITY);
        if (FlyingAxisSensitivity == 0) FlyingAxisSensitivity = 0.37f;
    }
    public static void UseWakakaFlyingMode()
    {
        controllerType = ControllerType.WakakaFlyingMode;
        PlayerPrefs.SetInt("Controller_Type", (int)controllerType);
    }
    public static void SetFlyingAxisSensitivity(float value)
    {
        FlyingAxisSensitivity = value;
        PlayerPrefs.SetFloat(PREFS_FLYING_AXIS_SENSITIVITY, FlyingAxisSensitivity);
    }
    public static void WakakaFlyingModeSetting(int numberCode)
    {
        ActiveWakakaButton.SetHotkey(numberCode);
    }

    private void WakakaFlyingAxisInput()
    {
        // -999為這一幀沒收到訊號，使用前一幀數值
        if (command.AxisX == -999) command.AxisX = controlValue[0];
        if (command.AxisY == -999) command.AxisY = controlValue[1];
        if (command.AxisH == -999) command.AxisH = controlValue[2];
        if (command.AxisV == -999) command.AxisV = controlValue[3];
        //virtualJoystick.localPosition = new Vector3(command.AxisX * 200, -command.AxisY * 200, 0);

        // 控制值
        if (controlMode == ControlMode.Flying)
        {
            controlValue[0] = command.AxisX == 0 ? Input.GetAxis("Mouse X") : command.AxisX;
            controlValue[1] = command.AxisY == 0 ? Input.GetAxis("Mouse Y") : command.AxisY;
            controlValue[2] = command.AxisH == 0 ? Input.GetAxis("Horizontal") : command.AxisH;
            controlValue[3] = command.AxisV == 0 ? Input.GetAxis("Vertical") : command.AxisV;
        }

        // 飛行輸入值
        roll = Mathf.Clamp(controlValue[indexRoll] * FlyingAxisSensitivity, -1, 1);
        pitch = Mathf.Clamp(controlValue[indexPitch] * FlyingAxisSensitivity, -1, 1);
        yaw = Mathf.Clamp(controlValue[indexYaw] * FlyingAxisSensitivity, -1, 1);
        throttle = Mathf.Clamp(controlValue[indexThrotte] * FlyingAxisSensitivity, -1, 1);

        axisLevel = 0.7f;


        // 軸映射鍵值
        for (int i = 0; i < 4; i++)
        {
            FlyingAxis[i].Value = controlValue[i];
            if (FlyingAxis[i].Value > sensitivityFlyingKey)
            {
                FlyingAxis[i].Plus.Pressed = true;
                MappingEvent(FlyingAxis[i].Plus.Keycode, 0);
            }
            else if (FlyingAxis[i].Value < -sensitivityFlyingKey)
            {
                FlyingAxis[i].Minus.Pressed = true;
                MappingEvent(FlyingAxis[i].Minus.Keycode, 0);
            }
            else
            {

                if (FlyingAxis[i].Plus.Pressed)
                {
                    FlyingAxis[i].Plus.Pressed = false;
                    MappingEvent(FlyingAxis[i].Plus.Keycode, 2);
                }
                if (FlyingAxis[i].Minus.Pressed)
                {
                    FlyingAxis[i].Minus.Pressed = false;
                    MappingEvent(FlyingAxis[i].Minus.Keycode, 2);
                }
            }
        }
    }

    private void WakakaModeButtonMapping()
    {
        if (command.Button == "Btn") return;

        if (command.Button.Contains(JoystickRoot.Press))
        {
            JoystickRoot.Pressed = true;
            MappingEvent(JoystickRoot.Keycode, 0);
        }
        else if (command.Button.Contains(JoystickRoot.Up))
        {
            if (JoystickRoot.Pressed)
            {
                JoystickRoot.Pressed = false;
                MappingEvent(JoystickRoot.Keycode, 2);
            }
        }

        if (command.Button.Contains(JoystickThumb.Press))
        {
            JoystickThumb.Pressed = true;
            MappingEvent(JoystickThumb.Keycode, 0);
        }
        else if (command.Button.Contains(JoystickThumb.Up))
        {
            if (JoystickThumb.Pressed)
            {
                JoystickThumb.Pressed = false;
                MappingEvent(JoystickThumb.Keycode, 2);
            }
        }

        if (command.Button.Contains(JoystickIndex.Press))
        {
            JoystickIndex.Pressed = true;
            MappingEvent(JoystickIndex.Keycode, 0);
        }
        else if (command.Button.Contains(JoystickIndex.Up))
        {
            if (JoystickIndex.Pressed)
            {
                JoystickIndex.Pressed = false;
                MappingEvent(JoystickIndex.Keycode, 2);
            }
        }

        if (command.Button.Contains(ThrustLeverRoot.Press))
        {
            ThrustLeverRoot.Pressed = true;
            MappingEvent(ThrustLeverRoot.Keycode, 0);
        }
        else if (command.Button.Contains(ThrustLeverRoot.Up))
        {
            if (ThrustLeverRoot.Pressed)
            {
                ThrustLeverRoot.Pressed = false;
                MappingEvent(ThrustLeverRoot.Keycode, 2);
            }
        }

        if (command.Button.Contains(ThrustLeverThumb.Press))
        {
            ThrustLeverThumb.Pressed = true;
            MappingEvent(ThrustLeverThumb.Keycode, 0);
        }
        else if (command.Button.Contains(ThrustLeverThumb.Up))
        {
            if (ThrustLeverThumb.Pressed)
            {
                ThrustLeverThumb.Pressed = false;
                MappingEvent(ThrustLeverThumb.Keycode, 2);
            }
        }

        if (command.Button.Contains(ThrustLeverIndex.Press))
        {
            ThrustLeverIndex.Pressed = true;
            MappingEvent(ThrustLeverIndex.Keycode, 0);
        }
        else if (command.Button.Contains(ThrustLeverIndex.Up))
        {
            if (ThrustLeverIndex.Pressed)
            {
                ThrustLeverIndex.Pressed = false;
                MappingEvent(ThrustLeverIndex.Keycode, 2);
            }
        }

        if (command.Button.Contains(ThrustLeverMiddle.Press))
        {
            ThrustLeverMiddle.Pressed = true;
            MappingEvent(ThrustLeverMiddle.Keycode, 0);
        }
        else if (command.Button.Contains(ThrustLeverMiddle.Up))
        {
            if (ThrustLeverMiddle.Pressed)
            {
                ThrustLeverMiddle.Pressed = false;
                MappingEvent(ThrustLeverMiddle.Keycode, 2);
            }
        }

        if (command.Button.Contains(ThrustLeverExtension.Press))
        {
            ThrustLeverExtension.Pressed = true;
            MappingEvent(ThrustLeverExtension.Keycode, 0);
        }
        else if (command.Button.Contains(ThrustLeverExtension.Up))
        {
            if (ThrustLeverExtension.Pressed)
            {
                ThrustLeverExtension.Pressed = false;
                MappingEvent(ThrustLeverExtension.Keycode, 2);
            }
        }
    }
}

public class FlyingAxis
{
    public float Value;
    public WakakaButton Plus;
    public WakakaButton Minus;
}

internal interface IHotkey
{
    void InitializeHotkey(Toggle hotkey, int unityCode);
    void SetHotkey(int unityCode);
    bool KeyIsExist(int unityCode);
}

public class WakakaButton: IHotkey
{
    public readonly string Press;
    public readonly string Up;
    public readonly string PrefKeySetting; // PlayerPrefs name
    public int Keycode; // Keyboard keycode
    public string KeyName; // Keyboard key name or maker
    public bool Pressed;
    public Toggle Hotkey;
    public TextMeshProUGUI HotkeyName;

    public WakakaButton(string press, string up, string pref)
    {
        Press = press;
        Up = up;
        PrefKeySetting = pref;
    }

    public void InitializeHotkey(Toggle hotkey, int unityCode)
    {
        Hotkey = hotkey;
        HotkeyName = Hotkey.GetComponentInChildren<TextMeshProUGUI>();
        int prefValue = PlayerPrefs.GetInt(PrefKeySetting);
        SetHotkey(prefValue == 0 ? unityCode : prefValue);

        Hotkey.onValueChanged.AddListener(isOn =>
        {
            if (isOn)
            {
                Controller.ActiveWakakaButton = this;
                //Controller.state = PanelState.HotkeyInput;
            }
            else
            {
                //Controller.state = PanelState.Ready;
            }
        });
    }

    public void SetHotkey(int unityCode)
    {
        if (KeyIsExist(unityCode))
        {
            PlayerPrefs.SetInt(PrefKeySetting, unityCode); // 儲存Unity鍵碼
            HotkeyName.text = KeyName;
        }
        Hotkey.isOn = false;
    }

    public bool KeyIsExist(int unityCode)
    {
        KeyCode unityKeyCode = (KeyCode)unityCode;
        Keycode = 0;
        KeyName = "";

        if (unityCode >= 48 && unityCode <= 57) // Alpha 數字
        {
            Keycode = unityCode;
            KeyName = "#" + (unityCode - 48);
            return true;
        }
        else if (unityCode >= 97 && unityCode <= 122) // 字母
        {
            Keycode = unityCode - 32;
            KeyName = unityKeyCode.ToString();
            return true;
        }
        else if (unityCode >= 256 && unityCode <= 265) // 數字鍵盤 96~105
        {
            Keycode = unityCode - 160;
            KeyName = "Num " + (unityCode - 256);
            return true;
        }
        else if (unityCode >= 282 && unityCode <= 293) // F1~F12
        {
            Keycode = unityCode - 170;
            KeyName = unityKeyCode.ToString();
            return true;
        }
        else
        {
            switch (unityKeyCode)
            {
                case KeyCode.Mouse0: Keycode = 323; KeyName = "LClick"; return true;
                case KeyCode.Mouse1: Keycode = 324; KeyName = "RClick"; return true;
                case KeyCode.Mouse2: Keycode = 325; KeyName = "Wheel"; return true;

                case KeyCode.Backspace: Keycode = 8; KeyName = unityKeyCode.ToString(); return true;
                case KeyCode.Tab: Keycode = 9; KeyName = unityKeyCode.ToString(); return true;
                case KeyCode.Clear: Keycode = 12; KeyName = unityKeyCode.ToString(); return true;
                case KeyCode.Return: Keycode = 13; KeyName = "Enter"; return true;

                case KeyCode.RightShift: Keycode = 16; KeyName = "RShift"; return true;
                case KeyCode.LeftShift: Keycode = 16; KeyName = "LShift"; return true;
                case KeyCode.RightControl: Keycode = 17; KeyName = "RCtrl"; return true;
                case KeyCode.LeftControl: Keycode = 17; KeyName = "LCtrl"; return true;
                case KeyCode.RightAlt: Keycode = 18; KeyName = "RAlt"; return true;
                case KeyCode.LeftAlt: Keycode = 18; KeyName = "LAlt"; return true;
                case KeyCode.AltGr: Keycode = 18; KeyName = "RAlt"; return true;

                case KeyCode.CapsLock: Keycode = 20; KeyName = unityKeyCode.ToString(); return true;
                case KeyCode.Pause: Keycode = 19; KeyName = unityKeyCode.ToString(); return true;
                case KeyCode.Escape: Keycode = 27; KeyName = "Esc"; return true;
                case KeyCode.Space: Keycode = 32; KeyName = unityKeyCode.ToString(); return true;
                case KeyCode.PageUp: Keycode = 33; KeyName = unityKeyCode.ToString(); return true;
                case KeyCode.PageDown: Keycode = 34; KeyName = unityKeyCode.ToString(); return true;
                case KeyCode.End: Keycode = 35; KeyName = unityKeyCode.ToString(); return true;
                case KeyCode.Home: Keycode = 36; KeyName = unityKeyCode.ToString(); return true;
                // 方向键
                case KeyCode.LeftArrow: Keycode = 37; KeyName = "Left"; return true;
                case KeyCode.UpArrow: Keycode = 38; KeyName = "Up"; return true;
                case KeyCode.RightArrow: Keycode = 39; KeyName = "Right"; return true;
                case KeyCode.DownArrow: Keycode = 40; KeyName = "Down"; return true;
                // 
                case KeyCode.Insert: Keycode = 45; KeyName = "Insert"; return true;
                case KeyCode.Delete: Keycode = 46; KeyName = "Delete"; return true;
                // 數字 48 ~ 57
                // 字母 65 ~ 90
                // 數字鍵盤 96 ~ 104
                case KeyCode.KeypadMultiply: Keycode = 106; KeyName = "Num *"; return true; // *
                case KeyCode.KeypadPlus: Keycode = 107; KeyName = "Num +"; return true; // +
                case KeyCode.KeypadEnter: Keycode = 108; KeyName = "Num Enter"; return true;
                case KeyCode.KeypadMinus: Keycode = 109; KeyName = "Num -"; return true; // -
                case KeyCode.KeypadPeriod: Keycode = 110; KeyName = "Num ."; return true; // . Del
                case KeyCode.KeypadDivide: Keycode = 111; KeyName = "Num /"; return true; // /
                // F1~F12鍵 112 ~ 123
                // 符號
                case KeyCode.Semicolon: Keycode = 186; KeyName = ";"; return true; // 分號 冒號
                case KeyCode.Equals: Keycode = 187; KeyName = "="; return true; // 等於 加號
                case KeyCode.Comma: Keycode = 188; KeyName = ","; return true; // 逗號 <
                case KeyCode.Minus: Keycode = 189; KeyName = "-"; return true; // 減號 底線
                case KeyCode.Period: Keycode = 190; KeyName = "."; return true; // 句號 >
                case KeyCode.Slash: Keycode = 191; KeyName = "/"; return true; // 斜線 問號
                case KeyCode.BackQuote: Keycode = 192; KeyName = "~"; return true; // 毛毛蟲~
                case KeyCode.LeftBracket: Keycode = 219; KeyName = "["; return true; // 前引號
                case KeyCode.Backslash: Keycode = 220; KeyName = "\\"; return true; // 反斜線 管線
                case KeyCode.RightBracket: Keycode = 221; KeyName = "]"; return true; // 後引號
                case KeyCode.Quote: Keycode = 222; KeyName = "'"; return true; // 英文引號
            }
        }
        return false;
    }
}