using UnityEngine;

namespace Kocmoca
{
    // KHAR 特快α射線炮（4管限定 / 6管限定）
    public static class KocmoHyperAlphaRay
    {
        public static readonly float projectileSpread = 0.07f; //彈道散布 degree
        public static readonly int ammoVelocity = 7730; // 飛行速率 m/s
        public static readonly int propulsion = ammoVelocity * 50; // 開火推力 propulsion = ammoVelocity / Time.fixedDeltaTime (1/0.02 = 50)
        public static readonly float flightTime = 0.5f; // 飛行時間 sec
        public static readonly float operationalRange = ammoVelocity * flightTime; // 射程 m
        public static readonly float fireRoundPerSecond = 0.7f; // 射速 rps
        public static readonly float rateFire = 1 / fireRoundPerSecond; // 開火速率 sec
        public static readonly float hullPenetration = 3.7f; // 機甲穿透
        public static readonly float shieldPenetration = 0.5f; // 護盾穿透
    }
    // KUPP 超高功率電漿砲
    public static class KocmoUltraPowerPlasma
    {
        public static readonly float projectileSpread = 0.99f; //彈道散布 degree
        public static readonly int ammoVelocity = 1900; // 飛行速率 m/s
        public static readonly int propulsion = ammoVelocity * 50; // 開火推力 propulsion = ammoVelocity / Time.fixedDeltaTime (1/0.02 = 50)
        public static readonly float flightTime = 0.37f; // 飛行時間 sec
        public static readonly float operationalRange = ammoVelocity * flightTime; // 射程 m
        public static readonly float fireRoundPerSecond = 10.0f; // 射速 rps
        public static readonly float rateFire = 1 / fireRoundPerSecond; // 開火速率 sec
        public static readonly float hullPenetration = 0.71f; // 機甲穿透
        public static readonly float shieldPenetration = 7.0f; // 護盾穿透
    }
}
