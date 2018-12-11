/**************************************************************************************** 
 * Wakaka Studio 2017
 * Author: iLYuSha Dawa-mumu Wakaka Kocmocovich Kocmocki KocmocA
 * Project: 0escape Medieval - Max Window
 * Version: Tools Package v1.001a
 * Tools: Unity 5/C# + Arduino/C++
 * Last Updated: 2017/07/29
 ****************************************************************************************/
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;

public class MaxWindow : MonoBehaviour
{
    public string windowName;
    private int time = 10;

    [DllImport("user32.dll")]
    public static extern bool ShowWindow(System.IntPtr hwnd, int nCmdShow);

    [DllImport("user32.dll", EntryPoint = "GetForegroundWindow")]

    public static extern System.IntPtr GetForegroundWindow();

    [DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
    public static extern int SetForegroundWindow(int hwnd);
    ///   ///   ///   ///   ///   ///   ///   ///   ///   ///   ///   ///

    [DllImport("User32.dll", EntryPoint = "FindWindow")]
    private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
    [DllImport("User32.dll", EntryPoint = "FindWindowEx")]
    private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpClassName, string lpWindowName);

    void Start()
    {
        StartCoroutine("CheckPic");  //开始10秒后 把自己全屏
    }

    public IEnumerator CheckPic()
    {
        while (true)
        {
            time--;
            if (time <= 0)
            {
                //时间到,窗口在最前端并且最大化
                OpenMain();
                time = 10;
            }
            yield return new WaitForSeconds(1.0f);
        }
    }

    private void OpenMain()
    {
        ShowWindow(GetForegroundWindow(), 3);
        IntPtr hWnd = FindWindow(null, windowName);
        ShowWindow(hWnd, 3);
        Debug.Log(windowName);
    }
}
