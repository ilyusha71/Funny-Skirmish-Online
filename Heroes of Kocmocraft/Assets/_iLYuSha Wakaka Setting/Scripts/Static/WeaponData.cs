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
        public static readonly int maxAmmoCapacity = 999; // 最大載彈量

        public static readonly float MaxAutoAimAngle = 3.0f; // 自动瞄准极限
        public static readonly int RoundsPerMinute = 584; // 开火射速 rpm
        public static readonly int RepeatingCount = 1; //连发模式
        public static readonly float MaxProjectileSpread = 0.39f; //彈道散布 degree
        public static readonly int ShockwaveDistance = 100; // 震波距离
        public static readonly int AmmoVelocity = 2970; // 飛行速率 m/s
        public static readonly float FlightTime = 0.37f;//0.71f; // 飛行時間 sec
        public static readonly float PenetrationShield = 4.1949f;
        public static readonly float PenetrationHull = 1.3035f;
    }
    public static class KocmoRocketLauncher
    {
        public static readonly int countPerBatch = 1; // 每批生成量
        public static readonly int maxPoorInventory = 100; // 最大物件池存量
        public static readonly float maxFireAngle = Mathf.Cos(13 * Mathf.Deg2Rad); // 最大開火夾角
        public static readonly int maxAmmoCapacity = 12; // 最大載彈量
        public static readonly int flightVelocity = 999; // 飛行速率 m/s
        public static readonly int thrust = flightVelocity * 50; // 基本推力
        public static readonly float FlightTime = 1.37f;//2.37f; // 飛行時間 sec
        public static readonly float FireRoundPerSecond = 3.37f; // 開火射速（每秒X發）RPS
        public static readonly float FireRate = 1 / FireRoundPerSecond; // 開火頻率（每X秒一輪）
        public static readonly float timeReload = 9.3f;
        public static WaitForSeconds waitRecovery = new WaitForSeconds(FlightTime);
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
        public static readonly float FlightTime = 7.0f; // 飛行時間 sec
        public static readonly float FireRoundPerSecond = 1.137f; // 開火射速（每秒X發）RPS
        public static readonly float FireRate = 1 / FireRoundPerSecond; // 開火頻率（每X秒一輪）
        public static readonly float timeReload = 15.0f;
        public static WaitForSeconds waitRecovery = new WaitForSeconds(FlightTime);
        public static readonly float coefficientDamageBasic = 7.0f;
        public static readonly float coefficientDamageHull = 1.37f;
        public static readonly float coefficientDamageShield = 2.37f;
    }


    // DTG 惡魔的溫情目光 + 極遠距雷達/超遠距雷達
    public static class DevilTenderGazer
    {
        public static readonly float MaxAutoAimAngle = 0.5f; // 自动瞄准极限
        public static readonly int RoundsPerMinute = 23; // 开火射速 rpm
        public static readonly int RepeatingCount = 1; //连发模式
        public static readonly float MaxProjectileSpread = 0.09f; // 发射散布
        public static readonly int ShockwaveDistance = 700; // 震波距离
        public static readonly int AmmoVelocity = 7990; // 飞行速率 mps
        public static readonly float FlightTime = 0.5f; // 飛行時間 sec
        public static readonly float RaySize = 7.0f;
        public static readonly float PenetrationShield = 1.97f; // 護盾穿透
        public static readonly float PenetrationHull = 3.33f; // 機甲穿透
    }
    // SMR 制裁者的極大磁軌炮 + 極遠距雷達/超遠距雷達/遠距雷達
    public static class SanctionerMegaRailgun
    {
        public static readonly float MaxAutoAimAngle = 0.7f; // 自动瞄准极限
        public static readonly int RoundsPerMinute = 20; // 开火射速 rpm
        public static readonly int RepeatingCount = 2; // 连发模式
        public static readonly float MaxProjectileSpread = 0.13f; // 发射散布
        public static readonly int ShockwaveDistance = 500; // 震波距离
        public static readonly int AmmoVelocity = 6670; // 飞行速率 mps
        public static readonly float FlightTime = 0.5f; // 飛行時間 sec
        public static readonly float RaySize = 5.0f;
        public static readonly float PenetrationShield = 1.7f; // 護盾穿透
        public static readonly float PenetrationHull = 2.4f; // 機甲穿透
    }

    // KIB 卡斯摩的離子脈衝 + 遠距雷達/中距雷達/短距雷達
    public static class KocmoIonPulsar
    {
        public static readonly float MaxAutoAimAngle = 1.6f; // 自动瞄准极限
        public static readonly int RoundsPerMinute = 77; // 开火射速 rpm
        public static readonly int RepeatingCount = 3; // 连发模式
        public static readonly float MaxProjectileSpread = 0.93f; // 发射散布
        public static readonly int ShockwaveDistance = 100; // 震波距离
        public static readonly int AmmoVelocity = 5710; // 飞行速率 mps
        public static readonly float FlightTime = 0.5f; // 飛行時間 sec
        public static readonly float RaySize = 0.5f;
        public static readonly float PenetrationShield = 5.9f; // 護盾穿透
        public static readonly float PenetrationHull = 0.49f; // 機甲穿透
    }
    // KR 磁軌砲 + 中距雷達
    public static class KocmoRailgun
    {
        public static readonly float MaxProjectileSpread = 0.36f; //彈道散布 degree
        public static readonly int ShockwaveDistance = 100; // 震波距离
        public static readonly int AmmoVelocity = 4550; // 飛行速率 m/s
        public static readonly int propulsion = AmmoVelocity * 50; // 開火推力 propulsion = AmmoVelocity / Time.fixedDeltaTime (1/0.02 = 50)
        public static readonly float FlightTime = 0.37f; // 飛行時間 sec
        public static readonly float RaySize = 0.5f;
        public static readonly float operationalRange = AmmoVelocity * FlightTime; // 射程 m
        public static readonly float FireRoundPerSecond = 3.0f; // 開火射速（每秒X發）RPS
        public static readonly float FireRate = 1 / FireRoundPerSecond; // 開火頻率（每X秒一輪）
        public static readonly int RepeatingCount = 1; // 每輪連發射擊次數
        public static WaitForSeconds waitRecovery = new WaitForSeconds(FlightTime);
        public static readonly float PenetrationHull = 1.7f; // 機甲穿透
        public static readonly float PenetrationShield = 0.73f; // 護盾穿透
        public static readonly int hullDamage = (int)(AmmoVelocity * AmmoVelocity * KocmoCannon.fixDamage * PenetrationHull); // 2323
        public static readonly int shieldDamage = (int)(AmmoVelocity * AmmoVelocity * KocmoCannon.fixDamage * PenetrationShield); // 997
        public static readonly int hullDPS = (int)(AmmoVelocity * AmmoVelocity * KocmoCannon.fixDamage * PenetrationHull * FireRoundPerSecond * RepeatingCount); // 6968
        public static readonly int shieldDPS = (int)(AmmoVelocity * AmmoVelocity * KocmoCannon.fixDamage * PenetrationShield * FireRoundPerSecond * RepeatingCount); // 2992
    }

    // KL 激光炮 + 短距雷達/超廣角雷達
    public static class KocmoLaser
    {
        public static readonly float MaxProjectileSpread = 0.39f; //彈道散布 degree
        public static readonly int ShockwaveDistance = 100; // 震波距离
        public static readonly int AmmoVelocity = 2370; // 飛行速率 m/s
        public static readonly int propulsion = AmmoVelocity * 50; // 開火推力 propulsion = AmmoVelocity / Time.fixedDeltaTime (1/0.02 = 50)
        public static readonly float FlightTime = 0.37f; // 飛行時間 sec
        public static readonly float RaySize = 0.5f;
        public static readonly float operationalRange = AmmoVelocity * FlightTime; // 射程 m
        public static readonly float FireRoundPerSecond = 7.1f; // 開火射速（每秒X發）RPS
        public static readonly float FireRate = 1 / FireRoundPerSecond; // 開火頻率（每X秒一輪）
        public static readonly int RepeatingCount = 1; // 每輪連發射擊次數
        public static WaitForSeconds waitRecovery = new WaitForSeconds(FlightTime);
        public static readonly float PenetrationHull = 1.3f; // 機甲穿透
        public static readonly float PenetrationShield = 4.3f; // 護盾穿透
        public static readonly int hullDamage = (int)(AmmoVelocity * AmmoVelocity * KocmoCannon.fixDamage * PenetrationHull); // 482
        public static readonly int shieldDamage = (int)(AmmoVelocity * AmmoVelocity * KocmoCannon.fixDamage * PenetrationShield); // 1594
        public static readonly int hullDPS = (int)(AmmoVelocity * AmmoVelocity * KocmoCannon.fixDamage * PenetrationHull * FireRoundPerSecond * RepeatingCount); // 3422
        public static readonly int shieldDPS = (int)(AmmoVelocity * AmmoVelocity * KocmoCannon.fixDamage * PenetrationShield * FireRoundPerSecond * RepeatingCount); // 11318
    }
    // KUPP 超高功率電漿炮 + 短距雷達/超廣角雷達
    public static class KocmoUltraPowerPlasma
    {
        public static readonly float MaxProjectileSpread = 1.0f; //彈道散布 degree
        public static readonly int ShockwaveDistance = 100; // 震波距离
        public static readonly int AmmoVelocity = 3370; // 飛行速率 m/s
        public static readonly int propulsion = AmmoVelocity * 50; // 開火推力 propulsion = AmmoVelocity / Time.fixedDeltaTime (1/0.02 = 50)
        public static readonly float FlightTime = 0.37f; // 飛行時間 sec
        public static readonly float RaySize = 0.5f;
        public static readonly float operationalRange = AmmoVelocity * FlightTime; // 射程 m
        public static readonly float FireRoundPerSecond = 10.0f; // 開火射速（每秒X發）RPS
        public static readonly float FireRate = 1 / FireRoundPerSecond; // 開火頻率（每X秒一輪）
        public static readonly int RepeatingCount = 1; // 每輪連發射擊次數
        public static WaitForSeconds waitRecovery = new WaitForSeconds(FlightTime);
        public static readonly float PenetrationHull = 0.37f; // 機甲穿透
        public static readonly float PenetrationShield = 5.2f; // 護盾穿透
        public static readonly int hullDamage = (int)(AmmoVelocity * AmmoVelocity * KocmoCannon.fixDamage * PenetrationHull); // 277
        public static readonly int shieldDamage = (int)(AmmoVelocity * AmmoVelocity * KocmoCannon.fixDamage * PenetrationShield); // 3898
        public static readonly int hullDPS = (int)(AmmoVelocity * AmmoVelocity * KocmoCannon.fixDamage * PenetrationHull * FireRoundPerSecond * RepeatingCount); // 2773
        public static readonly int shieldDPS = (int)(AmmoVelocity * AmmoVelocity * KocmoCannon.fixDamage * PenetrationShield * FireRoundPerSecond * RepeatingCount); // 38977
    }
    // KAP 穿甲機炮 + 短距雷達/超廣角雷達
    public static class KocmoArmorPiercing
    {
        public static readonly float MaxProjectileSpread = 2.37f; //彈道散布 degree
        public static readonly int ShockwaveDistance = 100; // 震波距离
        public static readonly int AmmoVelocity = 2970; // 飛行速率 m/s
        public static readonly int propulsion = AmmoVelocity * 50; // 開火推力 propulsion = AmmoVelocity / Time.fixedDeltaTime (1/0.02 = 50)
        public static readonly float FlightTime = 0.37f; // 飛行時間 sec
        public static readonly float RaySize = 0.5f;
        public static readonly float operationalRange = AmmoVelocity * FlightTime; // 射程 m
        public static readonly float FireRoundPerSecond = 9.3f; // 開火射速（每秒X發）RPS
        public static readonly float FireRate = 1 / FireRoundPerSecond; // 開火頻率（每X秒一輪）
        public static readonly int RepeatingCount = 1; // 每輪連發射擊次數
        public static WaitForSeconds waitRecovery = new WaitForSeconds(FlightTime);
        public static readonly float PenetrationHull = 3.7f; // 機甲穿透 DPS 20233
        public static readonly float PenetrationShield = 0.66f; // 護盾穿透  DPS 3573
        public static readonly int hullDamage = (int)(AmmoVelocity * AmmoVelocity * KocmoCannon.fixDamage * PenetrationHull); // 2154
        public static readonly int shieldDamage = (int)(AmmoVelocity * AmmoVelocity * KocmoCannon.fixDamage * PenetrationShield); // 384
        public static readonly int hullDPS = (int)(AmmoVelocity * AmmoVelocity * KocmoCannon.fixDamage * PenetrationHull * FireRoundPerSecond * RepeatingCount); // 21541
        public static readonly int shieldDPS = (int)(AmmoVelocity * AmmoVelocity * KocmoCannon.fixDamage * PenetrationShield * FireRoundPerSecond * RepeatingCount); // 3842
    }



    public static class EX
    {
        public static readonly float MaxAutoAimAngle = 0.5f; // 自动瞄准极限
        public static readonly float MaxAutoAimRange = Mathf.Cos(MaxAutoAimAngle * Mathf.Deg2Rad); // 最大自動瞄準範圍
        public static readonly float FireRoundPerSecond = 0.37f; // 開火射速（每秒X發）RPS
        public static readonly float FireRate = 1 / FireRoundPerSecond; // 開火頻率（每X秒一輪）
        public static readonly int RepeatingCount = 1; // 每輪連發射擊次數
        public static readonly float MaxProjectileSpread = 0.09f; // 最大發散夾角
        public static readonly int AmmoVelocity = 7990; // 飛行速率 m/s
        public static readonly float RaySize = 0.5f;
        public static readonly int propulsion = AmmoVelocity * 50; // 開火推力 propulsion = AmmoVelocity / Time.fixedDeltaTime (1/0.02 = 50)
        public static readonly float FlightTime = 0.5f; // 飛行時間 sec
        public static readonly float operationalRange = AmmoVelocity * FlightTime; // 射程 m（2管3995 / 4管3355 / 6管3076）
        public static WaitForSeconds waitRecovery = new WaitForSeconds(FlightTime);
        public static readonly float PenetrationHull = 3.33f; // 機甲穿透
        public static readonly float PenetrationShield = 1.97f; // 護盾穿透
        public static readonly int hullDamage = (int)(AmmoVelocity * AmmoVelocity * KocmoCannon.fixDamage * PenetrationHull); // 16812
        public static readonly int shieldDamage = (int)(AmmoVelocity * AmmoVelocity * KocmoCannon.fixDamage * PenetrationShield); // 9438
        public static readonly int hullDPS = (int)(AmmoVelocity * AmmoVelocity * KocmoCannon.fixDamage * PenetrationHull * FireRoundPerSecond * RepeatingCount); // 10913
        public static readonly int shieldDPS = (int)(AmmoVelocity * AmmoVelocity * KocmoCannon.fixDamage * PenetrationShield * FireRoundPerSecond * RepeatingCount); // 2271
    }
}
