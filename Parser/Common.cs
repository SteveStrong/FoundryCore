using System.IO;

namespace FoundryCore
{
    public static class Common
    {
        public static bool IsNumber(string sString)
		{
			try
			{
				char[] cTest = new char[] { (char)34, (char)39, (char)32, ';', '_', '@', '[', '(', '{', '*', '/', '%', ']', ')', '}', 'x', '#', '=' };

				if ( sString.Length == 0 || sString.IndexOfAny(cTest) >= 0 )
					return false;

				if ( sString.IndexOf('-') > 0 || sString.IndexOf('+') > 0 )
					return false;

				if ( char.IsLetter(sString,sString.Length-1) || char.IsLetter(sString,0) )
					return false;

				double.Parse(sString);
				return true;
			}
			catch {};
			return false;
		}

public static string WrapDQIfNeeded(string sString, bool bRemoveInternal)
		{
			string sDQ = ((char)'"').ToString();
			if ( sString.StartsWith(sDQ) && sString.EndsWith(sDQ) )
				return sString;

			if ( bRemoveInternal && sString.IndexOf(sDQ) != -1 )
				sString = sString.Replace(sDQ,"");


			return WrapDQ(sString);
		}
		public static string WrapDQIfNeeded(string sString)
		{
			string sDQ = ((char)'"').ToString();
			if ( sString.StartsWith(sDQ) && sString.EndsWith(sDQ) )
				return sString;

			return WrapDQ(sString);
		}
		public static string WrapSQIfNeeded(string sString)
		{
			if ( sString.StartsWith("'") && sString.EndsWith("'") )
				return sString;

			return WrapSQ(sString);
		}
		public static string WrapParenthesis(string sString)
		{
			return string.Format("({0})",sString);
		}
		public static string WrapDQ(string sString)
		{
			string sDQ = ((char)'"').ToString();
			return string.Format("{0}{1}{2}",sDQ,sString,sDQ);
		}
		public static string WrapSQ(string sString)
		{
			return string.Format("'{0}'",sString);
		}
		public static string DelimitedString(string sTarget, string sAddition)
		{
			sTarget += (sTarget.Length == 0) ? sAddition : ";" + sAddition;
			return sTarget;
		}
		public static string UnwrapBracket(string sString)
		{
			if ( sString.StartsWith("[") && sString.EndsWith("]") )
				return sString.Substring(1,sString.Length-2);
			return sString;
		}
		public static string UnwrapBrace(string sString)
		{
			if ( sString.StartsWith("{") && sString.EndsWith("}") )
				return sString.Substring(1,sString.Length-2);
			return sString;
		}
		public static string UnwrapParen(string sString)
		{
			if ( sString.StartsWith("(") && sString.EndsWith(")") )
				return sString.Substring(1,sString.Length-2);
			return sString;
		}
		public static string UnwrapSQ(string sString)
		{
			if ( sString.StartsWith("'") && sString.EndsWith("'") )
				return sString.Substring(1,sString.Length-2);
			return sString;
		}
		public static string UnwrapDQ(string sString)
		{
			string sDQ = ((char)'"').ToString();

			if ( sString.StartsWith(sDQ) && sString.EndsWith(sDQ) )
				return sString.Substring(1,sString.Length-2);
			return sString;
		}

    }    
}