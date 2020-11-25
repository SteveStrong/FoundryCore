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


        public static string CreateInternalName(this string sName)
		{
			string sAllow = @".[]_";  //for names of Visio Cells and references

			var sText = new System.Text.StringBuilder();
			foreach(char c in sName.Trim().ToCharArray())
			{
				if ( char.IsLetterOrDigit(c) )
					sText.Append(c);
				else if ( c == (char)' ' )
					sText.Append('_');
				else if ( sAllow.IndexOf(c) != -1 )
					sText.Append(c);
			}
			string sString = sText.ToString();
			return sString;
		}
    }

}