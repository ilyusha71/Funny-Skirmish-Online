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

    //// 極遠距雷達
    //public static class FantasticMirrorLongRangeRadar
    //{
    //    public static readonly string RadarName = "幻镜远距雷达";
    //    public static readonly string RadarDetail = "基于魔镜雷达所发展的梦幻版本，专注于极远距离的打击能力，但相对地在中距离与侧翼有很大的盲区。";
    //    public static readonly int MaxSearchRadius = 5000;
    //    public static readonly int MinSearchRadius = 1900;
    //    public static readonly int MaxSearchAngle = 16;
    //    public static readonly int MaxLockDistance = 3200;
    //    public static readonly int MaxLockAngle = 7;
    //}
    //// 超遠距雷達
    //public static class MarksmanRangeRadar
    //{
    //    public static readonly string RadarName = "神射手远距雷达";
    //    public static readonly string RadarDetail = "这是一款服务于狙击任务的标配雷达。";
    //    public static readonly int MaxSearchRadius = 4400;
    //    public static readonly int MinSearchRadius = 1700;
    //    public static readonly int MaxSearchAngle = 24;
    //    public static readonly int MaxLockDistance = 2700;
    //    public static readonly int MaxLockAngle = 11;
    //}
    //public static class MagicMirrorLongRangeRadar
    //{
    //    public static readonly string RadarName = "魔镜远距雷达";
    //    public static readonly string RadarDetail = "远距离的多用途雷达，可用於狙击与战术打击。";
    //    public static readonly int MaxSearchRadius = 4200;
    //    public static readonly int MinSearchRadius = 1100;
    //    public static readonly int MaxSearchAngle = 27;
    //    public static readonly int MaxLockDistance = 2500;
    //    public static readonly int MaxLockAngle = 13;
    //}
    //// 遠距雷達
    //public static class BrokenLongRangeRadar
    //{
    //    public static readonly string RadarName = "破晓远距雷达";
    //    public static readonly string RadarDetail = "针对长机指挥作战使用的多功能雷达。";
    //    public static readonly int MaxSearchRadius = 3700;
    //    public static readonly int MinSearchRadius = 700;
    //    public static readonly int MaxSearchAngle = 29;
    //    public static readonly int MaxLockDistance = 2300;
    //    public static readonly int MaxLockAngle = 16;
    //}
    //// 中距雷達
    //public static class KocmoStrategyRadar
    //{
    //    public static readonly string RadarName = "卡斯摩战术雷达";
    //    public static readonly string RadarDetail = "一款通用于各项任务的制式雷达";
    //    public static readonly int MaxSearchRadius = 3300;
    //    public static readonly int MinSearchRadius = 300;
    //    public static readonly int MaxSearchAngle = 31;
    //    public static readonly int MaxLockDistance = 2000;
    //    public static readonly int MaxLockAngle = 19;
    //}
    //// 短距雷達
    //public static class KaleidoscopeRadar
    //{
    //    public static readonly string RadarName = "万花筒短距雷达";
    //    public static readonly string RadarDetail = "僚机与护卫任伟适合的狗斗雷达。";
    //    public static readonly int MaxSearchRadius = 2900;
    //    public static readonly int MinSearchRadius = 120;
    //    public static readonly int MaxSearchAngle = 37;
    //    public static readonly int MaxLockDistance = 1600;
    //    public static readonly int MaxLockAngle = 27;
    //}
    //// 超廣角雷達
    //public static class SkynetRadar
    //{
    //    public static readonly string RadarName = "天网超广角雷达";
    //    public static readonly string RadarDetail = "近距离缠斗专用雷达，主要用于占据与战术打击任务，适合机动性优良的宇航机配置。";
    //    public static readonly int MaxSearchRadius = 1600;
    //    public static readonly int MinSearchRadius = 10;
    //    public static readonly int MaxSearchAngle = 60;
    //    public static readonly int MaxLockDistance = 700;
    //    public static readonly int MaxLockAngle = 45;
    //}
}
