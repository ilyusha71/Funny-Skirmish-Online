using UnityEngine;

namespace Kocmoca
{
    /*****************************************************************************
     * 全域類
     * Last Updated: 2019-04-20
     * 透過靜態類別KocmocaData初始化一系列宇航機的ModuleData類別
     * 減少實例化新物件時重新透過Switch-Case逐一關聯
     * 
     * 但Component的變數仍需在實例物件時重新設定
     * ModuleData 直接赋值速度最快
     * ModuleData[] 慢一倍
     * List<ModuleData> 极慢
     *****************************************************************************/
    public static class KocmocaData
    {
        // 神偷机兵
        public static readonly ModuleData MinionArmor = new ModuleData(
            Type.MinionArmor,AstromechDroid.ZvezdarkDeluxe,RadarF.KocmoStrategyRadar, TurretF.KocmoLaserCannon);

        // 红牛能量
        public static readonly ModuleData RedBullEnergy = new ModuleData(
            Type.RedBullEnergy,AstromechDroid.ZvezdarkDeluxe,RadarF.KocmoStrategyRadar, TurretF.KocmoLaserCannon);

        // 普鲸
        public static readonly ModuleData VladimirPutin = new ModuleData(
            Type.VladimirPutin,AstromechDroid.StarDestroyerX,RadarF.MagicMirrorLongRangeRadar, TurretF.SanctionerMegaRailgun);

        // 纸飞机
        public static readonly ModuleData PaperAeroplane = new ModuleData(
            Type.PaperAeroplane,AstromechDroid.ZvezdarkDeluxe,RadarF.KocmoStrategyRadar, TurretF.KocmoLaserCannon);

        // 咕咕鸡
        public static readonly ModuleData Cuckoo = new ModuleData(
            Type.Cuckoo,AstromechDroid.ZvezdarkDeluxe,RadarF.MagicMirrorLongRangeRadar, TurretF.DevilTenderGazer);

        // 炮弹比尔
        public static readonly ModuleData BulletBill = new ModuleData(
            Type.BulletBill,AstromechDroid.ZvezdarkDeluxe,RadarF.KocmoStrategyRadar, TurretF.KocmoLaserCannon);

        // 时光机
        public static readonly ModuleData TimeMachine = new ModuleData(
            Type.TimeMachine,AstromechDroid.ZvezdarkDeluxe,RadarF.KocmoStrategyRadar, TurretF.KocmoLaserCannon);

        // 王牌狗屋
        public static readonly ModuleData AceKennel = new ModuleData(
            Type.AceKennel,AstromechDroid.ZvezdarkDeluxe,RadarF.KocmoStrategyRadar, TurretF.KocmoLaserCannon);

        // 卡比之星
        public static readonly ModuleData KirbyStar = new ModuleData(
            Type.KirbyStar,AstromechDroid.ZvezdarkDeluxe,RadarF.KocmoStrategyRadar, TurretF.KocmoLaserCannon);

        // 蝎红
        public static readonly ModuleData ScorpioRouge = new ModuleData(
            Type.ScorpioRouge,AstromechDroid.ZvezdarkDeluxe,RadarF.KocmoStrategyRadar, TurretF.KocmoLaserCannon);

        // 恩威迪亚
        public static readonly ModuleData nWidia = new ModuleData(
            Type.nWidia,AstromechDroid.ZvezdarkDeluxe,RadarF.KocmoStrategyRadar, TurretF.KocmoLaserCannon);

        // 快餐侠
        public static readonly ModuleData FastFoodMan = new ModuleData(
            Type.FastFoodMan,AstromechDroid.ZvezdarkDeluxe,RadarF.KocmoStrategyRadar, TurretF.KocmoLaserCannon);

        // 驯鹿空运
        public static readonly ModuleData ReindeerTransport = new ModuleData(
            Type.ReindeerTransport,AstromechDroid.ZvezdarkDeluxe,RadarF.KocmoStrategyRadar, TurretF.KocmoLaserCannon);

        // 北极星特快
        public static readonly ModuleData PolarisExpress = new ModuleData(
            Type.PolarisExpress,AstromechDroid.ZvezdarkDeluxe,RadarF.KocmoStrategyRadar, TurretF.KocmoLaserCannon);

        // 远古飞鱼
        public static readonly ModuleData AncientFish = new ModuleData(
            Type.AncientFish,AstromechDroid.ZvezdarkDeluxe,RadarF.KocmoStrategyRadar, TurretF.KocmoLaserCannon);

        // 玩具独角兽
        public static readonly ModuleData PapoyUnicorn = new ModuleData(
            Type.PapoyUnicorn,AstromechDroid.ZvezdarkGaming,RadarF.FantasticMirrorLongRangeRadar, TurretF.DevilTenderGazer);

        // 南瓜魅影
        public static readonly ModuleData PumpkinGhost = new ModuleData(
            Type.PumpkinGhost,AstromechDroid.StarDestroyerV,RadarF.MarksmanRangeRadar, TurretF.SanctionerMegaRailgun);

        // 赏金猎人
        public static readonly ModuleData BoundyHunterMKII = new ModuleData(
            Type.BoundyHunterMKII,AstromechDroid.ZvezdarkDeluxe,RadarF.KocmoStrategyRadar, TurretF.KocmoLaserCannon);

        // 鹰纽特
        public static readonly ModuleData InuitEagle = new ModuleData(
            Type.InuitEagle,AstromechDroid.ZvezdarkDeluxe,RadarF.KocmoStrategyRadar, TurretF.KocmoLaserCannon);

        // 新葡鲸
        public static readonly ModuleData GrandLisboa = new ModuleData(
            Type.GrandLisboa,AstromechDroid.ZvezdarkDeluxe,RadarF.KocmoStrategyRadar, TurretF.KocmoLaserCannon);

        // 原型机
        public static readonly ModuleData Prototype = new ModuleData(
            Type.Prototype,AstromechDroid.Prototype,RadarF.Prototype, TurretF.Prototype);


        public static readonly ModuleData[] KocmocraftData = new ModuleData[24]{
            MinionArmor,RedBullEnergy,VladimirPutin,PaperAeroplane,Cuckoo,
            BulletBill,TimeMachine,AceKennel,KirbyStar,ScorpioRouge,
            nWidia,FastFoodMan,ReindeerTransport,PolarisExpress,AncientFish,
            PapoyUnicorn,PumpkinGhost,BoundyHunterMKII,InuitEagle,GrandLisboa,
            Prototype,Prototype,Prototype,Prototype};
        #region Engine
        public static readonly EngineData Turbojet = new EngineData(EngineType.Turbojet);
        public static readonly EngineData Turbofan = new EngineData(EngineType.Turbofan);
        public static readonly EngineData Turboprop = new EngineData(EngineType.Turboprop);
        public static readonly EngineData Turboshaft = new EngineData(EngineType.Turboshaft);
        public static readonly EngineData IonThruster = new EngineData(EngineType.IonThruster);
        public static readonly EngineData BiomassEnergy = new EngineData(EngineType.BiomassEnergy);
        public static readonly EngineData PulsedPlasmaThruster = new EngineData(EngineType.PulsedPlasmaThruster);
        public static readonly EngineData[] EngineData = new EngineData[7] {
            Turbojet,Turbofan,Turboprop,Turboshaft,
            IonThruster,BiomassEnergy,PulsedPlasmaThruster};
        #endregion

        public static readonly Vector3 invisible = new Vector3(0, 9999, -1);
    }
    [System.Serializable]
    public class ModuleData
    {
        public Type Type;
        // Performance
        public readonly int Shield;
        public readonly int Hull;
        public readonly int CruiseSpeed;
        public readonly int AfterburnerSpeed;
        public readonly int Energy;
        // Kocmomech Droid
        public readonly string DroidName;
        public readonly string DroidDetail;
        public readonly int ShieldRecharge;
        public readonly int CollisionResistance;
        public readonly float EnginePower;
        public readonly float LockTime;
        // Radar
        public readonly string RadarName;
        public readonly string RadarDetail;
        public readonly int MaxSearchRadius;
        public readonly int MaxSearchRadiusSqr;
        public readonly int MinSearchRadius;
        public readonly int MinSearchRadiusSqr;
        public readonly int MaxSearchAngle;
        public readonly float MaxSearchRange;
        public readonly int MaxLockDistance;
        public readonly int MaxLockDistanceSqr;
        public readonly int MaxLockAngle;
        public readonly float MaxLockRange;
        // Turret
        public readonly string TurretName;
        public readonly int TurretCount;
        public readonly float Decay;
        public readonly string DecayVelocity;
        public readonly string DecayDamage;
        public readonly float MaxAutoAimAngle;
        public readonly float MaxAutoAimRange;
        // Turret - FCS
        public readonly float RoundsPerMinute;
        public readonly float FireRate;
        public readonly int RepeatingCount;
        public readonly float MaxProjectileSpread;
        public AudioClip FireSound;
        public readonly int ShockwaveDistance;
        // Turret - Ammo
        public readonly float AmmoVelocity;
        public readonly float propulsion;
        public readonly float FlightTime;
        public readonly float operationalRange;
        public readonly float RaySize;
        public readonly WaitForSeconds waitRecovery;
        // Turret - Damage
        public readonly float Damage;
        public readonly float DPS;
        public readonly int ShieldPenetration;
        public readonly int HullPenetration;

