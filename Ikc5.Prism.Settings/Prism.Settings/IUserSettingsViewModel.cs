using System;
using Prism.Commands;
using Prism.Events;

namespace Ikc5.Prism.Settings
{
	public interface IUserSettingsViewModel
	{
		IUserSettings UserSettings { get; }

		event EventHandler<DataEventArgs<IUserSettings>> Saved;
		DelegateCommand<object> SaveCommand { get; }

		event EventHandler<DataEventArgs<IUserSettings>> Canceled;
		DelegateCommand<object> CancelCommand { get; }
	}
}
