using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Ikc5.TypeLibrary
{
	/// <summary>
	/// Contains the extensions that manipulate DefaultValueAttribute 
	/// and properties through reflection.
	/// </summary>
	public static class TypeExtensions
	{
		/// <summary>
		/// Set value to the property from DefaultValue attribute
		/// or do nothing if the attribute is not defined.
		/// </summary>
		/// <param name="thisObject">Object that is investigated.</param>
		/// <param name="propertyName">Property name, could be omitted.</param>
		/// <param name="returnOnNullAttribute">Method returns if attribute is null.</param>
		/// <param name="getAttributeValue">Function that gives value of default attribute.</param>
		/// <returns>TRUE if property value is set.</returns>
		private static bool SetDefaultValueBase(
			object thisObject,
			string propertyName,
			bool returnOnNullAttribute,
			Func<DefaultValueAttribute, object> getAttributeValue)
		{
			if (thisObject == null || string.IsNullOrEmpty(propertyName))
				return false;

			var propertyCollection = TypeDescriptor.GetProperties(thisObject);
			var property = propertyCollection[propertyName];
			if (property == null)
				return false;
			return SetDefaultValueToProperty(thisObject, property, returnOnNullAttribute, getAttributeValue);
		}

		/// <summary>
		/// Set value to the property from DefaultValue attribute
		/// or do nothing if the attribute is not defined.
		/// </summary>
		/// <param name="thisObject">Object that is investigated.</param>
		/// <param name="property">Object describes mentioned property.</param>
		/// <param name="returnOnNullAttribute">Method returns if attribute is null.</param>
		/// <param name="getAttributeValue">Function that gives value of default attribute.</param>
		/// <returns>TRUE if property value is set.</returns>
		private static bool SetDefaultValueToProperty(
			object thisObject,
			PropertyDescriptor property,
			bool returnOnNullAttribute,
			Func<DefaultValueAttribute, object> getAttributeValue)
		{
			if (thisObject == null || property == null)
				return false;
			var attribute = (DefaultValueAttribute)property.Attributes[typeof(DefaultValueAttribute)];
			if (attribute == null && returnOnNullAttribute)
				return false;

			// attribute.Value could has correct value 'null'
			var newValue = getAttributeValue(attribute);
			if (!property.IsReadOnly)
			{
				property.SetValue(thisObject, newValue);
			}
			else
			{
				var propertyInfo = thisObject.GetType().
						GetProperty(property.Name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
				if (propertyInfo == null)
					return false;
				propertyInfo.SetValue(thisObject, newValue);
			}
			return true;
		}

		/// <summary>
		/// Set value to the property from DefaultValue attribute
		/// or do nothing if the attribute is not defined.
		/// </summary>
		/// <param name="thisObject">Object that is investigated.</param>
		/// <param name="propertyName">Property name, could be omitted.</param>
		/// <returns>TRUE if property value is set.</returns>
		public static bool SetDefaultValue<T>(this object thisObject, [CallerMemberName]string propertyName = null)
		{
			return SetDefaultValueBase(
				thisObject,
				propertyName,
				true,
				attribute => (attribute.Value == null ? default(T) : Convert.ChangeType(attribute.Value, typeof(T))));
		}

		/// <summary>
		/// Set value to the property from DefaultValue attribute
		/// or do nothing if the attribute is not defined.
		/// </summary>
		/// <param name="thisObject">Object that is investigated.</param>
		/// <param name="propertyName">Property name, could be omitted.</param>
		/// <returns>TRUE if property value is set.</returns>
		public static bool SetDefaultValue(this object thisObject, [CallerMemberName]string propertyName = null)
		{
			return SetDefaultValueBase(
				thisObject,
				propertyName,
				true,
				attribute => attribute.Value);
		}

		/// <summary>
		/// Set value to the property from DefaultValue attribute
		/// or assign provided default value if the attribute is not defined.
		/// </summary>
		/// <param name="thisObject">Object that is investigated.</param>
		/// <param name="defaultValue">Default value that is used if attribute is not defined.</param>
		/// <param name="propertyName">Property name, could be omitted.</param>
		/// <returns>TRUE if property value is set.</returns>
		public static bool SetDefaultValue<T>(this object thisObject, T defaultValue, string propertyName)
		{
			return SetDefaultValueBase(
				thisObject,
				propertyName,
				false,
				attribute => (attribute == null ? defaultValue : (attribute.Value == null ? default(T) : Convert.ChangeType(attribute.Value, typeof(T)))));
		}

		/// <summary>
		/// Set default values to those properties that has DefaultValue attribute.
		/// </summary>
		/// <param name="thisObject">Object that is investigated.</param>
		public static void SetDefaultValues(this object thisObject)
		{
			if (thisObject == null)
				return;

			var properties = TypeDescriptor.GetProperties(thisObject);
			foreach (PropertyDescriptor property in properties)
			{
				SetDefaultValueToProperty(thisObject, property, true, attribute => attribute.Value);
			}
		}

		private static bool GetDefaultValueBase<T>(PropertyDescriptorCollection propertyCollection,
			ref T value,
			string propertyName)
		{
			var property = propertyCollection[propertyName];
			var attribute = (DefaultValueAttribute)property?.Attributes[typeof(DefaultValueAttribute)];
			if (attribute == null)
				return false;

			// attribute.Value could has correct value 'null'
			value = attribute.Value == null ? default(T) : (T)Convert.ChangeType(attribute.Value, typeof(T));
			return true;
		}

		/// <summary>
		/// Assign value to the variable from DefaultValue attribute
		/// or do nothing if the attribute is not defined.
		/// From MSDN: Call TypeDescriptor.GetProperties(Type) only if you haven't instance of the object.
		/// </summary>
		/// <param name="thisObjectType">Type of an object that is investigated.</param>
		/// <param name="value">Returned default value.</param>
		/// <param name="propertyName">Property name, could be omitted.</param>
		/// <returns>TRUE if variable value is set.</returns>
		public static bool GetDefaultValue<T>(this Type thisObjectType, ref T value, string propertyName = null)
		{
			if (thisObjectType == null || string.IsNullOrEmpty(propertyName))
				return false;

			var propertyCollection = TypeDescriptor.GetProperties(thisObjectType);
			return GetDefaultValueBase(propertyCollection, ref value, propertyName);
		}

		/// <summary>
		/// Assign value to the variable from DefaultValue attribute
		/// or do nothing if the attribute is not defined.
		/// </summary>
		/// <param name="thisObject">Object that is investigated.</param>
		/// <param name="value">Returned default value.</param>
		/// <param name="propertyName">Property name, could be omitted.</param>
		/// <returns>TRUE if variable value is set.</returns>
		public static bool GetDefaultValue<T>(this object thisObject, ref T value, string propertyName = null)
		{
			if (thisObject == null || string.IsNullOrEmpty(propertyName))
				return false;

			var propertyCollection = TypeDescriptor.GetProperties(thisObject);
			return GetDefaultValueBase(propertyCollection, ref value, propertyName);
		}

		/// <summary>
		/// Assign value to the variable from DefaultValue attribute
		/// or assign provided default value if the attribute is not defined.
		/// From MSDN: Call TypeDescriptor.GetProperties(Type) only if you haven't instance of the object.
		/// </summary>
		/// <param name="thisObjectType">Object that is investigated.</param>
		/// <param name="value">Returned default value.</param>
		/// <param name="defaultValue">Default value that is used if attribute is not defined.</param>
		/// <param name="propertyName">Property name, could be omitted.</param>
		/// <returns>TRUE if variable value is set.</returns>
		public static bool GetDefaultValue<T>(this Type thisObjectType, ref T value, T defaultValue, string propertyName = null)
		{
			if (thisObjectType == null || string.IsNullOrEmpty(propertyName))
				return false;

			value = defaultValue;
			var propertyCollection = TypeDescriptor.GetProperties(thisObjectType);
			return GetDefaultValueBase(propertyCollection, ref value, propertyName);
		}

		/// <summary>
		/// Assign value to the variable from DefaultValue attribute
		/// or assign provided default value if the attribute is not defined.
		/// </summary>
		/// <param name="thisObject">Object that is investigated.</param>
		/// <param name="value">Returned default value.</param>
		/// <param name="defaultValue">Default value that is used if attribute is not defined.</param>
		/// <param name="propertyName">Property name, could be omitted.</param>
		/// <returns>TRUE if variable value is set.</returns>
		public static bool GetDefaultValue<T>(this object thisObject, ref T value, T defaultValue, string propertyName = null)
		{
			if (thisObject == null || string.IsNullOrEmpty(propertyName))
				return false;

			value = defaultValue;
			var propertyCollection = TypeDescriptor.GetProperties(thisObject);
			return GetDefaultValueBase(propertyCollection, ref value, propertyName);
		}

		/// <summary>
		/// Copies property values to the current object from the another object using reflections.
		/// Considered properties that are declared in the thisObject's type.
		/// </summary>
		/// <param name="thisObject">Object where copied values.</param>
		/// <param name="anotherObject">Object that provide values.</param>
		/// <param name="top">If TRUE, lite object type contains properties that belong
		/// exactly to the type of parentObject. Otherwise, lite object contains all read-write
		/// properties of parent object.</param>
		/// <returns>thisObject with updates values.</returns>
		public static object CopyValuesFrom(this object thisObject, object anotherObject, bool top = true)
		{
			if (thisObject == null || anotherObject == null)
				return thisObject;

			var propertyInfos = thisObject.GetType()
				.GetProperties(BindingFlags.Public | BindingFlags.Instance)
				.Where(property => ((top && property.DeclaringType == thisObject.GetType()) || !top) && (property.CanRead && property.CanWrite));

			var anotherObjectType = anotherObject.GetType();

			foreach (var property in propertyInfos)
			{
				try
				{
					var anotherObjectProperty = anotherObjectType.GetProperty(property.Name);
					if (anotherObjectProperty == null)
						continue;
					property.SetValue(thisObject, anotherObjectProperty.GetValue(anotherObject));
				}
				catch (AmbiguousMatchException)
				{ }
				catch (ArgumentNullException)
				{ }
			}
			return thisObject;
		}

		/// <summary>
		/// Copies property values from the current object to the another object using reflections.
		/// Considered properties that are declared in the thisObject's type.
		/// </summary>
		/// <param name="thisObject">Object where copied values.</param>
		/// <param name="anotherObject">Object that provide values.</param>
		/// <param name="top">If TRUE, lite object type contains properties that belong
		/// exactly to the type of parentObject. Otherwise, lite object contains all read-write
		/// properties of parent object.</param>
		/// <returns>thisObject with updates values.</returns>
		public static object CopyValuesTo(this object thisObject, object anotherObject, bool top = true)
		{
			if (thisObject == null || anotherObject == null)
				return thisObject;

			var propertyInfos = thisObject.GetType()
				.GetProperties(BindingFlags.Public | BindingFlags.Instance)
				.Where(property => ((top && property.DeclaringType == thisObject.GetType()) || !top) && (property.CanRead && property.CanWrite));

			var anotherObjectType = anotherObject.GetType();

			foreach (var property in propertyInfos)
			{
				try
				{
					var anotherObjectProperty = anotherObjectType.GetProperty(property.Name);
					if (anotherObjectProperty == null)
						continue;
					anotherObjectProperty.SetValue(anotherObject, property.GetValue(thisObject));
				}
				catch (AmbiguousMatchException)
				{ }
				catch (ArgumentNullException)
				{ }
			}
			return thisObject;
		}

		/// <summary>
		/// Throw ArgumentNullException exception if object is null.
		/// </summary>
		/// <param name="thisObject">Object that is checked.</param>
		/// <param name="paramName"></param>
		/// <param name="message"></param>
		public static void ThrowIfNull(this object thisObject, string paramName = null, string message = null)
		{
			if (thisObject == null)
			{
				throw new ArgumentNullException(paramName, message ?? "Value cannot be null.");
			}
		}
	}
}
