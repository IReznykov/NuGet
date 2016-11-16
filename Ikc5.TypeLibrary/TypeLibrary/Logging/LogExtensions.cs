using System.Runtime.CompilerServices;
using System.Text;

namespace Ikc5.TypeLibrary.Logging
{
	/// <summary>
	/// Extensions with shorter logging method calls.
	/// </summary>
	public static class LogExtensions
	{
		/// <summary>
		/// Short call of ILogger.Log method.
		/// </summary>
		/// <param name="logger">Logger object.</param>
		/// <param name="message">Message to be written.</param>
		/// <param name="category">Category of the message.</param>
		/// <param name="priority">Priority of the message.</param>
		/// <param name="propertyName">Property or method name; if is not empty, is added before the method.</param>
		/// <returns>Logger object.</returns>
		public static ILogger Log(this ILogger logger,
			string message,
			Category category = Category.Debug,
			Priority priority = Priority.None,
			[CallerMemberName] string propertyName = null)
		{
			logger.Log(string.IsNullOrEmpty(propertyName) ? message : $"{propertyName}: {message}",
				category, Priority.None);
			return logger;
		}

		/// <summary>
		/// Short call that logs information about exception.
		/// </summary>
		/// <param name="logger">Logger object.</param>
		/// <param name="ex">Exception that was thrown.</param>
		/// <param name="category">Category of the message.</param>
		/// <param name="priority">Priority of the message.</param>
		/// <param name="propertyName">Property or method name; if is not empty, is added before the method.</param>
		/// <returns>Logger object.</returns>
		public static ILogger Exception(this ILogger logger,
			System.Exception ex,
			Category category = Category.Exception,
			Priority priority = Priority.High,
			[CallerMemberName] string propertyName = null)
		{
			logger.Log(string.IsNullOrEmpty(propertyName) ? "Exception: " : $"{propertyName}: Exception: ", category, priority);
			var innerException = ex;
			while (innerException != null)
			{
				var messageBuilder = new StringBuilder();
				messageBuilder.AppendLine(new string('-', 40));
				messageBuilder.AppendLine($"Type:\t\t {innerException.GetType().FullName}");
				messageBuilder.AppendLine($"Message:\t {innerException.Message}");
				messageBuilder.AppendLine($"Source:\t\t {innerException.Source}");
				messageBuilder.AppendLine($"StackTrace:\t {innerException.StackTrace}");

				logger.Log(messageBuilder.ToString(), category, priority);
				innerException = innerException.InnerException;
			}
			return logger;
		}

		/// <summary>
		/// Extension detects derived logger with possibility to write start and end timestamp of the method.
		/// If logger has such possibility, it call correspond method.
		/// </summary>
		/// <param name="logger">Logger object.</param>
		/// <param name="message">Message to be written.</param>
		/// <param name="category">Category of the message.</param>
		/// <param name="priority">Priority of the message.</param>
		/// <param name="propertyName">Property or method name; if is not empty, is added before the method.</param>
		/// <returns></returns>
		public static ILogger LogStart(this ILogger logger,
			string message,
			Category category = Category.Info,
			Priority priority = Priority.None,
			[CallerMemberName] string propertyName = null)
		{
			var timeFacade = logger as ITimestampLogger;
			if (timeFacade != null)
			{
				timeFacade.LogStart(message, category, priority, propertyName);
			}
			else
			{
				logger.Log(message, category, priority);
			}
			return logger;
		}

		/// <summary>
		/// Extension detects derived logger with possibility to write start and end timestamp of the method.
		/// If logger has such possibility, it call correspond method.
		/// </summary>
		/// <param name="logger">Logger object.</param>
		/// <param name="message">Message to be written.</param>
		/// <param name="category">Category of the message.</param>
		/// <param name="priority">Priority of the message.</param>
		/// <param name="propertyName">Property or method name; if is not empty, is added before the method.</param>
		/// <returns></returns>
		public static ILogger LogEnd(this ILogger logger, 
			string message,
			Category category = Category.Info,
			Priority priority = Priority.None,
			[CallerMemberName] string propertyName = null)
		{
			var timeFacade = logger as ITimestampLogger;
			if (timeFacade != null)
			{
				timeFacade.LogEnd(message, category, priority, propertyName);
			}
			else
			{
				logger.Log(message, category, priority);
			}
			return logger;
		}

	}
}
