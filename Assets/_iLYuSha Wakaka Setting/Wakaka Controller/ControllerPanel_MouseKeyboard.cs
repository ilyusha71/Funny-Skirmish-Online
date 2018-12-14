using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class ControllerPanel : MonoBehaviour
{
    [Header("Mouse & Keyboard")]
    public Toggle tabMouseKeyboard2;
    public GameObject panelMouseKeyboard2;
    [Header("Mouse & Keyboard - FunctionKey")]
    public Toggle[] hotkeyFunction;

    void InitializeMouseKeyboard()
    {
        Controller.InitializeHotkeyData();
        tabMouseKeyboard2.onValueChanged.AddListener(isOn =>
        {
            panelMouseKeyboard2.SetActive(isOn);
            if (isOn) Controller.UseKeyboardMouse();
        });
        if (Controller.controllerType == ControllerType.MouseAndKeyboard) tabMouseKeyboard2.isOn = true;
        hotkeyFunction = panelMouseKeyboard2.GetComponentsInChildren<Toggle>();

        Controller.Operation.InitializeHotkey(hotkeyFunction[0], (int)KeyCode.O);
        Controller.Hangar.InitializeHotkey(hotkeyFunction[1], (int)KeyCode.H);
        Controller.Vocal.InitializeHotkey(hotkeyFunction[2], (int)KeyCode.Space);
        Controller.WhoAttackU.InitializeHotkey(hotkeyFunction[3], (int)KeyCode.Return); //KeyCode.Joystick1Button2
        Controller.Respawn.InitializeHotkey(hotkeyFunction[4], (int)KeyCode.Backspace); // KeyCode.Joystick1Button3
        Controller.CockpitView.InitializeHotkey(hotkeyFunction[5], (int)KeyCode.C); // KeyCode.Joystick1Button0
        Controller.Laser.InitializeHotkey(hotkeyFunction[6], (int)KeyCode.Mouse0);
        Controller.Rocket.InitializeHotkey(hotkeyFunction[7], (int)KeyCode.Space);
        Controller.Missile.InitializeHotkey(hotkeyFunction[8], (int)KeyCode.Mouse1);
    }

}
