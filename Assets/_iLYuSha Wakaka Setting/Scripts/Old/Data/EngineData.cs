namespace AirSupremacy
{
    public enum EngineOption
    {
        None = 0,
        TurboJet = 1, // 渦輪噴射引擎
        TurboFan = 2, // 渦輪扇發動機
        Propeller = 3, // 螺旋槳
    }
    public static class EngineOptionData
    {
        public static readonly EngineOption[] engineConfiguration = new[]
        {
            EngineOption.TurboFan, // 神偷機
            EngineOption.TurboJet, // 紅牛能量
            EngineOption.TurboFan, // 普鯨
            EngineOption.TurboJet, // 紙飛機
            EngineOption.TurboJet, // 咕咕雞
            EngineOption.TurboFan, // 殺手
            EngineOption.TurboJet, // 時光機
            EngineOption.Propeller, // 王牌狗屋
            EngineOption.TurboJet, // 卡比阿爾法
            EngineOption.TurboFan, // 法拉拉
            EngineOption.TurboJet, // 星球戰士
            EngineOption.TurboFan, // 快餐俠
            EngineOption.TurboJet, // 聖誕速遞
            EngineOption.TurboFan, // 北極星
        };
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
        public static readonly float engineMaxVol = 0.06f;        //The maximum volume of the engine
        public static readonly float engineMinPitch = 0.8f;      //The minimum pitch of the engine
        public static readonly float engineMaxPitch = 1.3f;		//The maximum pitch of the engine
    }
    public static class Propeller
    {
        public static readonly float engineMinVol = 0f;         //The minimum volume of the engine
        public static readonly float engineMaxVol = 0.2f;        //The maximum volume of the engine
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
}