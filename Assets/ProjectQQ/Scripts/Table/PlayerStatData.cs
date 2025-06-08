public struct PlayerStatData : IData
{
    public short id;    // key
    public short playerLevel;   // ĳ���� ����
    public short heartMax;  // ������ �ִ� ü��
    public short baseAttack;    // ������ ĳ������ �⺻ ���ݷ�
    public float baseSpeed; // ������ ĳ������ �⺻ �̵� �ӵ�
    public float dodgeCooldown; // ȸ�� ��Ÿ�� ���ҷ�(%�� ����)

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
