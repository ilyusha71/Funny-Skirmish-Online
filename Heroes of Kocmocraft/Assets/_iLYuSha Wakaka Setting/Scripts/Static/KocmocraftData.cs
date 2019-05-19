using UnityEngine;

namespace Kocmoca
{
    [System.Serializable]
    public class PrototypeSize
    {
        public float Wingspan;
        public float Length;
        public float Height;
        public float MaxSize;
        public float HalfSize; // 用于三视图摄影机正交尺寸
        public float NearView;
        public float FarView;
        public Vector3 WingspanScale;
        public Vector3 LengthScale;
        public Vector3 HeightScale;

        public PrototypeSize(Vector3 size)
        {
            Wingspan = size.x;
            Length = size.z;
            Height = size.y;
            MaxSize = Mathf.Max(Wingspan, Length);
            MaxSize = Mathf.Max(MaxSize, Height);
            HalfSize = MaxSize * 0.5f;
            NearView = HalfSize + 2.7f;
            FarView = HalfSize + 19.3f;
            WingspanScale = new Vector3(Wingspan / MaxSize, 1, 1);
            LengthScale = new Vector3(Length / MaxSize, 1, 1);
            HeightScale = new Vector3(Height / MaxSize, 1, 1);
        }
    }

    [System.Serializable]
    public class SizeView
    {
        public float Wingspan;
        public float Length;
        public float Height;
        public float MaxSize;
        public float HalfSize; // 用于三视图摄影机正交尺寸
        public float NearView;
        public float FarView;
        public Vector3 WingspanScale;
        public Vector3 LengthScale;
        public Vector3 HeightScale;

        public SizeView(Vector3 size)
        {
            Wingspan = size.x;
            Length = size.z;
            Height = size.y;
            MaxSize = Mathf.Max(Wingspan, Length);
            MaxSize = Mathf.Max(MaxSize, Height);
            HalfSize = MaxSize * 0.5f;
            NearView = HalfSize + 2.7f;
            FarView = HalfSize + 19.3f;
            WingspanScale = new Vector3(Wingspan / MaxSize, 1, 1);
            LengthScale = new Vector3(Length / MaxSize, 1, 1);
            HeightScale = new Vector3(Height / MaxSize, 1, 1);
        }
    }
    public static class KocmocraftData
    {











        // 北极星特快 南瓜魅影 咕咕鸡 恩威迪亚 蝎红 15~20K    6~7星
        // 驯鹿空运 赏金猎人 远古飞鱼 玩具独角兽 10~15K 4~5星
        // 纸飞机  快餐侠 神偷机兵 9K~11K 3~4星
        // 炮弹比尔 普鲸 鹰纽特 红牛能量 6K~8K  2~3星
        // 王牌狗屋 卡比之星 时光机 3K~5K 1~2星
        public static readonly int[] Hull = {
            16049, // ★★★★★☆☆ 神偷机兵
            7831, //   ★★☆☆☆☆☆ 红牛能量
            10848, // ★★★☆☆☆☆ 普鲸
            12656, // ★★★☆☆☆☆ 纸飞机
            19439, // ★★★★★★☆ 咕咕鸡 
            15339, // ★★★★☆☆☆ 炮弹比尔
            5350, //   ★☆☆☆☆☆☆ 时光机
            11315, // ★★★☆☆☆☆ 王牌狗屋
            7476, //   ★★☆☆☆☆☆ 卡比之星
            17649, // ★★★★★☆☆ 蝎红
            21059, // ★★★★★★☆ 恩威迪亚
            12650, // ★★★☆☆☆☆ 快餐侠
            16326, // ★★★★★☆☆ 驯鹿空运
            20680, // ★★★★★★☆ 北极星特快
            15475, // ★★★★☆☆☆ 远古飞鱼
            15851, // ★★★★☆☆☆ 玩具独角兽
            22295, // ★★★★★★★ 南瓜魅影
            18308, // ★★★★★☆☆ 赏金猎人
            9143, //   ★★☆☆☆☆☆ 鹰纽特
            15789, // ★★★★☆☆☆ 新葡鲸
            0, // ★★★★★★★ 即将登场
            0, // ★★★★★★★ 即将登场
            0, // ★★★★★★★ 即将登场
            0, // ★★★★★★★ 即将登场
        };

