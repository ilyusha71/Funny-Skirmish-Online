namespace Kocmoca
{
    public static class LocalPlayer
    {
        public static int Number { get; set; }
        public static Faction Faction { get; set; }

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
}
