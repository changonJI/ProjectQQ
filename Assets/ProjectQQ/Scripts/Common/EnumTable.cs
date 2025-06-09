namespace QQ
{
    #region Table
    /// <summary>
    /// TableType == DataClassName
    /// </summary>
    public enum TableType : byte
    {
        None = 0,
        LanguageData = 1,
        PlayerStatData = 4,
        MonsterData = 12,
    }

    public enum PlayerDataType : byte
    {
        UserName = 0,
        FirstPlay = 1
    }

    public enum MonsterType : byte
    {
        None = 0,
        Normal = 1, // 일반 몬스터
        MiniBoss = 2, // 미니 보스 몬스터
        Boss = 3,   // 보스 몬스터
        Event = 4,  // 이벤트 몬스터
    }

    public enum MonsterAtkType : byte
    {
        None = 0,
        Melee = 1, // 근접 공격
        Ranged = 2, // 원거리 공격
    }
    #endregion

    #region UI
    public enum UIDepth : short
    {
        HUD = 0,
        Fixed1,
        Fixed2,
        Fixed3,
        Popup,
        Toast,
        Indicator = 9999,
    }

    public enum UIType : byte
    {
        Main = 0,
        Back = 1,
        Destroy = 2,
    }

    public enum Layer : byte
    {
        Default = 0,
        UI = 5,
    }
    #endregion

    #region Render
    public enum SortingLayerName : byte
    {
        Default = 0,
        Background,
        Object,
        ForeDecoration,
        Effect,
    }

    public enum OrderInSortingLayer : byte
    {
        // Background
        BGFar = 0,
        BGMid = 1,
        BGFloor = 2,

        // Object
        OBJEffectBack = 0,
        OBJBody = 10,
        OBJWeapon = 11,
        OBJEffectFront = 20,
    }
    #endregion

    #region Config
    public enum SceneType : byte
    {
        MainScene = 0,
        LoadingScene = 1,
        GameScene = 2
    }
    public enum ResType : byte
    {
        UI = 0,
        Sound = 1,
        Object = 2,
    }
    #endregion

    #region GameObject
    public enum GameObjectType : byte
    {
        Default = 0,
        Actor,
        Monster,
        Npc,
        Building,   // 맵에 배치된 오브젝트
        Item,
        SFX,
    }

    public enum SoundType : byte
    {
        BGM = 0,
        SFX = 1,
        UI = 2,
    }
    #endregion

    #region Animation
    public enum AnimSlotType : byte
    {
        None = 0,
        Body = 1, // 몸통
        Weapon = 2, // 무기
    }

    public enum AnimState : byte
    {
        None = 0,
        Idle = 1,
        Roll = 2,
        Run = 3,
    }
    #endregion
}