        public static readonly int[] Shield = {
            11754, // ★★★☆☆☆☆ 神偷机兵
            12934, // ★★★★☆☆☆ 红牛能量
            16450, // ★★★★★☆☆ 普鲸
            20741, // ★★★★★★☆ 纸飞机
            17801, // ★★★★★☆☆ 咕咕鸡 
            5229, //   ★☆☆☆☆☆☆ 炮弹比尔
            23740, // ★★★★★★★ 时光机
            14859, // ★★★★☆☆☆ 王牌狗屋
            10721, // ★★★☆☆☆☆ 卡比之星
            13915, // ★★★★☆☆☆ 蝎红
            10773, // ★★★☆☆☆☆ 恩威迪亚
            7945, //   ★★☆☆☆☆☆ 快餐侠
            8409, //   ★★☆☆☆☆☆ 驯鹿空运
            3999, //   ★☆☆☆☆☆☆ 北极星特快
            20884, // ★★★★★★☆ 远古飞鱼
            22058, // ★★★★★★★ 玩具独角兽
            19242, // ★★★★★★☆ 南瓜魅影
            13749, // ★★★★☆☆☆ 赏金猎人
            12934, // ★★★★☆☆☆ 鹰纽特
            24000, // ★★★★★★★ 新葡鲸
            0, // ★★★★★★★ 即将登场
            0, // ★★★★★★★ 即将登场
            0, // ★★★★★★★ 即将登场
            0, // ★★★★★★★ 即将登场
        };
        public static readonly int[] Energy = {
            1192, // ★★☆☆☆☆☆ 神偷机兵
            3690, // ★★★★★★★ 红牛能量
            1263, // ★★☆☆☆☆ 普鲸
            1597, // ★★★☆☆☆☆ 纸飞机
            2311, // ★★★★☆☆☆ 咕咕鸡 
            2075, // ★★★★☆☆☆ 炮弹比尔
            2773, // ★★★★★☆☆ 时光机
            2237, // ★★★★☆☆☆ 王牌狗屋
            888, //   ★☆☆☆☆☆☆ 卡比之星
            3392, // ★★★★★★★ 蝎红
            1398, // ★★☆☆☆☆☆ 恩威迪亚
            3154, // ★★★★★★☆ 快餐侠
            3131, // ★★★★★★☆ 驯鹿空运
            2498, // ★★★★★☆☆ 北极星特快
            1407, // ★★☆☆☆☆☆ 远古飞鱼
            2299, // ★★★★☆☆☆ 玩具独角兽
            1639, // ★★★☆☆☆☆ 南瓜魅影
            1553, // ★★★☆☆☆☆ 赏金猎人
            2444, // ★★★★★☆☆ 鹰纽特
            1774, // ★★★☆☆☆☆ 新葡鲸
            0, // ★★★★★★★ 即将登场
            0, // ★★★★★★★ 即将登场
            0, // ★★★★★★★ 即将登场
            0, // ★★★★★★★ 即将登场
        };
        public static readonly int[] CruiseSpeed = {
            53, // ★★★★★★☆ 神偷机兵
            28, // ★★☆☆☆☆☆ 红牛能量
            23, // ★☆☆☆☆☆☆ 普鲸
            59, // ★★★★★★★ 纸飞机
            37, // ★★★☆☆☆☆ 咕咕鸡 
            35, // ★★★☆☆☆☆ 炮弹比尔
            27, // ★★☆☆☆☆☆ 时光机
            54, // ★★★★★★☆ 王牌狗屋
            60, // ★★★★★★★ 卡比之星
            29, // ★★☆☆☆☆☆ 蝎红
            47, // ★★★☆☆☆☆ 恩威迪亚
            39, // ★★★★☆☆☆ 快餐侠
            48, // ★★★★★☆☆ 驯鹿空运
            38, // ★★★★☆☆☆ 北极星特快
            44, // ★★★★★☆☆ 远古飞鱼
            37, // ★★★☆☆☆☆ 玩具独角兽
            36, // ★★★☆☆☆☆ 南瓜魅影
            22, // ★☆☆☆☆☆☆ 赏金猎人
            28, // ★★☆☆☆☆☆ 鹰纽特
            30, // ★★☆☆☆☆☆ 新葡鲸
            0, // ★★★★★★★ 即将登场
            0, // ★★★★★★★ 即将登场
            0, // ★★★★★★★ 即将登场
            0, // ★★★★★★★ 即将登场
        };
        public static readonly int[] AfterburnerSpeed = {
            97, //   ★★★★☆☆☆ 神偷机兵
            114, // ★★★★★☆☆ 红牛能量
            116, // ★★★★★★☆ 普鲸
            75, //   ★☆☆☆☆☆☆ 纸飞机
            87, //   ★★☆☆☆☆☆ 咕咕鸡 
            133, // ★★★★★★★ 炮弹比尔
            127, // ★★★★★★★ 时光机
            85, //   ★★☆☆☆☆☆ 王牌狗屋
            120, // ★★★★★★☆ 卡比之星
            119, // ★★★★★★☆ 蝎红
            100, // ★★★★☆☆☆ 恩威迪亚
            129, // ★★★★★★★ 快餐侠
            117, // ★★★★★★☆ 驯鹿空运
            107, // ★★★★★☆☆ 北极星特快
            113, // ★★★★★☆☆ 远古飞鱼
            77, //   ★☆☆☆☆☆☆ 玩具独角兽
            91, //   ★★★☆☆☆☆ 南瓜魅影
            99, //   ★★★★☆☆☆ 赏金猎人
            123, // ★★★★★★☆ 鹰纽特
            105, // ★★★★☆☆☆ 新葡鲸
            0, // ★★★★★★★ 即将登场
            0, // ★★★★★★★ 即将登场
            0, // ★★★★★★★ 即将登场
            0, // ★★★★★★★ 即将登场
        };
        public static int GetSpeedLevel(int index)
        {
            float per = (float)(AfterburnerSpeed[index] - 70) / 63;
            if (per > 0.857142f)
                return 7;
            else if (per > 0.714285f)
                return 6;
            else if (per > 0.571428f)
                return 5;
            else if (per > 0.428571f)
                return 4;
            else if (per > 0.285714f)
                return 3;
            else if (per > 0.142857f)
                return 2;
            else
                return 1;
        }


