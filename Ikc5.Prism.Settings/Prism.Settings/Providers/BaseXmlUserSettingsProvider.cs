using System;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using Ikc5.Prism.Settings.Logging;
using Ikc5.TypeLibrary;
using Prism.Logging;

namespace Ikc5.Prism.Settings.Providers
{
	public abstract class BaseXmlUserSettingsProvider<T>
	{
		private ILiteObjectService LiteObjectService { get; }

		protected ILoggerFacade Logger { get; }

		protected BaseXmlUserSettingsProvider(ILiteObjectService liteObjectService, ILoggerFacade logger = null)
		{
			liteObjectService.ThrowIfNull(nameof(liteObjectService));
			LiteObjectService = liteObjectService;

			//logger.ThrowIfNull(nameof(logger));
			Logger = logger;

			var assembly = Assembly.GetEntryAssembly();
			AppName = Path.GetFileNameWithoutExtension(assembly.CodeBase);
			TypeName = $"{typeof(T).Namespace}.{typeof(T).Name}";

			// could be broken by 8.3 file name vs. long name issue
			//FileName = $"{AppName}.{TypeName}.xml";
			FileName = $"{TypeName}.xml";
		}

		public string AppName { get; }

		public string TypeName { get; }

		/// <summary>
		/// File name where setting object is serialized.
		/// </summary>
		public string FileName { get; protected set; }

		/// <summary>
		/// Write setting object to stream.
		/// </summary>
		/// <param name="userSettings">Object that should be serialized.</param>
		/// <param name="stream">Stream object; could be file, memory, other.</param>
		protected void Write(IUserSettings userSettings, Stream stream)
		{
			if (stream == null || userSettings == null)
				return;

			using (var writer = new StreamWriter(stream))
			{
				Logger?.Log("Serialize user settings");

				var liteUserSettings = LiteObjectService.GetLiteObject(userSettings);
				if (liteUserSettings == null)
					return;
				(new XmlSerializer(liteUserSettings.GetType())).Serialize(writer, liteUserSettings);
			}
		}

		/// <summary>
		/// Read setting object from stream.
		/// </summary>
		/// <param name="userSettings">Object that should be deserialized.</param>
		/// <param name="stream">Stream object; could be file, memory, other.</param>
		public void Read(IUserSettings userSettings, Stream stream)
		{
			if (stream == null || userSettings == null)
				return;

			using (var reader = new StreamReader(stream))
			{
				try
				{
					Logger?.Log("Deserialize user settings");

					var liteUserSettingsType = LiteObjectService.GetLiteObjectType(userSettings);
					if (liteUserSettingsType == null)
						return;
					var sourceSettings = (new XmlSerializer(liteUserSettingsType)).Deserialize(reader);
					LiteObjectService.CopyLiteObjectValues(userSettings, sourceSettings);
				}
				catch (InvalidOperationException)
				{ }
			}
		}
	}
}
