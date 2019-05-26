using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Kocmocraft Module", menuName = "KocmocA Data/Create Kocmocraft Module")]
public class KocmocraftModule : ScriptableObject
{
    [Header("Design")]
    [Tooltip("设计")]
    public Size size;
    public View view;
    [Header("Dubi")]
    [Tooltip("逗比")]
    public AudioClip talk;
    public Dubi chief;
    public Dubi reserve;
    [Header("Performance")]
    [Tooltip("性能")]
    public Shield shield;
    public Hull hull;
    public Speed speed;
    [Header("Engine")]
    [Tooltip("发动机")]
    public Engine engine;
    [Header("Radar")]
    [Tooltip("雷达")]
    public Radar radar;
    [Header("Turret")]
    [Tooltip("机炮")]
    public Turret turret;
    [Header("Astromech")]
    [Tooltip("宇航技工")]
    public Astromech astromech;

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
#if UNITY_EDITOR
    public abstract void Calculate(float design, int type);
#endif
}
[System.Serializable]
public class Shield : Performance
{
    public int emCrystal;
    public int emBooster;
    public int emBoosterLevel;
#if UNITY_EDITOR
    public override void Calculate(float area, int type)
    {
        Proficiency proficiency = UnityEditor.AssetDatabase.LoadAssetAtPath<Proficiency>("Assets/_iLYuSha Wakaka Setting/ScriptableObject/Proficiency/EM Booster Level.asset");
        int basic = 1200;
        int diff = 1800;
        emCrystal = (int)area;
        emBoosterLevel = proficiency.level[type];
        emBooster = basic + diff * emBoosterLevel;
        value = emCrystal + emBooster;
        for (int i = 7; i > 0; i--)
        {
            if (value >= basic + diff * i + (diff / 6 * (i - 1)))
            {
                level = i;
                break;
            }
        }        
    }
#endif
}
[System.Serializable]
public class Hull : Performance
{
    public int airframe;
    public int armor;
    public int armorLevel;
#if UNITY_EDITOR
    public override void Calculate(float volume, int type)
    {
        Proficiency proficiency = UnityEditor.AssetDatabase.LoadAssetAtPath<Proficiency>("Assets/_iLYuSha Wakaka Setting/ScriptableObject/Proficiency/Armor Level.asset");
        int basic = 1400;
        int diff = 2200;
        airframe = (int)volume;
        armorLevel = proficiency.level[type];
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
#endif
}
[System.Serializable]
public class Speed : Performance
{
    public int engine;
    public int afterburner;
    public int afterburnerLevel;
#if UNITY_EDITOR
    public override void Calculate(float power, int type)
    {
        Proficiency proficiency = UnityEditor.AssetDatabase.LoadAssetAtPath<Proficiency>("Assets/_iLYuSha Wakaka Setting/ScriptableObject/Proficiency/Afterburner Level.asset");
        int basic = 50;
        int diff = 12;
        engine = (int)power;
        afterburnerLevel = proficiency.level[type];
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
#endif
}