        public ModuleData(Type type, AstromechDroid droidType, RadarF radarType, TurretF turretType)
        {
            Type = type;
            switch (droidType)
            {
                case AstromechDroid.ZvezdarkDeluxe:
                    DroidName = "御星者修复机器喵 Deluxe";
                    DroidDetail = "";
                    ShieldRecharge = 309;
                    CollisionResistance = 93;
                    EnginePower = 7.9f;
                    LockTime = 1.23f;
                    break;
                case AstromechDroid.ZvezdarkGaming:
                    DroidName = "御星者修复机器喵 Gaming";
                    DroidDetail = "";
                    ShieldRecharge = 274;
                    CollisionResistance = 84;
                    EnginePower = 8.4f;
                    LockTime = 1.27f;
                    break;
                case AstromechDroid.ZvezdarkSouvenir:
                    DroidName = "御星者初代纪念款机器喵";
                    DroidDetail = "";
                    ShieldRecharge = 255;
                    CollisionResistance = 71;
                    EnginePower = 9.9f;
                    LockTime = 1.33f;
                    break;
                case AstromechDroid.StarDestroyerX:
                    DroidName = "歼星者X型战争技工";
                    DroidDetail = "";
                    ShieldRecharge = 93;
                    CollisionResistance = 22;
                    EnginePower = 10f;
                    LockTime = 0.16f;
                    break;
                case AstromechDroid.StarDestroyerV:
                    DroidName = "歼星者V型战争技工";
                    DroidDetail = "";
                    ShieldRecharge = 127;
                    CollisionResistance = 39;
                    EnginePower = 9f;
                    LockTime = 0.27f;
                    break;
                case AstromechDroid.Anonymous:
                    DroidName = "不愿具名的知情人士";
                    DroidDetail = "";
                    ShieldRecharge = 140;
                    CollisionResistance = 100;
                    EnginePower = 5.2f;
                    LockTime = 0.93f;
                    break;
                case AstromechDroid.AlphaZero:
                    DroidName = "Alpha Zero蓝星炒鸡计算机";
                    DroidDetail = "";
                    ShieldRecharge = 190;
                    CollisionResistance = 97;
                    EnginePower = 4.8f;
                    LockTime = 0.76f;
                    break;
                case AstromechDroid.Prototype:
                    DroidName = "未装配宇航技工";
                    DroidDetail = "";
                    ShieldRecharge = 0;
                    CollisionResistance = 0;
                    EnginePower = 0;
                    LockTime = 0;
                    break;
            }
            switch (radarType)
            {
                case RadarF.FantasticMirrorLongRangeRadar:
                    RadarName = "幻镜远距雷达";
                    RadarDetail = "基于魔镜雷达所发展的梦幻版本，专注于极远距离的威慑能力，但相对会在中距离与侧翼产生很大的盲区。";
                    MaxSearchRadius = 5000;
                    MinSearchRadius = 1900;
                    MaxSearchAngle = 16;
                    MaxLockDistance = 3200;
                    MaxLockAngle = 7;
                    break;
                case RadarF.MarksmanRangeRadar:
                    RadarName = "神射手远距雷达";
                    RadarDetail = "神射手是一套专门用于狙击系统武装的标配雷达。";
                    MaxSearchRadius = 4400;
                    MinSearchRadius = 1700;
                    MaxSearchAngle = 24;
                    MaxLockDistance = 2700;
                    MaxLockAngle = 11;
                    break;
                case RadarF.MagicMirrorLongRangeRadar:
                    RadarName = "魔镜远距雷达";
                    RadarDetail = "兼顾远距离打击的全方位雷达，也可用於狙击任务。";
                    MaxSearchRadius = 4200;
                    MinSearchRadius = 1100;
                    MaxSearchAngle = 27;
                    MaxLockDistance = 2500;
                    MaxLockAngle = 13;
                    break;
                case RadarF.BrokenLongRangeRadar:
                    RadarName = "破晓远距雷达";
                    RadarDetail = "破晓是针对指挥所掌控战场的多用途雷达，适合战术位置在大后方的宇航机配置。";
                    MaxSearchRadius = 3700;
                    MinSearchRadius = 700;
                    MaxSearchAngle = 29;
                    MaxLockDistance = 2300;
                    MaxLockAngle = 16;
                    break;
                case RadarF.KocmoStrategyRadar:
                    RadarName = "卡斯摩战术雷达";
                    RadarDetail = "多装配于警戒哨的制式雷达，同时也是一款通用于各项任务的战术雷达。";
                    MaxSearchRadius = 3300;
                    MinSearchRadius = 500;
                    MaxSearchAngle = 31;
                    MaxLockDistance = 2000;
                    MaxLockAngle = 19;
                    break;
                case RadarF.KaleidoscopeRadar:
                    RadarName = "万花筒短距雷达";
                    RadarDetail = "万花筒被研发用于近距离的护卫任务，适合僚机在任务中胜任狗逗技术的雷达。";
                    MaxSearchRadius = 2900;
                    MinSearchRadius = 300;
                    MaxSearchAngle = 37;
                    MaxLockDistance = 1600;
                    MaxLockAngle = 27;
                    break;
                case RadarF.NightWatcherRadar:
                    RadarName = "守夜人全方位雷达";
                    RadarDetail = "以据点防卫为主要任务的特化雷达，适合巨型宇航机取得制空权使用。";
                    MaxSearchRadius = 2200;
                    MinSearchRadius = 120;
                    MaxSearchAngle = 45;
                    MaxLockDistance = 1200;
                    MaxLockAngle = 37;
                    break;
                case RadarF.SkynetRadar:
                    RadarName = "天网超广角雷达";
                    RadarDetail = "近距离缠斗的首选雷达，主要用于占据与战术打击任务，适合机动性优良的宇航机配置。";
                    MaxSearchRadius = 1600;
                    MinSearchRadius = 10;
                    MaxSearchAngle = 60;
                    MaxLockDistance = 700;
                    MaxLockAngle = 45;
                    break;
                case RadarF.Prototype:
                    RadarName = "未装配雷达";
                    RadarDetail = "";
                    MaxSearchRadius = 0;
                    MinSearchRadius = 0;
                    MaxSearchAngle = 0;
                    MaxLockDistance = 0;
                    MaxLockAngle = 0;
                    break;
            }
            switch (turretType)
            {
                case TurretF.DevilTenderGazer:
                    TurretName = "恶魔的温情目光";
                    MaxAutoAimAngle = 0.5f; // 自动瞄准极限
                    RoundsPerMinute = 23; // 开火射速 rpm
                    RepeatingCount = 1; //连发模式
                    MaxProjectileSpread = 0.09f; // 发射散布
                    ShockwaveDistance = 700; // 震波距离
                    AmmoVelocity = 7990; // 飞行速率 mps
                    FlightTime = 0.5f; // 飛行時間 sec
                    RaySize = 7.0f;
                    ShieldPenetration = 23; // 護盾穿透
                    HullPenetration = 7; // 機甲穿透
                    break;
                case TurretF.SanctionerMegaRailgun:
                    TurretName = "制裁者的极大磁轨炮";
                    MaxAutoAimAngle = 0.7f; // 自动瞄准极限
                    RoundsPerMinute = 20; // 开火射速 rpm
                    RepeatingCount = 2; // 连发模式
                    MaxProjectileSpread = 0.13f; // 发射散布
                    ShockwaveDistance = 500; // 震波距离
                    AmmoVelocity = 6670; // 飞行速率 mps
                    FlightTime = 0.5f; // 飛行時間 sec
                    RaySize = 5.0f;
                    ShieldPenetration = 16; // 護盾穿透
                    HullPenetration = 3; // 機甲穿透
                    break;
                case TurretF.KocmoIonPulsar:
                    TurretName = "卡斯摩的离子脉冲";
                    MaxAutoAimAngle = 1.6f; // 自动瞄准极限
                    RoundsPerMinute = 77; // 开火射速 rpm
                    RepeatingCount = 3; // 连发模式
                    MaxProjectileSpread = 0.93f; // 发射散布
                    ShockwaveDistance = 100; // 震波距离
                    AmmoVelocity = 5710; // 飞行速率 mps
                    FlightTime = 0.5f; // 飛行時間 sec
                    RaySize = 0.5f;
                    ShieldPenetration = 3; // 護盾穿透
                    HullPenetration = 1; // 機甲穿透
                    break;
                case TurretF.KocmoLaserCannon:
                    TurretName = "卡斯摩的离子脉冲";
                    MaxAutoAimAngle = 3.0f; // 自动瞄准极限
                    RoundsPerMinute = 584; // 开火射速 rpm
                    RepeatingCount = 1; //连发模式
                    MaxProjectileSpread = 0.39f; //彈道散布 degree
                    ShockwaveDistance = 100; // 震波距离
                    AmmoVelocity = 2970; // 飛行速率 m/s
                    FlightTime = 0.37f;//0.71f; // 飛行時間 sec
                    RaySize = 0.5f;
                    ShieldPenetration = 7;
                    HullPenetration = 2;
                    break;
                case TurretF.KocmoRailgun:
                    TurretName = "卡斯摩的离子脉冲";
                    MaxProjectileSpread = 0.36f; //彈道散布 degree
                    ShockwaveDistance = 100; // 震波距离
                    AmmoVelocity = 4550; // 飛行速率 m/s
                    FlightTime = 0.37f; // 飛行時間 sec
                    RaySize = 0.5f;
                    RoundsPerMinute = 3.0f; // 開火射速（每秒X發）RPS
                    RepeatingCount = 1; // 每輪連發射擊次數
                    ShieldPenetration = 5; // 護盾穿透
                    HullPenetration = 5; // 機甲穿透
                    break;
                case TurretF.KocmoUltraPowerPlasma:
                    TurretName = "卡斯摩的离子脉冲";
                    MaxProjectileSpread = 1.0f; //彈道散布 degree
                    ShockwaveDistance = 100; // 震波距离
                    AmmoVelocity = 3370; // 飛行速率 m/s
                    FlightTime = 0.37f; // 飛行時間 sec
                    RaySize = 0.5f;
                    RoundsPerMinute = 10.0f; // 開火射速（每秒X發）RPS
                    RepeatingCount = 1; // 每輪連發射擊次數
                    ShieldPenetration = 5; // 護盾穿透
                    HullPenetration = 5; // 機甲穿透
                    break;
                case TurretF.KocmoArmorPiercing:
                    TurretName = "卡斯摩的离子脉冲";
                    MaxProjectileSpread = 2.37f; //彈道散布 degree
                    ShockwaveDistance = 100; // 震波距离
                    AmmoVelocity = 2970; // 飛行速率 m/s
                    FlightTime = 0.37f; // 飛行時間 sec
                    RaySize = 0.5f;
                    RoundsPerMinute = 9.3f; // 開火射速（每秒X發）RPS
                    RepeatingCount = 1; // 每輪連發射擊次數
                    ShieldPenetration = 5; // 護盾穿透
                    HullPenetration = 5; // 機甲穿透
                    break;
                case TurretF.Prototype:
                    TurretName = "未装配机炮";
                    MaxAutoAimAngle = 0;
                    RoundsPerMinute = 0;
                    RepeatingCount = 0;
                    MaxProjectileSpread = 0;
                    ShockwaveDistance = 0;
                    AmmoVelocity = 0;
                    FlightTime = 0;
                    RaySize = 0;
                    ShieldPenetration = 0;
                    HullPenetration = 0;
                    break;
            }

            //Kocmocraft Mech Droid
            Shield = KocmocraftData.Shield[(int)type];
            Hull = KocmocraftData.Hull[(int)type];
            Energy = KocmocraftData.Energy[(int)type];

            // Onboard Radar
            MaxSearchRadiusSqr = MaxSearchRadius * MaxSearchRadius;
            MinSearchRadiusSqr = MinSearchRadius * MinSearchRadius;
            MaxSearchRange = Mathf.Cos(MaxSearchAngle * Mathf.Deg2Rad);
            MaxLockDistanceSqr = MaxLockDistance * MaxLockDistance;
            MaxLockRange = Mathf.Cos(MaxLockAngle * Mathf.Deg2Rad);
            MaxAutoAimRange = Mathf.Cos(MaxAutoAimAngle * Mathf.Deg2Rad);
            // Turret.
            TurretCount = KocmocraftData.TurretCount[(int)type];
            Decay = GetDecay(TurretCount);
            DecayVelocity = "<size=29><color=green> >>> " + Mathf.RoundToInt(100 - Decay * 100) + "%衰减</color></size>";
            DecayDamage = "<size=29><color=green> >>> " + Mathf.RoundToInt(100 - Decay * Decay * 100) + "%衰减</color></size>";
            //FCS
            FireRate = 60 / RoundsPerMinute;
            // Ammo
            AmmoVelocity *= Decay;
            propulsion = AmmoVelocity * 50; // propulsion = AmmoVelocity / Time.fixedDeltaTime (1/0.02 = 50)
            operationalRange = AmmoVelocity * FlightTime;
            waitRecovery = new WaitForSeconds(FlightTime);
            // Damage
            Damage = (int)(AmmoVelocity * AmmoVelocity * WeaponData.fixDamage);
            DPS = (int)(AmmoVelocity * AmmoVelocity * WeaponData.fixDamage * RepeatingCount * RoundsPerMinute * TurretCount / 60);
        }
        private float GetDecay(int count)
        {
            switch (count)
            {
                case 0: return WeaponData.fix2Tube;
                case 2: return WeaponData.fix2Tube;
                case 4: return WeaponData.fix4Tube;
                case 6: return WeaponData.fix6Tube;
                default: return WeaponData.fix8Tube;
            }
        }
    }
    public class EngineData
    {
        public float MinVolume;
        public float MaxVolume;
        public float MinPitch;
        public float MaxPitch;

