/***************************************************************************
 * Controller - Mouse & Keyboard
 * 控制器 - 滑鼠鍵盤
 * Last Updated: 2018/11/30
 * Description:
 * 1. 
 ***************************************************************************/
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public partial class Controller : MonoBehaviour
{
    [Header("Mouse & Keyboard")]
    public Toggle tabMouseKeyboard2;
    public GameObject panelMouseKeyboard2;
    [Header("Mouse & Keyboard - FunctionKey")]
    public Toggle[] hotkeyFunction;
    public static FunctionKey Operation = new FunctionKey("FunctionKey-Operation");
    public static FunctionKey Hangar = new FunctionKey("FunctionKey-Hangar");
    public static FunctionKey Vocal = new FunctionKey("FunctionKey-Vocal");
    public static FunctionKey WhoAttackU = new FunctionKey("FunctionKey-WhoAttackU");
    public static FunctionKey Respawn = new FunctionKey("FunctionKey-Respawn");
    public static FunctionKey CockpitView = new FunctionKey("FunctionKey-CockpitView");
    public static FunctionKey Afterburner = new FunctionKey("FunctionKey-Afterburner");
    public static FunctionKey LockOn = new FunctionKey("FunctionKey-LockOn");
    public static FunctionKey Laser = new FunctionKey("FunctionKey-Laser");
    public static FunctionKey Rocket = new FunctionKey("FunctionKey-Rocket");
    public static FunctionKey Missile = new FunctionKey("FunctionKey-Missile");
    public static FunctionKey ActiveKey;

    void InitializeMouseKeyboard()
    {
        tabMouseKeyboard2.onValueChanged.AddListener(isOn => 
        {
            panelMouseKeyboard2.SetActive(isOn);
            controllerType = ControllerType.MouseAndKeyboard;
            PlayerPrefs.SetInt("saveType", (int)controllerType);
        });
        if (controllerType == ControllerType.MouseAndKeyboard) tabMouseKeyboard2.isOn = true;
        hotkeyFunction = panelMouseKeyboard2.GetComponentsInChildren<Toggle>();
        Operation.InitializeHotkey(hotkeyFunction[0], (int)KeyCode.O);
        Hangar.InitializeHotkey(hotkeyFunction[1], (int)KeyCode.H);
        Vocal.InitializeHotkey(hotkeyFunction[2], (int)KeyCode.Space);
        WhoAttackU.InitializeHotkey(hotkeyFunction[3], (int)KeyCode.Return); //KeyCode.Joystick1Button2
        Respawn.InitializeHotkey(hotkeyFunction[4], (int)KeyCode.Backspace); // KeyCode.Joystick1Button3
        CockpitView.InitializeHotkey(hotkeyFunction[5], (int)KeyCode.C); // KeyCode.Joystick1Button0
        Afterburner.InitializeHotkey(hotkeyFunction[6], (int)KeyCode.LeftShift);
        LockOn.InitializeHotkey(hotkeyFunction[7], (int)KeyCode.R);
        Laser.InitializeHotkey(hotkeyFunction[8], (int)KeyCode.Mouse0);
        Rocket.InitializeHotkey(hotkeyFunction[9], (int)KeyCode.Space);
        Missile.InitializeHotkey(hotkeyFunction[10], (int)KeyCode.Mouse1);
    }

    void MouseKeyboardHotkeySetting(int numberCode)
    {
        ActiveKey.SetHotkey(numberCode);
    }
}

public class FunctionKey : IHotkey
{
    public readonly string PrefKeySetting; // PlayerPrefs name
    public KeyCode KeyCode; // Unity keycode
    public string KeyName; // Keyboard key name or maker
    public Toggle Hotkey;
    public TextMeshProUGUI HotkeyName;

