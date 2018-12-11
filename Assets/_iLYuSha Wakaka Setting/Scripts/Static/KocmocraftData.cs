﻿using UnityEngine;

namespace Kocmoca
{
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
    public static class LobbyInfomation
    {
        public static readonly string SCENE_LOBBY = "Galaxy Lobby";
        public static readonly string SCENE_OPERATION = "Skirmish in Dusk Lakeside";
        public static readonly string PLAYER_READY = "IsPlayerReady";
        public static readonly string PLAYER_LOADING = "isLoading";
        public static readonly string PLAYER_LOADED_LEVEL = "PlayerLoadedLevel";
        public static readonly string PLAYER_DATA_KEY = "PlayerUnknownData"; // 用於PUN設定玩家數據
        public static readonly int PLAYER_DATA_VALUE = 999; // 用於PUN設定玩家數據之對應值
        public static readonly string PREFS_TYPE = "Prefs Type";
    }
    public static class KocmocraftData
    {
        // Display
        public static string GetTypeChineseName(int index)
        {
            switch (index)
            {
                case 0: return "神偷机兵";
                case 1: return "红牛能量";
                case 2: return "普鲸";
                case 3: return "纸飞机";
                case 4: return "咕咕鸡 ";
                case 5: return "炮弹比尔";
                case 6: return "时光机";
                case 7: return "王牌狗屋";
                case 8: return "卡比之星";
                case 9: return "蝎红";
                case 10: return "恩威迪亚";
                case 11: return "快餐侠";
                case 12: return "驯鹿空运";
                case 13: return "北极星特快";
                case 14: return "远古飞鱼";
                case 15: return "玩具独角兽";
                case 16: return "南瓜派对";
                case 17: return "即将登场";
                default: return "未知宇航机";
            }
        }
        // Spawn Kocmocraft
        public static int GetPortNumber(int kocmonautNumber) { return (kocmonautNumber - 93000) / 3; }
        public static string GetKocmocraftResourceName(int index)
        {
            switch (index)
            {
                case 0: return "Prototype - Minion Armor";
                case 1: return "Prototype - Red Bull Energy";
                case 2: return "Prototype - Vladimir Putin";
                case 3: return "Prototype - Paper Aeroplane";
                case 4: return "Prototype - Cuckoo";
                case 5: return "Prototype - Bullet Bill";
                case 6: return "Prototype - Time Machine";
                case 7: return "Prototype - Ace Kennel";
                case 8: return "Prototype - Kirby Star";
                case 9: return "Prototype - Scorpio Rouge";
                case 10: return "Prototype - Nwidia";
                case 11: return "Prototype - Fast Food Man";
                case 12: return "Prototype - Reindeer Transport";
                case 13: return "Prototype - Polaris Express";
                case 14: return "Prototype - Ancient Fish";
                case 15: return "Prototype - Unknown";
                case 16: return "Prototype - Unknown";
                case 17: return "Prototype - Unknown";
                case 18: return "Prototype - Unknown";
                case 19: return "Prototype - Unknown";
                case 20: return "Prototype - Unknown";
                default: return null;
            }
        }
        // Initialize Kocmocraft Data
        public static Type GetInstanceType(string name)
        {
            switch (name)
            {
                case "Kocmocraft 00 - Minion Armor(Clone)": return Type.MinionArmor;
                case "Kocmocraft 01 - Red Bull Energy(Clone)": return Type.RedBullEnergy;
                case "Kocmocraft 02 - Vladimir Putin(Clone)": return Type.VladimirPutin;
                case "Kocmocraft 03 - Paper Aeroplane(Clone)": return Type.PaperAeroplane;
                case "Kocmocraft 04 - Cuckoo(Clone)": return Type.Cuckoo;
                case "Kocmocraft 05 - Bullet Bill(Clone)": return Type.BulletBill;
                case "Kocmocraft 06 - Time Machine(Clone)": return Type.TimeMachine;
                case "Kocmocraft 07 - Ace Kennel(Clone)": return Type.AceKennel;
                case "Kocmocraft 08 - Kirby Star(Clone)": return Type.KirbyStar;
                case "Kocmocraft 09 - Scorpio Rouge(Clone)": return Type.ScorpioRouge;
                case "Kocmocraft 10 - Nwidia(Clone)": return Type.Nwidia;
                case "Kocmocraft 11 - Fast Food Man(Clone)": return Type.FastFoodMan;
                case "Kocmocraft 12 - Reindeer Transport(Clone)": return Type.ReindeerTransport;
                case "Kocmocraft 13 - Polaris Express(Clone)": return Type.PolarisExpress;
                case "Kocmocraft 14 - Ancient Fish(Clone)": return Type.AncientFish;
                case "Kocmocraft 15 - Papoy Unicorn(Clone)": return Type.PapoyUnicorn;
                default: return Type.TimeMachine;
            }
        }
        public static int GetKocmonautNumber(int portNumber) { return portNumber * 3 + 93000; }
        public static string GetBotName(int portNumber)
        {
            switch (portNumber)
            {
                case 0: return "[Bot] Magnussen";
                case 1: return "[Bot] Russell";
                case 2: return "[Bot] Sainz Jr."; 
                case 3: return "[Bot] Pérez";
                case 4: return "[Bot] Norris";
                case 5: return "[Bot] Kvyat";
                case 6: return "[Bot] Giovinazzi";
                case 7: return "[Bot] Gasly"; 
                case 8: return "[Bot] Hülkenberg";
                case 9: return "[Bot] Ricciardo";
                case 10: return "[Bot] Verstappen";
                case 11: return "[Bot] Grosjean"; 
                case 12: return "[Bot] Bottas";
                case 13: return "[Bot] Leclerc";
                case 14: return "[Bot] Schumacher";
                case 15: return "[Bot] Alonso"; 
                case 16: return "[Bot] Rosberg";
                case 17: return "[Bot] Räikkönen";
                case 18: return "[Bot] Hamilton";
                case 19: return "[Bot] Vettel";
                default: return "[Bot]---";
            }
        }
        public static string GetKocmocraftName(int indexType)
        {
            switch (indexType)
            {
                case 0: return "Minion Armor"; // 神偷机兵 - 双管 - 拦截机
                case 1: return "Red Bull Energy"; // 红牛能量 - 四管 - 拦截机
                case 2: return "Vladimir Putin"; // 普鲸 - 六管 - 拦截机
                case 3: return "Paper Aeroplane"; // 纸飞机 - 四管 - 攻击机
                case 4: return "Cuckoo"; // 咕咕鸡 - 六管 - 攻击机
                case 5: return "Bullet Bill"; // 炮弹比尔 - 双管 - 攻击机
                case 6: return "Time Machine"; // 时光机 - 双管 - 特攻机
                case 7: return "Ace Kennel"; // 王牌狗屋 - 双管 - 特攻机
                case 8: return "Kirby Star"; // 卡比之星 - 四管 - 特攻机
                case 9: return "Scorpio Rouge"; // 蝎红 -- 大型双管
                case 10: return "Nwidia"; // 恩威迪亚 -- 特大型四管
                case 11: return "Fast Food Man"; // 快餐侠 --- 中型双管
                case 12: return "Reindeer Transport"; // 驯鹿空运 -- 大型双管
                case 13: return "Polaris Express"; // 北极星特快 -- 特大型六管
                case 14: return "Ancient Fish"; // 远古鱼 -- 大型双管
                case 15: return "Papoy Unicorn";
                case 16: return "Pumpkin Party";
                case 17: return "Coming Soon";
                default: return "---";
            }
        }