        public EngineData(EngineType type)
        {
            switch (type)
            {
                case EngineType.Turbojet:
                    MaxVolume = Turbojet.MaxVolume;
                    MinPitch = Turbojet.MinPitch;
                    MaxPitch = Turbojet.MaxPitch;
                    break;
                case EngineType.Turbofan:
                    MaxVolume = Turbofan.MaxVolume;
                    MinPitch = Turbofan.MinPitch;
                    MaxPitch = Turbofan.MaxPitch;
                    break;
                case EngineType.Turboprop:
                    MaxVolume = Turboprop.MaxVolume;
                    MinPitch = Turboprop.MinPitch;
                    MaxPitch = Turboprop.MaxPitch;
                    break;
                case EngineType.Turboshaft:
                    MaxVolume = Turboshaft.MaxVolume;
                    MinPitch = Turboshaft.MinPitch;
                    MaxPitch = Turboshaft.MaxPitch;
                    break;
                case EngineType.IonThruster:
                    MaxVolume = IonThruster.MaxVolume;
                    MinPitch = IonThruster.MinPitch;
                    MaxPitch = IonThruster.MaxPitch;
                    break;
                case EngineType.BiomassEnergy:
                    MaxVolume = BiomassEnergy.MaxVolume;
                    MinPitch = BiomassEnergy.MinPitch;
                    MaxPitch = BiomassEnergy.MaxPitch;
                    break;
                case EngineType.PulsedPlasmaThruster:
                    MaxVolume = PulsedPlasmaThruster.MaxVolume;
                    MinPitch = PulsedPlasmaThruster.MinPitch;
                    MaxPitch = PulsedPlasmaThruster.MaxPitch;
                    break;
            }

        }

        public static readonly string[] Engine = {
            "涡轮扇发动机 x 1", // 神偷机兵
            "涡轮喷射发动机 x 1", // 红牛能量
            "涡轮扇发动机 x 2", // 普鲸
            "涡轮喷射发动机 x 1", // 纸飞机
            "生质能转换器 x 2", // 咕咕鸡
            "火箭推进器 x 1", // 炮弹比尔
            "量子引擎 x 2", // 时光机
            "涡轮螺旋桨发动机 x 1", // 王牌狗屋
            "量子引擎 x 1", // 卡比之星
            "涡轮扇发动机 x 1", // 蝎红
            "涡轮喷射发动机 x 2", // 恩威迪亚
            "涡轮扇发动机 x 2", // 快餐侠
            "涡轮扇发动机 x 4", // 驯鹿空运
            "涡轮扇发动机 x 5", // 北极星特快
            "涡轮轴发动机 x 4", // 远古飞鱼
            "火箭推进器 x 4", // 玩具独角兽
            "涡轮轴发动机 x 1", // 南瓜魅影
            "涡轮扇发动机 x 3", // 赏金猎人
            "火箭推进器", // 鹰纽特
            "生质能转换器 x 1", // 安格瑞
            "最高机密", // 即将登场
            "最高机密", // 即将登场
            "最高机密", // 即将登场
            "最高机密", // 即将登场
        };

    }

    public static class LobbyInfomation
    {
        public static readonly string SCENE_LOBBY = "New Galaxy Lobby";
        public static readonly string SCENE_HANGAR = "New Airport 3";
        public static readonly string SCENE_OPERATION = "New_Demo_Main";//"Skirmish in Dusk Lakeside";
        public static readonly string SCENE_LOADING = "Loading";
        public static readonly string SCENE_LOADING_ONLINE = "LoadingOnline";
        public static readonly string PLAYER_READY = "IsPlayerReady";
        public static readonly string PLAYER_LOADING = "isLoading";
        public static readonly string PLAYER_LOADED_LEVEL = "PlayerLoadedLevel";
        public static readonly string PLAYER_DATA_KEY = "PlayerUnknownData"; // 用於PUN設定玩家數據
        public static readonly int PLAYER_DATA_VALUE = 999; // 用於PUN設定玩家數據之對應值
        public static readonly string PLAYER_SKIN_OPTION = "Player Skin";
        public static readonly string PREFS_TYPE = "Prefs Type";
        public static readonly string PREFS_SKIN = "Prefs Skin";
        public static readonly string PREFS_LOAD_SCENE = "LoadScene";
        public static readonly string PROPERTY_TYPE = "Player Type";
        public static readonly string PROPERTY_SKIN = "Player Skin";

    }

