namespace QQ
{
    /// <summary>
    /// TableType == DataClassName
    /// </summary>
    public enum TableType
    {
        None = 0,
        LanguageData = 1,
        PlayerStatData = 4,
        MonsterData = 12,
    }

    public enum UIDepth
    {
        HUD = 0,
        Fixed1,
        Fixed2,
        Fixed3,
        Popup,
        Toast,
        Indicator = 9999,
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

    public enum SortingLayer
    {
        Default = 0,
        Background,
        Object,
        ForeDecoration,
    }

    public enum OrderInSortingLayer
    {
        // Background
        BGFar = 0,
        BGMid = 1,
        BGFloor = 2,

        // Object
        OBJEffectBack = 0,
        OBJBody = 10,
        OBJEffectFront = 20,
    }

    public enum ObjectType
    {
        Default = 0,
        Actor,
        Monster,
        Npc,
        Building,   // 맵에 배치된 오브젝트
        Item,
        SFX,
    }

    public enum ResType
    {
        UI = 0,
        Sound = 1,
        Object = 2,
    }

    public enum SoundType
    {
        BGM = 0,
        SFX = 1,
        UI = 2,
    }

    public enum PlayerDataType
    {
        UserName = 0,
        FirstPlay = 1
    }

    public enum SceneType
    {
        MainScene = 0,
        LoadingScene = 1,
        GameScene = 2
    }

    public enum MonsterType
    {
        None = 0,
        Normal = 1, // 일반 몬스터
        MiniBoss = 2, // 미니 보스 몬스터
        Boss = 3,   // 보스 몬스터
        Event = 4,  // 이벤트 몬스터
    }

    public enum MonsterAtkType
    {
        None = 0,
        Melee = 1, // 근접 공격
        Ranged = 2, // 원거리 공격
    }
}
