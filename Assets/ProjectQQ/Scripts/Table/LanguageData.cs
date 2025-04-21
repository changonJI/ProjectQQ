using System.Collections.Generic;

public struct LanguageData : IData
{
    public int ID;

    public void SetData(List<string> data)
    {
        int count = 0;

        ID = int.Parse(data[count++]);
    }
}
