using UnityEngine;

namespace Kocmoca
{
    public static class LobbyInfomation
    {
        public static readonly string SCENE_LOBBY = "New Galaxy Lobby";
        public static readonly string SCENE_HANGAR = "New Airport 3";
        public static readonly string SCENE_OPERATION = "Kocmocarkstan";//"Skirmish in Dusk Lakeside";
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
    }

    public static class HangarData
    {
        public static readonly Color32[] FrameColor = new Color32[] {
            new Color32(255,255,123,237), // 神偷机兵：黄123
            new Color32(255,255,255,237), // 红牛能量：白255
            new Color32(117,185,255,237), // 普鲸：蓝
            new Color32(255,255,255,237), // 纸飞机：白255
            new Color32(255,223,83,237), // 咕咕鸡：橘黄
            new Color32(163,163,163,237), // 炮弹比尔：黑163
            new Color32(161,213,255,237), // 时光机：淡蓝
            new Color32(255,71,71,237), // 王牌狗屋：红71
            new Color32(255,255,163,237), // 卡比之星：淡黄137
            new Color32(255,71,71,237), // 蝎红：红71
            new Color32(255,255,255,237), // 恩威迪亚：白255
            new Color32(255,207,155,237), // 快餐侠：淡棕
            new Color32(227,171,103,237), // 驯鹿空运：棕
            new Color32(163,163,163,237), // 北极星特快：黑163
            new Color32(163,147,255,237), // 远古飞鱼：紫
            new Color32(255,255,255,237), // 玩具独角兽：白255
            new Color32(255,127,0,237), // 南瓜魅影：橘127
            new Color32(127,255,127,237), // 赏金猎人：：绿127
            new Color32(227,171,103,237), // 鹰纽特：棕
            new Color32(163,147,255,237), // 安格瑞：紫
            new Color32(255,255,255,237), // 即将登场
            new Color32(255,255,255,237), // 即将登场
            new Color32(255,255,255,237), // 即将登场
            new Color32(255,255,255,237), // 即将登场
        };
        public static readonly Color32[] ButtonColor = new Color32[] {
            new Color32(0,163,255,232), // 神偷机兵：蓝
            new Color32(255,0,127,232), // 红牛能量：桃红
            new Color32(255,255,255,232), // 普鲸：白
            new Color32(0,255,0,232), // 纸飞机：绿255
            new Color32(255,71,71,232), // 咕咕鸡：红71
            new Color32(255,71,71,232), // 炮弹比尔：红71
            new Color32(255,255,137,232), // 时光机：淡黄137
            new Color32(0,255,0,232), // 王牌狗屋：绿255
            new Color32(255,97,255,232), // 卡比之星：粉红97
            new Color32(255,255,137,232), // 蝎红：淡黄137
            new Color32(137,255,0,232), // 恩威迪亚：黄绿
            new Color32(255,127,123,232), // 快餐侠：淡红
            new Color32(255,71,71,232), // 驯鹿空运：红71
            new Color32(255,193,117,232), // 北极星特快：淡橘
            new Color32(255,127,0,232), // 远古飞鱼：橘127
            new Color32(255,97,255,232), // 玩具独角兽：粉红97
            new Color32(0,255,0,232), // 南瓜魅影：绿255
            new Color32(255,73,0,232), // 赏金猎人：深橘73
            new Color32(255,255,255,232), // 鹰纽特：白
             new Color32(255,71,71,232), // 安格瑞：淡黄137
            new Color32(255,255,255,232), // 即将登场
            new Color32(255,255,255,232), // 即将登场
            new Color32(255,255,255,232), // 即将登场
            new Color32(255,255,255,232), // 即将登场
        };
        public static readonly Color32[] TextColor = new Color32[] {
            new Color32(255,255,123,255), // 神偷机兵：黄123
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
            new Color32(227,171,103,255), // 驯鹿空运
            new Color32(193,193,193,255), // 北极星特快：黑193
            new Color32(163,147,255,255), // 远古飞鱼
            new Color32(255,255,255,255), // 玩具独角兽：白255
            new Color32(255,127,0,255), // 南瓜魅影：橘127
            new Color32(127,255,127,255), // 赏金猎人：绿127
            new Color32(227,171,103,255), // 鹰纽特
            new Color32(163,147,255,255), // 安格瑞
            new Color32(255,255,255,255), // 即将登场
            new Color32(255,255,255,255), // 即将登场
            new Color32(255,255,255,255), // 即将登场
            new Color32(255,255,255,255), // 即将登场
        };

