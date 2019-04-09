/***************************************************************************
 * Wakaka Controller
 * 哇咔咔控制器（按鍵映射功能）
 * Last Updated: 2018/11/18
 * Description:
 * 1. 
 ***************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public partial class Controller : MonoBehaviour
{
    [DllImport("user32.dll", EntryPoint = "keybd_event")]
    public static extern void Keybd_event(
    byte bvk,//虚拟键值 ESC键对应的是27
    byte bScan,//0
    int dwFlags,//0为按下，1按住，2释放
    int dwExtraInfo//0
    );

    /// <summary>  
    /// 鼠标事件  
    /// </summary>  
    /// <param name="flags">事件类型</param>  
    /// <param name="dx">x坐标值(0~65535)</param>  
    /// <param name="dy">y坐标值(0~65535)</param>  
    /// <param name="data">滚动值(120一个单位)</param>  
    /// <param name="extraInfo">不支持</param>  
    [DllImport("user32.dll")]
    static extern void mouse_event(MouseEventFlag flags, int dx, int dy, uint data, UIntPtr extraInfo);
    [DllImport("user32.dll")]
    static extern void mouse_event(MouseEventFlag flags);
    [DllImport("user32")] // 使用 user32.dll ，這是系統的 Dll 檔，所以Unity會自動匯入，不用再手動加入 dll 檔
    static extern bool SetCursorPos(int X, int Y);

    /// <summary>  
    /// 鼠标操作标志位集合  
    /// </summary>  
    enum MouseEventFlag : uint
    {
        Move = 0x0001,
        LeftDown = 0x0002,
        LeftUp = 0x0004,
        RightDown = 0x0008,
        RightUp = 0x0010,
        MiddleDown = 0x0020,
        MiddleUp = 0x0040,
        XDown = 0x0080,
        XUp = 0x0100,
        Wheel = 0x0800,
        VirtualDesk = 0x4000,
        /// <summary>  
        /// 设置鼠标坐标为绝对位置（dx,dy）,否则为距离最后一次事件触发的相对位置  
        /// </summary>  
        Absolute = 0x8000
    }

    private static void DoLeftMouseClick()
    {
        int dx = (int)((double)Screen.width * 0.5f / Screen.width * 65535); //屏幕分辨率映射到0~65535(0xffff,即16位)之间  
        int dy = (int)((double)Screen.height * 0.5f / Screen.height * 0xffff); //转换为double类型运算，否则值为0、1  
        mouse_event(MouseEventFlag.Move | MouseEventFlag.LeftDown | MouseEventFlag.LeftUp | MouseEventFlag.Absolute, dx, dy, 0, new UIntPtr(0)); //点击  
    }
    private static void RightMouseDown()
    {
        int dx = (int)((double)Screen.width * 0.5f / Screen.width * 65535); //屏幕分辨率映射到0~65535(0xffff,即16位)之间  
        int dy = (int)((double)Screen.height * 0.5f / Screen.height * 0xffff); //转换为double类型运算，否则值为0、1  
        mouse_event(MouseEventFlag.Move | MouseEventFlag.RightDown | MouseEventFlag.Absolute, dx, dy, 0, new UIntPtr(0)); //点击  
    }
    private static void RightMouseUp()
    {
        int dx = (int)((double)Screen.width * 0.5f / Screen.width * 65535); //屏幕分辨率映射到0~65535(0xffff,即16位)之间  
        int dy = (int)((double)Screen.height * 0.5f / Screen.height * 0xffff); //转换为double类型运算，否则值为0、1  
        mouse_event(MouseEventFlag.Move | MouseEventFlag.RightUp | MouseEventFlag.Absolute, dx, dy, 0, new UIntPtr(0)); //点击  
    }

    void MappingEvent(int keycode, int state)
    {
        // Mouse 1: Mouse Left Button
        // Mouse 2: Mouse Right Button
        // Mouse 3: Mouse Middle Button
        // state 0: Press
        // state 1: Repeat
        // state 2: Release
        if (keycode == (int)KeyCode.Mouse0)
        {
            switch (state)
            {
                case 0: mouse_event(MouseEventFlag.LeftDown); break;
                case 1: mouse_event(MouseEventFlag.LeftDown | MouseEventFlag.LeftUp); break;
                case 2: mouse_event(MouseEventFlag.LeftUp); break;
            }
        }
        else if (keycode == (int)KeyCode.Mouse1)
        {
            switch (state)
            {
                case 0: mouse_event(MouseEventFlag.RightDown); break;
                case 1: mouse_event(MouseEventFlag.RightDown | MouseEventFlag.RightUp); break;
                case 2: mouse_event(MouseEventFlag.RightUp); break;
            }
        }
        else if (keycode == (int)KeyCode.Mouse2)
        {
            switch (state)
            {
                case 0: mouse_event(MouseEventFlag.MiddleDown); break;
                case 1: mouse_event(MouseEventFlag.MiddleDown | MouseEventFlag.MiddleUp); break;
                case 2: mouse_event(MouseEventFlag.MiddleUp); break;
            }
        }
        else
            Keybd_event((byte)keycode, 0, (byte)state, 0);
    }

    void CustomMapping(int keycode, int state)
    {
        // Mouse 1: Mouse Left Button
        // Mouse 2: Mouse Right Button
        // Mouse 3: Mouse Middle Button
        // state 0: Press
        // state 1: Repeat
        // state 2: Release
        if (keycode == (int)KeyCode.Mouse0)
        {
            switch (state)
            {
                case 0: mouse_event(MouseEventFlag.LeftDown); break;
                case 1: mouse_event(MouseEventFlag.LeftDown | MouseEventFlag.LeftUp); break;
                case 2: mouse_event(MouseEventFlag.LeftUp); break;
            }
        }
        else if (keycode == (int)KeyCode.Mouse1)
        {
            switch (state)
            {
                case 0: mouse_event(MouseEventFlag.RightDown); break;
                case 1: mouse_event(MouseEventFlag.RightDown | MouseEventFlag.RightUp); break;
                case 2: mouse_event(MouseEventFlag.RightUp); break;
            }
        }
        else if (keycode == (int)KeyCode.Mouse2)
        {
            switch (state)
            {
                case 0: mouse_event(MouseEventFlag.MiddleDown); break;
                case 1: mouse_event(MouseEventFlag.MiddleDown | MouseEventFlag.MiddleUp); break;
                case 2: mouse_event(MouseEventFlag.MiddleUp); break;
            }
        }
        else
            Keybd_event((byte)keycode, 0, (byte)state, 0);
    }


}
