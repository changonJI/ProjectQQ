using QQ;

public struct SkillData : IData
{
    public int id;
    public int nameId; 
    public SkillType type;
    public int desId;
    public string prefabName;
    public string soundName;
    public int cooltime;
    public int duration;
    public int range;
    public int speed;
    public int blowCount;
    public SkillOptionType mainOptionType;
    public int mainOptionValue;
    public SkillOptionType subOptionType;
    public int subOptionValue;
    
    public void Clear()
    {
        id = 0;
        nameId = 0;
        type = SkillType.None;
        desId = 0;
        prefabName = string.Empty;
        soundName = string.Empty;
        cooltime = 0;
        duration = 0;
        range = 0;
        speed = 0;
        blowCount = 0;
        mainOptionType = SkillOptionType.None;
        mainOptionValue = 0;
        subOptionType = SkillOptionType.None;
        subOptionValue = 0;
    }
}
