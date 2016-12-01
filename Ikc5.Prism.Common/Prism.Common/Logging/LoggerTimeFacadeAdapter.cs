using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ikc5.TypeLibrary;
using Ikc5.TypeLibrary.Logging;
using Prism.Logging;
using Category = Prism.Logging.Category;
using Priority = Prism.Logging.Priority;

namespace Ikc5.Prism.Common.Logging
{
	/// <summary>
	/// Adapter class that covers Prism's ILoggerFacade and shows it as ILogger.
	/// </summary>
	public class LoggerTimeFacadeAdapter : ILoggerTimeFacade, ITimestampLogger
	{
		private LoggerTimeFacadeAdapter()
		{ }

		public LoggerTimeFacadeAdapter(ILoggerFacade loggerFacade)
			: this()
		{
			loggerFacade.ThrowIfNull(nameof(loggerFacade));
			LoggerFacade = loggerFacade;
		}

		protected ILoggerFacade LoggerFacade { get; }

		#region Implementation of ILoggerFacade

		public void Log(string message, Category category, Priority priority)
		{
			LoggerFacade.Log(message, category, priority);
		}

		public void LogStart(string message, Category category, Priority priority, string propertyName = null)
		{
			LoggerFacade.LogStart(message, category, priority, propertyName);
		}

		public void LogEnd(string message, Category category, Priority priority, string propertyName = null)
		{
			LoggerFacade.LogEnd(message, category, priority, propertyName);
		}

		#endregion

		#region Implementation of ITimestampLogger

		public void Log(string message, TypeLibrary.Logging.Category category, TypeLibrary.Logging.Priority priority)
		{
			LoggerFacade.Log(message, category.ToPrismCategory(), priority.ToPrismPriority());
		}

		public void LogStart(string message, TypeLibrary.Logging.Category category, TypeLibrary.Logging.Priority priority, string propertyName = null)
		{
			LoggerFacade.LogStart(message, category.ToPrismCategory(), priority.ToPrismPriority(), propertyName);
		}

		public void LogEnd(string message, TypeLibrary.Logging.Category category, TypeLibrary.Logging.Priority priority, string propertyName = null)
		{
			LoggerFacade.LogEnd(message, category.ToPrismCategory(), priority.ToPrismPriority(), propertyName);
		}

		#endregion
	}
}
