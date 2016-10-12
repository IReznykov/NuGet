using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Ikc5.Prism.Settings.Logging;
using Ikc5.TypeLibrary;
using Prism.Logging;

namespace Ikc5.Prism.Settings.Providers
{
	/// <summary>
	/// Serialize and deserialize user settings from file in local application folder.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class LocalXmlUserSettingsProvider<T> : BaseXmlUserSettingsProvider<T>, IUserSettingsProvider<T> where T : class, IUserSettings
	{
		public LocalXmlUserSettingsProvider(ILiteObjectService liteObjectService, ILoggerFacade logger)
			: base(liteObjectService, logger)
		{
			var assembly = Assembly.GetEntryAssembly();
			FolderName = Path.Combine(Path.GetDirectoryName(assembly.Location) ?? "", "Settings");
		}

		public string FolderName { get; protected set; }

		/// <summary>
		/// Serialize user setting to file in isolated storage.
		/// </summary>
		/// <param name="userSettings"></param>
		public void Serialize(IUserSettings userSettings)
		{
			Logger?.Log($"Serialize settings in Xml, Type = {userSettings?.GetType().FullName ?? "null"}, Folder = {FolderName ?? "null"}, FileName = {FileName ?? "null"}");

			if (string.IsNullOrWhiteSpace(FileName) || string.IsNullOrEmpty(FolderName))
				return;
			if (!(userSettings is T))
				return;

			// create directory if it does not exist
			Logger?.Log("Create folder...");
			var directoryInfo = Directory.CreateDirectory(FolderName);
			//if( !directoryInfo.Exists)
			//	directoryInfo.Create();

			// Open or create a writable file.
			using (var stream = File.Open($"{FolderName}\\{FileName}", FileMode.Create, FileAccess.Write))
			{
				Write(userSettings, stream);
			}
		}

		/// <summary>
		/// Deseralize user setting from file in isolated storage.
		/// If file is not found, do nothing.
		/// </summary>
		/// <param name="userSettings"></param>
		public void Deserialize(IUserSettings userSettings)
		{
			Logger?.Log($"Deserialize settings in Xml, Type = {userSettings?.GetType().FullName ?? "null"}, Folder = {FolderName ?? "null"}, FileName = {FileName ?? "null"}");

			if (string.IsNullOrWhiteSpace(FileName) || string.IsNullOrEmpty(FolderName))
				return;
			if (userSettings == null)
				return;

			try
			{
				// create directory if it does not exist
				Logger?.Log("Check folder...");
				if (!Directory.Exists(FolderName))
				{
					Logger?.Log("Folder doesn't exist, nothing to deserialize");
					return;
				}
				Logger?.Log($"Folder contains files: {string.Join(Environment.NewLine, Directory.GetFiles(FolderName).Select(Path.GetFileName))}");

				// Open a readable file.
				using (var stream = File.Open($"{FolderName}\\{FileName}", FileMode.Open, FileAccess.Read, FileShare.Read))
				{
					Read(userSettings, stream);
				}
			}
			catch (FileNotFoundException)
			{
				// no file - do nothing
			}
		}
	}
}
