using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Kocmocraft Module", menuName = "KocmocA Data/Create Kocmocraft Module")]
public class KocmocraftModule : ScriptableObject
{
    public int X;
    public int Y;
    public int Z;
    [Header("Design")]
    public Size size;
    public View view;
    [Header("Dubi")]
    public AudioClip talk;
    public Dubi chief;
    public Dubi reserve;
    [Header("Performance")]
    public Shield shield;
    public Hull hull;
    public Speed speed;
    [Header("Engine")]
    public Engine engine;
    [Header("Turret")]
    public Turret turret;

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
[System.Serializable]
public class Size
{
    public float wingspan, length, height, wingspanScale, lengthScale, heightScale;
}
[System.Serializable]
public class View
{
    public float orthoSize, near, far;
}
public abstract class Performance
{
    public int value;
    public int level;
    public abstract void Calculate(float design, int type);
}
[System.Serializable]
public class Shield : Performance
{
    public Proficiency proficiency;
    public override void Calculate(float design, int type)
    {
        float basic = 1200;
        float diff = 1800;
        value = (int)(design + basic + diff * proficiency.m_Proficiency[type]);
        for (int i = 7; i > 0; i--)
        {
            if (value >= basic + diff * i + (diff / 6 * (i - 1)))
            {
                level = i;
                break;
            }
        }
    }
}
[System.Serializable]
public class Hull : Performance
{
    public Proficiency proficiency;
    public override void Calculate(float design, int type)
    {
        float basic = 1400;
        float diff = 2200;
        value = (int)(design + basic + diff * proficiency.m_Proficiency[type]);
        for (int i = 7; i > 0; i--)
        {
            if (value >= basic + diff * i + (diff / 6 * (i - 1)))
            {
                level = i;
                break;
            }
        }
    }
}
[System.Serializable]
public class Speed : Performance
{
    public Proficiency proficiency;
    public override void Calculate(float design, int type)
    {
        float basic = 50;
        float diff = 10;
        value = (int)(design + basic + diff * proficiency.m_Proficiency[type]);
        for (int i = 7; i > 0; i--)
        {
            if (value >= basic + diff * i + (diff / 6 * (i - 1)))
            {
                level = i;
                break;
            }
        }
    }
}