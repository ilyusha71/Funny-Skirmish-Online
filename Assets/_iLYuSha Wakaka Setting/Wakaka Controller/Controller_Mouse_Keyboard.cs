/***************************************************************************
 * Controller - Mouse & Keyboard
 * 控制器 - 滑鼠鍵盤
 * Last Updated: 2018/11/30
 * Description:
 * 1. 
 ***************************************************************************/
using UnityEngine;

public partial class Controller : MonoBehaviour
{
    private static readonly float MouseAxisSensitivity = 1.0f;
    private static readonly float KeyboardAxisSensitivity = 1.0f;

    public static void UseMouseKeyboard()
    {
        controllerType = ControllerType.MouseKeyboard;
        PlayerPrefs.SetInt(PREFS_CONTROLLER_TYPE, (int)controllerType);
    }

    private void MouseKeyboardAxisInput()
    {
        controlValue[0] = Input.GetAxis("Mouse X");
        controlValue[1] = Input.GetAxis("Mouse Y");
        controlValue[2] = Input.GetAxis("Horizontal"); // 方向鍵或WSAD鍵
        controlValue[3] = Input.GetAxis("Vertical");

        // 飛行輸入值
        roll = Mathf.Clamp(controlValue[indexRoll] * MouseAxisSensitivity, -1, 1);
        pitch = Mathf.Clamp(controlValue[indexPitch] * MouseAxisSensitivity, -1, 1);
        yaw = Mathf.Clamp(controlValue[indexYaw] * KeyboardAxisSensitivity, -1, 1);
        throttle = Mathf.Clamp(controlValue[indexThrotte] * KeyboardAxisSensitivity, -1, 1);

        axisLevel = 0.01f;
    }
}