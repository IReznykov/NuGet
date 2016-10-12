using System;
using System.ComponentModel;
using Ikc5.Prism.Settings;
using Ikc5.Prism.Settings.Models;

namespace $rootnamespace$.Models
{
	[Serializable]
	public class Settings : UserSettings, ISettings
	{
		public Settings(IUserSettingsService userSettingsService, IUserSettingsProvider<Settings> userSettingsProvider)
			: base(userSettingsService, userSettingsProvider)
		{
		}

		#region ISettings

		//private object _example;

		/// <summary>
		/// Example.
		/// </summary>
		//[DefaultValue("Value")]
		//public object Example
		//{
		//	get { return _example; }
		//	set { SetProperty(ref _example, value); }
		//}

		#endregion

	}
}
