using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public partial class ControllerPanel : MonoBehaviour
{
    public GameObject panelHotkey;

    public static PanelState State;
    public static HotkeyToggle ActiveHotkey;
    private readonly int[] values = (int[])System.Enum.GetValues(typeof(KeyCode));

    void Start ()
    {
        //values = (int[])System.Enum.GetValues(typeof(KeyCode));
        //InitializeWakakaMode();
        InitializeMouseKeyboard();
    }

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            if (!panelHotkey.activeSelf)
                MouseLock.nowState = MouseLock.MouseLocked;
            panelHotkey.SetActive(!panelHotkey.activeSelf);
            MouseLock.MouseLocked = panelHotkey.activeSelf ? false : MouseLock.nowState;
        }

        if (State == PanelState.HotkeyInput)
        {
            for (int i = 0; i < values.Length; i++)
            {
                if (Input.GetKey((KeyCode)values[i]))
                {
                    int numberCode = values[i];
                    if (numberCode == 27 || // 禁止映射Esc
                        (numberCode >= 300 && numberCode <= 302) || //禁止映射Lock鍵
                        (numberCode >= 309 && numberCode <= 312) || //禁止映射系統鍵
                        (numberCode >= 316 && numberCode <= 317))  //禁止映射Print/SysReq鍵
                        return;
                    ActiveHotkey.SetHotkey(numberCode);
                    State = PanelState.Ready;
                }
            }
        }
    }
}

public enum PanelState
{
    Ready = 1,
    HotkeyInput = 3,
}

public class HotkeyToggle
{
    private Toggle Listener;
    private KeyCode Hotkey;
    private string PrefKeySetting;

    public HotkeyToggle(Toggle toggle, KeyCode key, string prefCode, int defaultKey)
    {
        Listener = toggle;
        Hotkey = key;
        PrefKeySetting = prefCode;

        int prefKey = PlayerPrefs.GetInt(PrefKeySetting);
        SetHotkey(prefKey == 0 ? defaultKey : prefKey);

        Listener.onValueChanged.AddListener(isOn =>
        {
            if (isOn)
            {
                ControllerPanel.ActiveHotkey = this;
                ControllerPanel.State = PanelState.HotkeyInput;
            }
            else
                ControllerPanel.State = PanelState.Ready;
        });
    }

    public void SetHotkey(int unityCode)
    {
        if (KeyIsExist(unityCode))
            PlayerPrefs.SetInt(PrefKeySetting, unityCode); // 儲存Unity鍵碼
        Listener.isOn = false;
    }

    public bool KeyIsExist(int unityCode)
    {
        TextMeshProUGUI keyName = Listener.GetComponentInChildren<TextMeshProUGUI>();
        keyName.text = "NULL";
        Hotkey = (KeyCode)unityCode;

        if (unityCode >= 48 && unityCode <= 57) // Alpha 數字
        {
            keyName.text = "#" + (unityCode - 48);
            return true;
        }
        else if (unityCode >= 97 && unityCode <= 122) // 字母
        {
            keyName.text = Hotkey.ToString();
            return true;
        }
        else if (unityCode >= 256 && unityCode <= 265) // 數字鍵盤
        {
            keyName.text = "Num " + (unityCode - 256);
            return true;
        }
        else if (unityCode >= 277 && unityCode <= 296) // 中央功能鍵 + F1~F12
        {
            keyName.text = Hotkey.ToString();
            return true;
        }
        else
        {
            switch (Hotkey)
            {
                case KeyCode.Mouse0: keyName.text = "LClick"; return true;
                case KeyCode.Mouse1: keyName.text = "RClick"; return true;
                case KeyCode.Mouse2: keyName.text = "Wheel"; return true;

                case KeyCode.Backspace: keyName.text = Hotkey.ToString(); return true;
                case KeyCode.Tab: keyName.text = Hotkey.ToString(); return true;
                case KeyCode.Clear: keyName.text = Hotkey.ToString(); return true;
                case KeyCode.Return: keyName.text = "Enter"; return true;

                case KeyCode.RightShift: keyName.text = "RShift"; return true;
                case KeyCode.LeftShift: keyName.text = "LShift"; return true;
                case KeyCode.RightControl: keyName.text = "RCtrl"; return true;
                case KeyCode.LeftControl: keyName.text = "LCtrl"; return true;
                case KeyCode.RightAlt: keyName.text = "RAlt"; return true;
                case KeyCode.LeftAlt: keyName.text = "LAlt"; return true;
                case KeyCode.AltGr: keyName.text = "RAlt"; return true;

                case KeyCode.CapsLock: keyName.text = Hotkey.ToString(); return true;
                case KeyCode.Pause: keyName.text = Hotkey.ToString(); return true;
                case KeyCode.Escape: keyName.text = "Esc"; return true;
                case KeyCode.Space: keyName.text = Hotkey.ToString(); return true;
                case KeyCode.Delete: keyName.text = Hotkey.ToString(); return true;

                // 方向键 273~276
                case KeyCode.UpArrow: keyName.text = "Up"; return true;
                case KeyCode.DownArrow: keyName.text = "Down"; return true;
                case KeyCode.RightArrow: keyName.text = "Right"; return true;
                case KeyCode.LeftArrow: keyName.text = "Left"; return true;
                // 數字鍵盤 266~271
                case KeyCode.KeypadPeriod: keyName.text = "Num ."; return true; // . Del
                case KeyCode.KeypadDivide: keyName.text = "Num /"; return true; // /
                case KeyCode.KeypadMultiply: keyName.text = "Num *"; return true; // *
                case KeyCode.KeypadMinus: keyName.text = "Num -"; return true; // -
                case KeyCode.KeypadPlus: keyName.text = "Num +"; return true; // +
                case KeyCode.KeypadEnter: keyName.text = "Num Enter"; return true;
                // 符號
                case KeyCode.Quote: keyName.text = "'"; return true; // 英文引號
                case KeyCode.Comma: keyName.text = ","; return true; // 逗號 <
                case KeyCode.Minus: keyName.text = "-"; return true; // 減號 底線
                case KeyCode.Period: keyName.text = "."; return true; // 句號 >
                case KeyCode.Slash: keyName.text = "/"; return true; // 斜線 問號
                case KeyCode.Semicolon: keyName.text = ";"; return true; // 分號 冒號
                case KeyCode.Equals: keyName.text = "="; return true; // 等於 加號
                case KeyCode.LeftBracket: keyName.text = "["; return true; // 前引號
                case KeyCode.Backslash: keyName.text = "\\"; return true; // 反斜線 管線
                case KeyCode.RightBracket: keyName.text = "]"; return true; // 後引號
                case KeyCode.BackQuote: keyName.text = "~"; return true; // 毛毛蟲~
            }
        }
        return false;
    }
}