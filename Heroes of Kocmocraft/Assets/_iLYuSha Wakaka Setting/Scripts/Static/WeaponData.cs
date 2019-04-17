using UnityEngine;

namespace Kocmoca
{
    public static class KocmoCannon
    {
        public static readonly float fixDamage = 0.000066f;
        public static readonly float fix2Tube = 1.0f;
        public static readonly float fix4Tube = 0.843f;
        public static readonly float fix6Tube = 0.779f;
        public static readonly float fix8Tube = 0.771f;
    }

    public static class KocmoLaserCannon
    {
        public static readonly int countPerBatch = 25; // 每批生成量
        public static readonly int maxPoorInventory = 2500; // 最大物件池存量
        public static readonly float maxFireAngle = Mathf.Cos(3 * Mathf.Deg2Rad); // 最大開火夾角
        public static readonly int maxAmmoCapacity = 999; // 最大載彈量
        public static readonly float projectileSpread = 0.39f; //彈道散布 degree
        public static readonly int flightVelocity = 2970; // 飛行速率 m/s
        public static readonly int propulsion = flightVelocity * 50; // 開火推力 propulsion = flightVelocity / Time.fixedDeltaTime (1/0.02 = 50)
        public static readonly float flightTime = 0.37f;//0.71f; // 飛行時間 sec
        public static readonly float operationalRange = flightVelocity * flightTime; // 射程 m
        public static readonly float fireRoundPerSecond = 9.73f; // 射速 rps
        public static readonly float rateFire = 1 / fireRoundPerSecond; // 開火速率 sec
        public static readonly float coefficientMinDamage = 1.3035f;
        public static readonly float coefficientMaxDamage = 4.1949f;
    }
    public static class KocmoRocketLauncher
    {
        public static readonly int countPerBatch = 1; // 每批生成量
        public static readonly int maxPoorInventory = 100; // 最大物件池存量
        public static readonly float maxFireAngle = Mathf.Cos(13 * Mathf.Deg2Rad); // 最大開火夾角
        public static readonly int maxAmmoCapacity = 12; // 最大載彈量
        public static readonly int flightVelocity = 999; // 飛行速率 m/s
        public static readonly int thrust = flightVelocity * 50; // 基本推力
        public static readonly float flightTime = 1.37f;//2.37f; // 飛行時間 sec
        public static readonly float fireRoundPerSecond = 3.37f; // 射速 rps
        public static readonly float rateFire = 1 / fireRoundPerSecond; // 開火速率 sec
        public static readonly float timeReload = 9.3f;
        public static readonly float coefficientDamageBasic = 2.97f;
        public static readonly float coefficientDamageHull = 1.73f;
        public static readonly float coefficientDamageShield = 0.47f;
    }
    public static class KocmoMissileLauncher
    {
        public static readonly int countPerBatch = 1; // 每批生成量
        public static readonly int maxPoorInventory = 100; // 最大物件池存量
        public static readonly float maxFireAngle = Mathf.Cos(21 * Mathf.Deg2Rad); // 最大開火夾角
        public static readonly int maxAmmoCapacity = 7; // 最大載彈量
        public static readonly int flightVelocity = 137; // 飛行速率 m/s
        public static readonly int minThrust = flightVelocity * 50; // 基本推力
        public static readonly float maxThrust = minThrust * 3.97f; // 最大推力
        public static readonly float acceleration = maxThrust * 0.0793f; // 加速度
        public static readonly float flightTime = 7.0f; // 飛行時間 sec
        public static readonly float fireRoundPerSecond = 1.137f; // 射速 rps
        public static readonly float rateFire = 1 / fireRoundPerSecond; // 開火速率 sec
        public static readonly float timeReload = 15.0f;
        public static readonly float coefficientDamageBasic = 7.0f;
        public static readonly float coefficientDamageHull = 1.37f;
        public static readonly float coefficientDamageShield = 2.37f;
    }




