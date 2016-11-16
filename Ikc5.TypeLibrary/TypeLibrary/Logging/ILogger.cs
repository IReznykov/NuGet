using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ikc5.TypeLibrary.Logging
{
  /// <summary>
  /// Interface for logger.
  /// Extended version includes methods with timestamps.
  /// </summary>
  public interface ILogger
	{
		/// <summary>
		/// Write a new log entry with the specified category and priority.
		/// </summary>
		/// <param name="message">Message to be written.</param>
		/// <param name="category">Category of the entry.</param>
		/// <param name="priority">The priority of the entry.</param>
		void Log(string message, Category category, Priority priority);
	}
}
