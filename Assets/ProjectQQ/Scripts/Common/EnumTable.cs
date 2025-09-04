namespace QQ
{
    #region Table
    /// <summary>
    /// TableType == Data script Name
    /// Table Save & Load 시 script 이름으로 불러온다.
    /// </summary>
    public enum TableType : byte
    {
        None = 0,
        LanguageData = 1,
        StageData = 2,
        DialogueData = 3,
        PlayerStatData = 4,
        ItemData = 5,
        WeaponData = 6,
        ExpData = 7,
        MonsterSpawnData = 8,
        MonsterData = 9,
        DropData = 10,
        BossPatternData = 11,
        SkillData = 12,
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

    public enum MonsterAIType : byte
    {
        None = 0,
        ChasePlayer = 1,    // 플레이어 추적
        Strafe = 2,         // 플레이어 주변 회피/원형 이동
        AOE = 3,            // 범위 중심 공격
        Charge = 4,         // 돌진
        Trap = 5,           // 트랩 또는 지형 기반 공격
        Summon = 6,         // 소환
        StatusInflict = 7,  // 상태이상
    }

    public enum DialogueImgPosType : byte
    {
        None = 0,
        Left = 1, 
        Right = 2,
        CenterL = 3,
        CenterR = 4,
    }

    public enum ItemType : byte
    {
        Shop = 0,   // 상점 전용 아이템
        Consumable, // 소비형 아이템 (ex. 회복)
        Attack,     // 공격형 아이템
        Passive,    // 패시브 아이템
        Box,        // 상자형 아이템
        Combine,   // 조합 재료/결과 아이템
        None = 99
    }

    public enum SkillType : byte
    {
        None = 0,
        bullet = 1, 
        box = 2,
        item = 3,
    }

    public enum SkillOptionType : byte
    {
        None = 0,
        Damage = 1, // 범위 데미지
        Explosion = 2, // 폭발 이후 스킬 id 값
        Xp_Pull = 3, // 경험치 획득범위 증가
        Heal = 4, // 체력 회복
        MoveSpdUp = 5, // 이동 속도 증가
        MoveSpdDown = 6, // 이동 속도 감소
        Stun = 7, // 기절 시간
        Invincible = 8, // 무적
    }

    #endregion

    #region PlayerPref
    public enum PlayerDataType : int
    {
        // Player
        UserName = 0,
        FirstPlay = 1,

        // Preferences
        Country = 100,

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
        Indicator = 3,
    }

    public enum Layer : byte
    {
        Default = 0,
        Player = 3,
        UI = 5,
        Enemy = 7,
        Item = 8,
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

    public enum ConturyType : byte
    {
        Korean = 0,
        English = 1,
        Chinese = 2,
        Japanese = 3,
    }

    public enum LanguageType : int // 언어 타입
    {
        UI = 1,
        Dialouge = 2,
        Item = 3,
        ItemDes = 4,
        Map = 5,
        Monster = 6,
        Skill = 7,
        Scenario = 9,
    }

    public enum ResType : byte
    {
        UI = 0,
        Sound = 1,
        Object = 2,
        Stage = 3,
        Texture = 4,
        Effect = 5,
        Sprite = 6,
        Animation = 7,
    }

    public enum InputMap : byte // 키 입력 상황
    {
        Player = 0,
        UI,

        Texture = 3,
    }

    public enum CameraType : byte // 카메라 타입
    {
        Default = 0, // Main 고정 카메라
        Player = 1, // 플레이어 따라가는 카메라
        Boss = 2, // 보스 몬스터 따라가는 카메라
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
        Stage,      // 맵
    }

    public enum SoundType : byte
    {
        BGM = 0,
        SFX = 1,
        UI = 2,
    }

    public enum EffectType : byte
    {
        None = 0,
        Roll = 1,
        Skill = 2,
    }

    public enum ScannerType : byte
    { 
        None = 0,
        Attack = 1,
        Item = 2,
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
        Hit = 4,
        Die = 5,
    }
    #endregion

    #region FSM
    public enum FSMState : byte
    {
        None = 0,
        Idle = 1,
        Move = 2,
        Roll = 3,
        Knockback = 4,
        Die = 5,
    }
    #endregion
}