        public static readonly string[] Info = {
            "格鲁实验室早期为了称霸逗比界开发了「逗丁勇者」，但随着战场上升至星际空间后，被改良为适用于星战的宇航机，此举开创了宇航机竞技的时代。", // 神偷机兵
            "每年定期举办的宇航机赛事吸引着不少能量饮料厂牌赞助，红牛能量正是红牛为宇航机竞技推出的首款概念机。", // 红牛能量;
            "普鲸生活在北极海的极深处，毛熊设计局看中其能承受高压环境的特性将其改造为重型宇航机，然而随着逗比界设计的宇航机越来越大，普鲸被迫改良为高速武装机体。", // 普鲸
            "纸飞机是传统的空力飞行器，简单的制程与廉价是其依旧能在逗比界生存的主要原因，辣鸡设计局也是红牛航天与南方公园设计局的主要材料供应商。", // 纸飞机
            "辣鸡设计局於1000周年纪念所推出的吉祥物宇航机，令许多逗比都感到震惊的是这艘庞然大物竟然能称得上是宇航机，哇咔咔。", // 咕咕鸡 
            "任天堂设计局於21世纪也开始进行宇航机的开发，炮弹比尔是第一款正式量产的宇航机，最大的特色是可控式火箭发动机，这使得目前的宇航机都很难击落它。", // 炮弹比尔
            "尽管是未来百货公司岁末促销的赠品，时光机拥有轻巧的机体与灵活的机动性绝对是作为佯攻的最佳炮灰机种，因此非常受到逗比界青睐。", // 时光机
            "为了史努比的飞行梦，辣鸡设计局决定将咕咕鸡剩余的废料尽数改装在狗屋上，这使得狗屋的有机物外壳更加耐用，优秀的生存能力也让其获得了王牌的荣耀。", // 王牌狗屋
            "逗比界的新宠儿卡比也加入了哇咔咔世界，任天堂设计局为其打造了轻巧的高速武装机。", // 卡比之星
            "逗比皇家设计局最新投入宇航机的首发实验机种，使用混合动力技术的蝎红是第一款高速巨型机。", // 蝎红
            "恩威迪亚是由发动机大厂恩威哒动力公司委托毛熊设计局开发的巨型运输机所改良，主要目的是反制宇宙海盗，保护时空隧道。", // 恩威迪亚
            "知名快餐龙头肯打鸡在地沟油回收系统的开发上，引导了涡轮扇发动机在能量循环利用的先进技术，南方公园设计局的高速宇航机开发项目「快餐侠」应运而生。", // 快餐侠
            "随着星际交流的高速拓展，圣诞礼物工作室的业务也拓展到了哇咔咔星系，为此「驯鹿空运」被节庆保卫中心列为首要项目，同时它带有摧毁流星体的轻型武装。", //驯鹿空运
            "毛熊设计局在渡过经济萧条的困难后重新开展了巨型宇航机的研发，改良的特快车加装了五具涡轮扇发动机提供极佳的续航力，同时北极星特快也是逗比界最大的宇航机。", // 北极星特快
            "神隐许久的辣鸡设计局终于在机体材料的研究中取得了重大突破，这项技术运用在远古飞鱼的鳞片上，其拥有特殊变化的自然频率可大幅提升护盾强度。", // 远古飞鱼
            "南方公园设计局盗取了格鲁实验室的独角兽设计图，这是一项打造超巨型机的实验计划，它具备海量的护盾晶体与大型电池，用於吸引敌方炮火并再度修复", // 玩具独角兽
            "南瓜魅影是万圣地区为了世界崩坏而诞生的巅峰之作，新型的涡轮轴发动机能负担其庞大的机体，这使得节庆保卫中心在重型机的发展上称霸逗比界。", // 南瓜魅影
            "为因应其他逗比势力陆续推出新型的重型宇航机，南方公园设计局提出了「赏金猎人」计划，使用超重金属为侧翼增加防御，但相对地就会牺牲掉宇航机应有的速度。", // 赏金猎人
            "南方公园设计局开发项目", // 鹰纽特";
            "作为蓝星第一赌城的澳门打算规划宇航机的地面赛事，澳门赌王决定投资毛熊设计局进行新赛季宇航机的开发，新葡鲸将是首款概念机。", // 安格瑞";
            "", // 即将登场";
            "", // 即将登场";
            "", // 即将登场";
            "", // 即将登场";
        };

    }

