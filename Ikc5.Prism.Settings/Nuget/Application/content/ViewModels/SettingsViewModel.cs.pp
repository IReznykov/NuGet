using Ikc5.Prism.Settings;
using Ikc5.Prism.Settings.ViewModels;
using $rootnamespace$.Models;

namespace $rootnamespace$.ViewModels
{
	public class SettingsViewModel : UserSettingsViewModel<ISettings>, ISettingsViewModel
	{
		public SettingsViewModel(ISettings settingsModel, IUserSettingsService userSettingsService)
			: base(settingsModel as IUserSettings, userSettingsService)
		{
		}

		#region ISettings

		//private object _example;

		#endregion ISettings

		#region ISettingsViewModel

		/// <summary>
		/// Example.
		/// </summary>
		//[DefaultValue("Value")]
		//public object Example
		//{
		//	get { return _example; }
		//	set { SetProperty(ref _example, value); }
		//}

		#endregion ISettingsViewModel

	}
}