        // Spawn Kocmocraft
        public static int GetPortNumber(int kocmonautNumber) { return (kocmonautNumber - 93000) / 3; }
        // Initialize Kocmocraft Data
        public static int GetKocmonautNumber(int portNumber) { return portNumber * 3 + 93000; }
        public static string GetBotName(int portNumber)
        {
            switch (portNumber)
            {
                //case 0: return "[Bot] Magnussen";
                //case 1: return "[Bot] Russell";
                //case 2: return "[Bot] Sainz Jr."; 
                //case 3: return "[Bot] Pérez";
                //case 4: return "[Bot] Norris";
                //case 5: return "[Bot] Kvyat";
                //case 6: return "[Bot] Giovinazzi";
                //case 7: return "[Bot] Gasly"; 
                //case 8: return "[Bot] Hülkenberg";
                //case 9: return "[Bot] Ricciardo";
                //case 10: return "[Bot] Verstappen";
                //case 11: return "[Bot] Grosjean"; 
                //case 12: return "[Bot] Bottas";
                //case 13: return "[Bot] Leclerc";
                //case 14: return "[Bot] Schumacher";
                //case 15: return "[Bot] Alonso"; 
                //case 16: return "[Bot] Rosberg";
                //case 17: return "[Bot] Räikkönen";
                //case 18: return "[Bot] Hamilton";
                //case 19: return "[Bot] Vettel";
                default: return "[Bot] " + portNumber;
            }
        }




