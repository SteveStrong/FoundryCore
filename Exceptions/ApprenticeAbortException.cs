using System;

namespace FoundryCore
{
	/// <summary>
	/// Summary description for ApprenticeServerException.
	/// </summary>
	public sealed class ApprenticeAbortException : ApprenticeException
	{
		public ApprenticeAbortException() : base("User abort while expanding model.")
		{
		}
		public ApprenticeAbortException(string sName, Exception eException) : base(sName,eException)
		{
		}
	}
}