    public static class HangarData
    {
        public static readonly Color32[] FrameColor = new Color32[] {
            new Color32(237,237,237,251),  // 神偷机兵：银237
            new Color32(255,255,255,251), // 红牛能量：白255
            new Color32(117,185,255,251), // 普鲸：蓝
            new Color32(255,255,255,251), // 纸飞机：白255
            new Color32(255,255,191,251), // 咕咕鸡：淡黄
            new Color32(163,163,163,251), // 炮弹比尔：黑163
            new Color32(137,179,255,251), // 时光机：淡蓝偏紫
            new Color32(255,71,71,251), // 王牌狗屋：红71
            new Color32(255,255,163,251), // 卡比之星：淡黄137
            new Color32(255,71,71,251), // 蝎红：红71
            new Color32(255,255,255,251), // 恩威迪亚：白255
            new Color32(255,207,155,251), // 快餐侠：淡棕
            new Color32(255,71,71,251),  // 驯鹿空运：红71
            new Color32(163,163,163,251), // 北极星特快：黑163
            new Color32(163,147,255,251), // 远古飞鱼：紫
            new Color32(255,255,255,251), // 玩具独角兽：白255
            new Color32(255,127,0,251), // 南瓜魅影：橘127
            new Color32(97,255,37,251), // 赏金猎人：：绿127
            new Color32(227,171,103,251), // 鹰纽特：棕
            new Color32(163,147,255,251), // 安格瑞：紫
            new Color32(255,255,255,251), // 即将登场
            new Color32(255,255,255,251), // 即将登场
            new Color32(255,255,255,251), // 即将登场
            new Color32(255,255,255,251), // 即将登场
        };
        public static readonly Color32[] ButtonColor = new Color32[] {
            new Color32(255,71,71,247), // 神偷机兵：红71     //new Color32(0,163,255,232), // 神偷机兵：蓝
            new Color32(0,97,255,247), // 红牛能量：蓝97       //   new Color32(255,0,127,232), // 红牛能量：桃红
            new Color32(255,255,255,247), // 普鲸：白
            new Color32(0,255,0,247), // 纸飞机：绿255
            new Color32(255,71,71,247), // 咕咕鸡：红71
            new Color32(255,255,255,247), // 炮弹比尔：白
            new Color32(255,237,137,247), // 时光机：淡黄137偏橘
            new Color32(0,255,0,247), // 王牌狗屋：绿255
            new Color32(163,237,255,247), // 卡比之星：粉红97
            new Color32(255,255,137,247), // 蝎红：淡黄137
            new Color32(137,255,0,247), // 恩威迪亚：黄绿
            new Color32(255,127,123,247), // 快餐侠：淡红
          new Color32(255,255,255,247), // 驯鹿空运：白
            new Color32(255,227,127,247), // 北极星特快：淡橘
            new Color32(255,127,0,247), // 远古飞鱼：橘127
            new Color32(255,97,255,247), // 玩具独角兽：粉红97
            new Color32(0,255,0,247), // 南瓜魅影：绿255
            new Color32(255,73,0,247), // 赏金猎人：深橘73
            new Color32(255,255,255,247), // 鹰纽特：白
             new Color32(255,71,71,247), // 安格瑞：淡黄137
            new Color32(255,255,255,247), // 即将登场
            new Color32(255,255,255,247), // 即将登场
            new Color32(255,255,255,247), // 即将登场
            new Color32(255,255,255,247), // 即将登场
        };
        public static readonly Color32[] TextColor = new Color32[] {
            new Color32(237,237,237,255), // 神偷机兵：黄123
            new Color32(255,255,255,255), // 红牛能量
            new Color32(117,185,255,255), // 普鲸
            new Color32(255,255,255,255), // 纸飞机：白255
            new Color32(255,223,83,255), // 咕咕鸡
            new Color32(193,193,193,255), // 炮弹比尔：黑193
            new Color32(161,213,255,255), // 时光机
            new Color32(255,137,137,255), // 王牌狗屋：淡红137
            new Color32(255,255,163,255), // 卡比之星：淡黄163
            new Color32(255,137,137,255), // 蝎红：淡红137
            new Color32(255,255,255,255), // 恩威迪亚：白255
            new Color32(255,207,155,255), // 快餐侠
            new Color32(255,137,137,255), // 驯鹿空运：淡红137
            new Color32(193,193,193,255), // 北极星特快：黑193
            new Color32(163,147,255,255), // 远古飞鱼
            new Color32(255,255,255,255), // 玩具独角兽：白255
            new Color32(255,127,0,255), // 南瓜魅影：橘127
            new Color32(97,255,37,255), // 赏金猎人：绿127
            new Color32(227,171,103,255), // 鹰纽特
            new Color32(163,147,255,255), // 安格瑞
            new Color32(255,255,255,255), // 即将登场
            new Color32(255,255,255,255), // 即将登场
            new Color32(255,255,255,255), // 即将登场
            new Color32(255,255,255,255), // 即将登场
        };
        public static readonly Color32[] TabColor = new Color32[] {
            new Color32(255,255,97,255), // 神偷机兵：蓝
            new Color32(255,91,91,255), // 红牛能量：红91     //new Color32(255,107,177,255), // 红牛能量：深粉红
            new Color32(163,255,0,255), // 普鲸：草绿色
            new Color32(255,237,193,255), // 纸飞机：米
            new Color32(255,223,83,251), // 咕咕鸡：黄
            new Color32(255,91,91,255), // 炮弹比尔：红91
            new Color32(93,179,255,255), // 时光机：淡黄137
            new Color32(255,91,91,255), // 王牌狗屋：绿255
            new Color32(255,173,237,255), // 卡比之星：粉红137
            new Color32(255,91,91,255),  // 蝎红：淡黄137
            new Color32(137,255,0,255), // 恩威迪亚：黄绿
            new Color32(255,127,123,255), // 快餐侠：淡红
            new Color32(227,171,103,255), // 驯鹿空运：红71
            new Color32(255,207,91,255), // 北极星特快：淡橘
            new Color32(255,237,91,255), // 远古飞鱼：橘127
            new Color32(255,127,37,255), // 玩具独角兽：粉红97
            new Color32(93,179,255,255), // 南瓜魅影：绿255
            new Color32(137,255,37,255), // 赏金猎人：深橘73
            new Color32(255,113,93,255), // 鹰纽特：白
            new Color32(255,113,37,255), // 安格瑞：淡黄137
            new Color32(255,255,255,255), // 即将登场
            new Color32(255,255,255,255), // 即将登场
            new Color32(255,255,255,255), // 即将登场
            new Color32(255,255,255,255), // 即将登场
        };


    }
    
    public static class DubiData
    {
        public static readonly string[] Dubi = {
            "小小兵", // 神偷机兵
            "红牛之翼学院", // 红牛能量
            "达瓦里希", // 普鲸
            "纸箱工坊", // 纸飞机
            "咕咕蛋培育中心", // 咕咕鸡 
            "蘑菇王国", // 炮弹比尔
            "机器人学校", // 时光机
            "王牌飞行员联盟", // 王牌狗屋
            "卡比", // 卡比之星
            "无面人", // 蝎红
            "熊猫人", // 恩威迪亚
            "阿痞", // 快餐侠
            "地鼠", // 驯鹿空运
            "表情帝", // 北极星特快
            "海绵宝宝", // 远古飞鱼
            "阿尼", // 玩具独角兽
            "哆啦啦", // 南瓜魅影
            "凯子", // 赏金猎人
            "屎蛋", // 鹰纽特
            "安格瑞", // 安格瑞
        };
        
        public static readonly string[] ChiefPilot = {
            "戴夫", // 神偷机兵
            "维特尔", // 红牛能量
            "大毛", // 普鲸
            "阿楞", // 纸飞机
            "咕咕蛋", // 咕咕鸡 
            "马里奥", // 炮弹比尔
            "哆啦A梦", // 时光机
            "史努比", // 王牌狗屋
            "卡比", // 卡比之星
            "无面人", // 蝎红
            "张学友", // 恩威迪亚
            "阿痞", // 快餐侠
            "地鼠", // 驯鹿空运
            "滑稽", // 北极星特快
            "海绵宝宝", // 远古飞鱼
            "公主阿尼", // 玩具独角兽
            "哆啦啦", // 南瓜魅影
            "赏金猎人凯子", // 赏金猎人
            "印第安屎蛋", // 鹰纽特
            "安格瑞", // 安格瑞
        };
        public static readonly string[] ChiefResume = {
            "小小兵是格鲁实验室的一群黄色逗比，平时热爱打电动的戴夫在经过层层海选中脱颖而出，成为一名合格地神偷机兵飞行员。", // 神偷机兵
            "维特尔是蓝星最知名的红牛火星车的首席赛车手，曾取得蓝星四个赛季的总冠军。为了宇航机竞技第四赛季，红牛再次征召这位老将。", // 红牛能量
            "大毛作为新毛熊时代的老大，为了延续毛熊设计局一贯的作风，而成为战斗民族的首位宇航机飞行员，将与宇航机老将萌总一同参与第四赛季。", // 普鲸
            "作为一名合格的纸飞机宇航员，同为纸类的纸箱人阿楞应该最能胜任。", // 纸飞机
            "辣鸡设计局为咕咕鸡招募的同款宇航员—咕咕蛋。", // 咕咕鸡 
            "任天堂大当家马里奥，理所当然首位宇航员非他莫属。", // 炮弹比尔
            "22世纪的逗比机器喵，空飞地毯型时光机的首席飞行员，是哇咔咔世界第一批进驻的七名勇者之一。", // 时光机
            "作为一名王牌飞行员，史努比在梦中的战绩可谓是无狗能及，与其座驾受邀加入宇航机竞技第一赛季。", // 王牌狗屋
            "卡比", // 卡比之星
            "无面人是蓝星一个鲜为人知的族群，目前各星球对它的了解仍是甚少，是哇咔咔世界第一批进驻的七名勇者之一。", // 蝎红
            "张学友是近年来最知名的熊猫人，宇航机竞技当然也少不了贼位嘴遁砖家的加入，第二赛季加入恩威迪亚作为首席飞行员。", // 恩威迪亚
            "阿痞，本名Eric Cartman，南方四贱客之一，南方公园首屈一指的肯德基走私大佬，受到唐马儒的邀请加入肯打鸡集团担任快餐侠首席飞行员。", // 快餐侠
            "地鼠，哇咔咔世界第一批进驻的七名勇者之一，由于节庆保卫中心人力不足，而由宇航机赛事主席介绍担任试飞员。", // 驯鹿空运
            "表情帝滑稽", // 北极星特快
            "海绵宝宝", // 远古飞鱼
            "公主阿尼", // 玩具独角兽
            "22世纪的逗比机器喵，空飞地毯型时光机的首席飞行员，是哇咔咔世界第一批进驻的七名勇者之一。", // 南瓜魅影
            "赏金猎人凯子", // 赏金猎人
            "印第安屎蛋", // 鹰纽特
            "安格瑞", // 安格瑞
        };
        public static readonly int[] ChiefHeight = {
            146, // 戴夫
            146, // 维特尔
            146, // 大毛
            135, // 阿楞
            123, // 咕咕蛋
            0, // 炮弹比尔
            146, // 哆啦啦
            0, // 史努比
            0, // 卡比
            160, // 无面人
            146, // 张学友
            146, // 阿痞
            146, // 地鼠
            146, // 滑稽
            137, // 海绵宝宝
            0, // 公主阿尼
            146, // 哆啦啦
            0, // 赏金猎人凯子
            0, // 印第安屎蛋
            146, // 安格瑞
        };

