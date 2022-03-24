namespace ru.zorro.static_select
{
    public static class ObjectHelper
    {
        public static object GetPropertyValue(this object T, string PropName)
        {
            return T.GetType().GetProperty(PropName) == null ? null : T.GetType().GetProperty(PropName).GetValue(T, null);
        }

        public static void SetPropertyValue(this object T, string PropName, object value)
        {
            if (T.GetType().GetProperty(PropName) != null)
            {
                T.GetType().GetProperty(PropName).SetValue(T, value);
            }
        }
    }
}
