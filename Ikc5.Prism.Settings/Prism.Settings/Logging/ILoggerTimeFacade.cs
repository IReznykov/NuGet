using System.Runtime.CompilerServices;
using Prism.Logging;

namespace Ikc5.Prism.Settings.Logging
{
	public interface ILoggerTimeFacade : ILoggerFacade
	{
		/// <summary>
		/// Method logs message and write start time of the method.
		/// </summary>
		/// <param name="message">Message to be written.</param>
		/// <param name="category">Category of the message.</param>
		/// <param name="priority">Priority of the method.</param>
		/// <param name="propertyName">Property or method name; if is not empty, is added before the method.</param>
		void LogStart(string message, Category category, Priority priority, [CallerMemberName] string propertyName = null);

		/// <summary>
		/// Method logs message and write end time of the method.
		/// </summary>
		/// <param name="message">Message to be written.</param>
		/// <param name="category">Category of the message.</param>
		/// <param name="priority">Priority of the method.</param>
		/// <param name="propertyName">Property or method name; if is not empty, is added before the method.</param>
		void LogEnd(string message, Category category, Priority priority, [CallerMemberName] string propertyName = null);
	}
}