        public static readonly string[] SecondPilot = {
            "邪恶小小兵", // 神偷机兵
            "王尼玛", // 红牛能量
            "萌总", // 普鲸
            "Awesom-O 666", // 纸飞机
            "招募中", // 咕咕鸡 
            "招募中", // 炮弹比尔
            "迷你哆啦", // 时光机
            "招募中", // 王牌狗屋
            "招募中", // 卡比之星
            "招募中", // 蝎红
            "金馆长", // 恩威迪亚
            "Awesom-O 4000", // 快餐侠
            "招募中", // 驯鹿空运
            "招募中", // 北极星特快
            "粉红海绵宝宝", // 远古飞鱼
            "招募中", // 玩具独角兽
            "迷你哆啦", // 南瓜魅影
            "招募中", // 赏金猎人
            "招募中", // 鹰纽特
            "招募中", // 安格瑞
        };
        public static readonly string[] SecondResume = {
            "强化过後的邪恶小小兵会变异成紫色逗比，它们操作的神偷机兵更倾向於近距离与对手一同毁灭，故有自爆机兵的称号。", // 神偷机兵
            "王尼玛是号称两千名员工的暴走漫画CEO，受到宇航机赛事主席伊琉沙的邀请为红牛之翼学院担任试飞教官。", // 红牛能量
            "前同萌会总理，简称萌总，其追随者以秃子最为知名，故作为其代表形象。毛熊设计局的初代试飞员。", // 普鲸
            "Awesome-O 4000的纸箱人兄弟，将搭配阿楞一同出战第四赛季。", // 纸飞机
            "", // 咕咕鸡 
            "", // 炮弹比尔
            "迷你哆啦是哆啦A梦的迷你型限量纪念版，有多种颜色，其中以红色迷你哆啦最为活跃，在宇航机竞技中担任助手飞行员。", // 时光机
            "", // 王牌狗屋
            "", // 卡比之星
            "", // 蝎红
            "作为第一代熊猫人宗师，金馆长获邀在第四赛季为恩威迪亚出战宇航机赛事，届时将上演表情包大战。", // 恩威迪亚
            "Awesom-O 4000的本体为阿痞，在Awesom-O模式下，可以增强伪装与情搜能力。", // 快餐侠
            "", // 驯鹿空运
            "", // 北极星特快
            "海绵宝宝的粉红色版本，与一般黄色的海绵宝宝无异。", // 远古飞鱼
            "", // 玩具独角兽
            "迷你哆啦是哆啦A梦的迷你型限量纪念版，有多种颜色，其中以红色迷你哆啦最为活跃，在宇航机竞技中担任助手飞行员。", // 南瓜魅影
            "", // 赏金猎人
            "", // 鹰纽特
            "", // 安格瑞
        };
        public static readonly int[] SecondHeight = {
            146, // 邪恶小小兵
            146, // 王尼玛
            146, // 萌总
            135, // Awesom-O 666
            0, // 
            0, // 
            102, // 迷你哆啦
            0, // 
            0, // 
            0, // 
            146, // 金馆长
            127, // Awesom-O 4000
            0, // 
            0, // 
            137, // 
            0, // 
            102, // 迷你哆啦
            0, // 
            0, // 
            0, // 
        };

    }
    public static class PerformanceData
    {
        public static readonly int[] PilotTier = {
            3, // ★☆☆☆☆☆☆ 神偷机兵
            1, // ★★☆☆☆☆☆ 红牛能量
            2, // ★★★☆☆☆☆ 普鲸
            3, // ★★★☆☆☆☆ 纸飞机
            2, // ★★★★★★☆ 咕咕鸡 
            1, // ★☆☆☆☆☆☆ 炮弹比尔
            2, // ★☆☆☆☆☆☆ 时光机
            1, // ★☆☆☆☆☆☆ 王牌狗屋
            2, // ★☆☆☆☆☆☆ 卡比之星
            1, // ★★★★★☆☆ 蝎红
            2, // ★★★★★★☆ 恩威迪亚
            2, // ★★★☆☆☆☆ 快餐侠
            1, // ★★★★★☆☆ 驯鹿空运
            3, // ★★★★★★☆ 北极星特快
            1, // ★★★★☆☆☆ 远古飞鱼
            2, // ★★★★☆☆☆ 玩具独角兽
            2, // ★★★★★★★ 南瓜魅影
            1, // ★★★★★☆☆ 赏金猎人
            3, // ★★☆☆☆☆☆ 鹰纽特
            2, // ★★★★☆☆☆ 新葡鲸
        };
        public static readonly int BaseShield = 1200, DiffShield = 1800;
        public static readonly int BaseHull = 1400, DiffHull = 2100;
        public static readonly int BaseSpeed = 50, DiffSpeed = 12;
        public static int GetShieldProficiency(int index)
        {
            return BaseShield + DiffShield * (ShieldProficiency[index]);
        }
        public static int GetShieldLevel(int value)
        {
            for (int i = 7; i > 0; i--)
            {
                if (value >= BaseShield + DiffShield * i + (DiffShield / 6 * (i - 1)))
                    return i;
            }
            return 0;
        }
        public static int GetHullProficiency(int index)
        {
            return BaseHull + DiffHull * (HullProficiency[index]);
        }
        public static int GetHullLevel(int value)
        {
            for (int i = 7; i > 0; i--)
            {
                if (value >= BaseHull + DiffHull * i + (DiffHull / 6 * (i - 1)))
                    return i;
            }
            return 0;
        }
        public static int GetSpeedProficiency(int index)
        {
            return BaseSpeed + DiffSpeed * (ShieldProficiency[index]);
        }
        public static int GetSpeedLevel(int value)
        {
            for (int i = 7; i > 0; i--)
            {
                if (value >= BaseSpeed + DiffSpeed * i + (DiffSpeed / 6 * (i - 1)))
                    return i;
            }
            return 0;
        }

        public static readonly int[] ShieldProficiency = {
            1, // ★☆☆☆☆☆☆ 神偷机兵
            2, // ★★☆☆☆☆☆ 红牛能量
            4, // ★★★☆☆☆☆ 普鲸
            2, // ★★★☆☆☆☆ 纸飞机
            5, // ★★★★★★☆ 咕咕鸡 
            1, // ★☆☆☆☆☆☆ 炮弹比尔
            3, // ★☆☆☆☆☆☆ 时光机
            1, // ★☆☆☆☆☆☆ 王牌狗屋
            1, // ★☆☆☆☆☆☆ 卡比之星
            4, // ★★★★★☆☆ 蝎红
            5, // ★★★★★★☆ 恩威迪亚
            1, // ★★★☆☆☆☆ 快餐侠
            3, // ★★★★★☆☆ 驯鹿空运
            3, // ★★★★★★☆ 北极星特快
            6, // ★★★★☆☆☆ 远古飞鱼
            7, // ★★★★☆☆☆ 玩具独角兽
            2, // ★★★★★★★ 南瓜魅影
            2, // ★★★★★☆☆ 赏金猎人
            3, // ★★☆☆☆☆☆ 鹰纽特
            3, // ★★★★☆☆☆ 新葡鲸
        };
        public static readonly int[] HullProficiency = {
            7, // ★☆☆☆☆☆☆ 神偷机兵
            1, // ★★☆☆☆☆☆ 红牛能量
            5, // ★★★☆☆☆☆ 普鲸
            3, // ★★★☆☆☆☆ 纸飞机
            4, // ★★★★★★☆ 咕咕鸡 
            2, // ★☆☆☆☆☆☆ 炮弹比尔
            1, // ★☆☆☆☆☆☆ 时光机
            4, // ★☆☆☆☆☆☆ 王牌狗屋
            2, // ★☆☆☆☆☆☆ 卡比之星
            3, // ★★★★★☆☆ 蝎红
            1, // ★★★★★★☆ 恩威迪亚
            3, // ★★★☆☆☆☆ 快餐侠
            2, // ★★★★★☆☆ 驯鹿空运
            3, // ★★★★★★☆ 北极星特快
            1, // ★★★★☆☆☆ 远古飞鱼
            2, // ★★★★☆☆☆ 玩具独角兽
            7, // ★★★★★★★ 南瓜魅影
            6, // ★★★★★☆☆ 赏金猎人
            4, // ★★☆☆☆☆☆ 鹰纽特
            4, // ★★★★☆☆☆ 新葡鲸
        };
        public static readonly int[] SpeedProficiency = {
            2, // ★☆☆☆☆☆☆ 神偷机兵
            7, // ★★☆☆☆☆☆ 红牛能量
            1, // ★★★☆☆☆☆ 普鲸
            5, // ★★★☆☆☆☆ 纸飞机
            1, // ★★★★★★☆ 咕咕鸡 
            7, // ★☆☆☆☆☆☆ 炮弹比尔
            6, // ★☆☆☆☆☆☆ 时光机
            5, // ★☆☆☆☆☆☆ 王牌狗屋
            7, // ★☆☆☆☆☆☆ 卡比之星
            3, // ★★★★★☆☆ 蝎红
            4, // ★★★★★★☆ 恩威迪亚
            6, // ★★★☆☆☆☆ 快餐侠
            5, // ★★★★★☆☆ 驯鹿空运
            4, // ★★★★★★☆ 北极星特快
            3, // ★★★★☆☆☆ 远古飞鱼
            1, // ★★★★☆☆☆ 玩具独角兽
            1, // ★★★★★★★ 南瓜魅影
            2, // ★★★★★☆☆ 赏金猎人
            3, // ★★☆☆☆☆☆ 鹰纽特
            3, // ★★★★☆☆☆ 新葡鲸
        };
        public static readonly int[] EngineSpeed = {
            9, // ★☆☆☆☆☆☆ 神偷机兵
            17, // ★★☆☆☆☆☆ 红牛能量
            16, // ★★★☆☆☆☆ 普鲸
            7, // ★★★☆☆☆☆ 纸飞机
            0, // ★★★★★★☆ 咕咕鸡 
            15, // ★☆☆☆☆☆☆ 炮弹比尔
            14, // ★☆☆☆☆☆☆ 时光机
            9, // ★☆☆☆☆☆☆ 王牌狗屋
            23, // ★☆☆☆☆☆☆ 卡比之星
            10, // ★★★★★☆☆ 蝎红
            20, // ★★★★★★☆ 恩威迪亚
            22, // ★★★☆☆☆☆ 快餐侠
            28, // ★★★★★☆☆ 驯鹿空运
            35, // ★★★★★★☆ 北极星特快
            28, // ★★★★☆☆☆ 远古飞鱼
            19, // ★★★★☆☆☆ 玩具独角兽
            15, // ★★★★★★★ 南瓜魅影
            15, // ★★★★★☆☆ 赏金猎人
            20, // ★★☆☆☆☆☆ 鹰纽特
            23, // ★★★★☆☆☆ 新葡鲸
        };



    }
    public static class GeneralData
    {
        public static Color32 GetBaseColor(int index)
        {
            switch (index)
            {
                case 0: return new Color32(0, 69, 8, 255);
                case 1: return new Color32(69, 32, 20, 255);
                case 100: return new Color32(19, 69, 25, 255);
                case 101: return new Color32(69, 42, 32, 255);
                default: return Color.black;
            }
        }
        public static Color32 GeTextColor(int index)
        {
            switch (index)
            {
                case 0: return new Color32(115, 255, 0, 255);
                case 1: return new Color32(255, 157, 0, 255);
                case 100: return new Color32(153, 255, 69, 255);
                case 101: return new Color32(255, 184, 69, 255);
                default: return Color.black;
            }
        }
    }



