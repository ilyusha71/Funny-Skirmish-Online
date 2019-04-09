using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AirSupremacy
{
    //Minigun.Strafing.FiraRate







    //public static float[] minigunFiraRate = new[] { 0.05f, 0.093f, 0.237f };
    ////2.7f,        1.9f,        5.0f,        2.1f,        3.9f,        1.0f,        0.7f,        1.1f
    //public static float[] minigunShootingSpread = new[] { 3.97f, 2.73f, 1.37f };


    public enum Controller
    {
        Ready = 0,
        Keyboard = 1,
        Joystick = 2,
        Arduino = 3,
    }
    public enum Stage
    {
        Snow = 0,
        Mount = 1,
        Mario = 2,
    }
    public enum BattleFaction
    {
        Faction1 = 0,
        Faction2 = 1,
        Ready = 2,
    }
    public enum TypeText
    {
        Minion = 0,
        Redbull = 1,
        Putin = 2,
        Paper = 3,
        Cuckoo = 4,
        Killer = 5,
        Dorara = 6,
        Doggy = 7,
        Kalpy = 8,
        Sco = 9,
        Fighter = 10,
        FastFoodMan = 11,
        Santa = 12,
        Polaris = 13,
    }
    public enum TypeTitle
    {
        豆丁勇者 = 0,
        紅牛勇者 = 1,
        北國勇者 = 2,
        沉默勇者 = 3,
        美味勇者 = 4,
        速度勇者 = 5,
        時空勇者 = 6,
        飛行勇者 = 7,
        卡比勇者 = 8,
        蝎子勇者 = 9,
        蚊子勇者 = 10,
        火車勇者 = 11,
        熱狗勇者 = 12,
        聖誕勇者 = 13,
    }
    public enum TypeName
    {
        神偷機兵 = 0,
        紅牛能量 = 1,
        普鯨 = 2,
        紙飛機 = 3,
        咕咕雞 = 4,
        殺手 = 5,
        時光機 = 6,
        王牌狗屋 = 7,
        卡比阿爾法 = 8,
        法拉蝎 = 9,
        星球戰士 = 10,
        北極星特快 = 11,
        快餐俠 = 12,
        聖誕狂歡 = 13,
    }


    public static class FactionManager
    {
        //public static string InverseFactionString(BattlefieldFaction faction)
        //{
        //    if (faction == BattlefieldFaction.Faction1)
        //        return BattlefieldFaction.Faction2.ToString();
        //    else if (faction == BattlefieldFaction.Faction2)
        //        return BattlefieldFaction.Faction3.ToString();
        //    else if (faction == BattlefieldFaction.Faction3)
        //        return BattlefieldFaction.Faction4.ToString();
        //    else
        //        return BattlefieldFaction.Faction1.ToString();
        //}

        //public static string InverseIntString(int faction)
        //{
        //    if (faction == 0)
        //        return BattlefieldFaction.Faction2.ToString();
        //    else if (faction == 1)
        //        return BattlefieldFaction.Faction3.ToString();
        //    else if (faction == 2)
        //        return BattlefieldFaction.Faction4.ToString();
        //    else
        //        return BattlefieldFaction.Faction1.ToString();
        //}
    }

    /* 此處新增遊戲參數通用值 */
    public static class GeneralInfo
    {
        public static string saveMap = "MapSelected";
        public static string saveAircraft = "AircraftSelected";


        public static float sensitivity = 1;
        public static float missileReload = 15;
        public static Color[] factionColor = new Color[2] { new Color(0.0f, 55.0f / 256.0f, 150.0f / 256.0f, 1.0f), new Color(146.0f / 255.0f, 0.0f, 0.0f, 1.0f) };
        public static string[] factionHexColor = new string[2] { "<color=#003796>", "<color=#920000>" };
        public static float timeTakeOff;
        public static float joystickSensitivity = 1.5f;


        public static int[] distanceIdentify = new[]
        {
            2700, // 神偷機兵
            2700, // 紅牛能量
            2700, // 普鯨
            2700, // 紙飛機
            2700, // 咕咕雞
            2700, // 殺手
            2112,//3970, // 時光機
            2700, // 王牌狗屋
            2700, // 卡比阿爾法
            2700, // 法拉蝎
            2700, // 星球戰士
            2700, // 北極星特快
            2700, // 快餐俠
            2700, // 聖誕狂歡


        //1040,
        //2720,
        //1760,
        //1020,
        //1520,
        //2430,
        //1630,
        //990,
        //1500,
        //    1500,
        //    1500,
        //    1500,
        //    1500,
        //    1500,
        };
        public static int[] distanceFCR = new[] {
        990,
        970,
        1190,
        1030,
        1050,
        980,
        770,//1130,
        960};
        public static int[] distanceLock = new[] 
        {
            1500, // 神偷機兵
            1500, // 紅牛能量
            1500, // 普鯨
            1500, // 紙飛機
            1500, // 咕咕雞
            1500, // 殺手
            1293,//2700, // 時光機
            1500, // 王牌狗屋
            1500, // 卡比阿爾法
            1500, // 法拉蝎
            1500, // 星球戰士
            1500, // 北極星特快
            1500, // 快餐俠
            1500, // 聖誕狂歡
        //890,
        //870,
        //1090,
        //930,
        //950,
        //880,
        //1030,
        //860,
        //        1500,
        //    1500,
        //    1500,
        //    1500,
        //    1500,
        //    1500,
        };
        public static float[] angleLock = new[] 
        {
          0.992546f,
          0.992546f,
          0.992546f,
          0.992546f,
          0.992546f,
          0.992546f,
          0.7f, // time
          0.992546f,
            0.992546f,
          0.992546f,
          0.992546f,
          0.992546f,
          0.992546f,
          0.992546f,
        //0.981627f,
        //0.981627f,
        //0.987688f,
        //0.981627f,
        //0.981627f,
        //0.981627f,
        //0.987688f,
        //0.974370f
        };

        // 一般子彈經驗公式
        // 射程 = Force * 0.01
        // 飛行速度 = Force * 0.02
        //137000,        171000,        273000,        146000,        197000,        224000,        211293,        163000
        public static int[] minigunShootingForce = new[] { 137000, 163000, 273000 };
        public static int[] missileAmount = new[] {
        3,
        2,
        6,
        4,
        3,
        3,
        5,
        4,
        6,
        6,
        6,
        6,
        6,
        6,
        };
    }
}