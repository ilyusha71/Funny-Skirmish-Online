using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ControllerPanel : MonoBehaviour
{
    public GameObject panelHotkey;
    void Start ()
    {
        InitializeWakakaMode();
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
    }
}
