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
    public int shieldCrystal;
    public int emFieldLevel;
    public int emField;

    public override void Calculate(float design, int type)
    {
        int basic = 1200;
        int diff = 1800;
        shieldCrystal = (int)design;
        emFieldLevel = proficiency.m_Proficiency[type];
        emField = basic + diff * emFieldLevel;
        value = shieldCrystal+ emField;
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
    public int airframe;
    public int armorLevel;
    public int armor;

    public override void Calculate(float design, int type)
    {
        int basic = 1400;
        int diff = 2200;
        airframe = (int)design;
        armorLevel = proficiency.m_Proficiency[type];
        armor = basic + diff * armorLevel;
        value = airframe + armor;
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
    public int engine;
    public int afterburnerLevel;
    public int afterburner;

    public override void Calculate(float design, int type)
    {
        int basic = 50;
        int diff = 10;
        engine = (int)design;
        afterburnerLevel = proficiency.m_Proficiency[type];
        afterburner = basic + diff * afterburnerLevel;
        value = engine + afterburner;
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