    public FunctionKey(string pref)
    {
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
                Controller.ActiveKey = this;
                Controller.state = PanelState.HotkeyInput;
            }
            else
            {
                Controller.state = PanelState.Ready;
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
        KeyCode = (KeyCode)unityCode;
        KeyName = "";

        if (unityCode >= 48 && unityCode <= 57) // Alpha 數字
        {
            KeyName = "#" + (unityCode - 48);
            return true;
        }
        else if (unityCode >= 97 && unityCode <= 122) // 字母
        {
            KeyName = KeyCode.ToString();
            return true;
        }
        else if (unityCode >= 256 && unityCode <= 265) // 數字鍵盤
        {
            KeyName = "Num " + (unityCode - 256);
            return true;
        }
        else if (unityCode >= 277 && unityCode <= 296) // 中央功能鍵 + F1~F12
        {
            KeyName = KeyCode.ToString();
            return true;
        }
        else
        {
            switch (KeyCode)
            {
                case KeyCode.Mouse0: KeyName = "LClick"; return true;
                case KeyCode.Mouse1: KeyName = "RClick"; return true;
                case KeyCode.Mouse2: KeyName = "Wheel"; return true;

                case KeyCode.Backspace: KeyName = KeyCode.ToString(); return true;
                case KeyCode.Tab: KeyName = KeyCode.ToString(); return true;
                case KeyCode.Clear: KeyName = KeyCode.ToString(); return true;
                case KeyCode.Return: KeyName = "Enter"; return true;

                case KeyCode.RightShift: KeyName = "RShift"; return true;
                case KeyCode.LeftShift: KeyName = "LShift"; return true;
                case KeyCode.RightControl: KeyName = "RCtrl"; return true;
                case KeyCode.LeftControl: KeyName = "LCtrl"; return true;
                case KeyCode.RightAlt: KeyName = "RAlt"; return true;
                case KeyCode.LeftAlt: KeyName = "LAlt"; return true;
                case KeyCode.AltGr: KeyName = "RAlt"; return true;

                case KeyCode.CapsLock: KeyName = KeyCode.ToString(); return true;
                case KeyCode.Pause: KeyName = KeyCode.ToString(); return true;
                case KeyCode.Escape: KeyName = "Esc"; return true;
                case KeyCode.Space: KeyName = KeyCode.ToString(); return true;
                case KeyCode.Delete: KeyName = KeyCode.ToString(); return true;

                // 方向键 273~276
                case KeyCode.UpArrow: KeyName = "Up"; return true;
                case KeyCode.DownArrow: KeyName = "Down"; return true;
                case KeyCode.RightArrow: KeyName = "Right"; return true;
                case KeyCode.LeftArrow: KeyName = "Left"; return true;
                // 數字鍵盤 266~271
                case KeyCode.KeypadPeriod: KeyName = "Num ."; return true; // . Del
                case KeyCode.KeypadDivide: KeyName = "Num /"; return true; // /
                case KeyCode.KeypadMultiply: KeyName = "Num *"; return true; // *
                case KeyCode.KeypadMinus: KeyName = "Num -"; return true; // -
                case KeyCode.KeypadPlus: KeyName = "Num +"; return true; // +
                case KeyCode.KeypadEnter: KeyName = "Num Enter"; return true;
                // 符號
                case KeyCode.Quote: KeyName = "'"; return true; // 英文引號
                case KeyCode.Comma: KeyName = ","; return true; // 逗號 <
                case KeyCode.Minus: KeyName = "-"; return true; // 減號 底線
                case KeyCode.Period: KeyName = "."; return true; // 句號 >
                case KeyCode.Slash: KeyName = "/"; return true; // 斜線 問號
                case KeyCode.Semicolon: KeyName = ";"; return true; // 分號 冒號
                case KeyCode.Equals: KeyName = "="; return true; // 等於 加號
                case KeyCode.LeftBracket: KeyName = "["; return true; // 前引號
                case KeyCode.Backslash: KeyName = "\\"; return true; // 反斜線 管線
                case KeyCode.RightBracket: KeyName = "]"; return true; // 後引號
                case KeyCode.BackQuote: KeyName = "~"; return true; // 毛毛蟲~
            }
        }
        return false;
    }
}