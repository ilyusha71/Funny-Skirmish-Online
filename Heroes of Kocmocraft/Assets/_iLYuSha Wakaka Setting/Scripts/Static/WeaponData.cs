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
        public static readonly float MaxProjectileSpread = 0.39f; //彈道散布 degree
        public static readonly int flightVelocity = 2970; // 飛行速率 m/s
        public static readonly int propulsion = flightVelocity * 50; // 開火推力 propulsion = flightVelocity / Time.fixedDeltaTime (1/0.02 = 50)
        public static readonly float flightTime = 0.37f;//0.71f; // 飛行時間 sec
        public static readonly float operationalRange = flightVelocity * flightTime; // 射程 m
        public static readonly float FireRoundPerSecond = 9.73f; // 開火射速（每秒X發）RPS
        public static readonly float FireRate = 1 / FireRoundPerSecond; // 開火頻率（每X秒一輪）
        public static WaitForSeconds waitRecovery = new WaitForSeconds(flightTime);
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
        public static readonly float FireRoundPerSecond = 3.37f; // 開火射速（每秒X發）RPS
        public static readonly float FireRate = 1 / FireRoundPerSecond; // 開火頻率（每X秒一輪）
        public static readonly float timeReload = 9.3f;
        public static WaitForSeconds waitRecovery = new WaitForSeconds(flightTime);
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
        public static readonly float FireRoundPerSecond = 1.137f; // 開火射速（每秒X發）RPS
        public static readonly float FireRate = 1 / FireRoundPerSecond; // 開火頻率（每X秒一輪）
        public static readonly float timeReload = 15.0f;
        public static WaitForSeconds waitRecovery = new WaitForSeconds(flightTime);
        public static readonly float coefficientDamageBasic = 7.0f;
        public static readonly float coefficientDamageHull = 1.37f;
        public static readonly float coefficientDamageShield = 2.37f;
    }




    // DTG 惡魔溫情的目光 + 極遠距雷達/超遠距雷達
    public static class DevilTenderGazer
    {
        public static readonly float MaxAutoAimRange = Mathf.Cos(0.5f * Mathf.Deg2Rad); // 最大自動瞄準範圍
        public static readonly float MaxProjectileSpread = 0.09f; // 最大發散夾角
        public static readonly int ammoVelocity = 7990; // 飛行速率 m/s
        public static readonly int propulsion = ammoVelocity * 50; // 開火推力 propulsion = ammoVelocity / Time.fixedDeltaTime (1/0.02 = 50)
        public static readonly float flightTime = 0.5f; // 飛行時間 sec
        public static readonly float operationalRange = ammoVelocity * flightTime; // 射程 m（2管3995 / 4管3355 / 6管3076）
        public static readonly float FireRoundPerSecond = 0.37f; // 開火射速（每秒X發）RPS
        public static readonly float FireRate = 1 / FireRoundPerSecond; // 開火頻率（每X秒一輪）
        public static readonly int RepeatingCount = 1; // 每輪連發射擊次數
        public static WaitForSeconds waitRecovery = new WaitForSeconds(flightTime);
        public static readonly float hullPenetration = 3.33f; // 機甲穿透
        public static readonly float shieldPenetration = 1.97f; // 護盾穿透
        public static readonly int hullDamage = (int)(ammoVelocity * ammoVelocity * KocmoCannon.fixDamage * hullPenetration); // 16812
        public static readonly int shieldDamage = (int)(ammoVelocity * ammoVelocity * KocmoCannon.fixDamage * shieldPenetration); // 9438
        public static readonly int hullDPS = (int)(ammoVelocity * ammoVelocity * KocmoCannon.fixDamage * hullPenetration * FireRoundPerSecond * RepeatingCount); // 10913
        public static readonly int shieldDPS = (int)(ammoVelocity * ammoVelocity * KocmoCannon.fixDamage * shieldPenetration * FireRoundPerSecond * RepeatingCount); // 2271
    }
    // KRG 極大磁軌炮 + 極遠距雷達/超遠距雷達/遠距雷達
    public static class KocmoMegaRailgun
    {
        public static readonly float MaxAutoAimRange = Mathf.Cos(0.7f * Mathf.Deg2Rad); // 最大自動瞄準範圍
        public static readonly float FireRoundPerSecond = 0.37f; // 開火射速（每秒X發）RPS
        public static readonly float FireRate = 1 / FireRoundPerSecond; // 開火頻率（每X秒一輪）
        public static readonly int RepeatingCount = 2; // 每輪連發射擊次數
        public static readonly float MaxProjectileSpread = 0.13f; // 最大發散夾角
        public static readonly int ammoVelocity = 6670; // 飛行速率 m/s
        public static readonly int propulsion = ammoVelocity * 50; // 開火推力 propulsion = ammoVelocity / Time.fixedDeltaTime (1/0.02 = 50)
        public static readonly float flightTime = 0.5f; // 飛行時間 sec
        public static readonly float operationalRange = ammoVelocity * flightTime; // 射程 m（2管3335 /  4管2801 / 6管2568）
        public static WaitForSeconds waitRecovery = new WaitForSeconds(flightTime);
        public static readonly float hullPenetration = 2.4f; // 機甲穿透
        public static readonly float shieldPenetration = 1.7f; // 護盾穿透
        public static readonly int hullDamage = (int)(ammoVelocity * ammoVelocity * KocmoCannon.fixDamage * hullPenetration); // 7047
        public static readonly int shieldDamage = (int)(ammoVelocity * ammoVelocity * KocmoCannon.fixDamage * shieldPenetration); // 4992
        public static readonly int hullDPS = (int)(ammoVelocity * ammoVelocity * KocmoCannon.fixDamage * hullPenetration * FireRoundPerSecond * RepeatingCount); // 8456
        public static readonly int shieldDPS = (int)(ammoVelocity * ammoVelocity * KocmoCannon.fixDamage * shieldPenetration * FireRoundPerSecond * RepeatingCount); // 5990
    }

    // KHIB 高速離子炮 + 遠距雷達/中距雷達/短距雷達
    // 適合機甲不高、雙管機炮具速度優勢機種
    public static class KocmoHighspeedIonBlaster
    {
        public static readonly float FireRoundPerSecond = 1.3f; // 開火射速（每秒X發）RPS
        public static readonly float FireRate = 1 / FireRoundPerSecond; // 開火頻率（每X秒一輪）
        public static readonly int RepeatingCount = 3; // 每輪連發射擊次數
        public static readonly float MaxProjectileSpread = 0.93f; //彈道散布 degree
        public static readonly int ammoVelocity = 5710; // 飛行速率 m/s
        public static readonly int propulsion = ammoVelocity * 50; // 開火推力 propulsion = ammoVelocity / Time.fixedDeltaTime (1/0.02 = 50)
        public static readonly float flightTime = 0.5f; // 飛行時間 sec
        public static readonly float operationalRange = ammoVelocity * flightTime; // 射程 m（2管2820 /  4管2368 / 6管2198）
        public static WaitForSeconds waitRecovery = new WaitForSeconds(flightTime);
        public static readonly float hullPenetration = 0.49f; // 機甲穿透
        public static readonly float shieldPenetration = 5.9f; // 護盾穿透
        public static readonly int hullDamage = (int)(ammoVelocity * ammoVelocity * KocmoCannon.fixDamage * hullPenetration); // 839
        public static readonly int shieldDamage = (int)(ammoVelocity * ammoVelocity * KocmoCannon.fixDamage * shieldPenetration); // 12696
        public static readonly int hullDPS = (int)(ammoVelocity * ammoVelocity * KocmoCannon.fixDamage * hullPenetration * FireRoundPerSecond * RepeatingCount); // 3273
        public static readonly int shieldDPS = (int)(ammoVelocity * ammoVelocity * KocmoCannon.fixDamage * shieldPenetration * FireRoundPerSecond * RepeatingCount); // 49515
    }
    // KR 磁軌砲 + 中距雷達
    public static class KocmoRailgun
    {
        public static readonly float MaxProjectileSpread = 0.36f; //彈道散布 degree
        public static readonly int ammoVelocity = 4550; // 飛行速率 m/s
        public static readonly int propulsion = ammoVelocity * 50; // 開火推力 propulsion = ammoVelocity / Time.fixedDeltaTime (1/0.02 = 50)
        public static readonly float flightTime = 0.37f; // 飛行時間 sec
        public static readonly float operationalRange = ammoVelocity * flightTime; // 射程 m
        public static readonly float FireRoundPerSecond = 3.0f; // 開火射速（每秒X發）RPS
        public static readonly float FireRate = 1 / FireRoundPerSecond; // 開火頻率（每X秒一輪）
        public static readonly int RepeatingCount = 1; // 每輪連發射擊次數
        public static WaitForSeconds waitRecovery = new WaitForSeconds(flightTime);
        public static readonly float hullPenetration = 1.7f; // 機甲穿透
        public static readonly float shieldPenetration = 0.73f; // 護盾穿透
        public static readonly int hullDamage = (int)(ammoVelocity * ammoVelocity * KocmoCannon.fixDamage * hullPenetration); // 2323
        public static readonly int shieldDamage = (int)(ammoVelocity * ammoVelocity * KocmoCannon.fixDamage * shieldPenetration); // 997
        public static readonly int hullDPS = (int)(ammoVelocity * ammoVelocity * KocmoCannon.fixDamage * hullPenetration * FireRoundPerSecond * RepeatingCount); // 6968
        public static readonly int shieldDPS = (int)(ammoVelocity * ammoVelocity * KocmoCannon.fixDamage * shieldPenetration * FireRoundPerSecond * RepeatingCount); // 2992
    }

    // KL 激光炮 + 短距雷達/超廣角雷達
    public static class KocmoLaser
    {
        public static readonly float MaxProjectileSpread = 0.39f; //彈道散布 degree
        public static readonly int ammoVelocity = 2370; // 飛行速率 m/s
        public static readonly int propulsion = ammoVelocity * 50; // 開火推力 propulsion = ammoVelocity / Time.fixedDeltaTime (1/0.02 = 50)
        public static readonly float flightTime = 0.37f; // 飛行時間 sec
        public static readonly float operationalRange = ammoVelocity * flightTime; // 射程 m
        public static readonly float FireRoundPerSecond = 7.1f; // 開火射速（每秒X發）RPS
        public static readonly float FireRate = 1 / FireRoundPerSecond; // 開火頻率（每X秒一輪）
        public static readonly int RepeatingCount = 1; // 每輪連發射擊次數
        public static WaitForSeconds waitRecovery = new WaitForSeconds(flightTime);
        public static readonly float hullPenetration = 1.3f; // 機甲穿透
        public static readonly float shieldPenetration = 4.3f; // 護盾穿透
        public static readonly int hullDamage = (int)(ammoVelocity * ammoVelocity * KocmoCannon.fixDamage * hullPenetration); // 482
        public static readonly int shieldDamage = (int)(ammoVelocity * ammoVelocity * KocmoCannon.fixDamage * shieldPenetration); // 1594
        public static readonly int hullDPS = (int)(ammoVelocity * ammoVelocity * KocmoCannon.fixDamage * hullPenetration * FireRoundPerSecond * RepeatingCount); // 3422
        public static readonly int shieldDPS = (int)(ammoVelocity * ammoVelocity * KocmoCannon.fixDamage * shieldPenetration * FireRoundPerSecond * RepeatingCount); // 11318
    }
    // KUPP 超高功率電漿炮 + 短距雷達/超廣角雷達
    public static class KocmoUltraPowerPlasma
    {
        public static readonly float MaxProjectileSpread = 1.0f; //彈道散布 degree
        public static readonly int ammoVelocity = 3370; // 飛行速率 m/s
        public static readonly int propulsion = ammoVelocity * 50; // 開火推力 propulsion = ammoVelocity / Time.fixedDeltaTime (1/0.02 = 50)
        public static readonly float flightTime = 0.37f; // 飛行時間 sec
        public static readonly float operationalRange = ammoVelocity * flightTime; // 射程 m
        public static readonly float FireRoundPerSecond = 10.0f; // 開火射速（每秒X發）RPS
        public static readonly float FireRate = 1 / FireRoundPerSecond; // 開火頻率（每X秒一輪）
        public static readonly int RepeatingCount = 1; // 每輪連發射擊次數
        public static WaitForSeconds waitRecovery = new WaitForSeconds(flightTime);
        public static readonly float hullPenetration = 0.37f; // 機甲穿透
        public static readonly float shieldPenetration = 5.2f; // 護盾穿透
        public static readonly int hullDamage = (int)(ammoVelocity * ammoVelocity * KocmoCannon.fixDamage * hullPenetration); // 277
        public static readonly int shieldDamage = (int)(ammoVelocity * ammoVelocity * KocmoCannon.fixDamage * shieldPenetration); // 3898
        public static readonly int hullDPS = (int)(ammoVelocity * ammoVelocity * KocmoCannon.fixDamage * hullPenetration * FireRoundPerSecond * RepeatingCount); // 2773
        public static readonly int shieldDPS = (int)(ammoVelocity * ammoVelocity * KocmoCannon.fixDamage * shieldPenetration * FireRoundPerSecond * RepeatingCount); // 38977
    }
    // KAP 穿甲機炮 + 短距雷達/超廣角雷達
    public static class KocmoArmorPiercing
    {
        public static readonly float MaxProjectileSpread = 2.37f; //彈道散布 degree
        public static readonly int ammoVelocity = 2970; // 飛行速率 m/s
        public static readonly int propulsion = ammoVelocity * 50; // 開火推力 propulsion = ammoVelocity / Time.fixedDeltaTime (1/0.02 = 50)
        public static readonly float flightTime = 0.37f; // 飛行時間 sec
        public static readonly float operationalRange = ammoVelocity * flightTime; // 射程 m
        public static readonly float FireRoundPerSecond = 9.3f; // 開火射速（每秒X發）RPS
        public static readonly float FireRate = 1 / FireRoundPerSecond; // 開火頻率（每X秒一輪）
        public static readonly int RepeatingCount = 1; // 每輪連發射擊次數
        public static WaitForSeconds waitRecovery = new WaitForSeconds(flightTime);
        public static readonly float hullPenetration = 3.7f; // 機甲穿透 DPS 20233
        public static readonly float shieldPenetration = 0.66f; // 護盾穿透  DPS 3573
        public static readonly int hullDamage = (int)(ammoVelocity * ammoVelocity * KocmoCannon.fixDamage * hullPenetration); // 2154
        public static readonly int shieldDamage = (int)(ammoVelocity * ammoVelocity * KocmoCannon.fixDamage * shieldPenetration); // 384
        public static readonly int hullDPS = (int)(ammoVelocity * ammoVelocity * KocmoCannon.fixDamage * hullPenetration * FireRoundPerSecond * RepeatingCount); // 21541
        public static readonly int shieldDPS = (int)(ammoVelocity * ammoVelocity * KocmoCannon.fixDamage * shieldPenetration * FireRoundPerSecond * RepeatingCount); // 3842
    }
}
