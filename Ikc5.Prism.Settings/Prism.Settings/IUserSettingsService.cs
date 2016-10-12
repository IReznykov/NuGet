using Prism.Commands;

namespace Ikc5.Prism.Settings
{
	public interface IUserSettingsService
	{
		CompositeCommand SerializeCommand { get; }

		CompositeCommand DeserializeCommand { get; }

		CompositeCommand SaveCommand { get; }
		
		CompositeCommand CancelCommand { get; }
	}
}
