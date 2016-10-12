using Prism.Logging;
using System.Runtime.CompilerServices;
using System.Text;

namespace Ikc5.Prism.Settings.Logging
{
	/// <summary>
	/// Extensions with shorter logging method calls.
	/// </summary>
	public static class LogExtensions
	{
		/// <summary>
		/// Short call of ILoggerFacade.Log method.
		/// </summary>
		/// <param name="logger">Logger object from Unity Container.</param>
		/// <param name="message">Message to be written.</param>
		/// <param name="category">Category of the message.</param>
		/// <param name="priority">Priority of the method.</param>
		/// <param name="propertyName">Property or method name; if is not empty, is added before the method.</param>
		/// <returns>Logger object.</returns>
		public static ILoggerFacade Log(this ILoggerFacade logger, string message,
			Category category = Category.Debug, Priority priority = Priority.None,
			[CallerMemberName] string propertyName = null)
		{
			logger.Log(string.IsNullOrEmpty(propertyName) ? message : $"{propertyName}: {message}",
				category, Priority.None);
			return logger;
		}

		/// <summary>
		/// Short call that logs information about exception.
		/// </summary>
		/// <param name="logger">Logger object from Unity Container.</param>
		/// <param name="ex">Exception that was thrown.</param>
		/// <param name="category">Category of the message.</param>
		/// <param name="propertyName">Property or method name; if is not empty, is added before the method.</param>
		/// <returns>Logger object.</returns>
		public static ILoggerFacade Exception(this ILoggerFacade logger, System.Exception ex, Category category = Category.Exception,
			[CallerMemberName] string propertyName = null)
		{
			logger.Log(string.IsNullOrEmpty(propertyName) ? "Exception: " : $"{propertyName}: Exception: ", category, Priority.High);
			var innerException = ex;
			while (innerException != null)
			{
				var messageBuilder = new StringBuilder();
				messageBuilder.AppendLine(new string('-', 40));
				messageBuilder.AppendLine($"Type:\t\t {innerException.GetType().FullName}");
				messageBuilder.AppendLine($"Message:\t {innerException.Message}");
				messageBuilder.AppendLine($"Source:\t\t {innerException.Source}");
				messageBuilder.AppendLine($"StackTrace:\t {innerException.StackTrace}");

				logger.Log(messageBuilder.ToString(), category, Priority.None);
				innerException = innerException.InnerException;
			}
			return logger;
		}

		/// <summary>
		/// Extension detects derived logger with possibility to write start and end time of the method.
		/// If logger has such possibility, it call correspond method.
		/// </summary>
		/// <param name="logger">Logger object from Unity Container.</param>
		/// <param name="message">Message to be written.</param>
		/// <param name="category">Category of the message.</param>
		/// <param name="priority">Priority of the method.</param>
		/// <param name="propertyName">Property or method name; if is not empty, is added before the method.</param>
		/// <returns></returns>
		public static ILoggerFacade LogStart(
			this ILoggerFacade logger, string message,
			Category category = Category.Info, Priority priority = Priority.None,
			[CallerMemberName] string propertyName = null)
		{
			var timeFacade = logger as ILoggerTimeFacade;
			if (timeFacade != null)
			{
				timeFacade.LogStart(message, category, Priority.None, propertyName);
			}
			else
			{
				logger.Log(message, category, priority);
			}
			return logger;
		}

		/// <summary>
		/// Extension detects derived logger with possibility to write start and end time of the method.
		/// If logger has such possibility, it call correspond method.
		/// </summary>
		/// <param name="logger">Logger object from Unity Container.</param>
		/// <param name="message">Message to be written.</param>
		/// <param name="category">Category of the message.</param>
		/// <param name="priority">Priority of the method.</param>
		/// <param name="propertyName">Property or method name; if is not empty, is added before the method.</param>
		/// <returns></returns>
		public static ILoggerFacade LogEnd(
			this ILoggerFacade logger, string message,
			Category category = Category.Info, Priority priority = Priority.None,
			[CallerMemberName] string propertyName = null)
		{
			var timeFacade = logger as ILoggerTimeFacade;
			if (timeFacade != null)
			{
				timeFacade.LogEnd(message, category, Priority.None, propertyName);
			}
			else
			{
				logger.Log(message, category, priority);
			}
			return logger;
		}

	}
}