    public static class DesignData
    {
        public static readonly string[] OKB = {
            "格鲁实验室", // 神偷机兵
            "红牛航天", // 红牛能量;
            "毛熊设计局", //   普鲸;
            "辣鸡设计局", // 纸飞机";
            "辣鸡设计局", // 咕咕鸡 ";
            "任天堂设计局", // 炮弹比尔";
            "22世纪百货公司", // 时光机";
            "辣鸡设计局", // 王牌狗屋";
            "任天堂设计局", // 卡比之星";
            "逗比皇家设计局", // 蝎红";
            "毛熊设计局", // 恩威迪亚";
            "南方公园设计局", // 快餐侠";
            "节庆保卫中心", // 驯鹿空运";
            "毛熊设计局", // 北极星特快";
            "辣鸡设计局", // 远古飞鱼";
            "南方公园设计局", // 玩具独角兽";
            "节庆保卫中心", // 南瓜魅影";
            "南方公园设计局", // 赏金猎人";
            "南方公园设计局", // 鹰纽特";
            "毛熊设计局", // 安格瑞";
            "尚未进驻", // 即将登场";
            "尚未进驻", // 即将登场";
            "尚未进驻", // 即将登场";
            "尚未进驻", // 即将登场";
        };
        public static readonly string[] Kocmocraft = {
            "神偷机兵",
            "红牛能量",
            "普鲸",
            "纸飞机",
            "咕咕鸡 ",
            "炮弹比尔",
             "时光机",
            "王牌狗屋",
            "卡比之星",
            "蝎红",
            "恩威迪亚",
            "快餐侠",
            "驯鹿空运",
            "北极星特快",
            "远古飞鱼",
            "玩具独角兽",
            "南瓜魅影",
            "赏金猎人",
            "鹰纽特",
            "新葡鲸",
            "设计阶段",
            "设计阶段",
            "设计阶段",
            "设计阶段",
        };
        public static readonly string[] Code = {
            "Minion Armor", // 神偷机兵
            "Red Bull Energy", // 红牛能量;
            "Vladimir Putin", // 普鲸
            "Paper Aeroplane", // 纸飞机
            "Cuckoo", // 咕咕鸡
            "Bullet Bill", // 炮弹比尔
            "Time Machine", // 时光机
            "Ace Kennel", // 王牌狗屋
            "Kirby Star", // 卡比之星
            "Scorpio Rouge", // 蝎红
            "nWidia", // 恩威迪亚
            "Fast Food Man", // 快餐侠
            "Reindeer Transport", // 驯鹿空运
            "Polaris Express", // 北极星特快
            "Ancient Fish", // 远古飞鱼
            "Papoy Unicorn", // 玩具独角兽
            "Pumpkin Ghost", // 南瓜魅影
            "Boundy Hunter MK.II", // 赏金猎人
            "Inuit Eagle", // 鹰纽特
            "Grand Lisboa", // 安格瑞
            "Kocmocraft 20", // 即将登场
            "Kocmocraft 21", // 即将登场
            "Kocmocraft 22", // 即将登场
            "Kocmocraft 23", // 即将登场
        };
        public static readonly string[] Dubi = {
            "小小兵戴夫", // 神偷机兵
            "暴走漫画王尼玛", // 红牛能量
            "秃子萌总", // 普鲸
            "纸箱人阿楞", // 纸飞机
            "咕咕鸡", // 咕咕鸡 
            "马里奥", // 炮弹比尔
            "机器喵哆啦A梦", // 时光机
            "史努比", // 王牌狗屋
            "卡比", // 卡比之星
            "无面人", // 蝎红
            "熊猫人张学友", // 恩威迪亚
            "阿痞", // 快餐侠
            "地鼠", // 驯鹿空运
            "表情帝滑稽", // 北极星特快
            "海绵宝宝", // 远古飞鱼
            "公主阿尼", // 玩具独角兽
            "哆啦啦", // 南瓜魅影
            "赏金猎人凯子", // 赏金猎人
            "印第安屎蛋", // 鹰纽特
            "安格瑞", // 安格瑞
            "招募中", // 即将登场
            "招募中", // 即将登场
            "招募中", // 即将登场
            "招募中", // 即将登场
        };
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

