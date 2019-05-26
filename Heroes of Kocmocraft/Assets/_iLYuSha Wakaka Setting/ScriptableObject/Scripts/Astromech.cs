using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Astromech Data", menuName = "KocmocA Data/Create Astromech Data")]
[System.Serializable]
public class Astromech : ScriptableObject
{
    [Header("Type")]
    [Tooltip("型号")]
    public string type;

    [Header("Regeneration")]
    [Tooltip("护盾再生")]
    public int regeneration;

    [Header("Flexibility")]
    [Tooltip("翻滚")]
    public float roll;
    [Tooltip("俯仰")]
    public float pitch;
    [Tooltip("偏转")]
    public float yaw;
    [Tooltip("加速度")]
    public float acceleration;
    [Tooltip("减速度")]
    public float deceleration;

    [Header("Lock Time")]
    [Tooltip("锁定耗时")]
    public float lockTime;

    [Header("Missile")]
    [Tooltip("Guided")]
    public int missileCount;
    public float missileReloadTime;
    [Tooltip("Unguided")]
    public int rocketCount;
    public float rocketReloadTime;    
}