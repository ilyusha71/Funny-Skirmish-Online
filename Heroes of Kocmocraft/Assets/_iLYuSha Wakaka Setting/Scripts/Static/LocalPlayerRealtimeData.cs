namespace Kocmoca
{
    public static class LocalPlayerRealtimeData
    {
        public static int Number { get; set; }
        public static Faction Faction { get; set; }
        public static FlyingStatus Status { get; set; }

        public static Identification CheckFriendOrFoe(Faction target)
        {
            if (target == Faction.Unknown)
                return Identification.Unknown;
            else if (target == Faction)
                return Identification.Friend;
            else
                return Identification.Foe;
        }
    }

    public enum FlyingStatus
    {
        Waiting = 0,
        Flying = 1,
        Crash = 2,
        Respawn =3,
    }
}
