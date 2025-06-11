using QQ;

public struct MonsterData : IData
{
    public short id;    // key
    public int nameId;   // ���� �̸�
    public int desId;  // ���� ����
    public MonsterType type;    // ���� Ÿ��
    public short hp; // �ִ� Hp
    public short attack; // ������
    public MonsterAtkType atkType; // ���� Ÿ��(1 : ����, 2 : ���Ÿ�)
    public short skill; // ?
    public float attackLag; // ������ �ð�
    public float attackAng; // ���� ����(����)
    public float speed; // �ӵ�
    public string spriteName;

    public void Clear()
    {
        id = 0;
        nameId = 0;
        desId = 0;
        type = MonsterType.None;
        hp = 0;
        attack = 0;
        atkType = MonsterAtkType.None;
        skill = 0;
        attackLag = 0f;
        attackAng = 0f;
        speed = 0f;
        spriteName = string.Empty;
    }

    public void Set(MonsterData data)
    {
        id = data.id;
        nameId = data.nameId;
        desId = data.desId;
        type = data.type;
        hp = data.hp;
        attack = data.attack;
        atkType = data.atkType;
        skill = data.skill;
        attackLag = data.attackLag;
        attackAng = data.attackAng;
        speed = data.speed;
        spriteName = data.spriteName;
    }
}
