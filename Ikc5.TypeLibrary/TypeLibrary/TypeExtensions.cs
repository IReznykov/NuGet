using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Ikc5.TypeLibrary
{
	public static class TypeExtensions
	{
		private static bool GetDefaultValueBase<T>(PropertyDescriptorCollection propertyCollection,
			ref T value,
			string propertyName)
		{
			var property = propertyCollection[propertyName];
			var attribute = (DefaultValueAttribute)property?.Attributes[typeof(DefaultValueAttribute)];
			if (attribute?.Value == null)
				return false;

			value = (T)System.Convert.ChangeType(attribute.Value, typeof(T));
			return true;
		}

		/// <summary>
		/// Returns default value for property from DefaultValue attributes 
		/// or null if it is not defined.
		/// From MSDN: Call TypeDescriptor.GetProperties(Type) only if you haven't instance of the object.
		/// </summary>
		/// <param name="thisObjectType">Type of an object that is investigated.</param>
		/// <param name="value">Returned default value.</param>
		/// <param name="propertyName">Property name, could be omitted.</param>
		/// <returns></returns>
		public static bool GetDefaultValue<T>(this Type thisObjectType, ref T value, string propertyName = null)
		{
			if (thisObjectType == null || string.IsNullOrEmpty(propertyName))
				return false;

			var propertyCollection = TypeDescriptor.GetProperties(thisObjectType);
			return GetDefaultValueBase(propertyCollection, ref value, propertyName);
		}

		/// <summary>
		/// Returns default value for property from DefaultValue attributes 
		/// or null if it is not defined.
		/// </summary>
		/// <param name="thisObject">Object that is investigated.</param>
		/// <param name="value">Returned default value.</param>
		/// <param name="propertyName">Property name, could be omitted.</param>
		/// <returns></returns>
		public static bool GetDefaultValue<T>(this object thisObject, ref T value, string propertyName = null)
		{
			if (thisObject == null || string.IsNullOrEmpty(propertyName))
				return false;

			var propertyCollection = TypeDescriptor.GetProperties(thisObject);
			return GetDefaultValueBase(propertyCollection, ref value, propertyName);
		}

		/// <summary>
		/// Returns default value for property from DefaultValue attributes 
		/// or null if it is not defined.
		/// </summary>
		/// <param name="thisObjectType">Object that is investigated.</param>
		/// <param name="value">Returned default value.</param>
		/// <param name="defaultValue">Default value that is used if attribute is not defined.</param>
		/// <param name="propertyName">Property name, could be omitted.</param>
		/// <returns></returns>
		public static bool GetDefaultValue<T>(this Type thisObjectType, ref T value, T defaultValue, string propertyName = null)
		{
			if (thisObjectType == null || string.IsNullOrEmpty(propertyName))
				return false;

			value = defaultValue;
			var propertyCollection = TypeDescriptor.GetProperties(thisObjectType);
			return GetDefaultValueBase(propertyCollection, ref value, propertyName);
		}

		/// <summary>
		/// Returns default value for property from DefaultValue attributes 
		/// or null if it is not defined.
		/// </summary>
		/// <param name="thisObject">Object that is investigated.</param>
		/// <param name="value">Returned default value.</param>
		/// <param name="defaultValue">Default value that is used if attribute is not defined.</param>
		/// <param name="propertyName">Property name, could be omitted.</param>
		/// <returns></returns>
		public static bool GetDefaultValue<T>(this object thisObject, ref T value, T defaultValue, string propertyName = null)
		{
			if (thisObject == null || string.IsNullOrEmpty(propertyName))
				return false;

			value = defaultValue;
			var propertyCollection = TypeDescriptor.GetProperties(thisObject);
			return GetDefaultValueBase(propertyCollection, ref value, propertyName);
		}

		/// <summary>
		/// Copies to current object values from another objct using reflections.
		/// Considered properties that are declared in the thisObject's type.
		/// </summary>
		/// <param name="thisObject">Object where copied values.</param>
		/// <param name="anotherObject">Object that provide values.</param>
		/// <param name="top">If TRUE, lite object type contains properties that belong
		/// exactly to type of parentObject. Otherwise, lite object contains all read-write
		/// properties of parent object.</param>
		/// <returns></returns>
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
		/// Copies to current object values from another objct using reflections.
		/// Considered properties that are declared in the thisObject's type.
		/// </summary>
		/// <param name="thisObject">Object where copied values.</param>
		/// <param name="anotherObject">Object that provide values.</param>
		/// <param name="top">If TRUE, lite object type contains properties that belong
		/// exactly to type of parentObject. Otherwise, lite object contains all read-write
		/// properties of parent object.</param>
		/// <returns></returns>
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
	}
}
