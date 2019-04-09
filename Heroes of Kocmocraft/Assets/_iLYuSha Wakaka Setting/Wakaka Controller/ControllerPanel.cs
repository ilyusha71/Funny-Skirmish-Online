using System;
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

    [Header("Panel")]
    public Toggle tabMouseKeyboard;
    public Toggle tabXbox360;
    public GameObject panelMouseKeyboard;
    public GameObject panelXbox360;
    [Header("Toggle")]
    public Toggle[] hotkeyMouseKeyboard;
    public Toggle[] hotkeyXbox360;
    // Hotkey
    public static HotkeyToggle KEYBOARD_CockpitView;
    public static HotkeyToggle KEYBOARD_Afterburner;
    public static HotkeyToggle KEYBOARD_LockOn;
    public static HotkeyToggle XBOX360_CockpitView;
    public static HotkeyToggle XBOX360_Afterburner;
    public static HotkeyToggle XBOX360_LockOn;

    void Start ()
    {
        //values = (int[])System.Enum.GetValues(typeof(KeyCode));
        //InitializeWakakaMode();

        tabMouseKeyboard.onValueChanged.AddListener(isOn =>
        {
            panelMouseKeyboard.SetActive(isOn);
            if (isOn) Controller.UseMouseKeyboard();
        });
        tabXbox360.onValueChanged.AddListener(isOn =>
        {
            panelXbox360.SetActive(isOn);
            if (isOn) Controller.UseXbox360();
        });

        switch (Controller.controllerType)
        {
            case ControllerType.MouseKeyboard: tabMouseKeyboard.isOn = true;break;
            case ControllerType.Xbox360: tabXbox360.isOn = true; break;
        }

        hotkeyMouseKeyboard = panelMouseKeyboard.GetComponentsInChildren<Toggle>();
        KEYBOARD_CockpitView = new HotkeyToggle(hotkeyMouseKeyboard[0], "KEYBOARD_CockpitView", (int)KeyCode.C);
        KEYBOARD_Afterburner = new HotkeyToggle(hotkeyMouseKeyboard[1], "KEYBOARD_Afterburner", (int)KeyCode.LeftShift);
        KEYBOARD_LockOn = new HotkeyToggle(hotkeyMouseKeyboard[2], "KEYBOARD_LockOn", (int)KeyCode.R);

        hotkeyXbox360 = panelXbox360.GetComponentsInChildren<Toggle>();
        XBOX360_CockpitView = new HotkeyToggle(hotkeyXbox360[0], "XBOX360_CockpitView", (int)KeyCode.Joystick1Button0);
        XBOX360_Afterburner = new HotkeyToggle(hotkeyXbox360[1], "XBOX360_Afterburner", (int)KeyCode.Joystick1Button2);
        XBOX360_LockOn = new HotkeyToggle(hotkeyXbox360[2], "XBOX360_LockOn", (int)KeyCode.Joystick1Button1);

        SaveControllerSetting();
    }

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.F5))
            SetController();

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

    public void SetController()
    {
        if (!panelHotkey.activeSelf)
            MouseLock.nowState = MouseLock.MouseLocked;
        else
            SaveControllerSetting();
        panelHotkey.SetActive(!panelHotkey.activeSelf);
        MouseLock.MouseLocked = panelHotkey.activeSelf ? false : MouseLock.nowState;
    }

    void SaveControllerSetting()
    {
        Controller.KEYBOARD_CockpitView = KEYBOARD_CockpitView.Hotkey;
        Controller.KEYBOARD_Afterburner = KEYBOARD_Afterburner.Hotkey;
        Controller.KEYBOARD_LockOn = KEYBOARD_LockOn.Hotkey;
        Controller.XBOX360_CockpitView = XBOX360_CockpitView.Hotkey;
        Controller.XBOX360_Afterburner = XBOX360_Afterburner.Hotkey;
        Controller.XBOX360_LockOn = XBOX360_LockOn.Hotkey;
    }
}

public enum PanelState
{
    Ready = 1,
    HotkeyInput = 3,
}

public static class MouseLock
{
    public static bool nowState;
    private static bool mouseLocked;
    public static bool MouseLocked
    {
        get
        { return mouseLocked; }
        set
        {
            mouseLocked = value;
#if UNITY_4_6
            Screen.lockCursor = mouseLocked;
#else
            Cursor.visible = !value;
            Cursor.lockState = Cursor.visible ? CursorLockMode.None : CursorLockMode.Locked;
#endif
        }
    }
}

public class HotkeyToggle
{
    private Toggle Listener;
    public KeyCode Hotkey;
    private string PrefKeySetting;

    public HotkeyToggle(Toggle toggle, string prefCode, int defaultKey)
    {
        Listener = toggle;
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
                // 搖桿
                case KeyCode.Joystick1Button0: keyName.text = "A"; return true; // A Button
                case KeyCode.Joystick1Button1: keyName.text = "B"; return true; // B Button
                case KeyCode.Joystick1Button2: keyName.text = "X"; return true; // X Button
                case KeyCode.Joystick1Button3: keyName.text = "Y"; return true; // Y Button
                case KeyCode.Joystick1Button4: keyName.text = "LB"; return true; // Left Bumper
                case KeyCode.Joystick1Button5: keyName.text = "RB"; return true; // Right Bumper
                case KeyCode.Joystick1Button6: keyName.text = "Back"; return true; // Back Button
                case KeyCode.Joystick1Button7: keyName.text = "Start"; return true; // Start Button
            }
        }
        return false;
    }
}