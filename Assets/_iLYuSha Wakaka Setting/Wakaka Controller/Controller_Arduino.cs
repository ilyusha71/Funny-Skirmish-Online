using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Controller : MonoBehaviour
{
    private bool useSteeringWheel; // 使用方向盤
    public static float thruster, rudder, brake; // Driving Mode
    public static float roll, pitch, yaw, throttle; // Flying Mode



    private int rightHorizontalState, rightVerticalState, leftHorizontalState, leftVerticalState, accelState;
    public static float sensitivityWSAD; // 映射WSAD靈敏度
    public static float axisSensasity = 3.0f;
    public static float xAccelLimit = 3.0f;

    // 手把四按鍵狀態
    private bool stateSteeringWheel; // 四鍵齊按
    private bool stateOptionA, stateOptionB, stateOptionC; // 其他組合鍵
    private bool stateRightFire, stateRightThumb, stateLeftFire, stateLeftThumb; // 基本四鍵
    // 蘑菇頭按鍵狀態
    private bool stateRFC, stateLFC; // 暫時未使用
    // 儀表板大按鍵狀態
    private bool stateBack, stateStop, statePlay;


    [Header("Arduino")]
    public MessageBox msgBox;
    private const int commandsCount = 6;
    private int countQueue;
    private Command command = new Command()
    {
        AxisX = -999, // -999 代表沒訊號，使用前一幀的值，不然會變成0
        AxisY = -999,
        AxisH = -999,
        AxisV = -999,
        Wheel = -999,
        Button = "Btn"
    };

}

public struct Command
{
    internal float AxisX;
    internal float AxisY;
    internal float AxisH;
    internal float AxisV;
    internal float Wheel;
    internal string Button;

    internal void Reset()
    {
        AxisX = -999;
        AxisY = -999;
        AxisH = -999;
        AxisV = -999;
        Wheel = -999;
        Button = "Btn";
    }

    internal void Input(string[] input)
    {
        AxisX = float.Parse(input[0]);
        AxisY = float.Parse(input[1]);
        AxisH = float.Parse(input[2]);
        AxisV = float.Parse(input[3]);
        Wheel = float.Parse(input[4]);
        Button = input[5];
    }
}
