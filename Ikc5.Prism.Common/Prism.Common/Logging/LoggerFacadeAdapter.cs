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
	public class LoggerFacadeAdapter : ILoggerFacade, ILogger
	{
		private LoggerFacadeAdapter()
		{ }

		public LoggerFacadeAdapter(ILoggerFacade loggerFacade)
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

		#endregion

		#region Implementation of ILogger

		public void Log(string message, TypeLibrary.Logging.Category category, TypeLibrary.Logging.Priority priority)
		{
			LoggerFacade.Log(message, category.ToPrismCategory(), priority.ToPrismPriority());
		}

		#endregion
	}
}
