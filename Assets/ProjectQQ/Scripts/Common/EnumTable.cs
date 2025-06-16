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
        Normal = 1, // �Ϲ� ����
        MiniBoss = 2, // �̴� ���� ����
        Boss = 3,   // ���� ����
        Event = 4,  // �̺�Ʈ ����
    }

    public enum MonsterAtkType : byte
    {
        None = 0,
        Melee = 1, // ���� ����
        Ranged = 2, // ���Ÿ� ����
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

    public enum UIDialouguePos : byte
    {
        None = 0,
        Left = 1,  
        Right = 2,  
        Center = 3, 
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
        Stage = 3,
        Texture = 4,
    }
    public enum InputContext : byte
    {
        Player = 0,
        UI,

        Texture = 3,
    }
    #endregion

    #region GameObject
    public enum GameObjectType : byte
    {
        Default = 0,
        Actor,
        Monster,
        Npc,
        Building,   // �ʿ� ��ġ�� ������Ʈ
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
        Body = 1, // ����
        Weapon = 2, // ����
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
