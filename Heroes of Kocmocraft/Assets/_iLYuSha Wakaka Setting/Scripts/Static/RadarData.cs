using UnityEngine;

namespace Kocmoca
{
    // ELRR 極遠距雷達
    public static class ExtremelyLongRangeRadar
    {
        public static readonly int MaxSearchRadius = 5000;
        public static readonly int MaxLockDistance = 3500;
        public static readonly int MaxSearchRadiusSqr = MaxSearchRadius * MaxSearchRadius;
        public static readonly int MaxLockDistanceSqr = MaxLockDistance * MaxLockDistance;
        public static readonly float MaxSearchAngle = Mathf.Cos(17 * Mathf.Deg2Rad);
        public static readonly float MaxLockAngle = Mathf.Cos(7 * Mathf.Deg2Rad);
        public static readonly float MaxAutoAim = Mathf.Cos(0.7f * Mathf.Deg2Rad);
    }
    // 遠距雷達
    public static class LongRangeRadar
    {
        public static readonly int MaxSearchRadius = 4300;
        public static readonly int MaxLockDistance = 2300;
        public static readonly int MaxSearchRadiusSqr = MaxSearchRadius * MaxSearchRadius;
        public static readonly int MaxLockDistanceSqr = MaxLockDistance * MaxLockDistance;
        public static readonly float MaxSearchAngle = Mathf.Cos(21 * Mathf.Deg2Rad);
        public static readonly float MaxLockAngle = Mathf.Cos(9 * Mathf.Deg2Rad);
        public static readonly float MaxAutoAim = Mathf.Cos(2 * Mathf.Deg2Rad);
    }
    // 中距雷達
    public static class MediumRangeRadar
    {
        public static readonly int MaxSearchRadius = 3600;
        public static readonly int MaxLockDistance = 1700;
        public static readonly int MaxSearchRadiusSqr = MaxSearchRadius * MaxSearchRadius;
        public static readonly int MaxLockDistanceSqr = MaxLockDistance * MaxLockDistance;
        public static readonly float MaxSearchAngle = Mathf.Cos(29 * Mathf.Deg2Rad);
        public static readonly float MaxLockAngle = Mathf.Cos(13 * Mathf.Deg2Rad);
        public static readonly float MaxAutoAim = Mathf.Cos(3 * Mathf.Deg2Rad);
    }
    // 短距雷達
    public static class ShortRangeRadar
    {
        public static readonly int MaxSearchRadius = 2500;
        public static readonly int MaxLockDistance = 1200;
        public static readonly int MaxSearchRadiusSqr = MaxSearchRadius * MaxSearchRadius;
        public static readonly int MaxLockDistanceSqr = MaxLockDistance * MaxLockDistance;
        public static readonly float MaxSearchAngle = Mathf.Cos(37 * Mathf.Deg2Rad);
        public static readonly float MaxLockAngle = Mathf.Cos(19 * Mathf.Deg2Rad);
        public static readonly float MaxAutoAim = Mathf.Cos(5 * Mathf.Deg2Rad);
    }
    // 超廣角雷達
    public static class UltraWideRangeRadar
    {
        public static readonly int MaxSearchRadius = 1600;
        public static readonly int MaxLockDistance = 700;
        public static readonly int MaxSearchRadiusSqr = MaxSearchRadius * MaxSearchRadius;
        public static readonly int MaxLockDistanceSqr = MaxLockDistance * MaxLockDistance;
        public static readonly float MaxSearchAngle = Mathf.Cos(45 * Mathf.Deg2Rad);
        public static readonly float MaxLockAngle = Mathf.Cos(27 * Mathf.Deg2Rad);
        public static readonly float MaxAutoAim = Mathf.Cos(7 * Mathf.Deg2Rad);
    }
}