    // KHAR 特快α射線炮（4管限定 / 6管限定） + 極遠距雷達/遠距雷達
    public static class KocmoHyperAlphaRay
    {
        public static readonly float projectileSpread = 0.07f; //彈道散布 degree
        public static readonly int ammoVelocity = 7730; // 飛行速率 m/s
        public static readonly int propulsion = ammoVelocity * 50; // 開火推力 propulsion = ammoVelocity / Time.fixedDeltaTime (1/0.02 = 50)
        public static readonly float flightTime = 0.5f; // 飛行時間 sec
        public static readonly float operationalRange = ammoVelocity * flightTime; // 射程 m
        public static readonly float fireRoundPerSecond = 0.7f; // 射速 rps
        public static readonly float rateFire = 1 / fireRoundPerSecond; // 開火速率 sec
        public static readonly int repeating = 1; // 連發次數
        public static readonly float hullPenetration = 3.7f; // 機甲穿透
        public static readonly float shieldPenetration = 0.5f; // 護盾穿透
        public static readonly int hullDamage = (int)(ammoVelocity * ammoVelocity * KocmoCannon.fixDamage * hullPenetration); // 14592
        public static readonly int shieldDamage = (int)(ammoVelocity * ammoVelocity * KocmoCannon.fixDamage * shieldPenetration); // 1972
        public static readonly int hullDPS = (int)(ammoVelocity * ammoVelocity * KocmoCannon.fixDamage * hullPenetration * fireRoundPerSecond * repeating); // 10214
        public static readonly int shieldDPS = (int)(ammoVelocity * ammoVelocity * KocmoCannon.fixDamage * shieldPenetration * fireRoundPerSecond * repeating); // 1380
    }
    // KRG 極大磁軌炮 + 遠距雷達/中距雷達
    public static class KocmoMegaRailgun
    {
        public static readonly float projectileSpread = 0.16f; //彈道散布 degree
        public static readonly int ammoVelocity = 5050; // 飛行速率 m/s
        public static readonly int propulsion = ammoVelocity * 50; // 開火推力 propulsion = ammoVelocity / Time.fixedDeltaTime (1/0.02 = 50)
        public static readonly float flightTime = 0.5f; // 飛行時間 sec
        public static readonly float operationalRange = ammoVelocity * flightTime; // 射程 m
        public static readonly float fireRoundPerSecond = 0.7f; // 射速 rps
        public static readonly float rateFire = 1 / fireRoundPerSecond; // 開火速率 sec
        public static readonly int repeating = 2; // 連發次數
        public static readonly float hullPenetration = 2.4f; // 機甲穿透
        public static readonly float shieldPenetration = 3.3f; // 護盾穿透
        public static readonly int hullDamage = (int)(ammoVelocity * ammoVelocity * KocmoCannon.fixDamage * hullPenetration); // 4040
        public static readonly int shieldDamage = (int)(ammoVelocity * ammoVelocity * KocmoCannon.fixDamage * shieldPenetration); // 5554
        public static readonly int hullDPS = (int)(ammoVelocity * ammoVelocity * KocmoCannon.fixDamage * hullPenetration * fireRoundPerSecond * repeating); // DPS 5655
        public static readonly int shieldDPS = (int)(ammoVelocity * ammoVelocity * KocmoCannon.fixDamage * shieldPenetration * fireRoundPerSecond * repeating); // DPS 7776
    }
    // KL 重型激光炮 + 中距雷達/短距雷達
    public static class KocmoHeavyLaser
    {
        public static readonly float projectileSpread = 0.21f; //彈道散布 degree
        public static readonly int ammoVelocity = 3950; // 飛行速率 m/s
        public static readonly int propulsion = ammoVelocity * 50; // 開火推力 propulsion = ammoVelocity / Time.fixedDeltaTime (1/0.02 = 50)
        public static readonly float flightTime = 0.37f; // 飛行時間 sec
        public static readonly float operationalRange = ammoVelocity * flightTime; // 射程 m
        public static readonly float fireRoundPerSecond = 1.0f; // 射速 rps
        public static readonly float rateFire = 1 / fireRoundPerSecond; // 開火速率 sec
        public static readonly int repeating = 3; // 連發次數
        public static readonly float hullPenetration = 0.72f; // 機甲穿透
        public static readonly float shieldPenetration = 2.2f; // 護盾穿透
        public static readonly int hullDamage = (int)(ammoVelocity * ammoVelocity * KocmoCannon.fixDamage * hullPenetration); // 741
        public static readonly int shieldDamage = (int)(ammoVelocity * ammoVelocity * KocmoCannon.fixDamage * shieldPenetration); // 2265
        public static readonly int hullDPS = (int)(ammoVelocity * ammoVelocity * KocmoCannon.fixDamage * hullPenetration * fireRoundPerSecond * repeating); // 2224
        public static readonly int shieldDPS = (int)(ammoVelocity * ammoVelocity * KocmoCannon.fixDamage * shieldPenetration * fireRoundPerSecond * repeating); // 6796
    }
    // KL 激光炮 + 短距雷達/超廣角雷達
    public static class KocmoLaser
    {
        public static readonly float projectileSpread = 0.39f; //彈道散布 degree
        public static readonly int ammoVelocity = 2370; // 飛行速率 m/s
        public static readonly int propulsion = ammoVelocity * 50; // 開火推力 propulsion = ammoVelocity / Time.fixedDeltaTime (1/0.02 = 50)
        public static readonly float flightTime = 0.37f; // 飛行時間 sec
        public static readonly float operationalRange = ammoVelocity * flightTime; // 射程 m
        public static readonly float fireRoundPerSecond = 7.1f; // 射速 rps
        public static readonly float rateFire = 1 / fireRoundPerSecond; // 開火速率 sec
        public static readonly int repeating = 1; // 連發次數
        public static readonly float hullPenetration = 1.3f; // 機甲穿透 DPS 3422
        public static readonly float shieldPenetration = 4.3f; // 護盾穿透 DPS 11318
        public static readonly int hullDamage = (int)(ammoVelocity * ammoVelocity * KocmoCannon.fixDamage * hullPenetration);
        public static readonly int shieldDamage = (int)(ammoVelocity * ammoVelocity * KocmoCannon.fixDamage * shieldPenetration);
        public static readonly int hullDPS = (int)(ammoVelocity * ammoVelocity * KocmoCannon.fixDamage * hullPenetration * fireRoundPerSecond * repeating);
        public static readonly int shieldDPS = (int)(ammoVelocity * ammoVelocity * KocmoCannon.fixDamage * shieldPenetration * fireRoundPerSecond * repeating);
    }
    // KUPP 超高功率電漿炮 + 短距雷達/超廣角雷達
    public static class KocmoUltraPowerPlasma
    {
        public static readonly float projectileSpread = 1.0f; //彈道散布 degree
        public static readonly int ammoVelocity = 3370; // 飛行速率 m/s
        public static readonly int propulsion = ammoVelocity * 50; // 開火推力 propulsion = ammoVelocity / Time.fixedDeltaTime (1/0.02 = 50)
        public static readonly float flightTime = 0.37f; // 飛行時間 sec
        public static readonly float operationalRange = ammoVelocity * flightTime; // 射程 m
        public static readonly float fireRoundPerSecond = 10.0f; // 射速 rps
        public static readonly float rateFire = 1 / fireRoundPerSecond; // 開火速率 sec
        public static readonly int repeating = 1; // 連發次數
        public static readonly float hullPenetration = 0.37f; // 機甲穿透
        public static readonly float shieldPenetration = 5.2f; // 護盾穿透
        public static readonly int hullDamage = (int)(ammoVelocity * ammoVelocity * KocmoCannon.fixDamage * hullPenetration); // 277
        public static readonly int shieldDamage = (int)(ammoVelocity * ammoVelocity * KocmoCannon.fixDamage * shieldPenetration); // 3898
        public static readonly int hullDPS = (int)(ammoVelocity * ammoVelocity * KocmoCannon.fixDamage * hullPenetration * fireRoundPerSecond * repeating); // 2773
        public static readonly int shieldDPS = (int)(ammoVelocity * ammoVelocity * KocmoCannon.fixDamage * shieldPenetration * fireRoundPerSecond * repeating); // 38977
    }
    // KAP 穿甲機炮 + 短距雷達/超廣角雷達
    public static class KocmoArmorPiercing
    {
        public static readonly float projectileSpread = 2.37f; //彈道散布 degree
        public static readonly int ammoVelocity = 2970; // 飛行速率 m/s
        public static readonly int propulsion = ammoVelocity * 50; // 開火推力 propulsion = ammoVelocity / Time.fixedDeltaTime (1/0.02 = 50)
        public static readonly float flightTime = 0.37f; // 飛行時間 sec
        public static readonly float operationalRange = ammoVelocity * flightTime; // 射程 m
        public static readonly float fireRoundPerSecond = 9.3f; // 射速 rps
        public static readonly float rateFire = 1 / fireRoundPerSecond; // 開火速率 sec
        public static readonly int repeating = 1; // 連發次數
        public static readonly float hullPenetration = 3.7f; // 機甲穿透 DPS 20233
        public static readonly float shieldPenetration = 0.66f; // 護盾穿透  DPS 3573
        public static readonly int hullDamage = (int)(ammoVelocity * ammoVelocity * KocmoCannon.fixDamage * hullPenetration);
        public static readonly int shieldDamage = (int)(ammoVelocity * ammoVelocity * KocmoCannon.fixDamage * shieldPenetration);
        public static readonly int hullDPS = (int)(ammoVelocity * ammoVelocity * KocmoCannon.fixDamage * hullPenetration * fireRoundPerSecond * repeating);
        public static readonly int shieldDPS = (int)(ammoVelocity * ammoVelocity * KocmoCannon.fixDamage * shieldPenetration * fireRoundPerSecond * repeating);
    }
}
