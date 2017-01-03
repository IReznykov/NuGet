using System;
using System.Diagnostics;

namespace Ikc5.TypeLibrary.Logging
{
	public class EmptyLogger : ILogger
	{
		public void Log(string message, Category category, Priority priority)
		{
			Debug.WriteLine($"{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()} (Category={category}, Priority={priority}): {message}");
		}
	}
}