        // Huge: 12~13k
        // Large: 10~11k
        // Medium: 9k
        // Small: 8k
        // Mini: 7k
        public static int GetMaxHull(Type type)
        {
            switch (type)
            {
                case Type.MinionArmor: return 8480; // 神偷机兵：Small
                case Type.RedBullEnergy: return 8270; // 红牛能量：Small
                case Type.VladimirPutin: return 8880; // 普鲸：Small
                case Type.PaperAeroplane: return 9620; // 纸飞机：Medium
                case Type.Cuckoo: return 9550; // 咕咕鸡：Medium
                case Type.BulletBill: return 9370; // 炮弹比尔：Medium
                case Type.TimeMachine: return 7310; // 时光机：Mini
                case Type.AceKennel: return 7770; // 王牌狗屋：Mini
                case Type.KirbyStar: return 7910; // 卡比之星：Mini
                case Type.ScorpioRouge: return 11720; // 蝎红：Large
                case Type.Nwidia: return 12050; // 恩威迪亚：Huge
                case Type.FastFoodMan: return 9730; // 快餐侠：Medium
                case Type.ReindeerTransport: return 10740; // 驯鹿空运：Large
                case Type.PolarisExpress: return 13790; // 北极星特快：Huge
                case Type.AncientFish: return 10990; // 远古鱼：Large
                case Type.PapoyUnicorn: return 10350; // 玩具独角兽：Large
                case Type.PumpkinParty: return 15500; // 南瓜派对：Huge
                case Type.ComingSoon: return 0; // 南瓜派对：Large
                default: return 21120903;
            }
        }
        // 巡航7k > 特攻7k > 征服6k > 拦截5k > 攻击5k > 战术4k
        public static int GetMaxShield(Type type)
        {
            switch (type)
            {
                case Type.MinionArmor: return 8480; // 神偷机兵：双管 - 小型 - 护盾 - 高速机
                case Type.RedBullEnergy: return 9310; // 红牛能量 - 四管 - 拦截机
                case Type.VladimirPutin: return 11390; // 普鲸 - 六管 - 拦截机
                case Type.PaperAeroplane: return 7130; // 纸飞机 - 四管 - 巡航机
                case Type.Cuckoo: return 11980; // 咕咕鸡 - 六管 - 攻击机
                case Type.BulletBill: return 5640; // 炮弹比尔 - 双管 - 攻击机
                case Type.TimeMachine: return 9730; // 时光机 - 双管 - 特攻机
                case Type.AceKennel: return 8920; // 王牌狗屋 - 双管 - 特攻机
                case Type.KirbyStar: return 7750; // 卡比之星 - 四管 - 特攻机
                case Type.ScorpioRouge: return 6630; // 蝎红 - 双管 - 征服舰
                case Type.Nwidia: return 12970; // 恩威迪亚 - 四管 - 征服舰
                case Type.FastFoodMan: return 9340; // 快餐侠 - 双管 - 巡航机
                case Type.ReindeerTransport: return 7320; // 驯鹿空运 - 双管 - 战术舰
                case Type.PolarisExpress: return 7490; // 北极星特快 - 六管 - 征服舰
                case Type.AncientFish: return 7660; // 远古鱼 - 双管 - 战术舰
                case Type.PapoyUnicorn: return 11990; // 玩具独角兽：Large
                case Type.PumpkinParty: return 9420; // 南瓜派对：Large
                case Type.ComingSoon: return 0; // 南瓜派对：Large
                default: return 21120903;
            }
        }
        // Hyperspeed: 101~111
        // High Speed: 94~97
        // Medium Speed: 88~91
        // Low Speed: 83~85
        public static int GetMaxEnergy(Type type)
        {
            switch (type)
            {
                case Type.MinionArmor: return 3370; // 神偷机兵：双管 - 小型 - 护盾 - 高速机
                case Type.RedBullEnergy: return 3990; // 红牛能量 - 四管 - 拦截机
                case Type.VladimirPutin: return 2920; // 普鲸 - 六管 - 拦截机
                case Type.PaperAeroplane: return 3330; // 纸飞机 - 四管 - 巡航机
                case Type.Cuckoo: return 2270; // 咕咕鸡 - 六管 - 攻击机
                case Type.BulletBill: return 2880; // 炮弹比尔 - 双管 - 攻击机
                case Type.TimeMachine: return 2730; // 时光机 - 双管 - 特攻机
                case Type.AceKennel: return 3110; // 王牌狗屋 - 双管 - 特攻机
                case Type.KirbyStar: return 2480; // 卡比之星 - 四管 - 特攻机
                case Type.ScorpioRouge: return 3650; // 蝎红 - 双管 - 征服舰
                case Type.Nwidia: return 2350; // 恩威迪亚 - 四管 - 征服舰
                case Type.FastFoodMan: return 2940; // 快餐侠 - 双管 - 巡航机
                case Type.ReindeerTransport: return 3090; // 驯鹿空运 - 双管 - 战术舰
                case Type.PolarisExpress: return 2520; // 北极星特快 - 六管 - 征服舰
                case Type.AncientFish: return 2980; // 远古鱼 - 双管 - 战术舰
                case Type.PapoyUnicorn: return 3250; // 玩具独角兽：Large
                case Type.PumpkinParty: return 2880; // 南瓜派对：Large
                case Type.ComingSoon: return 0; // 南瓜派对：Large
                default: return 5000;
            }
        }
        // Large: 39~44
        // Medium: 37~41
        // Small: 35~39
        // Mini: 33~36
        public static int GetCruiseSpeed(Type type)
        {
            switch (type)
            {
                case Type.MinionArmor: return 37; // 神偷机兵：Small
                case Type.RedBullEnergy: return 35; // 红牛能量：Small
                case Type.VladimirPutin: return 39; // 普鲸：Small
                case Type.PaperAeroplane: return 38; // 纸飞机：Medium
                case Type.Cuckoo: return 41; // 咕咕鸡：Medium
                case Type.BulletBill: return 37; // 炮弹比尔：Medium
                case Type.TimeMachine: return 34; // 时光机：Mini
                case Type.AceKennel: return 33; // 王牌狗屋：Mini
                case Type.KirbyStar: return 36; // 卡比之星：Mini
                case Type.ScorpioRouge: return 40; // 蝎红：Large
                case Type.Nwidia: return 39; // 恩威迪亚：Large
                case Type.FastFoodMan: return 39; // 快餐侠：双管 - Medium
                case Type.ReindeerTransport: return 41; // 驯鹿空运：Large
                case Type.PolarisExpress: return 44; // 北极星特快：Large
                case Type.AncientFish: return 42; // 远古鱼：Large
                case Type.PapoyUnicorn: return 43; // 玩具独角兽：Large
                case Type.PumpkinParty: return 46; // 南瓜派对：Large
                case Type.ComingSoon: return 0; // 南瓜派对：Large
                default: return 50;
            }
        }
        // Hyperspeed: 101~111
        // High Speed: 94~97
        // Medium Speed: 88~91
        // Low Speed: 83~85
        public static int GetMaxSpeed(Type type)
        {
            switch (type)
            {
                case Type.MinionArmor: return 94; // 神偷机兵：High Speed
                case Type.RedBullEnergy: return 96; // 红牛能量：High Speed
                case Type.VladimirPutin: return 91; // 普鲸：Medium Speed
                case Type.PaperAeroplane: return 90; // 纸飞机：Medium Speed
                case Type.Cuckoo: return 89; // 咕咕鸡：Medium Speed
                case Type.BulletBill: return 111; // 炮弹比尔：Hyperspeed
                case Type.TimeMachine: return 107; // 时光机：Hyperspeed
                case Type.AceKennel: return 103; // 王牌狗屋：Hyperspeed
                case Type.KirbyStar: return 101; // 卡比之星：Hyperspeed
                case Type.ScorpioRouge: return 89; // 蝎红：Medium Speed
                case Type.Nwidia: return 88; // 恩威迪亚：Medium Speed
                case Type.FastFoodMan: return 97; // 快餐侠：High Speed
                case Type.ReindeerTransport: return 91; // 驯鹿空运：Medium Speed
                case Type.PolarisExpress: return 83; // 北极星特快：Low Speed
                case Type.AncientFish: return 85; // 远古鱼：Low Speed
                case Type.PapoyUnicorn: return 88; // 玩具独角兽：Medium Speed
                case Type.PumpkinParty: return 87; // 南瓜派对：Large
                case Type.ComingSoon: return 0; // 南瓜派对：Large
                default: return 300;
            }
        }

