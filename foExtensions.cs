using System;


namespace FoundryCore
{
    public static class Extensions
    {
        public static String Blank(this String me)
        {
            return String.Empty;
        }

        public static T GetValue<T>(this String value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }
        public static T Blank<T>(this T me)
        {
            var tot = typeof(T);
            return tot.IsValueType
              ? default(T)
              : (T)Activator.CreateInstance(tot)
              ;
        }
    }

}