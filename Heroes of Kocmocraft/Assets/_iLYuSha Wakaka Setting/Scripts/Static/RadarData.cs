using UnityEngine;

namespace Kocmoca
{

    public static class RadarParameter
    {
        public static readonly int maxSearchRadiusSqr = 4000000; // 2000m
        public static readonly int maxLockDistanceSqr = 739100; // 860m
        public static readonly float maxSearchAngle = Mathf.Cos(37 * Mathf.Deg2Rad);
        public static readonly float maxLockAngle = Mathf.Cos(27 * Mathf.Deg2Rad);

        // ★☆☆☆☆☆☆ 1000 ~ 1500 m
        // ★★☆☆☆☆☆ 1500 ~ 2000 m
        // ★★★☆☆☆☆ 2000 ~ 2500 m
        // ★★★★☆☆☆ 2500 ~ 3000 m
        // ★★★★★☆☆ 3000 ~ 3500 m
        // ★★★★★★☆ 3500 ~ 4000 m
        // ★★★★★★★ 4000 ~ 4500 m
    }

    // ELRR 極遠距雷達
    public static class ExtremelyLongRangeRadar
    {
        public static readonly int MaxSearchRadius = 5000;
        public static readonly int MaxLockDistance = 3200;
        public static readonly int MaxSearchAngle = 22;
        public static readonly int MaxLockAngle = 7;
    }
    // 超遠距雷達
    public static class VeryLongRangeRadar
    {
        public static readonly int MaxSearchRadius = 4300;
        public static readonly int MaxLockDistance = 2700;
        public static readonly int MaxSearchAngle = 24;
        public static readonly int MaxLockAngle = 9;
    }
    // 遠距雷達
    public static class LongRangeRadar
    {
        public static readonly int MaxSearchRadius = 3700;
        public static readonly int MaxLockDistance = 2300;
        public static readonly int MaxSearchAngle = 27;
        public static readonly int MaxLockAngle = 13;
    }
    // 中距雷達
    public static class MediumRangeRadar
    {
        public static readonly int MaxSearchRadius = 3300;
        public static readonly int MaxLockDistance = 2000;
        public static readonly int MaxSearchAngle = 31;
        public static readonly int MaxLockAngle = 19;
    }
    // 短距雷達
    public static class ShortRangeRadar
    {
        public static readonly int MaxSearchRadius = 2900;
        public static readonly int MaxLockDistance = 1600;
        public static readonly int MaxSearchAngle = 37;
        public static readonly int MaxLockAngle = 27;
    }
    // 超廣角雷達
    public static class UltraWideRangeRadar
    {
        public static readonly int MaxSearchRadius = 1600;
        public static readonly int MaxLockDistance = 700;
        public static readonly int MaxSearchAngle = 45;
        public static readonly int MaxLockAngle = 37;
    }
}
