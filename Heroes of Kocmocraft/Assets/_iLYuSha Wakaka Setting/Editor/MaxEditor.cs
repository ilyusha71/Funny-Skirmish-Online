/**************************************************************************************** 
 * Wakaka Studio 2017
 * Author: iLYuSha Dawa-mumu Wakaka Kocmocovich Kocmocki KocmocA
 * Project: 0escape Medieval - Max Window
 * Version: Tools Package v1.001a
 * Tools: Unity 5/C# + Arduino/C++
 * Last Updated: 2017/07/29
 ****************************************************************************************/
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MaxWindow))]
public class MaxEditor : Editor
{
    MaxWindow maxWindow;
    public void OnEnable()
    {
        maxWindow = (MaxWindow)target;
    }

    public override void OnInspectorGUI()
    {
        maxWindow.windowName = PlayerSettings.productName;
        GUILayout.Label(maxWindow.windowName);
    }
}