        public static EngineType GetEngineType(Type type)
        {
            switch (type)
            {
                case Type.MinionArmor: return EngineType.TurboFan;
                case Type.RedBullEnergy: return EngineType.TurboJet;
                case Type.VladimirPutin: return EngineType.TurboFan;
                case Type.PaperAeroplane: return EngineType.TurboJet;
                case Type.Cuckoo: return EngineType.TurboJet;
                case Type.BulletBill: return EngineType.TurboFan;
                case Type.TimeMachine: return EngineType.TurboJet;
                case Type.AceKennel: return EngineType.TurboProp;
                case Type.KirbyStar: return EngineType.TurboJet;
                case Type.ScorpioRouge: return EngineType.TurboFan;
                case Type.Nwidia: return EngineType.TurboJet;
                case Type.FastFoodMan: return EngineType.TurboFan;
                case Type.ReindeerTransport: return EngineType.TurboFan;
                case Type.PolarisExpress: return EngineType.TurboFan;
                case Type.AncientFish: return EngineType.TurboProp;
                case Type.PapoyUnicorn: return EngineType.TurboFan;
                case Type.PumpkinParty: return EngineType.TurboProp;
                default: return EngineType.None;
            }
        }

        public static FireControlSystemType GetFCS(string name)
        {
            switch (name)
            {
                case "FCS - Laser": return FireControlSystemType.Laser;
                case "FCS - Rocket": return FireControlSystemType.Rocket;
                case "FCS - Missile": return FireControlSystemType.Missile;
                default: return FireControlSystemType.Unknown;
            }
        }
        public static Vector3 GetCameraOffset(Type type)
        {
            switch (type)
            {
                case Type.BulletBill: return new Vector3(0,3.37f,7);
                case Type.TimeMachine: return new Vector3(0, 2.97f, 0);
                case Type.AceKennel: return new Vector3(0, 1.37f, 0);
                case Type.KirbyStar: return new Vector3(0, 2.37f, 0);
                case Type.AncientFish: return new Vector3(0, 3.93f, 10.0f);
                case Type.PumpkinParty: return new Vector3(0, 5.37f, 10.0f);
                default: return new Vector3(0, 3.37f, 7);
            }
        }
        public static float TurretDamageFix(Type type)
        {
            switch (type)
            {
                case Type.MinionArmor: return 1.0f; // 神偷机兵 - 双管 - 拦截机
                case Type.RedBullEnergy: return 0.73f; // 红牛能量 - 四管 - 拦截机
                case Type.VladimirPutin: return 0.61f; // 普鲸 - 六管 - 拦截机
                case Type.PaperAeroplane: return 0.73f; // 纸飞机 - 四管 - 巡航机
                case Type.Cuckoo: return 0.61f; // 咕咕鸡 - 六管 - 攻击机
                case Type.BulletBill: return 1.0f; // 炮弹比尔 - 双管 - 攻击机
                case Type.TimeMachine: return 1.0f; // 时光机 - 双管 - 特攻机
                case Type.AceKennel: return 1.0f; // 王牌狗屋 - 双管 - 特攻机
                case Type.KirbyStar: return 0.73f; // 卡比之星 - 四管 - 特攻机
                case Type.ScorpioRouge: return 1.0f; // 蝎红 - 双管 - 征服舰
                case Type.Nwidia: return 0.73f; // 恩威迪亚 - 四管 - 征服舰
                case Type.FastFoodMan: return 1.0f; // 快餐侠 - 双管 - 巡航机
                case Type.ReindeerTransport: return 1.0f; // 驯鹿空运 - 双管 - 战术舰
                case Type.PolarisExpress: return 0.61f; // 北极星特快 - 六管 - 征服舰
                case Type.AncientFish: return 1.0f; // 远古鱼 - 双管 - 战术舰
                case Type.PapoyUnicorn: return 0.73f; // 玩具独角兽：Medium Speed
                case Type.PumpkinParty: return 0.73f; // 南瓜派对：Large
                case Type.ComingSoon: return 0; // 南瓜派对：Large
                default: return 1.0f;
            }
        }
        // Display
        public static int GetTurretCount(Type type)
        {
            switch (type)
            {
                case Type.MinionArmor: return 2; // 神偷机兵 - 双管 - 拦截机
                case Type.RedBullEnergy: return 4; // 红牛能量 - 四管 - 拦截机
                case Type.VladimirPutin: return 6; // 普鲸 - 六管 - 拦截机
                case Type.PaperAeroplane: return 4; // 纸飞机 - 四管 - 巡航机
                case Type.Cuckoo: return 6; // 咕咕鸡 - 六管 - 攻击机
                case Type.BulletBill: return 2; // 炮弹比尔 - 双管 - 攻击机
                case Type.TimeMachine: return 2; // 时光机 - 双管 - 特攻机
                case Type.AceKennel: return 2; // 王牌狗屋 - 双管 - 特攻机
                case Type.KirbyStar: return 4; // 卡比之星 - 四管 - 特攻机
                case Type.ScorpioRouge: return 2; // 蝎红 - 双管 - 征服舰
                case Type.Nwidia: return 4; // 恩威迪亚 - 四管 - 征服舰
                case Type.FastFoodMan: return 2; // 快餐侠 - 双管 - 巡航机
                case Type.ReindeerTransport: return 2; // 驯鹿空运 - 双管 - 战术舰
                case Type.PolarisExpress: return 6; // 北极星特快 - 六管 - 征服舰
                case Type.AncientFish: return 2; // 远古鱼 - 双管 - 战术舰
                case Type.PapoyUnicorn: return 4; // 玩具独角兽：Medium Speed
                case Type.PumpkinParty: return 4; // 南瓜派对：Large
                case Type.ComingSoon: return 0; // 南瓜派对：Large
                default: return 2;
            }
        }

