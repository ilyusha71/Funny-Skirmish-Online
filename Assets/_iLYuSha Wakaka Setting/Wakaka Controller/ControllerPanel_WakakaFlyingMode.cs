using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public partial class ControllerPanel : MonoBehaviour
{
    [Header("Wakaka Flying Mode")]
    public Toggle tabWakakaFlyingMode;
    public GameObject panelWakakaFlyingMode;
    [Header("Wakaka Flying Mode - Flying Axis")]
    public Slider sliderFlyingAxis;
    public TextMeshProUGUI textSensitivityFlyingAxis;
    public Toggle[] hotkeyWakakaButton;

    void InitializeWakakaMode()
    {
        Controller.InitializeWakakaModeData();
        tabWakakaFlyingMode.onValueChanged.AddListener(isOn =>
        {
            panelWakakaFlyingMode.SetActive(isOn);
            if (isOn) Controller.UseWakakaFlyingMode();
        });
        if (Controller.controllerType == ControllerType.WakakaFlyingMode) tabWakakaFlyingMode.isOn = true;
        hotkeyWakakaButton = panelWakakaFlyingMode.GetComponentsInChildren<Toggle>();
        // Slider Bar
        sliderFlyingAxis.value = Controller.FlyingAxisSensitivity;
        textSensitivityFlyingAxis.text = sliderFlyingAxis.value.ToString("0.00");
        sliderFlyingAxis.onValueChanged.AddListener(Value =>
        {
            textSensitivityFlyingAxis.text = Value.ToString("0.00");
            Controller.SetFlyingAxisSensitivity(Value);
        });

        //Auto loading Wakaka Button setting or use default key mapping
        Controller.FlyingAxis[0].Plus.InitializeHotkey(hotkeyWakakaButton[0], (int)KeyCode.Keypad6);
        Controller.FlyingAxis[0].Minus.InitializeHotkey(hotkeyWakakaButton[1], (int)KeyCode.Keypad4);
        Controller.FlyingAxis[1].Plus.InitializeHotkey(hotkeyWakakaButton[2], (int)KeyCode.Keypad8);
        Controller.FlyingAxis[1].Minus.InitializeHotkey(hotkeyWakakaButton[3], (int)KeyCode.Keypad2);
        Controller.FlyingAxis[2].Plus.InitializeHotkey(hotkeyWakakaButton[4], (int)KeyCode.D);
        Controller.FlyingAxis[2].Minus.InitializeHotkey(hotkeyWakakaButton[5], (int)KeyCode.A);
        Controller.FlyingAxis[3].Plus.InitializeHotkey(hotkeyWakakaButton[6], (int)KeyCode.W);
        Controller.FlyingAxis[3].Minus.InitializeHotkey(hotkeyWakakaButton[7], (int)KeyCode.S);
        Controller.JoystickRoot.InitializeHotkey(hotkeyWakakaButton[8], (int)KeyCode.Mouse2);
        Controller.JoystickThumb.InitializeHotkey(hotkeyWakakaButton[9], (int)KeyCode.Mouse1);
        Controller.JoystickIndex.InitializeHotkey(hotkeyWakakaButton[10], (int)KeyCode.Mouse0);
        //Controller.Back.InitializeHotkey((int)KeyCode.Escape, hotkeyWakakaButton[0]);
        //Controller.Stop.InitializeHotkey((int)KeyCode.Space, hotkeyWakakaButton[0]);
        //Controller.Play.InitializeHotkey((int)KeyCode.Return, hotkeyWakakaButton[0]);
        Controller.ThrustLeverRoot.InitializeHotkey(hotkeyWakakaButton[11], (int)KeyCode.Backspace);
        Controller.ThrustLeverThumb.InitializeHotkey(hotkeyWakakaButton[12], (int)KeyCode.LeftShift);
        Controller.ThrustLeverIndex.InitializeHotkey(hotkeyWakakaButton[13], (int)KeyCode.Space);
        Controller.ThrustLeverMiddle.InitializeHotkey(hotkeyWakakaButton[14], (int)KeyCode.R);
        Controller.ThrustLeverExtension.InitializeHotkey(hotkeyWakakaButton[15], (int)KeyCode.C);
    }
}
