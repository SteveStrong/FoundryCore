using System;

namespace FoundryCore
{
	/// <summary>
	/// Summary description for ApprenticeServerException.
	/// </summary>
	public sealed class ApprenticeServerException : ApprenticeException
	{
		public ApprenticeServerException() : base("Exception while running on server")
		{
		}
		public ApprenticeServerException(string sMessage) : base(sMessage)
		{
		}
		public ApprenticeServerException(string sName, Exception eException) : base(sName,eException)
		{
		}
	}
}
