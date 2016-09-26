using System.ComponentModel;

namespace Ikc5.TypeLibrary
{
	/// <summary>
	/// Class is used for Lite Object Types and contains default constructor
	///  that initiates properties by default value from attribute.
	/// </summary>
	public class LiteObjectBase
	{
		public LiteObjectBase()
		{
			var properties = TypeDescriptor.GetProperties(GetType());
			foreach (PropertyDescriptor property in properties)
			{
				var attribute = (DefaultValueAttribute)property?.Attributes[typeof(DefaultValueAttribute)];
				if (attribute == null)
					continue;

				property.SetValue(this, attribute.Value);
			}
		}
	}
}
