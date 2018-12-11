namespace AirSupremacy
{
    public enum BattlefieldFaction
    {
        Faction1 = 0,
        Faction2 = 1,
        Faction3 = 2,
        Faction4 = 3,
        KocmocA = -999,
    }
    public enum FormationFlyingOrder
    {
        Leader = 0,
        WingmanPrimary = 1,
        WingmanSecondary = 2,
        WingmanTertiary = 3,
        Wingman4th = 4,
        Wingman5th = 5,
        Wingman6th = 6,
        Wingman7th = 7,
        Wingman8th = 8,
        Wingman9th = 9,
        KocmocA = -999,
    }
    public enum AircraftType
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
        快餐俠 = 11,
        聖誕狂歡 = 12,
        北極星特快 = 13,
        KocmocA = -999,
    }
    public enum ControlType
    {
        LocalPlayer = 0,
        LocalAI = -1,
        RemotePlayer = 100,
        RemoteAI = 99,
        KocmocA = -999,
    }
    public enum AircraftUse
    {
        Default = 0,
        Random = 1,
        Optional = 2,
    }
    public struct Pilot
    {
        public BattlefieldFaction BattlefieldFaction;
        public FormationFlyingOrder FormationFlyingOrder;
        public AircraftType AircraftType;
        public int FlightNumber;
        public string AircraftName;
        public ControlType ControlType;

        public Pilot(BattlefieldFaction setBattlefieldFaction, FormationFlyingOrder setFormationFlyingOrder, AircraftType setAircraftType, int setFlightNumber, string setAircraftName, ControlType setControlType)
        {
            BattlefieldFaction = setBattlefieldFaction;
            FormationFlyingOrder = setFormationFlyingOrder;
            AircraftType = setAircraftType;
            FlightNumber = setFlightNumber;
            AircraftName = setAircraftName;
            ControlType = setControlType;
        }
    }
}