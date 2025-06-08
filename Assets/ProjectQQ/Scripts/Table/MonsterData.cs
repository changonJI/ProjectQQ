using QQ;

public struct MonsterData : IData
{
    public short id;    // key
    public int nameId;   // 몬스터 이름
    public int desId;  // 몬스터 설명
    public MonsterType type;    // 몬스터 타입
    public short hp; // 최대 Hp
    public short attack; // 데미지
    public MonsterAtkType atkType; // 공격 타입(1 : 근접, 2 : 원거리)
    public short skill; // ?
    public float attackLag; // 딜레이 시간
    public float attackAng; // 공격 범위(각도)
    public float speed; // 속도
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
