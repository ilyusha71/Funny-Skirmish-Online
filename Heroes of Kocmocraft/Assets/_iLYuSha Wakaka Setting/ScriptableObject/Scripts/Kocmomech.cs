using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Kocmomech Data", menuName = "KocmocA Data/Create Kocmomech Data")]
[System.Serializable]
public class Kocmomech : ScriptableObject
{
    [Header("Level")]
    [SerializeField]
    [Tooltip("Regeneration Level")]
    private int regenerationLevel;
    [SerializeField]
    [Tooltip("Flexibility Level")]
    private int flexibilityLevel;
    [SerializeField]
    [Tooltip("Acceleration Level")]
    private int accelerationLevel;
    [SerializeField]
    [Tooltip("Lock Time Level")]
    private int lockTimeLevel;
    [SerializeField]
    [Tooltip("Missile Level")]
    private int missileLevel;

    [Header("Type")]
    [Tooltip("型号")]
    public string type;
    [Tooltip("描述")]
    [TextArea(3, 7)]
    public string description;

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

    public void Calculate()
    {
        int[] regenerationData = { 110, 330, 550, 770, 990 };
        regeneration = regenerationData[regenerationLevel-1];

        float[] rollData = { 0.9f, 1.1f, 1.5f, 2.1f, 3.0f };
        float[] pitchData = { 0.6f, 0.9f, 1.3f, 1.8f, 2.4f };
        float[] yawData = { 0.5f, 0.6f, 0.7f, 0.8f, 1.0f };
        roll = rollData[flexibilityLevel-1];
        pitch = pitchData[flexibilityLevel-1];
        yaw = yawData[flexibilityLevel-1];

        float[] accelerationData = { 3.6f, 6.1f, 8.7f, 11.2f, 13.7f };
        acceleration = accelerationData[accelerationLevel-1];
        deceleration = acceleration * 1.8f;

        float[] lockTimeData = { 3.7f, 2.9f, 2.1f, 1.4f, 0.7f };
        lockTime = lockTimeData[lockTimeLevel-1];

        int[] missileCountData = { 3, 4, 4, 5, 5 };
        int[] missileReloadData = { 71, 59, 47, 33, 20 };
        int[] RocketCountData = { 4, 4, 6, 7, 9 };
        int[] RocketReloadData = { 55, 47, 36, 23, 11 };
        missileCount = missileCountData[missileLevel - 1];
        missileReloadTime = missileReloadData[missileLevel - 1];
        rocketCount = RocketCountData[missileLevel - 1];
        rocketReloadTime = RocketReloadData[missileLevel - 1];
    }
}