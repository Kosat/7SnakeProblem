using System;

namespace _7SnakeProblem
{
	/// <inheritdoc />
	/// <summary>
	/// This exception is used by <see cref="T:_7SnakeProblem.CSVFileService" /> to signal that loading of a grid data failed.
	/// </summary>
	public class CSVFileServiceException : Exception
	{
		public CSVFileServiceException(string message) : base(message) {}

		public CSVFileServiceException(string message, Exception ex) : base(message, ex) {}
	}
}