    public static class KocmocraftData
    {
        // 北极星特快 南瓜魅影 咕咕鸡 恩威迪亚 蝎红 15~20K    6~7星
        // 驯鹿空运 赏金猎人 远古飞鱼 玩具独角兽 10~15K 4~5星
        // 纸飞机  快餐侠 神偷机兵 9K~11K 3~4星
        // 炮弹比尔 普鲸 鹰纽特 红牛能量 6K~8K  2~3星
        // 王牌狗屋 卡比之星 时光机 3K~5K 1~2星
        public static readonly int[] MaxHull = {
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
        public static readonly int[] MaxShieldl = {
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
        public static readonly int[] MaxEnergy = {
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


        // Spawn Kocmocraft
        public static int GetPortNumber(int kocmonautNumber) { return (kocmonautNumber - 93000) / 3; }
        // Initialize Kocmocraft Data
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
                case Type.Nwidia: return EngineType.IonThruster;
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
                case Type.RedBullEnergy: return new Vector3(0, 3.0f, 7.0f);
                case Type.VladimirPutin: return new Vector3(0, 3.0f, 5.0f);
                case Type.PaperAeroplane: return new Vector3(0, 3.0f, 7.0f);

                case Type.BulletBill: return new Vector3(0,3.37f,7);
                case Type.TimeMachine: return new Vector3(0, 2.97f, 0);
                case Type.AceKennel: return new Vector3(0, 1.37f, 0);
                case Type.KirbyStar: return new Vector3(0, 2.37f, 0);

                case Type.ScorpioRouge: return new Vector3(0, 5.0f, 10.0f);

                case Type.AncientFish: return new Vector3(0, 3.93f, 10.0f);
                case Type.PumpkinGhost: return new Vector3(0, 7.0f, 10.0f);
                default: return new Vector3(0, 3.37f, 7);
            }
        }
        
    }

    public static class WeaponData
    {
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
            2, // ★★☆☆☆☆ 快餐侠
            2, // ★★☆☆☆☆ 驯鹿空运
            6, // ★★★★★★ 北极星特快
            2, // ★★☆☆☆☆ 远古飞鱼
            4, // ★★★★☆☆ 玩具独角兽
            4, // ★★★★☆☆ 南瓜魅影
            6, // ★★★★★★ 赏金猎人
            4, // ★★★★☆☆ 鹰纽特
            6, // ★★★★☆☆ 新葡鲸
            2, // ★★☆☆☆☆ 即将登场
            2, // ★★☆☆☆☆ 即将登场
            2, // ★★☆☆☆☆ 即将登场
            2, // ★★☆☆☆☆ 即将登场
        };
        private static float TurretDamageCoefficient;
        private static float AmmoVelocity;
        public static int MinDamage;
        public static int MaxDamage;
        public static int MaxRange;

        // For hangar data using
        public static void GetWeaponData(int type)
        {
            switch (TurretCount[type])
            {
                case 2: TurretDamageCoefficient = 1.0f;break;
                case 4: TurretDamageCoefficient = 0.843f; break;
                case 6: TurretDamageCoefficient = 0.779f; break;
            }
            AmmoVelocity = KocmoLaserCannon.flightVelocity * TurretDamageCoefficient;
            MinDamage = (int)((AmmoVelocity + KocmocraftData.CruiseSpeed[type]*5) * (AmmoVelocity + KocmocraftData.CruiseSpeed[type]*5) * 
                TurretCount[type] * KocmoLaserCannon.coefficientMinDamage * 0.000033f);
            MaxDamage = (int)((AmmoVelocity + KocmocraftData.AfterburnerSpeed[type]*5) * (AmmoVelocity + KocmocraftData.AfterburnerSpeed[type]*5) *
                TurretCount[type] * KocmoLaserCannon.coefficientMaxDamage * 0.000033f);
            MaxRange = (int)((KocmoLaserCannon.flightVelocity * TurretDamageCoefficient + KocmocraftData.AfterburnerSpeed[type]) * KocmoLaserCannon.flightTime);
        }

