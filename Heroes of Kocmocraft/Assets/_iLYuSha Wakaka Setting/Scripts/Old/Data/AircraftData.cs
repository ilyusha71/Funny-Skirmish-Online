namespace AirSupremacy
{
    public static class AircraftData
    {
        public static readonly string[] aircraftDisplayName = new[]
        {
            "Minion Armor",
            "Red Bull Energy",
            "Vladimir Putin",
            "Paper Aeroplane",
            "Cuckoo",
            "Killer",
            "Time Machine",
            "Ace Kennel",
            "Kalpy Alpha",
            "Soul Scopio",
            "Star Nwidia",
            "Fast Food Man",
            "Reindeer Track",
            "Polaris Express",
        };
        public static readonly int[] aircraftMaxHull = new[]
        {
            4620, // 神偷機兵
            3690, // 紅牛能量
            3970, // 普鯨
            5000, // 紙飛機
            5970, // 咕咕雞
            4990, // 殺手
            3370, // 時光機
            3390, // 王牌狗屋
            3030, // 卡比阿爾法
            5130, // 法拉蝎
            5550, // 恩威迪亞
            4440, // 快餐俠
            4710, // 馴鹿速遞
            5780, // 北極星特快
        };
        public static readonly int[] aircraftMaxShield = new[]
        {
            4330, // 神偷機兵 8950
            5150, // 紅牛能量 8840
            5990, // 普鯨 9960
            4300, // 紙飛機 9300
            3930, // 咕咕雞 9900
            3910, // 殺手 8900
            5330, // 時光機 8700
            4970, // 王牌狗屋 8360
            5030, // 卡比阿爾法 8060
            3770, // 法拉蝎 8900
            3670, // 恩威迪亞 9220
            4440, // 快餐俠 8880
            4730, // 馴鹿速遞 9440
            4060, // 北極星特快 9840
        };
        public static readonly int[] aircraftSpeed = new[]
        {
            55, // 神偷機兵
            63, // 紅牛能量
            44, // 普鯨
            49, // 紙飛機
            42, // 咕咕雞
            51, // 殺手
            57,//57, // 時光機
            52, // 王牌狗屋
            66, // 卡比阿爾法
            45, // 法拉蝎
            42, // 星球戰士
            54, // 快餐俠
            47, // 聖誕狂歡
            39, // 北極星特快
        };
        public static readonly int[] aircraftMaxSpeed = new[]
        {
            237, // 神偷機兵
            274, // 紅牛能量
            173, // 普鯨
            196, // 紙飛機
            154, // 咕咕雞
            297, // 殺手
            233,//233, // 時光機
            220, // 王牌狗屋
            227, // 卡比阿爾法
            171, // 法拉蝎
            157, // 星球戰士
            204, // 快餐俠
            173, // 聖誕狂歡
            179, // 北極星特快
    };
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
}
