using System;

namespace Ikc5.TypeLibrary
{
	/// <summary>
	/// Interface contains methods that allow to create and manipulate
	/// lite object that are objects with public write-read properties
	/// of provided parent object.
	/// </summary>
	public interface ILiteObjectService
	{
		/// <summary>
		/// Returns type object that describes lite object with public properties
		/// from parent object.
		/// </summary>
		/// <param name="parentObject">Parent object for lite object.</param>
		/// <param name="top">If TRUE, lite object type contains properties that belong
		/// exactly to the type of parentObject. Otherwise, lite object contains all read-write
		/// properties of parent object.</param>
		/// <returns>Type of lite object. Could be used for object creation.</returns>
		Type GetLiteObjectType(object parentObject, bool top = true);

		/// <summary>
		/// Returns instance of the lite object with public properties values
		/// that are taken from parent object.
		/// </summary>
		/// <param name="parentObject">Parent object for lite object.</param>
		/// <param name="top">If TRUE, lite object type contains properties that belong
		/// exactly to the type of parentObject. Otherwise, lite object contains all read-write
		/// properties of parent object.</param>
		/// <returns>Instance of the lite object. Could be used for serialization.</returns>
		object GetLiteObject(object parentObject, bool top = true);

		/// <summary>
		/// Copies values of public properties from the lite object
		/// to the correspond public properties of parent object.
		/// </summary>
		/// <param name="parentObject">Parent object for lite object.</param>
		/// <param name="liteObject">Lite object with values, that should be copied to 
		/// parent object.</param>
		/// <param name="top">If TRUE, lite object type contains properties that belong
		/// exactly to the type of parentObject. Otherwise, lite object contains all read-write
		/// properties of parent object.</param>
		/// <returns>Parent object with updated values.</returns>
		object CopyLiteObjectValues(object parentObject, object liteObject, bool top = true);
	}
}
