using QQ;

public struct DialogueData : IData
{
    public short id;                // 대사 ID
    public short dialogueGroup;     // 대사 그룹 ID
    public byte speakerType;        // 화자 타입 (enum 변환 가능)
    public int speakerNameId;       // 캐릭터 이름 ID
    public int dialogueTextId;      // 대사 텍스트 ID
    public byte emotionType;        // 감정 타입 (enum 변환 가능)
    public string portraitId;       // 초상화 리소스명
    public string voiceClipId;      // 음성 리소스명
    public short nextDialogueId;    // 다음 대사 ID
    public string triggerEvent;     // 트리거 이벤트명
    public bool autoAdvance;        // 자동 진행 여부
    public float advanceDelay;      // 자동 진행 지연 시간

    public void Clear()
    {
        id = 0;
        dialogueGroup = 0;
        speakerType = 0;
        speakerNameId = 0;
        dialogueTextId = 0;
        emotionType = 0;
        portraitId = string.Empty;
        voiceClipId = string.Empty;
        nextDialogueId = 0;
        triggerEvent = string.Empty;
        autoAdvance = false;
        advanceDelay = 0f;
    }
}
