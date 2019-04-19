public static class LevelsCount {
    private static int CurLevel, TotLevel;

    public static int CurrentLevel
    {
        get
        {
            return CurLevel;
        }
        set
        {
            CurLevel = value;
        }
    }
    public static int TotalLevel
    {
        get
        {
            return TotLevel;
        }
        set
        {
            TotLevel = value;
        }
    }
}
