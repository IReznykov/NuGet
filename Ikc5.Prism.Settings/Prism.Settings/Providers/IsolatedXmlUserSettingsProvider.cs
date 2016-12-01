using System.IO;
using System.IO.IsolatedStorage;
using Ikc5.Prism.Common.Logging;
using Ikc5.TypeLibrary;
using Prism.Logging;

namespace Ikc5.Prism.Settings.Providers
{
	/// <summary>
	/// Serialize and deserialize user settings from file in isolated application folder.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class IsolatedXmlUserSettingsProvider<T> : BaseXmlUserSettingsProvider<T>, IUserSettingsProvider<T> where T : class, IUserSettings
	{
		public IsolatedXmlUserSettingsProvider(ILiteObjectService liteObjectService, ILoggerFacade logger)
			: base(liteObjectService, logger)
		{
		}

		/// <summary>
		/// Serialize user setting to file in isolated storage.
		/// </summary>
		/// <param name="userSettings"></param>
		public virtual void Serialize(IUserSettings userSettings)
		{
			Logger?.Log($"Serialize settings in Xml, Type = {userSettings?.GetType().FullName ?? "null"}, FileName = {FileName ?? "null"}");

			if (string.IsNullOrWhiteSpace(FileName))
				return;
			if (!(userSettings is T))
				return;

			Logger?.Log("Try access storage...");
			// Get a User store with type evidence for the current Domain and the Assembly.
			using (var storage = IsolatedStorageFile.GetUserStoreForAssembly())
			// Open or create a writable file.
			using (var stream = new IsolatedStorageFileStream(FileName, FileMode.Create,
				FileAccess.Write, storage))
			{
				Write(userSettings, stream);
			}
		}

		/// <summary>
		/// Deseralize user setting from file in isolated storage.
		/// If file is not found, do nothing.
		/// </summary>
		/// <param name="userSettings"></param>
		public virtual void Deserialize(IUserSettings userSettings)
		{
			Logger?.Log($"Deserialize settings in Xml, Type = {userSettings?.GetType().FullName ?? "null"}, FileName = {FileName ?? "null"}");

			if (string.IsNullOrWhiteSpace(FileName))
				return;
			if (userSettings == null)
				return;

			try
			{
				Logger?.Log("Try access storage...");
				// Get a User store with type evidence for the current Domain and the Assembly.
				using (var storage = IsolatedStorageFile.GetUserStoreForAssembly())
				{
					//Logger?.Log($"Storage contains files: {string.Join(Environment.NewLine, storage.GetFileNames())}");
					if (!storage.FileExists(FileName))
						return;

					// Open a readable file.
					using (var stream = new IsolatedStorageFileStream(FileName, FileMode.Open,
												FileAccess.Read, FileShare.Read, storage))
					{
						Read(userSettings, stream);
					}
				}
			}
			catch (FileNotFoundException)
			{
				// no file - do nothing
			}
		}
	}
}
