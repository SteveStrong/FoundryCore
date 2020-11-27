using System;

namespace FoundryCore
{
	/// <summary>
	/// Summary description for ApprenticeSyntaxException.
	/// </summary>
	public class ApprenticeSyntaxException : ApprenticeException
	{
		private readonly string m_sTitle;
		private readonly string m_sBuffer;

		public ApprenticeSyntaxException() : base()
		{
		}
		public ApprenticeSyntaxException(string sTitle, string sBuffer) : base("Expression not Found")
		{
			m_sTitle = sTitle;
			m_sBuffer = sBuffer;
		}
		public ApprenticeSyntaxException(string sTitle, string sBuffer, Token oTok) : base(oTok.Text)
		{
			m_sTitle = sTitle;
			m_sBuffer = sBuffer;
		}
		public ApprenticeSyntaxException(string sTitle, string sBuffer, string sMessage) : base(string.Format("Expecting a {0}",sMessage))
		{
			m_sTitle = sTitle;
			m_sBuffer = sBuffer;
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
					return m_sTitle + ": " + m_sBuffer + " Loc:" + sLoc;
				else
					return m_sTitle + ": " + m_sBuffer;
			}
		}
		public override string DisplayMessage
		{
			get 
			{
				return m_sTitle + ": " + m_sBuffer + "/r" + base.DisplayMessage;
			}
		}
	}
}
