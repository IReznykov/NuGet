using System;

namespace Ikc5.TypeLibrary.Logging
{
	public class ConsoleLogger : ITimestampLogger
	{
		public void Log(string message, Category category, Priority priority)
		{
			switch (category)
			{
			case Category.Error:
			case Category.Exception:
			case Category.Warn:
				Console.Error.WriteLine($"{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()} (Category={category}, Priority={priority}): {message}");
				Console.Out.WriteLine($"{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()} (Category={category}, Priority={priority}): {message}");
				break;

			default:
				Console.Out.WriteLine($"{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()} (Category={category}, Priority={priority}): {message}");
				break;
			}
		}

		public void LogStart(string message, Category category, Priority priority, string propertyName = null)
		{
			if (!string.IsNullOrWhiteSpace(propertyName))
				message = $"Start {propertyName}: {message}";
			Log(message, category, priority);
		}

		public void LogEnd(string message, Category category, Priority priority, string propertyName = null)
		{
			if (!string.IsNullOrWhiteSpace(propertyName))
				message = $"End {propertyName}: {message}";
			Log(message, category, priority);
		}
	}
}
