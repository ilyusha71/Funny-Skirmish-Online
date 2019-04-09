namespace AirSupremacy
{
    public enum WeaponType
    {
        None = 0,
        Minigun = 1,
        Rocket = 2,
        Missile = 3,
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
        RapidFireNailer= 8, // 速射釘槍 179% 55%
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
        public static readonly float flightTime = (float)operationalRange / flightVelocity; // 飛行時間 sec
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
        public static readonly float flightTime = (float)operationalRange / flightVelocity; // 飛行時間 sec
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
        public static readonly float flightTime = (float)operationalRange / flightVelocity; // 飛行時間 sec
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
        public static readonly float flightTime = (float)operationalRange / flightVelocity; // 飛行時間 sec
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
        public static readonly float flightTime = (float)operationalRange / flightVelocity; // 飛行時間 sec
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
        public static readonly float flightTime = (float)operationalRange / flightVelocity; // 飛行時間 sec
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
        public static readonly float flightTime = (float)operationalRange / flightVelocity; // 飛行時間 sec
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
        public static readonly float flightTime = (float)operationalRange / flightVelocity; // 飛行時間 sec
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
        public static readonly float flightTime = (float)operationalRange / flightVelocity; // 飛行時間 sec
        public static readonly float fireRoundPerSecond = 2.73f; // 射速 rps
        public static readonly float rateFire = 1 / fireRoundPerSecond; // 開火速率 sec
        public static readonly DamageType damageType = DamageType.UltraPulseEmitter;
    }
}
