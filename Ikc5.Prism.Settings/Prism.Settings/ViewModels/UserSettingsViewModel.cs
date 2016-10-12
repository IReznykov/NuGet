using System;
using Ikc5.TypeLibrary;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace Ikc5.Prism.Settings.ViewModels
{
	public abstract class UserSettingsViewModel<TSettings> : BindableBase, IUserSettingsViewModel where TSettings : class
	{
		private IUserSettings _userSettings;

		protected UserSettingsViewModel(IUserSettings userSettings, IUserSettingsService userSettingsService)
		{
			userSettings.ThrowIfNull(nameof(userSettings));
			userSettingsService.ThrowIfNull(nameof(userSettingsService));

			UserSettings = userSettings;
			UserSettings.Deserialized += (sender, args) => CopyValuesToViewModel(args.Value);

			// create commands that save new settings to model objects
			UserSettingsService = userSettingsService;

			SaveCommand = new DelegateCommand<object>(Save, CanSave);
			CancelCommand = new DelegateCommand<object>(Cancel, CanCancel);

			UserSettingsService.SaveCommand.RegisterCommand(SaveCommand);
			UserSettingsService.CancelCommand.RegisterCommand(CancelCommand);
		}

		public IUserSettings UserSettings
		{
			get
			{
				return _userSettings;
			}
			private set
			{
				if (value == null)
					return;

				_userSettings = value;
				CopyValuesToViewModel(_userSettings);
			}
		}

		/// <summary>
		/// Module-specific settings object.
		/// </summary>
		public TSettings Settings => UserSettings as TSettings;

		protected IUserSettingsService UserSettingsService { get; }

		/// <summary>
		/// Get settings from model object and store them in viewmodel class.
		/// </summary>
		/// <param name="value">Common IUserSettings object.</param>
		protected void CopyValuesToViewModel(IUserSettings value)
		{
			CopyValuesToViewModel(value as TSettings);
		}

		/// <summary>
		/// Get settings from model object and put it to viewmodel class.
		/// </summary>
		/// <param name="value">Module-specific settings object.</param>
		protected virtual void CopyValuesToViewModel(TSettings value)
		{
			value.CopyValuesTo(this);
		}

		/// <summary>
		/// Put settings from viewmodel object to model class.
		/// </summary>
		/// <param name="value">Common IUserSettings object.</param>
		protected void CopyValuesToModel(IUserSettings value)
		{
			CopyValuesToModel(value as TSettings);
		}

		/// <summary>
		/// Put settings from viewmodel object to model class.
		/// </summary>
		/// <param name="value">Module-specific settings object.</param>
		protected virtual void CopyValuesToModel(TSettings value)
		{
			value.CopyValuesFrom(this);
		}

		#region IUserSettingsViewModel

		public event EventHandler<DataEventArgs<IUserSettings>> Saved;

		public DelegateCommand<object> SaveCommand { get; }

		protected virtual bool CanSave(object arg)
		{
			return (Settings != null);
		}

		protected virtual void Save(object arg)
		{
			if (Settings == null)
				return;

			CopyValuesToModel(UserSettings);

			// Notify that the settings were saved.
			OnSaved(new DataEventArgs<IUserSettings>(UserSettings));
		}

		protected virtual void OnSaved(DataEventArgs<IUserSettings> e)
		{
			Saved?.Invoke(this, e);
		}

		public event EventHandler<DataEventArgs<IUserSettings>> Canceled;

		public DelegateCommand<object> CancelCommand { get; }

		protected virtual bool CanCancel(object arg)
		{
			return true;
		}

		protected virtual void Cancel(object arg)
		{
			// Notify that new settings were canceled.
			OnCanceled(new DataEventArgs<IUserSettings>(UserSettings));
		}

		protected virtual void OnCanceled(DataEventArgs<IUserSettings> e)
		{
			Canceled?.Invoke(this, e);
		}

		#endregion


	}
}
