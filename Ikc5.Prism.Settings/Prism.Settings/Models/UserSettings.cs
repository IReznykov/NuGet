using System;
using Ikc5.TypeLibrary;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace Ikc5.Prism.Settings.Models
{
	public abstract class UserSettings : BindableBase, IUserSettings
	{
		protected UserSettings(IUserSettingsService userSettingsService, IUserSettingsProvider<IUserSettings> userSettingsProvider)
		{
			userSettingsService.ThrowIfNull(nameof(userSettingsService));
			UserSettingsService = userSettingsService;

			userSettingsProvider.ThrowIfNull(nameof(userSettingsProvider));
			UserSettingsProvider = userSettingsProvider;

			// register command that allow serialize and deserialize user settings object
			SerializeCommand = new DelegateCommand<object>(Serialize, CanSerialize);
			DeserializeCommand = new DelegateCommand<object>(Deserialize, CanDeserialize);

			UserSettingsService.SerializeCommand.RegisterCommand(SerializeCommand);
			UserSettingsService.DeserializeCommand.RegisterCommand(DeserializeCommand);

			// Init all public properties by default values from DefaultValueAttribute.
			// Method considers properties from derived classes, too.
			this.SetDefaultValues();
			UserSettingsProvider.Deserialize(this);
		}

		protected IUserSettingsService UserSettingsService { get; }

		protected IUserSettingsProvider<IUserSettings> UserSettingsProvider { get; }

		#region IUserSettings

		public event EventHandler<DataEventArgs<IUserSettings>> Serialized;

		public DelegateCommand<object> SerializeCommand { get; }

		protected virtual bool CanSerialize(object arg)
		{
			return true;
		}

		protected virtual void Serialize(object arg)
		{
			UserSettingsProvider.Serialize(this);

			// Notify that the settings were serialized.
			OnSerialized(new DataEventArgs<IUserSettings>(this));
		}

		protected virtual void OnSerialized(DataEventArgs<IUserSettings> e)
		{
			Serialized?.Invoke(this, e);
		}

		public event EventHandler<DataEventArgs<IUserSettings>> Deserialized;

		public DelegateCommand<object> DeserializeCommand { get; }

		protected virtual bool CanDeserialize(object arg)
		{
			return true;
		}

		protected virtual void Deserialize(object arg)
		{
			UserSettingsProvider.Deserialize(this);

			// Notify that the settings were deserialized.
			OnDeserialized(new DataEventArgs<IUserSettings>(this));
		}

		protected virtual void OnDeserialized(DataEventArgs<IUserSettings> e)
		{
			Deserialized?.Invoke(this, e);
		}

		#endregion

		#region Helpers

		/// <summary>
		/// Init all public properties by default values from DefaultValueAttribute.
		/// Method considers properties from derived classes, too.
		/// </summary>
		//private void InitDefaultValues()
		//{
		//	var properties = TypeDescriptor.GetProperties(GetType());
		//	foreach (PropertyDescriptor property in properties)
		//	{
		//		var attribute = (DefaultValueAttribute)property?.Attributes[typeof(DefaultValueAttribute)];
		//		if (attribute == null)
		//			continue;

		//		property.SetValue(this, attribute.Value);
		//	}
		//}

		#endregion
	}
}
