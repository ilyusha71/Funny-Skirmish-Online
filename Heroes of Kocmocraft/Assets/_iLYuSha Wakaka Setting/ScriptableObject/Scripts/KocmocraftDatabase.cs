﻿using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Kocmocraft Index", menuName = "KocmocA Data/Create Kocmocraft Index")]
public class KocmocraftDatabase : ScriptableObject
{
    public List<KocmocraftModule> kocmocraft;
    //public AutoLevelSetting autoLevel;


#if UNITY_EDITOR
    public void SaveDatabase()
    {
        //for (int i = 0; i < 20; i++)
        //{
        //    UnityEditor.AssetDatabase.RenameAsset("Assets/_iLYuSha Wakaka Setting/ScriptableObject/Kocmocraft Module " + i + ".asset", index.kocmocraft[i + 20].name + ".asset");
        //}
        Debug.Log("<color=yellow>Database has been updated!</color>");
        UnityEditor.AssetDatabase.SaveAssets();
    }
#endif
}

//public class AutoLevelSetting
//{
//    public float attitudeLimit = 16 * Mathf.Deg2Rad; // 飞行姿态限制 
//    public float decayAngle = 9 * Mathf.Deg2Rad;
//    public float inverseAngle = decayAngle * 0.75f;
//    public float terminalAngle = decayAngle * 0.5f;
//}