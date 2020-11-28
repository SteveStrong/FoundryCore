using System;

namespace FoundryCore
{
	/// <summary>
	/// Summary description for ApprenticeSyntaxException.
	/// </summary>
	public class ApprenticeSyntaxException : ApprenticeException
	{
		private readonly string _Title;
		private readonly string _Buffer;

		public ApprenticeSyntaxException() : base()
		{
		}
		public ApprenticeSyntaxException(string sTitle, string sBuffer) : base("Expression not Found")
		{
			_Title = sTitle;
			_Buffer = sBuffer;
		}
		public ApprenticeSyntaxException(string sTitle, string sBuffer, Token oTok) : base(oTok.Text)
		{
			_Title = sTitle;
			_Buffer = sBuffer;
		}
		public ApprenticeSyntaxException(string sTitle, string sBuffer, string sMessage) : base(string.Format("Expecting a {0}",sMessage))
		{
			_Title = sTitle;
			_Buffer = sBuffer;
		}
		public override string DisplayCaption
		{
			get
			{
				return "Syntax Error";
			}
		}
		public string ErrorMessage
		{
			get 
			{
				string sLoc = base.DisplayMessage;
				if ( sLoc != "" )
					return _Title + ": " + _Buffer + " Loc:" + sLoc;
				else
					return _Title + ": " + _Buffer;
			}
		}
		public override string DisplayMessage
		{
			get 
			{
				return _Title + ": " + _Buffer + "/r" + base.DisplayMessage;
			}
		}
	}
}
