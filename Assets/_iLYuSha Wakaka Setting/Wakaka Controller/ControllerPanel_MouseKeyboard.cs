using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public partial class ControllerPanel : MonoBehaviour
{
    [Header("Mouse & Keyboard")]
    public Toggle tabMouseKeyboard;
    public GameObject panelMouseKeyboard;
    [Header("Mouse & Keyboard - FunctionKey")]
    public Toggle[] hotkeyFunction;

    private HotkeyToggle CockpitView;
    private HotkeyToggle Afterburner;
    private HotkeyToggle LockOn;
    private HotkeyToggle Laser;
    private HotkeyToggle Rocket;
    private HotkeyToggle Missile;


    void InitializeMouseKeyboard()
    {
        Controller.InitializeHotkeyData();
        tabMouseKeyboard.onValueChanged.AddListener(isOn =>
        {
            panelMouseKeyboard.SetActive(isOn);
            if (isOn) Controller.UseKeyboardMouse();
        });
        if (Controller.controllerType == ControllerType.MouseAndKeyboard) tabMouseKeyboard.isOn = true;
        hotkeyFunction = panelMouseKeyboard.GetComponentsInChildren<Toggle>();

        CockpitView = new HotkeyToggle(hotkeyFunction[0], Controller.KEY_CockpitView, "HotkeyToggle-CockpitView", (int)KeyCode.C);
        Afterburner = new HotkeyToggle(hotkeyFunction[1], Controller.KEY_Afterburner, "HotkeyToggle-Afterburner", (int)KeyCode.LeftShift);
        LockOn = new HotkeyToggle(hotkeyFunction[2], Controller.KEY_LockOn, "HotkeyToggle-LockOn", (int)KeyCode.R);
        Laser = new HotkeyToggle(hotkeyFunction[3], Controller.KEY_Laser, "HotkeyToggle-Laser", (int)KeyCode.Mouse0);
        Rocket = new HotkeyToggle(hotkeyFunction[4], Controller.KEY_Rocket, "HotkeyToggle-Rocket", (int)KeyCode.Space);
        Missile = new HotkeyToggle(hotkeyFunction[5], Controller.KEY_Missile, "HotkeyToggle-Missile", (int)KeyCode.Mouse1);




        //Controller.Operation.InitializeHotkey(hotkeyFunction[0], (int)KeyCode.O);
        //Controller.Hangar.InitializeHotkey(hotkeyFunction[1], (int)KeyCode.H);
        //Controller.Vocal.InitializeHotkey(hotkeyFunction[2], (int)KeyCode.Space);
        //Controller.WhoAttackU.InitializeHotkey(hotkeyFunction[3], (int)KeyCode.Return); //KeyCode.Joystick1Button2
        //Controller.Respawn.InitializeHotkey(hotkeyFunction[4], (int)KeyCode.Backspace); // KeyCode.Joystick1Button3
        //Controller.CockpitView.InitializeHotkey(hotkeyFunction[5], (int)KeyCode.C); // KeyCode.Joystick1Button0

        //Controller.Afterburner.InitializeHotkey(hotkeyFunction[6], (int)KeyCode.LeftShift);
        //Controller.LockOn.InitializeHotkey(hotkeyFunction[7], (int)KeyCode.R);
        //Controller.Laser.InitializeHotkey(hotkeyFunction[8], (int)KeyCode.Mouse0);
        //Controller.Rocket.InitializeHotkey(hotkeyFunction[9], (int)KeyCode.Space);
        //Controller.Missile.InitializeHotkey(hotkeyFunction[10], (int)KeyCode.Mouse1);
    }
}
