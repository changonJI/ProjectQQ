namespace QQ
{
    public static class ExtensionMethods
    {
        public static bool IsValidRange(this object[] objects, int index)
        {
            if (objects == null)
            {
                LogHelper.LogError("objects is null");

                return false;
            }

            if(objects.Length > index)
            {
                return true;
            }
            else
            {
                LogHelper.LogError("Over Arrange");

                return false;
            }
                
        }
    }

}