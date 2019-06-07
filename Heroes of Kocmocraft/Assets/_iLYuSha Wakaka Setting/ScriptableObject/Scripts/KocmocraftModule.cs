using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Kocmocraft Module", menuName = "KocmocA Data/Create Kocmocraft Module")]
public class KocmocraftModule : ScriptableObject
{
    public Kocmoca.Type type;
    [Header("Design")]
    public Size size;
    public View view;
    [Header("Pilot")]
    public AudioClip radio;
    public Dubi chief;
    public Dubi reserve;
    [Header("Performance")]
    public Shield shield;
    public Hull hull;
    public Speed speed;
    [Header("Engine")]
    public PowerUnit powerUnit;
    [Header("Radar")]
    public Radar radar;
    [Header("Turret")]
    public Turret turret;
    [Header("Kocmomech")]
    public Kocmomech kocmomech;

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
    public void Calculate()
    {
        var dataPath = "Assets/_iLYuSha Wakaka Setting/ScriptableObject/";
        var emBooster = UnityEditor.AssetDatabase.LoadAssetAtPath<Proficiency>(dataPath + "Proficiency/EM Booster Level.asset");
        var armor = UnityEditor.AssetDatabase.LoadAssetAtPath<Proficiency>(dataPath + "Proficiency/Armor Level.asset");
        var afterburner = UnityEditor.AssetDatabase.LoadAssetAtPath<Proficiency>(dataPath + "Proficiency/Afterburner Level.asset");
        shield.Calculate(size.SurfaceArea, emBooster.level[(int)type]);
        hull.Calculate(size.Volume, armor.level[(int)type]);
        speed.Calculate(powerUnit.Calculate(), afterburner.level[(int)type]);
        turret.Calculate();
        kocmomech.Calculate();
    }
#endif
}
[System.Serializable]
public class Size
{
    public float wingspan, length, height, wingspanScale, lengthScale, heightScale;
#if UNITY_EDITOR
    public float SurfaceArea { get { return 2 * (wingspan * length + length * height + height * wingspan); } }
    public float Volume { get { return wingspan * length * height; } }
#endif
}
[System.Serializable]
public class View
{
    public float orthoSize, near, far;
}
public abstract class Performance
{
    public int level;
    public int maximum;
#if UNITY_EDITOR
    // public abstract void Calculate(int design, int level);
    // public abstract void Calculate(int power);
#endif
}
[System.Serializable]
public class Shield : Performance
{
    [Header("电磁晶体")]
    public int emCrystal;
    [Header("电磁加速器")]
    public int emBoosterLevel;
    public int emBooster;
#if UNITY_EDITOR
    public void Calculate(float surfaceArea, int proficiency)
    {
        emCrystal = (int)(surfaceArea * 10);
        int basic = 1200;
        int diff = 1800;
        emBoosterLevel = proficiency;
        emBooster = basic + diff * emBoosterLevel;
        maximum = emCrystal + emBooster;
        for (int i = 7; i > 0; i--)
        {
            if (maximum >= basic + diff * i + (diff / 6 * (i - 1)))
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
    [Header("机体")]
    public int airframe;
    [Header("装甲")]
    public int armorLevel;
    public int armor;
#if UNITY_EDITOR
    public void Calculate(float volume, int proficiency)
    {
        airframe = (int)(volume * 10);
        int basic = 1400;
        int diff = 2100;
        armorLevel = proficiency;
        armor = basic + diff * armorLevel;
        maximum = airframe + armor;
        for (int i = 7; i > 0; i--)
        {
            if (maximum >= basic + diff * i + (diff / 6 * (i - 1)))
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
    [Header("动力单元")]
    public int powerUnit;
    [Header("后燃器")]
    public int afterburnerLevel;
    public int afterburner;
#if UNITY_EDITOR
    public void Calculate(int power, int proficiency)
    {
        powerUnit = power;
        int basic = 50;
        int diff = 12;
        afterburnerLevel = proficiency;
        afterburner = basic + diff * afterburnerLevel;
        maximum = powerUnit + afterburner;
        for (int i = 7; i > 0; i--)
        {
            if (maximum >= basic + diff * i + (diff / 6 * (i - 1)))
            {
                level = i;
                break;
            }
        }
    }
#endif
}
[System.Serializable]
public class PowerUnit
{
    public int totalPower;
    [Header("Main Power")]
    public Engine[] mainEngine;
    public int mainEngineCount;
    public int mainPower;
    [Header("Auxiliary Power")]
    public Engine[] auxiliaryPowerUnit;
    public int auxiliaryPowerUnitCount;
    public int auxiliaryPower;

#if UNITY_EDITOR
    public int Calculate()
    {
        // Main Power
        var power = 0.0f;
        mainEngineCount = mainEngine.Length;
        for (int i = 0; i < mainEngineCount; i++)
        {
            power += mainEngine[i].power;
        }
        mainPower = (int)power;
        // Auxiliary Power
        auxiliaryPowerUnitCount = auxiliaryPowerUnit.Length;
        power = 0.0f;
        for (int i = 0; i < auxiliaryPowerUnitCount; i++)
        {
            power += auxiliaryPowerUnit[i].power * 0.5f;
        }
        auxiliaryPower = (int)power;

        totalPower = mainPower + auxiliaryPower;
        return totalPower;
    }
#endif
}