        public static string GetPowerData(Type type)
        {
            float velocity = KocmoLaserCannon.flightVelocity * TurretDamageFix(type);
            float minDamage = velocity * velocity * 0.0001f * KocmoLaserCannon.coefficientMinDamage * GetTurretCount(type) * 0.5f;
            float maxDamage = (velocity + GetMaxSpeed(type)) * (velocity + GetMaxSpeed(type)) * 0.0001f * KocmoLaserCannon.coefficientMaxDamage * GetTurretCount(type) * 0.5f;
            return (int)minDamage + "~" + (int)maxDamage;
        }
        public static int GetMaxRangeData(Type type)
        {
            return (int)((KocmoLaserCannon.flightVelocity * TurretDamageFix(type) + GetMaxSpeed(type)) * KocmoLaserCannon.flightTime);
        }
    }

    public enum EngineType
    {
        None = 0,
        TurboJet = 1, // 渦輪噴射
        TurboFan = 2, // 渦輪扇
        TurboProp = 3, // 渦輪螺旋槳
    }

    public static class TurboJet
    {
        public static readonly float engineMinVol = 0f;         //The minimum volume of the engine
        public static readonly float engineMaxVol = 0.137f;        //The maximum volume of the engine
        public static readonly float engineMinPitch = 0.77f;      //The minimum pitch of the engine
        public static readonly float engineMaxPitch = 1f;		//The maximum pitch of the engine
    }
    public static class TurboFan
    {
        public static readonly float engineMinVol = 0f;         //The minimum volume of the engine
        public static readonly float engineMaxVol = 0.097f;        //The maximum volume of the engine
        public static readonly float engineMinPitch = 0.8f;      //The minimum pitch of the engine
        public static readonly float engineMaxPitch = 1.3f;		//The maximum pitch of the engine
    }
    public static class Propeller
    {
        public static readonly float engineMinVol = 0f;         //The minimum volume of the engine
        public static readonly float engineMaxVol = 0.237f;        //The maximum volume of the engine
        public static readonly float engineMinPitch = .9f;      //The minimum pitch of the engine
        public static readonly float engineMaxPitch = 1.1f;		//The maximum pitch of the engine
    }
    public static class OriginalSettingEngine
    {
        public static readonly float engineMinVol = 0f;         //The minimum volume of the engine
        public static readonly float engineMaxVol = .6f;        //The maximum volume of the engine
        public static readonly float engineMinPitch = .3f;      //The minimum pitch of the engine
        public static readonly float engineMaxPitch = .8f;      //The maximum pitch of the engine
    }

