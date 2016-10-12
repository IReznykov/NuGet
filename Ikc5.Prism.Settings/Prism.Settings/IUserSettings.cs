using System;
using System.ComponentModel;
using Prism.Commands;
using Prism.Events;

namespace Ikc5.Prism.Settings
{
	public interface IUserSettings : INotifyPropertyChanged
	{
		event EventHandler<DataEventArgs<IUserSettings>> Serialized;
		DelegateCommand<object> SerializeCommand { get; }

		event EventHandler<DataEventArgs<IUserSettings>> Deserialized;
		DelegateCommand<object> DeserializeCommand { get; }
	}
	
}
