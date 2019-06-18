using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Kocmocraft Index", menuName = "KocmocA Data/Create Kocmocraft Index")]
public class KocmocraftDatabase : ScriptableObject
{
    public List<KocmocraftModule> kocmocraft;
    //public AutoLevelSetting autoLevel;
 public List<BrickType> Types = new List<BrickType>();
    public string typeName;
    public LayerMask whatIsPlayer;   
 
    private BoxCollider2D m_boxCollider2D;

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

//如果此類別有被拿來做成 Public Array 或 Public 變數
//[System.Serializable] 會叫 Unity 去對此類別做序列化
[System.Serializable]
public class BrickType
{
    public string Name;
    public Color HitColor;
}