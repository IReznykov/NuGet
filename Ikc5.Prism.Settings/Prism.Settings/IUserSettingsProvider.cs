namespace Ikc5.Prism.Settings
{
	/// <summary>
	/// Contains method for serializaing and deserializing settings to storage.
	/// </summary>
	public interface IUserSettingsProvider<out T> where T: IUserSettings
	{
		void Serialize(IUserSettings userSettings);

		void Deserialize(IUserSettings userSettings);
	}
}
