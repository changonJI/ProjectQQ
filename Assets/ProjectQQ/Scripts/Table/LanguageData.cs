public struct LanguageData : IData
{
    public int id;
    public string korean;
    public string english;
    public string chinese;
    public string japanese;

    public void Clear()
    {
        id = 0;
        korean = string.Empty;
        english = string.Empty;
        chinese = string.Empty;
        japanese = string.Empty;
    }
}
