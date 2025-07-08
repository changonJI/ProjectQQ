public struct BaseData : IData
{
    public short id;    // key
    public short unit;   // 
    public object data;

    public void Clear()
    {
        id = 0;
        unit = 0;
        data = null;
    }
}
