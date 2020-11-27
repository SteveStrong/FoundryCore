using System;

namespace FoundryCore
{
	/// <summary>
	/// Summary description for ApprenticeException.
	/// </summary>
	public class ApprenticeException : ApplicationException
	{
		public object SourceObject = null;
		public Exception SourceException = null;

		public ApprenticeException() : base()
		{
		}
		public ApprenticeException(string sMessage) : base(sMessage)
		{
		}
		public ApprenticeException(object oSource, string sMessage) : base(sMessage)
		{
			SourceObject = oSource;
		}
		public ApprenticeException(object oSource, Exception eException, string sMessage) : base(sMessage, eException)
		{
			SourceObject = oSource;
			SourceException = eException;
		}
		public ApprenticeException(string sMessage, Exception eException) : base(sMessage, eException)
		{
			SourceException = eException;
		}
		public virtual string DisplayMessage
		{
			get
			{
				return Message;
			}
		}
		public virtual string DisplayCaption
		{
			get
			{
				return "Apprentice Exception";
			}
		}
	}
}