    public enum EngineType
    {
        Turbojet = 100, //
        Turbofan = 101, // TurboFan
        Turboprop = 102, //
        Turboshaft = 103, //
        IonThruster = 104, //
        BiomassEnergy = 106, // 生質能動力引擎
        PulsedPlasmaThruster = 107, // 脈衝等離子推進器
    }

    public static class Turbojet
    {
        public static readonly float MaxVolume = 0.137f;        //The maximum volume of the engine
        public static readonly float MinPitch = 0.77f;      //The minimum pitch of the engine
        public static readonly float MaxPitch = 1f;		//The maximum pitch of the engine
    }
    public static class Turbofan
    {
        public static readonly float MaxVolume = 0.173f;        //The maximum volume of the engine
        public static readonly float MinPitch = 0.8f;      //The minimum pitch of the engine
        public static readonly float MaxPitch = 1.3f;		//The maximum pitch of the engine
    }
    public static class Turboprop
    {
        public static readonly float MaxVolume = 0.237f;        //The maximum volume of the engine
        public static readonly float MinPitch = .9f;      //The minimum pitch of the engine
        public static readonly float MaxPitch = 1.1f;		//The maximum pitch of the engine
    }
    public static class Turboshaft
    {
        public static readonly float MaxVolume = 0.55f;        //The maximum volume of the engine
        public static readonly float MinPitch = .7f;      //The minimum pitch of the engine
        public static readonly float MaxPitch = 1.3f;		//The maximum pitch of the engine
    }

    public static class IonThruster
    {
        public static readonly float MaxVolume = 0.71f;        //The maximum volume of the engine
        public static readonly float MinPitch = 0.7f;      //The minimum pitch of the engine
        public static readonly float MaxPitch = 1.3f;		//The maximum pitch of the engine
    }
    public static class BiomassEnergy
    {
        public static readonly float MaxVolume = 0.73f;        //The maximum volume of the engine
        public static readonly float MinPitch = 0.8f;      //The minimum pitch of the engine
        public static readonly float MaxPitch = 1.2f;		//The maximum pitch of the engine
    }
    public static class PulsedPlasmaThruster
    {
        public static readonly float MaxVolume = 0.73f;        //The maximum volume of the engine
        public static readonly float MinPitch = .7f;      //The minimum pitch of the engine
        public static readonly float MaxPitch = 1.3f;		//The maximum pitch of the engine
    }



    public static class OriginalSettingEngine
    {
        public static readonly float engineMinVol = 0f;         //The minimum volume of the engine
        public static readonly float engineMaxVol = .6f;        //The maximum volume of the engine
        public static readonly float engineMinPitch = .3f;      //The minimum pitch of the engine
        public static readonly float engineMaxPitch = .8f;      //The maximum pitch of the engine
    }






    public delegate void LockOnEventHandler(object sender, LockOnEventArgs e);
    public class LockOnEventArgs : System.EventArgs
    {
        public readonly bool isLockOn;
        public LockOnEventArgs(bool isTracking)
        {
            isLockOn = isTracking;
        }
    }
    public delegate void HitLocalPlayerEventHandler(object sender, HitEventArgs e);
    public class HitEventArgs : System.EventArgs
    {
        public readonly int valueHull;
        public HitEventArgs(int nowHull)
        {
            valueHull = nowHull;
        }
    }
    public delegate void LocalPlayerUIEventHandler(object sender, BarUIEventArgs e);
    public class BarUIEventArgs : System.EventArgs
    {
        public readonly int valueBar;
        public readonly int valueMax;

        public BarUIEventArgs(int value)
        {
            valueBar = value;
        }
    }


    public enum MinigunOption
    {
        None = 0,
        StrafingCuckooCannon = 1, // 布穀彈幕砲 95% 136%
        RepeatingKineticCannon = 2, // 連發動能砲 293% 37%
        PrecisionMegaCannon = 3, // 精準巨砲 79% 173%
        MiniScattergun = 4, // 迷你霰彈槍 123% 97%
        AssaultGun = 5, // 突擊機槍 144% 86%
        HeavyPlasmaCannon = 6, // 重型電漿砲 66% 196%
        CrazyArrow = 7, // 瘋狂箭頭 93% 144%
        RapidFireNailer = 8, // 速射釘槍 179% 55%
        UltraPulseEmitter = 9, // 高能脈衝發射器 37% 237%
    }
    public enum DamageType
    {
        StrafingCuckooCannon = 1, // 布穀彈幕砲 95% 40%
        RepeatingKineticCannon = 2, // 連發動能砲 190% 5%
        PrecisionMegaCannon = 3, // 精準巨砲 70% 110%
        MiniScattergun = 4, // 迷你霰彈槍 100% 30%
        AssaultGun = 5, // 突擊機槍 110% 25%
        HeavyPlasmaCannon = 6, // 重型電漿砲 50% 190%
        CrazyArrow = 7, // 瘋狂箭頭 80% 70%
        RapidFireNailer = 8, // 速射釘槍 150% 20%
        UltraPulseEmitter = 9, // 高能脈衝發射器 30% 230%
        Rocket = 11,
        Missile = 12,
    }
    public static class WeaponOptionData
    {
        public static readonly MinigunOption[] minigunConfiguration = new[]
        {
            MinigunOption.AssaultGun, // 神偷機
            MinigunOption.MiniScattergun, // 紅牛能量
            MinigunOption.RepeatingKineticCannon, // 普鯨
            MinigunOption.HeavyPlasmaCannon, // 紙飛機
            MinigunOption.StrafingCuckooCannon, // 咕咕雞
            MinigunOption.RapidFireNailer, // 殺手
            MinigunOption.UltraPulseEmitter, // 時光機
            MinigunOption.CrazyArrow, // 王牌狗屋
            MinigunOption.MiniScattergun, // 卡比阿爾法
            MinigunOption.RapidFireNailer, // 奪魂者
            MinigunOption.AssaultGun, // 恩威迪亞
            MinigunOption.HeavyPlasmaCannon, // 快餐俠
            MinigunOption.UltraPulseEmitter, // 馴鹿速遞
            MinigunOption.PrecisionMegaCannon, // 北極星特快
        };
    }














