using System.Collections;
using System.Collections.Generic;
using Kocmoca;
using UnityEngine;

[CreateAssetMenu (fileName = "Turret Data", menuName = "KocmocA Data/Create Turret Data")]
[System.Serializable]
public class Turret : ScriptableObject
{
    [Header ("Cannon")]
    public string cannonName;
    public string typeEN;
    public string typeCN;
    public string cannonTypeEN;
    public string cannonTypeCN;
    [TextArea (3, 7)]
    public string develoment;
    [Tooltip ("機炮數量")]
    public int cannonCount;
    [Tooltip ("自動瞄準角")]
    public float maxAutoAimAngle;
    [Tooltip ("Calculate: 自動瞄準角的餘弦值")]
    public float maxAutoAimRange;
    [Header ("Fire Control System")]
    [Tooltip ("每分鐘射速")]
    public float roundsPerMinute;
    [Tooltip ("Calculate: 每輪開火間隔時間")]
    public float fireRate;
    public int repeatingCount;
    public float maxProjectileSpread;
    public GameObject shockwave;

    [Header ("Ammo")]
    public string ammoName;
    public string ammoTypeEN;
    public string ammoTypeCN;
    [Tooltip ("彈藥飛行速率")]
    public float ammoVelocity;
    [Tooltip ("Calculate: 推力")]
    public float propulsion;
    [Tooltip ("飛行時間")]
    public float flightTime;
    [Tooltip ("Calculate: 最大射程")]
    public float operationalRange;
    public float raySize;
    public WaitForSeconds waitRecovery;
    [Header ("Damage")]
    public float coefficient = 0.000071f;
    [Tooltip ("Calculate: 單發傷害")]
    public float damage;
    [Tooltip ("Calculate: 每秒鐘平均傷害")]
    public float dPS;
    [Tooltip ("穿透值：護盾的穿透百分比")]
    public int penetration;
    [Tooltip ("穿甲值：機甲的穿甲機率")]
    public int piercing;

    public void Calculate ()
    {
        maxAutoAimRange = Mathf.Cos (maxAutoAimAngle * Mathf.Deg2Rad);
        fireRate = 60 / roundsPerMinute;
        propulsion = ammoVelocity * 50;
        operationalRange = ammoVelocity * flightTime;
        damage = (int) (ammoVelocity * ammoVelocity * coefficient);
        dPS = (int) (ammoVelocity * ammoVelocity * coefficient * (roundsPerMinute / 60) * (cannonCount / 2) * repeatingCount);
    }
}