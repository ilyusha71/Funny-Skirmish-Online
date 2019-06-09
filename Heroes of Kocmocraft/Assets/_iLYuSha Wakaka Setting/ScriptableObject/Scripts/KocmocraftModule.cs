﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Kocmocraft Module", menuName = "KocmocA Data/Create Kocmocraft Module")]
public class KocmocraftModule : ScriptableObject
{
    public Kocmoca.Type type;
    [Header("Design")]
    public Design design;
    [Header("Pilot")]
    public AudioClip radio;
    public Dubi chief;
    public Dubi reserve;
    [Header("Performance")]
    public Shield shield;
    public Hull hull;
    public Speed speed;
    [Header("Engine")]
    public PowerSystem powerSystem;
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
        shield.Calculate(design.size.SurfaceArea, emBooster.level[(int)type]);
        hull.Calculate(design.size.Volume, armor.level[(int)type]);
        powerSystem.Calculate(design.size.Mass);
        speed.Calculate(powerSystem.cruiseSpeed, afterburner.level[(int)type]);
        turret.Calculate();
        kocmomech.Calculate();
    }
#endif
}
[System.Serializable]
public class Design
{
    [System.Serializable]
    public class View { public float orthoSize, near, far; }
    [System.Serializable]
    public class Size
    {
        public float wingspan, length, height, wingspanScale, lengthScale, heightScale;
#if UNITY_EDITOR
        public float SurfaceArea { get { return 2 * (wingspan * length + length * height + height * wingspan); } }
        public float Volume { get { return wingspan * length * height; } }
        public float Mass { get { return 30 * (wingspan * length + length * height + height * wingspan) + wingspan * length * height; } }
#endif
    }
    public string code, project, OKB, debut, mission;
    [TextArea(3, 7)]
    public string development;
    public View view;
    public Size size;
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
    [Header("动力系统")]
    public int powerSystem;
    [Header("后燃器")]
    public int afterburnerLevel;
    public int afterburner;
#if UNITY_EDITOR
    public void Calculate(int power, int proficiency)
    {
        powerSystem = power;
        int basic = 50;
        int diff = 12;
        afterburnerLevel = proficiency;
        afterburner = basic + diff * afterburnerLevel;
        maximum = powerSystem + afterburner;
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
public class PowerSystem
{
    [System.Serializable]
    public class PowerUnit
    {
        public int thrust, speed, acceleration;
    }
    [Header("Power System (PS)")]
    public int cruiseSpeed;
    public float acceleration;
    public PowerUnit total;
    [Header("Main Power Unit (MPU)")]
    public Engine[] listMPU;
    public int mpuCount;
    public PowerUnit mpu;
    public PowerUnit mainPower;
    [Header("Auxiliary Power Unit (APU)")]
    public Engine[] listAPU;
    public int apuCount;
    public PowerUnit apu;
    public PowerUnit auxiliaryPower;
#if UNITY_EDITOR
    public void Calculate(float mass)
    {
        var thrustFactor = 976f;
        // Main Power Unit
        var mpuPower = listMPU[0].power;
        mpuCount = listMPU.Length;
        mpu.thrust = Mathf.RoundToInt(mpuPower * thrustFactor);
        mpu.speed = Mathf.RoundToInt(mpuPower * 3.6f);
        mpu.acceleration = Mathf.RoundToInt(mpuPower * 3.6f * thrustFactor / mass);
        // Main Power
        var mp = mpuCount * mpuPower;
        mainPower.thrust = Mathf.RoundToInt(mp * thrustFactor);
        mainPower.speed = Mathf.RoundToInt(mp * 3.6f);
        mainPower.acceleration = Mathf.RoundToInt(mp * 3.6f * thrustFactor / mass);
        // Auxiliary Power Unit
        apuCount = listAPU.Length;
        if (apuCount > 0)
        {
            var apuPower = listAPU[0].power * 0.5f;
            apu.thrust = Mathf.RoundToInt(apuPower * thrustFactor);
            apu.speed = Mathf.RoundToInt(apuPower * 3.6f);
            apu.acceleration = Mathf.RoundToInt(apuPower * 3.6f * thrustFactor / mass);
            // Auxiliary Power
            var ap = apuCount * apuPower;
            auxiliaryPower.thrust = Mathf.RoundToInt(ap * thrustFactor);
            auxiliaryPower.speed = Mathf.RoundToInt(ap * 3.6f);
            auxiliaryPower.acceleration = Mathf.RoundToInt(ap * 3.6f * thrustFactor / mass);
            // Output
            cruiseSpeed = Mathf.RoundToInt(mp + ap);
            acceleration = (mp + ap) * thrustFactor / mass;
        }
        else
        {
            cruiseSpeed = Mathf.RoundToInt(mp);
            acceleration = mp * thrustFactor / mass;
        }
        // Total
        total.thrust = mainPower.thrust + auxiliaryPower.thrust;
        total.speed = mainPower.speed + auxiliaryPower.speed;
        total.acceleration = mainPower.acceleration + auxiliaryPower.acceleration;
    }
#endif
}