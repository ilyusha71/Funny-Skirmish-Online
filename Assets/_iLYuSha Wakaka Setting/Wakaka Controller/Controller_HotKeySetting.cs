/***************************************************************************
 * Wakaka Controller - Hotkey Setting
 * 熱鍵設定
 * Last Updated: 2018/11/10
 * Description:
 * 1. 
 ***************************************************************************/
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public partial class Controller : MonoBehaviour
{
    // Key Setting - Lobby
    public static readonly KeyCode KEY_Hangar = KeyCode.F3;
    public static readonly KeyCode KEYBOARD_Hangar = KeyCode.H;
    public static readonly KeyCode XBOX360_Hangar = KeyCode.Joystick1Button3; // Y Button
    public static readonly KeyCode KEY_Operation = KeyCode.F1;
    public static readonly KeyCode KEYBOARD_Operation = KeyCode.O;
    public static readonly KeyCode XBOX360_Operation = KeyCode.Joystick1Button1; // B Button
    public static readonly KeyCode KEY_Controller = KeyCode.F5;
    public static readonly KeyCode KEYBOARD_Controller = KeyCode.K;
    public static readonly KeyCode XBOX360_Controller = KeyCode.Joystick1Button2; // X Button
    public static readonly KeyCode KEY_Escape = KeyCode.Escape;
    public static readonly KeyCode XBOX360_Escape = KeyCode.Joystick1Button0; // Back Button
    // Key Setting - Hangar
    public static readonly KeyCode KEY_PreviousHangar = KeyCode.Keypad4;
    public static readonly KeyCode KEY_NextHangar = KeyCode.Keypad6;
    public static readonly KeyCode KEYBOARD_Panel = KeyCode.P;
    public static readonly KeyCode XBOX360_Panel = KeyCode.Joystick1Button3; // Y Button
    public static readonly KeyCode KEYBOARD_Vocal = KeyCode.Space;
    public static readonly KeyCode XBOX360_Vocal = KeyCode.Joystick1Button2; // X Button
    // Key Setting - Operation
    public static readonly KeyCode KEY_WhoAttackU = KeyCode.F2;
    public static readonly KeyCode XBOX360_WhoAttackU = KeyCode.Joystick1Button2; // X Button
    public static readonly KeyCode KEY_Respawn = KeyCode.F3;
    public static readonly KeyCode XBOX360_Respawn = KeyCode.Joystick1Button3; // Y Button
    public static readonly KeyCode KEY_PreviousKocmocraft = KeyCode.Keypad4;
    public static readonly KeyCode KEY_NextKocmocraft = KeyCode.Keypad6;
    // Key Setting - Kocmocraft
    public static KeyCode KEYBOARD_CockpitView = KeyCode.C; // C
    public static KeyCode XBOX360_CockpitView; // A Button
    public static KeyCode KEYBOARD_Afterburner = KeyCode.LeftShift; // Left Shift
    public static KeyCode XBOX360_Afterburner; // X Button
    public static KeyCode KEYBOARD_LockOn = KeyCode.R; // R
    public static KeyCode XBOX360_LockOn; // B Button
    // Key Setting - Weapon
    public static readonly KeyCode KEYBOARD_Laser = KeyCode.Mouse0; // Left Mouse Button
    public static readonly KeyCode KEYBOARD_Rocket = KeyCode.Space; // Space
    public static readonly KeyCode KEYBOARD_Missile = KeyCode.Mouse1; // Right Mouse Button






















    [Header("Hotkey - Mouse Keyboard")]
    public Toggle tabMouseKeyboard;
    public GameObject panelMouseKeyboard;
    [Header("Hotkey Setting")]
    public GameObject panelHotkey;
    public Transform tabXbox360;
    public Toggle[] hotkeyMouseKeyboard;
    private Toggle[] hotkeyXbox360;
    private static Dictionary<string, KeyCode> listHotkeySetting = new Dictionary<string, KeyCode>();
    // Setting
    private Toggle activeHotkey;
    private TextMeshProUGUI activeHotkeyName;
    private string activeFunctionName;

    void InitializeHotkeySetting()
    {
        tabMouseKeyboard.onValueChanged.AddListener(isOn => { panelMouseKeyboard.SetActive(isOn); });

        //LoadHotkeySetting("KEYBOARD_Operation", ref KEYBOARD_Operation, KeyCode.O);
        //LoadHotkeySetting("KEYBOARD_Hangar", ref KEYBOARD_Hangar, KeyCode.H);
        //LoadHotkeySetting("KEYBOARD_Vocal", ref KEYBOARD_Vocal, KeyCode.Space);
        //LoadHotkeySetting("KEYBOARD_WhoAttackU", ref KEYBOARD_WhoAttackU, KeyCode.Return); // Enter
        //LoadHotkeySetting("KEYBOARD_Respawn", ref KEYBOARD_Respawn, KeyCode.K);
        //LoadHotkeySetting("KEYBOARD_CockpitView", ref KEYBOARD_CockpitView, KeyCode.C);
        //LoadHotkeySetting("XBOX360_WhoAttackU", ref XBOX360_WhoAttackU, KeyCode.Joystick1Button2); // X Button
        //LoadHotkeySetting("XBOX360_Respawn", ref XBOX360_Respawn, KeyCode.Joystick1Button3); // Y Button
        //LoadHotkeySetting("XBOX360_CockpitView", ref XBOX360_CockpitView, KeyCode.Joystick1Button0); // A Button
        //LoadHotkeySetting("KEYBOARD_Afterburner", ref KEYBOARD_ActiveAfterburner, KeyCode.LeftShift);
        //LoadHotkeySetting("KEYBOARD_LockOn", ref KEYBOARD_LockOn, KeyCode.R);
        //LoadHotkeySetting("KEYBOARD_Laser", ref KEYBOARD_Laser, KeyCode.Mouse0);
        //LoadHotkeySetting("KEYBOARD_Rocket", ref KEYBOARD_Rocket, KeyCode.Space);
        //LoadHotkeySetting("KEYBOARD_Missile", ref KEYBOARD_Missile, KeyCode.Mouse1);

        hotkeyMouseKeyboard = tabMouseKeyboard.GetComponentsInChildren<Toggle>();
        int count = hotkeyMouseKeyboard.Length;
        for (int i = 0; i < count-3; i++)
        {
            Toggle hotkey = hotkeyMouseKeyboard[i];
            TextMeshProUGUI textHotkey = hotkey.GetComponentInChildren<TextMeshProUGUI>();
            textHotkey.text = GetUnityKeyCodeName(listHotkeySetting[hotkey.name]);

            hotkey.onValueChanged.AddListener(isOn =>
            {
                if (isOn)
                {
                    activeHotkey = hotkey;
                    activeHotkeyName = hotkey.GetComponentInChildren<TextMeshProUGUI>();
                    activeFunctionName = hotkey.name;
                    //state = PanelState.HotkeyInput;
                    Debug.LogWarning("On");
                }
                else
                {
                    //state = PanelState.Ready;
                    Debug.LogWarning("Off");
                }
            });
        }
        //keyMappingXbox360 = tabXbox360.GetComponentsInChildren<Toggle>();
        OnHotKeyChanged?.Invoke();
    }
    private static void LoadHotkeySetting(string name, ref KeyCode functionHotkey, KeyCode defaultHotkey)
    {
        int numberCode = PlayerPrefs.GetInt(name);
        functionHotkey = numberCode == 0 ? defaultHotkey : (KeyCode)numberCode;
        listHotkeySetting.Add(name, functionHotkey);
    }
    private static void SaveHotkeySetting(string name, KeyCode keyCode)
    {
        listHotkeySetting[name] = keyCode;
        PlayerPrefs.SetInt(name,(int)keyCode);
    }

    private void MouseKeyboardSetting(int numberCode)
    {
        PlayerPrefs.SetInt(activeFunctionName, numberCode); // 儲存熱鍵
        KeyCode keyCode = (KeyCode)numberCode;
        Debug.Log(numberCode + " / " + keyCode + " / " + GetUnityKeyCodeName(keyCode));

        listHotkeySetting[activeFunctionName] = keyCode; //設定熱鍵
        activeHotkeyName.text = GetUnityKeyCodeName(keyCode); // 顯示熱鍵
        activeHotkey.isOn = false; // 反激活
    }

    public static string GetUnityKeyCodeName(KeyCode keyCode)
    {
        int numberCode = (int)keyCode;

        if (numberCode >= 48 && numberCode <= 57) // Alpha 數字
            return "#" + (numberCode - 48);
        else if (numberCode >= 97 && numberCode <= 122) // 字母
            return keyCode.ToString();
        else if (numberCode >= 256 && numberCode <= 265) // 數字鍵盤
            return "Num " + (numberCode - 256);
        else if (numberCode >= 277 && numberCode <= 296) // 中央功能鍵 + F1~F12
            return keyCode.ToString();
        else
        {
            switch (keyCode)
            {
                case KeyCode.Mouse0: return "L-Mouse";
                case KeyCode.Mouse1: return "R-Mouse";
                case KeyCode.Mouse2: return "M-Mouse";
                // 8~13
                case KeyCode.Backspace: return keyCode.ToString();
                case KeyCode.Tab: return keyCode.ToString();
                case KeyCode.Clear: return keyCode.ToString();
                case KeyCode.Return: return "Enter";
                // 303~308+313
                case KeyCode.RightShift: return "R-Shift";
                case KeyCode.LeftShift: return "L-Shift";
                case KeyCode.RightControl: return "R-Ctrl";
                case KeyCode.LeftControl: return "L-Ctrl";
                case KeyCode.RightAlt: return "R-Alt";
                case KeyCode.LeftAlt: return "L-Alt";
                case KeyCode.AltGr: return "R-Alt";
                // 19~32

                case KeyCode.Escape: return "Esc";
                case KeyCode.Space: return keyCode.ToString();
                // 277~281

                case KeyCode.Print: return keyCode.ToString();
                case KeyCode.SysReq: return keyCode.ToString(); // 與Print鍵同
                case KeyCode.ScrollLock: return keyCode.ToString();
                case KeyCode.Pause: return keyCode.ToString();
                case KeyCode.Break: return keyCode.ToString(); // 與Pause鍵同


                case KeyCode.Delete: return keyCode.ToString();

                // 方向键 273~276
                case KeyCode.UpArrow: return "Up";
                case KeyCode.DownArrow: return "Down";
                case KeyCode.RightArrow: return "Right";
                case KeyCode.LeftArrow: return "Left";
                // 數字鍵盤 266~271
                case KeyCode.KeypadPeriod: return "Num .";
                case KeyCode.KeypadDivide: return "Num /";
                case KeyCode.KeypadMultiply: return "Num *";
                case KeyCode.KeypadMinus: return "Num -";
                case KeyCode.KeypadPlus: return "Num +";
                case KeyCode.KeypadEnter: return "Num Enter";
                // 符號
                case KeyCode.Quote: return "'";  // 英文引號
                case KeyCode.Comma: return ",";  // 逗號 <
                case KeyCode.Minus: return "-";  // 減號 底線
                case KeyCode.Period: return ".";  // 句號 >
                case KeyCode.Slash: return "/";  // 斜線 問號
                case KeyCode.Semicolon: return ";";  // 分號 冒號
                case KeyCode.Equals: return "=";  // 等於 加號
                case KeyCode.LeftBracket: return "[";  // 前引號
                case KeyCode.Backslash: return "\\";  // 反斜線 管線
                case KeyCode.RightBracket: return "]";  // 後引號
                case KeyCode.BackQuote: return "~";  // 毛毛蟲~
            }
        }
        return null;
    }



}
