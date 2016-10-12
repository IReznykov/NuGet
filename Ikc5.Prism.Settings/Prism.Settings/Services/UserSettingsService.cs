using Prism.Commands;

namespace Ikc5.Prism.Settings.Services
{
	/// <summary>
	/// User settings service provides composite commands for 
	/// serializing and deserializing user settings objects in modules.
	/// </summary>
	public class UserSettingsService : IUserSettingsService
	{
		public CompositeCommand SerializeCommand { get; } = new CompositeCommand();

		public CompositeCommand DeserializeCommand { get; } = new CompositeCommand();

		public CompositeCommand SaveCommand { get; } = new CompositeCommand();

		public CompositeCommand CancelCommand { get; } = new CompositeCommand();
	}
}
