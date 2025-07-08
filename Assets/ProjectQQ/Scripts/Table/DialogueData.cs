using QQ;

public struct DialogueData : IData
{
    public short id;    // key
    /// <summary>
    /// 언어테이블 참조 값
    /// </summary>
    public int nameId;   // 캐릭터 이름
    public int desTextId;  // 언어테이블에서 불러올 캐릭터 대사 id
    public DialogueImgPosType positionType1;    // 이미지 위치
    public string imgName1; // 이미지 리소스명
    public DialogueImgPosType positionType2;    // 이미지 위치
    public string imgName2; // 이미지 리소스명
    public string backgroundName; // 배경 이미지 리소스명

    public void Clear()
    {
        id = 0;
        nameId = 0;
        desTextId = 0;
        positionType1 = DialogueImgPosType.None;
        imgName1 = string.Empty;
        positionType2 = DialogueImgPosType.None;
        imgName2 = string.Empty;
        backgroundName = string.Empty;
    }
}