        // For KocmoLaserFlying.cs using
        public static float GetCoefficient(Type type)
        {
            switch (TurretCount[(int)type])
            {
                case 2: return 1.0f; 
                case 4: return 0.843f; 
                case 6: return 0.779f;
                default: return 1.0f;
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
        public static readonly float engineMaxVolume = 0.137f;        //The maximum volume of the engine
        public static readonly float engineMinThrottlePitch = 0.77f;      //The minimum pitch of the engine
        public static readonly float engineMaxThrottlePitch = 1f;		//The maximum pitch of the engine
    }
    public static class Turbofan
    {
        public static readonly float engineMaxVolume = 0.173f;        //The maximum volume of the engine
        public static readonly float engineMinThrottlePitch = 0.8f;      //The minimum pitch of the engine
        public static readonly float engineMaxThrottlePitch = 1.3f;		//The maximum pitch of the engine
    }
    public static class Turboprop
    {
        public static readonly float engineMaxVolume = 0.237f;        //The maximum volume of the engine
        public static readonly float engineMinThrottlePitch = .9f;      //The minimum pitch of the engine
        public static readonly float engineMaxThrottlePitch = 1.1f;		//The maximum pitch of the engine
    }
    public static class Turboshaft
    {
        public static readonly float engineMaxVolume = 0.55f;        //The maximum volume of the engine
        public static readonly float engineMinThrottlePitch = .7f;      //The minimum pitch of the engine
        public static readonly float engineMaxThrottlePitch = 1.3f;		//The maximum pitch of the engine
    }

    public static class IonThruster
    {
        public static readonly float engineMaxVolume = 0.71f;        //The maximum volume of the engine
        public static readonly float engineMinThrottlePitch = 0.7f;      //The minimum pitch of the engine
        public static readonly float engineMaxThrottlePitch = 1.3f;		//The maximum pitch of the engine
    }
    public static class BiomassEnergy
    {
        public static readonly float engineMaxVolume = 0.73f;        //The maximum volume of the engine
        public static readonly float engineMinThrottlePitch = 0.8f;      //The minimum pitch of the engine
        public static readonly float engineMaxThrottlePitch = 1.2f;		//The maximum pitch of the engine
    }
    public static class PulsedPlasmaThruster
    {
        public static readonly float engineMaxVolume = 0.73f;        //The maximum volume of the engine
        public static readonly float engineMinThrottlePitch = .7f;      //The minimum pitch of the engine
        public static readonly float engineMaxThrottlePitch = 1.3f;		//The maximum pitch of the engine
    }



    public static class OriginalSettingEngine
    {
        public static readonly float engineMinVol = 0f;         //The minimum volume of the engine
        public static readonly float engineMaxVol = .6f;        //The maximum volume of the engine
        public static readonly float engineMinPitch = .3f;      //The minimum pitch of the engine
        public static readonly float engineMaxPitch = .8f;      //The maximum pitch of the engine
    }

    public static class BigLaserCannon
    {
        public static readonly int countPerBatch = 5; // 每批生成量
        public static readonly int maxPoorInventory = 500; // 最大物件池存量
        public static readonly float maxFireAngle = Mathf.Cos(7 * Mathf.Deg2Rad); // 最大開火夾角
        public static readonly int maxAmmoCapacity = 999; // 最大載彈量
        public static readonly float projectileSpread = 0.39f; //彈道散布 degree
        public static readonly int flightVelocity = 16300;//2970; // 飛行速率 m/s
        public static readonly int propulsion = flightVelocity * 50; // 開火推力
        public static readonly float flightTime = 0.71f; // 飛行時間 sec
        public static readonly float operationalRange = flightVelocity * flightTime; // 射程 m
        public static readonly float fireRoundPerSecond = 4.77f; // 射速 rps
        public static readonly float rateFire = 1 / fireRoundPerSecond; // 開火速率 sec
        public static readonly float coefficientMinDamage = 1.3035f;
        public static readonly float coefficientMaxDamage = 4.1949f;
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
        public static readonly float flightTime = 0.71f; // 飛行時間 sec
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
        Nwidia = 10,
        FastFoodMan = 11,
        ReindeerTransport = 12,
        PolarisExpress = 13,
        AncientFish=14,
        PapoyUnicorn = 15,
        PumpkinGhost = 16,
        BoundyHunterMKII = 17,
        Unknown = -999,
    }
    public enum FireControlSystemType
    {
        Laser = 0,
        Rocket = 1,
        Missile = 2,
        Unknown = -999,
    }


    public class QQ
    {
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
                case Type.PumpkinGhost: return 4; // 南瓜魅影：Large
                case Type.BoundyHunterMKII: return 6; // 南瓜魅影：Large
                default: return 2;
            }
        }
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
        }//

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
                case 16: return "Pumpkin Ghost";
                case 17: return "Boundy Hunter MK.II";
                default: return "---";
            }
        }

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
                case 16: return "南瓜魅影";
                case 17: return "即将登场";
                default: return "未知宇航机";
            }
        }
        public static readonly float[] TopViewSize = {
            4.6f, // ★★★★★★☆ 神偷机兵
            3.1f, // ★★☆☆☆☆☆ 红牛能量
            3.5f, // ★☆☆☆☆☆☆ 普鲸
            5.9f, // ★★★★★★★ 纸飞机
            5.2f, // ★★★☆☆☆☆ 咕咕鸡 
            3.7f, // ★★★☆☆☆☆ 炮弹比尔
            2.0f, // ★★☆☆☆☆☆ 时光机
            2.1f, // ★★★★★★☆ 王牌狗屋
            2.4f, // ★★★★★★★ 卡比之星
            6.1f, // ★★☆☆☆☆☆ 蝎红
            5.0f, // ★★★☆☆☆☆ 恩威迪亚
            5.1f, // ★★★★☆☆☆ 快餐侠
            6.0f, // ★★★★★☆☆ 驯鹿空运
            5.1f, // ★★★★☆☆☆ 北极星特快
            6.9f, // ★★★★★☆☆ 远古飞鱼
            40, // ★★★★☆☆☆ 玩具独角兽
            36, // ★★★☆☆☆☆ 南瓜魅影
            22, // ★☆☆☆☆☆☆ 赏金猎人
            45, // ★★★★★☆☆ 鹰纽特
            34, // ★★★☆☆☆☆ 安格瑞
            0, // ★★★★★★★ 即将登场
            0, // ★★★★★★★ 即将登场
            0, // ★★★★★★★ 即将登场
            0, // ★★★★★★★ 即将登场
        };


        public static readonly float[] Collider = {
            38.3091173f, // 神偷机兵
            20.21441f, // 红牛能量;
            18.41935f, //  普鲸;
             57.69397f, // 纸飞机";
            95.76454f, // 咕咕鸡 ";
            31.09758f, // 炮弹比尔";
            4.496421f, // 时光机";
            21.234167503f, // 王牌狗屋";
            6.356274f, // 卡比之星";
            115.0642f, // 蝎红";
            136.12f, // 恩威迪亚";
            60.54652f, // 快餐侠";
            93.2667f, // 驯鹿空运";
            152.6686f, // 北极星特快";
            70.73558282f, // 远古飞鱼";
            68.74551578f, // 玩具独角兽";
            122.675175f, // 南瓜魅影";
            111.4062f, // 赏金猎人";
            99999, // 鹰纽特";
            99999, // 安格瑞";
            99999, // 即将登场";
            99999, // 即将登场";
            99999, // 即将登场";
            99999, // 即将登场";
        };

        public static readonly float[] Volume = {
            34.7250337f, // 神偷机兵
            17.2552f, // 红牛能量;
            25.10557f, //  普鲸;
            25.26623f, // 纸飞机";
            99.89777f, // 咕咕鸡 ";
            50.93384f, // 炮弹比尔";
            3.745697f, // 时光机";
            10.13892941f, // 王牌狗屋";
            8.762174f, // 卡比之星";
            65.28907f, // 蝎红";
            95.869f, // 恩威迪亚";
            43.4229f, // 快餐侠";
            89.29118f, // 驯鹿空运";
            124.7412f, // 北极星特快";
            50.4355016f, // 远古飞鱼";
            64.1464130587f, // 玩具独角兽";
            101.595858f, // 南瓜魅影";
            64.00921f, // 赏金猎人";
            99999, // 鹰纽特";
            99999, // 安格瑞";
            99999, // 即将登场";
            99999, // 即将登场";
            99999, // 即将登场";
            99999, // 即将登场";
        };

        public static readonly float[] RCS = {
            116.450658f, // 神偷机兵
            78.87015f, // 红牛能量;
            69.46844f, //  普鲸;
            205.8228f, // 纸飞机";
            195.8923f, // 咕咕鸡 ";
            97.26662f, // 炮弹比尔";
            39.28357f, // 时光机";
            45.327086f, // 王牌狗屋";
            31.72817f, // 卡比之星";
            263.8345f, // 蝎红";
            302.3377f, // 恩威迪亚";
            205.5217f, // 快餐侠";
            252.9747f, // 驯鹿空运";
            343.213f, // 北极星特快";
            246.857114f, // 远古飞鱼";
            197.10966682f, // 玩具独角兽";
            302.01148f, // 南瓜魅影";
            279.8407f, // 赏金猎人";
            99999, // 鹰纽特";
            99999, // 安格瑞";
            99999, // 即将登场";
            99999, // 即将登场";
            99999, // 即将登场";
            99999, // 即将登场";
        };

    }
}
