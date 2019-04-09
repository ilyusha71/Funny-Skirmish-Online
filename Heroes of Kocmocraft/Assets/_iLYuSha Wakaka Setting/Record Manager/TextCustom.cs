public static class TextCustom
{
    public static string hexGold = "#FFD700";
    /* Basic Type */
    public static string TextColor(string colorHex, string text)
    {
        return "<color=" + colorHex + ">" + text + "</color>";
    }

    public static string TextSize(int size, string text)
    {
        return "<size=" + size + ">" + text + "</size>";
    }

    /* Special Type */
    public static string TextGoldColor(string text)
    {
        return TextColor(hexGold, text);
    }
}