    public static class StrafingCuckooCannon // 六管限定 - 咕咕雞
    {
        public static readonly int maxAmmoCapacity = 999; // 最大載彈量
        public static readonly float projectileSpread = 4.44f; //彈道散布 degree
        public static readonly int flightVelocity = 2290; // 飛行速率 m/s
        public static readonly int propulsion = flightVelocity * 50; // 開火推力
        public static readonly int operationalRange = 1090; // 射程 m
        public static readonly float FlightTime = (float)operationalRange / flightVelocity; // 飛行時間 sec
        public static readonly float fireRoundPerSecond = 11.97f; // 射速 rps
        public static readonly float rateFire = 1 / fireRoundPerSecond; // 開火速率 sec
        public static readonly DamageType damageType = DamageType.StrafingCuckooCannon;
    }
    public static class RepeatingKineticCannon // 六管限定 - 普鯨
    {
        public static readonly int maxAmmoCapacity = 999; // 最大載彈量
        public static readonly float projectileSpread = 3.37f; //彈道散布 degree
        public static readonly int flightVelocity = 2550; // 飛行速率 m/s
        public static readonly int propulsion = flightVelocity * 50; // 開火推力
        public static readonly int operationalRange = 1790; // 射程 m
        public static readonly float FlightTime = (float)operationalRange / flightVelocity; // 飛行時間 sec
        public static readonly float fireRoundPerSecond = 5.9f; // 射速 rps
        public static readonly float rateFire = 1 / fireRoundPerSecond; // 開火速率 sec
        public static readonly DamageType damageType = DamageType.RepeatingKineticCannon;
    }
    public static class PrecisionMegaCannon // 六管限定 - 北極星特快
    {
        public static readonly int maxAmmoCapacity = 999; // 最大載彈量
        public static readonly float projectileSpread = 0.93f; //彈道散布 degree
        public static readonly int flightVelocity = 2990; // 飛行速率 m/s
        public static readonly int propulsion = flightVelocity * 50; // 開火推力
        public static readonly int operationalRange = 1970; // 射程 m
        public static readonly float FlightTime = (float)operationalRange / flightVelocity; // 飛行時間 sec
        public static readonly float fireRoundPerSecond = 2.1f; // 射速 rps
        public static readonly float rateFire = 1 / fireRoundPerSecond; // 開火速率 sec
        public static readonly DamageType damageType = DamageType.PrecisionMegaCannon;
    }
    public static class MiniScattergun // 四管限定 - 紅牛能量、卡比阿爾法
    {
        public static readonly int maxAmmoCapacity = 999; // 最大載彈量
        public static readonly float projectileSpread = 2.97f; //彈道散布 degree
        public static readonly int flightVelocity = 2360; // 飛行速率 m/s
        public static readonly int propulsion = flightVelocity * 50; // 開火推力
        public static readonly int operationalRange = 1230; // 射程 m
        public static readonly float FlightTime = (float)operationalRange / flightVelocity; // 飛行時間 sec
        public static readonly float fireRoundPerSecond = 10.96f; // 射速 rps
        public static readonly float rateFire = 1 / fireRoundPerSecond; // 開火速率 sec
        public static readonly DamageType damageType = DamageType.MiniScattergun;
    }
    public static class AssaultGun // 四管限定 - 神偷機、恩威迪亞
    {
        public static readonly int maxAmmoCapacity = 999; // 最大載彈量
        public static readonly float projectileSpread = 3.73f; //彈道散布 degree
        public static readonly int flightVelocity = 2710; // 飛行速率 m/s
        public static readonly int propulsion = flightVelocity * 50; // 開火推力
        public static readonly int operationalRange = 1570; // 射程 m
        public static readonly float FlightTime = (float)operationalRange / flightVelocity; // 飛行時間 sec
        public static readonly float fireRoundPerSecond = 8.84f; // 射速 rps
        public static readonly float rateFire = 1 / fireRoundPerSecond; // 開火速率 sec
        public static readonly DamageType damageType = DamageType.AssaultGun;
    }
    public static class HeavyPlasmaCannon // 四管限定 - 紙飛機、快餐俠
    {
        public static readonly int maxAmmoCapacity = 999; // 最大載彈量
        public static readonly float projectileSpread = 0.71f; //彈道散布 degree
        public static readonly int flightVelocity = 3060; // 飛行速率 m/s
        public static readonly int propulsion = flightVelocity * 50; // 開火推力
        public static readonly int operationalRange = 2030; // 射程 m
        public static readonly float FlightTime = (float)operationalRange / flightVelocity; // 飛行時間 sec
        public static readonly float fireRoundPerSecond = 2.54f; // 射速 rps
        public static readonly float rateFire = 1 / fireRoundPerSecond; // 開火速率 sec
        public static readonly DamageType damageType = DamageType.HeavyPlasmaCannon;
    }
    public static class CrazyArrow // 双管限定 - 王牌
    {
        public static readonly int maxAmmoCapacity = 999; // 最大載彈量
        public static readonly float projectileSpread = 2.16f; //彈道散布 degree
        public static readonly int flightVelocity = 2540; // 飛行速率 m/s
        public static readonly int propulsion = flightVelocity * 50; // 開火推力
        public static readonly int operationalRange = 1160; // 射程 m
        public static readonly float FlightTime = (float)operationalRange / flightVelocity; // 飛行時間 sec
        public static readonly float fireRoundPerSecond = 14.22f; // 射速 rps
        public static readonly float rateFire = 1 / fireRoundPerSecond; // 開火速率 sec
        public static readonly DamageType damageType = DamageType.CrazyArrow;
    }
    public static class RapidFireNailer // 双管 - 殺手 蠍子
    {
        public static readonly int maxAmmoCapacity = 999; // 最大載彈量
        public static readonly float projectileSpread = 1.97f; //彈道散布 degree
        public static readonly int flightVelocity = 2930; // 飛行速率 m/s
        public static readonly int propulsion = flightVelocity * 50; // 開火推力
        public static readonly int operationalRange = 1880; // 射程 m
        public static readonly float FlightTime = (float)operationalRange / flightVelocity; // 飛行時間 sec
        public static readonly float fireRoundPerSecond = 7.73f; // 射速 rps
        public static readonly float rateFire = 1 / fireRoundPerSecond; // 開火速率 sec
        public static readonly DamageType damageType = DamageType.RapidFireNailer;
    }
    public static class UltraPulseEmitter // 双管限定 - 哆啦 馴鹿
    {
        public static readonly int maxAmmoCapacity = 999; // 最大載彈量
        public static readonly float projectileSpread = 0.37f; //彈道散布 degree
        public static readonly int flightVelocity = 3270; // 飛行速率 m/s
        public static readonly int propulsion = flightVelocity * 50; // 開火推力
        public static readonly int operationalRange = 2490; // 射程 m
        public static readonly float FlightTime = (float)operationalRange / flightVelocity; // 飛行時間 sec
        public static readonly float fireRoundPerSecond = 2.73f; // 射速 rps
        public static readonly float rateFire = 1 / fireRoundPerSecond; // 開火速率 sec
        public static readonly DamageType damageType = DamageType.UltraPulseEmitter;
    }


    public struct Kocmonaut
    {
        public ControlUnit ControlUnit;
        public Type Type;
        public int Number;
        public Faction Faction;
        public Order Order;
        public string Name;

        public Kocmonaut(Faction setFaction, Order setOrder, Type setType, int setNumber, string setName, ControlUnit setCore)
        {
            Faction = setFaction;
            Order = setOrder;
            Type = setType;
            Number = setNumber;
            Name = setName;
            ControlUnit = setCore;
        }
    }

    public enum ControlUnit
    {
        None = 0,
        LocalPlayer,
        LocalBot,
        RemotePlayer,
        RemoteBot,
        Unknown,
    }
    public enum Faction
    {
        Apovaka = 0,
        Perivaka = 1,
        Unknown,
    }
    public enum Identification
    {
        Unknown = 0,
        Friend = 1,
        Foe = 2,
    }
    public enum Order
    {
        Leader = 0,
        WingmanPrimary = 1,
        WingmanSecondary = 2,
        WingmanTertiary = 3,
        Wingman4th = 4,
        Unknown = -999,
    }
    public enum Type
    {
        MinionArmor = 0,
        RedBullEnergy = 1,
        VladimirPutin = 2,
        PaperAeroplane = 3,
        Cuckoo = 4,
        BulletBill = 5,
        TimeMachine = 6,
        AceKennel = 7,
        KirbyStar = 8,
        ScorpioRouge = 9,
        nWidia = 10,
        FastFoodMan = 11,
        ReindeerTransport = 12,
        PolarisExpress = 13,
        AncientFish = 14,
        PapoyUnicorn = 15,
        PumpkinGhost = 16,
        BoundyHunterMKII = 17,
        InuitEagle = 18,
        GrandLisboa = 19,
        Prototype = 20,
    }



    //public class QQ
    //{
    //    // Display
    //    public static int GetTurretCount(Type type)
    //    {
    //        switch (type)
    //        {
    //            case Type.MinionArmor: return 2; // 神偷机兵 - 双管 - 拦截机
    //            case Type.RedBullEnergy: return 4; // 红牛能量 - 四管 - 拦截机
    //            case Type.VladimirPutin: return 6; // 普鲸 - 六管 - 拦截机
    //            case Type.PaperAeroplane: return 4; // 纸飞机 - 四管 - 巡航机
    //            case Type.Cuckoo: return 6; // 咕咕鸡 - 六管 - 攻击机
    //            case Type.BulletBill: return 2; // 炮弹比尔 - 双管 - 攻击机
    //            case Type.TimeMachine: return 2; // 时光机 - 双管 - 特攻机
    //            case Type.AceKennel: return 2; // 王牌狗屋 - 双管 - 特攻机
    //            case Type.KirbyStar: return 4; // 卡比之星 - 四管 - 特攻机
    //            case Type.ScorpioRouge: return 2; // 蝎红 - 双管 - 征服舰
    //            case Type.nWidia: return 4; // 恩威迪亚 - 四管 - 征服舰
    //            case Type.FastFoodMan: return 2; // 快餐侠 - 双管 - 巡航机
    //            case Type.ReindeerTransport: return 2; // 驯鹿空运 - 双管 - 战术舰
    //            case Type.PolarisExpress: return 6; // 北极星特快 - 六管 - 征服舰
    //            case Type.AncientFish: return 2; // 远古鱼 - 双管 - 战术舰
    //            case Type.PapoyUnicorn: return 4; // 玩具独角兽：Medium Speed
    //            case Type.PumpkinGhost: return 4; // 南瓜魅影：Large
    //            case Type.BoundyHunterMKII: return 6; // 南瓜魅影：Large
    //            default: return 2;
    //        }
    //    }
    //    public static Type GetInstanceType(string name)
    //    {
    //        switch (name)
    //        {
    //            case "Kocmocraft 00 - Minion Armor(Clone)": return Type.MinionArmor;
    //            case "Kocmocraft 01 - Red Bull Energy(Clone)": return Type.RedBullEnergy;
    //            case "Kocmocraft 02 - Vladimir Putin(Clone)": return Type.VladimirPutin;
    //            case "Kocmocraft 03 - Paper Aeroplane(Clone)": return Type.PaperAeroplane;
    //            case "Kocmocraft 04 - Cuckoo(Clone)": return Type.Cuckoo;
    //            case "Kocmocraft 05 - Bullet Bill(Clone)": return Type.BulletBill;
    //            case "Kocmocraft 06 - Time Machine(Clone)": return Type.TimeMachine;
    //            case "Kocmocraft 07 - Ace Kennel(Clone)": return Type.AceKennel;
    //            case "Kocmocraft 08 - Kirby Star(Clone)": return Type.KirbyStar;
    //            case "Kocmocraft 09 - Scorpio Rouge(Clone)": return Type.ScorpioRouge;
    //            case "Kocmocraft 10 - nWidia(Clone)": return Type.nWidia;
    //            case "Kocmocraft 11 - Fast Food Man(Clone)": return Type.FastFoodMan;
    //            case "Kocmocraft 12 - Reindeer Transport(Clone)": return Type.ReindeerTransport;
    //            case "Kocmocraft 13 - Polaris Express(Clone)": return Type.PolarisExpress;
    //            case "Kocmocraft 14 - Ancient Fish(Clone)": return Type.AncientFish;
    //            case "Kocmocraft 15 - Papoy Unicorn(Clone)": return Type.PapoyUnicorn;
    //            default: return Type.TimeMachine;
    //        }
    //    }//

