public struct PlayerStatData : IData
{
    public short id;    // key
    public short playerLevel;   // 캐릭터 레벨
    public short heartMax;  // 레벨별 최대 체력
    public short baseAttack;    // 레벨별 캐릭터의 기본 공격력
    public float baseSpeed; // 레벨별 캐릭터의 기본 이동 속도
    public float dodgeCooldown; // 회피 쿨타임 감소량(%로 적용)

    public void Clear()
    {
        id = 0;
        playerLevel = 0;
        heartMax = 0;
        baseAttack = 0;
        baseSpeed = 0f;
        dodgeCooldown = 0f;
    }

    public void Set(PlayerStatData data)
    {
        id = data.id;
        playerLevel = data.playerLevel;
        heartMax = data.heartMax;
        baseAttack = data.baseAttack;
        baseSpeed = data.baseSpeed;
        dodgeCooldown = data.dodgeCooldown;
    }
}