    public static class KocmoLaserCannon
    {
        public static readonly int countPerBatch = 5; // 每批生成量
        public static readonly int maxPoorInventory = 500; // 最大物件池存量
        public static readonly float maxFireAngle = Mathf.Cos(7 * Mathf.Deg2Rad); // 最大開火夾角
        public static readonly int maxAmmoCapacity = 999; // 最大載彈量
        public static readonly float projectileSpread = 0.39f; //彈道散布 degree
        public static readonly int flightVelocity = 1630;//2970; // 飛行速率 m/s
        public static readonly int propulsion = flightVelocity * 50; // 開火推力
        public static readonly float flightTime = 0.73f; // 飛行時間 sec
        public static readonly float operationalRange = flightVelocity * flightTime; // 射程 m
        public static readonly float fireRoundPerSecond = 4.77f; // 射速 rps
        public static readonly float rateFire = 1 / fireRoundPerSecond; // 開火速率 sec
        public static readonly float coefficientMinDamage = 1.3035f;
        public static readonly float coefficientMaxDamage = 4.1949f;
    }
    public static class KocmoRocketLauncher
    {
        public static readonly int countPerBatch = 1; // 每批生成量
        public static readonly int maxPoorInventory = 100; // 最大物件池存量
        public static readonly float maxFireAngle = Mathf.Cos(13 * Mathf.Deg2Rad); // 最大開火夾角
        public static readonly int maxAmmoCapacity = 12; // 最大載彈量
        public static readonly int flightVelocity = 703; // 飛行速率 m/s
        public static readonly int thrust = flightVelocity * 50; // 基本推力
        public static readonly float flightTime = 2.37f; // 飛行時間 sec
        public static readonly float fireRoundPerSecond = 3.37f; // 射速 rps
        public static readonly float rateFire = 1 / fireRoundPerSecond; // 開火速率 sec
        public static readonly float timeReload = 9.3f;
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
        public static readonly float fireRoundPerSecond = 1.137f; // 射速 rps
        public static readonly float rateFire = 1 / fireRoundPerSecond; // 開火速率 sec
        public static readonly float timeReload = 15.0f;
        public static readonly float coefficientDamageBasic = 7.0f;
        public static readonly float coefficientDamageHull = 1.37f;
        public static readonly float coefficientDamageShield = 2.37f;
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


    public struct Kocmonaut
    {
        public Core Core;
        public Type Type;
        public int Number;
        public Faction Faction;
        public Order Order;
        public string Name;

        public Kocmonaut(Faction setFaction, Order setOrder, Type setType, int setNumber, string setName, Core setCore)
        {
            Faction = setFaction;
            Order = setOrder;
            Type = setType;
            Number = setNumber;
            Name = setName;
            Core = setCore;
        }
    }

    public enum Core
    {
        LocalPlayer = 0,
        LocalBot = -1,
        RemotePlayer = 100,
        RemoteBot = 99,
        Unknown = -999,
    }
    public enum Faction
    {
        Apovaka = 0,
        Perivaka = 1,
        Unknown = -999,
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
        Nwidia = 10,
        FastFoodMan = 11,
        ReindeerTransport = 12,
        PolarisExpress = 13,
        AncientFish=14,
        PapoyUnicorn = 15,
        PumpkinParty = 16,
        ComingSoon = 17,
        Unknown = -999,
    }
    public enum FireControlSystemType
    {
        Laser = 0,
        Rocket = 1,
        Missile = 2,
        Unknown = -999,
    }
}