    //    public static string GetKocmocraftName(int indexType)
    //    {
    //        switch (indexType)
    //        {
    //            case 0: return "Minion Armor"; // 神偷机兵 - 双管 - 拦截机
    //            case 1: return "Red Bull Energy"; // 红牛能量 - 四管 - 拦截机
    //            case 2: return "Vladimir Putin"; // 普鲸 - 六管 - 拦截机
    //            case 3: return "Paper Aeroplane"; // 纸飞机 - 四管 - 攻击机
    //            case 4: return "Cuckoo"; // 咕咕鸡 - 六管 - 攻击机
    //            case 5: return "Bullet Bill"; // 炮弹比尔 - 双管 - 攻击机
    //            case 6: return "Time Machine"; // 时光机 - 双管 - 特攻机
    //            case 7: return "Ace Kennel"; // 王牌狗屋 - 双管 - 特攻机
    //            case 8: return "Kirby Star"; // 卡比之星 - 四管 - 特攻机
    //            case 9: return "Scorpio Rouge"; // 蝎红 -- 大型双管
    //            case 10: return "nWidia"; // 恩威迪亚 -- 特大型四管
    //            case 11: return "Fast Food Man"; // 快餐侠 --- 中型双管
    //            case 12: return "Reindeer Transport"; // 驯鹿空运 -- 大型双管
    //            case 13: return "Polaris Express"; // 北极星特快 -- 特大型六管
    //            case 14: return "Ancient Fish"; // 远古鱼 -- 大型双管
    //            case 15: return "Papoy Unicorn";
    //            case 16: return "Pumpkin Ghost";
    //            case 17: return "Boundy Hunter MK.II";
    //            default: return "---";
    //        }
    //    }

    //    public static string GetKocmocraftResourceName(int index)
    //    {
    //        switch (index)
    //        {
    //            case 0: return "Prototype - Minion Armor";
    //            case 1: return "Prototype - Red Bull Energy";
    //            case 2: return "Prototype - Vladimir Putin";
    //            case 3: return "Prototype - Paper Aeroplane";
    //            case 4: return "Prototype - Cuckoo";
    //            case 5: return "Prototype - Bullet Bill";
    //            case 6: return "Prototype - Time Machine";
    //            case 7: return "Prototype - Ace Kennel";
    //            case 8: return "Prototype - Kirby Star";
    //            case 9: return "Prototype - Scorpio Rouge";
    //            case 10: return "Prototype - nWidia";
    //            case 11: return "Prototype - Fast Food Man";
    //            case 12: return "Prototype - Reindeer Transport";
    //            case 13: return "Prototype - Polaris Express";
    //            case 14: return "Prototype - Ancient Fish";
    //            case 15: return "Prototype - Unknown";
    //            case 16: return "Prototype - Unknown";
    //            case 17: return "Prototype - Unknown";
    //            case 18: return "Prototype - Unknown";
    //            case 19: return "Prototype - Unknown";
    //            case 20: return "Prototype - Unknown";
    //            default: return null;
    //        }
    //    }

    //    // Display
    //    public static string GetTypeChineseName(int index)
    //    {
    //        switch (index)
    //        {
    //            case 0: return "神偷机兵";
    //            case 1: return "红牛能量";
    //            case 2: return "普鲸";
    //            case 3: return "纸飞机";
    //            case 4: return "咕咕鸡 ";
    //            case 5: return "炮弹比尔";
    //            case 6: return "时光机";
    //            case 7: return "王牌狗屋";
    //            case 8: return "卡比之星";
    //            case 9: return "蝎红";
    //            case 10: return "恩威迪亚";
    //            case 11: return "快餐侠";
    //            case 12: return "驯鹿空运";
    //            case 13: return "北极星特快";
    //            case 14: return "远古飞鱼";
    //            case 15: return "玩具独角兽";
    //            case 16: return "南瓜魅影";
    //            case 17: return "即将登场";
    //            default: return "未知宇航机";
    //        }
    //    }
    //    public static readonly float[] TopViewSize = {
    //        4.6f, // ★★★★★★☆ 神偷机兵
    //        3.1f, // ★★☆☆☆☆☆ 红牛能量
    //        3.5f, // ★☆☆☆☆☆☆ 普鲸
    //        5.9f, // ★★★★★★★ 纸飞机
    //        5.2f, // ★★★☆☆☆☆ 咕咕鸡 
    //        3.7f, // ★★★☆☆☆☆ 炮弹比尔
    //        2.0f, // ★★☆☆☆☆☆ 时光机
    //        2.1f, // ★★★★★★☆ 王牌狗屋
    //        2.4f, // ★★★★★★★ 卡比之星
    //        6.1f, // ★★☆☆☆☆☆ 蝎红
    //        5.0f, // ★★★☆☆☆☆ 恩威迪亚
    //        5.1f, // ★★★★☆☆☆ 快餐侠
    //        6.0f, // ★★★★★☆☆ 驯鹿空运
    //        5.1f, // ★★★★☆☆☆ 北极星特快
    //        6.9f, // ★★★★★☆☆ 远古飞鱼
    //        40, // ★★★★☆☆☆ 玩具独角兽
    //        36, // ★★★☆☆☆☆ 南瓜魅影
    //        22, // ★☆☆☆☆☆☆ 赏金猎人
    //        45, // ★★★★★☆☆ 鹰纽特
    //        34, // ★★★☆☆☆☆ 安格瑞
    //        0, // ★★★★★★★ 即将登场
    //        0, // ★★★★★★★ 即将登场
    //        0, // ★★★★★★★ 即将登场
    //        0, // ★★★★★★★ 即将登场
    //    };


    //    public static readonly float[] Collider = {
    //        38.3091173f, // 神偷机兵
    //        20.21441f, // 红牛能量;
    //        18.41935f, //  普鲸;
    //         57.69397f, // 纸飞机";
    //        95.76454f, // 咕咕鸡 ";
    //        31.09758f, // 炮弹比尔";
    //        4.496421f, // 时光机";
    //        21.234167503f, // 王牌狗屋";
    //        6.356274f, // 卡比之星";
    //        115.0642f, // 蝎红";
    //        136.12f, // 恩威迪亚";
    //        60.54652f, // 快餐侠";
    //        93.2667f, // 驯鹿空运";
    //        152.6686f, // 北极星特快";
    //        70.73558282f, // 远古飞鱼";
    //        68.74551578f, // 玩具独角兽";
    //        122.675175f, // 南瓜魅影";
    //        111.4062f, // 赏金猎人";
    //        99999, // 鹰纽特";
    //        99999, // 安格瑞";
    //        99999, // 即将登场";
    //        99999, // 即将登场";
    //        99999, // 即将登场";
    //        99999, // 即将登场";
    //    };

    //    public static readonly float[] Volume = {
    //        34.7250337f, // 神偷机兵
    //        17.2552f, // 红牛能量;
    //        25.10557f, //  普鲸;
    //        25.26623f, // 纸飞机";
    //        99.89777f, // 咕咕鸡 ";
    //        50.93384f, // 炮弹比尔";
    //        3.745697f, // 时光机";
    //        10.13892941f, // 王牌狗屋";
    //        8.762174f, // 卡比之星";
    //        65.28907f, // 蝎红";
    //        95.869f, // 恩威迪亚";
    //        43.4229f, // 快餐侠";
    //        89.29118f, // 驯鹿空运";
    //        124.7412f, // 北极星特快";
    //        50.4355016f, // 远古飞鱼";
    //        64.1464130587f, // 玩具独角兽";
    //        101.595858f, // 南瓜魅影";
    //        64.00921f, // 赏金猎人";
    //        99999, // 鹰纽特";
    //        99999, // 安格瑞";
    //        99999, // 即将登场";
    //        99999, // 即将登场";
    //        99999, // 即将登场";
    //        99999, // 即将登场";
    //    };

    //    public static readonly float[] RCS = {
    //        116.450658f, // 神偷机兵
    //        78.87015f, // 红牛能量;
    //        69.46844f, //  普鲸;
    //        205.8228f, // 纸飞机";
    //        195.8923f, // 咕咕鸡 ";
    //        97.26662f, // 炮弹比尔";
    //        39.28357f, // 时光机";
    //        45.327086f, // 王牌狗屋";
    //        31.72817f, // 卡比之星";
    //        263.8345f, // 蝎红";
    //        302.3377f, // 恩威迪亚";
    //        205.5217f, // 快餐侠";
    //        252.9747f, // 驯鹿空运";
    //        343.213f, // 北极星特快";
    //        246.857114f, // 远古飞鱼";
    //        197.10966682f, // 玩具独角兽";
    //        302.01148f, // 南瓜魅影";
    //        279.8407f, // 赏金猎人";
    //        99999, // 鹰纽特";
    //        99999, // 安格瑞";
    //        99999, // 即将登场";
    //        99999, // 即将登场";
    //        99999, // 即将登场";
    //        99999, // 即将登场";
    //    };

    //}
}
