namespace QQ
{
    /// <summary>
    /// TableType == DataClassName
    /// </summary>
    public enum TableType
    {
        None = 0,
        LanguageData = 1,
    }

    public enum UIDepth
    {
        HUD = 0,
        Fixed1,
        Fixed2,
        Fixed3,
        Popup,
    }

    public enum UIType
    {
        Main = 0,
        Back = 1,
        Destroy = 2,
    }

    public enum Layer
    {
        Default = 0,
        UI = 5,
    }

    public enum ResType
    {
        UI = 0,
        Sound = 1,
    }

    public enum SoundType
    {
        BGM = 0,
        SFX = 1,
        UI = 2,
    }
}