        public static EngineType GetEngineType(Type type)
        {
            switch (type)
            {
                case Type.MinionArmor: return EngineType.Turbofan;
                case Type.RedBullEnergy: return EngineType.Turbojet;
                case Type.VladimirPutin: return EngineType.PulsedPlasmaThruster;
                case Type.PaperAeroplane: return EngineType.IonThruster;
                case Type.Cuckoo: return EngineType.BiomassEnergy;
                case Type.BulletBill: return EngineType.Turbofan;
                case Type.TimeMachine: return EngineType.IonThruster;
                case Type.AceKennel: return EngineType.Turboprop;
                case Type.KirbyStar: return EngineType.Turbojet;
                case Type.ScorpioRouge: return EngineType.Turbofan;
                case Type.nWidia: return EngineType.IonThruster;
                case Type.FastFoodMan: return EngineType.Turbofan;
                case Type.ReindeerTransport: return EngineType.BiomassEnergy;
                case Type.PolarisExpress: return EngineType.Turbofan;
                case Type.AncientFish: return EngineType.Turboshaft;
                case Type.PapoyUnicorn: return EngineType.PulsedPlasmaThruster;
                case Type.PumpkinGhost: return EngineType.Turboshaft;
                case Type.BoundyHunterMKII: return EngineType.IonThruster;
                default: return EngineType.Turbofan;
            }
        }

        public static Vector3 GetCameraOffset(Type type)
        {
            switch (type)
            {
                //case Type.RedBullEnergy: return new Vector3(0, 3.0f, 7.0f);
                //case Type.VladimirPutin: return new Vector3(0, 3.0f, 5.0f);
                //case Type.PaperAeroplane: return new Vector3(0, 3.0f, 7.0f);

                //case Type.BulletBill: return new Vector3(0,3.37f,7);
                //case Type.TimeMachine: return new Vector3(0, 2.97f, 0);
                //case Type.AceKennel: return new Vector3(0, 1.37f, 0);
                //case Type.KirbyStar: return new Vector3(0, 2.37f, 0);

                //case Type.ScorpioRouge: return new Vector3(0, 5.0f, 10.0f);

                //case Type.AncientFish: return new Vector3(0, 3.93f, 10.0f);
                //case Type.PumpkinGhost: return new Vector3(0, 7.0f, 10.0f);
                default: return new Vector3(0, 5.0f, 10.0f);
            }
        }

        public static readonly int[] TurretCount = {
            2, // ★★☆☆☆☆ 神偷机兵
            4, // ★★★★☆☆ 红牛能量
            6, // ★★★★★★ 普鲸
            4, // ★★★★☆☆ 纸飞机
            6, // ★★★★★★ 咕咕鸡 
            2, // ★★☆☆☆☆ 炮弹比尔
            2, // ★★☆☆☆☆ 时光机
            2, // ★★☆☆☆☆ 王牌狗屋
            4, // ★★★★☆☆ 卡比之星
            2, // ★★☆☆☆☆ 蝎红
            4, // ★★★★☆☆ 恩威迪亚
            4, // ★★☆☆☆☆ 快餐侠
            2, // ★★☆☆☆☆ 驯鹿空运
            6, // ★★★★★★ 北极星特快
            2, // ★★☆☆☆☆ 远古飞鱼
            4, // ★★★★☆☆ 玩具独角兽
            4, // ★★★★☆☆ 南瓜魅影
            6, // ★★★★★★ 赏金猎人
            4, // ★★★★☆☆ 鹰纽特
            6, // ★★★★☆☆ 新葡鲸
            0, // ★★☆☆☆☆ 即将登场
            0, // ★★☆☆☆☆ 即将登场
            0, // ★★☆☆☆☆ 即将登场
            0, // ★★☆☆☆☆ 即将登场
        };
    }

}