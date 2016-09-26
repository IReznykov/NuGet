using System;

namespace Ikc5.TypeLibrary
{
	public interface ILiteObjectService
	{
		/// <summary>
		/// Returns type object that describes lite object with public properties
		/// from parent object.
		/// </summary>
		/// <param name="parentObject">Parent object for lite object.</param>
		/// <param name="top">If TRUE, lite object type contains properties that belong
		/// exactly to type of parentObject. Otherwise, lite object contains all read-write
		/// properties of parent object.</param>
		/// <returns>Type of lite object. Could be used for object creation.</returns>
		Type GetLiteObjectType(object parentObject, bool top = true);

		/// <summary>
		/// Returns instance of lite object that contains public properties
		/// from parent object.
		/// </summary>
		/// <param name="parentObject">Parent object for lite object.</param>
		/// <param name="top">If TRUE, lite object type contains properties that belong
		/// exactly to type of parentObject. Otherwise, lite object contains all read-write
		/// properties of parent object.</param>
		/// <returns>Instance of the lite object. Could be used for serialization.</returns>
		object GetLiteObject(object parentObject, bool top = true);

		/// <summary>
		/// Copies values of properties from lite object that contains public properties
		/// from parent object.
		/// </summary>
		/// <param name="parentObject">Parent object for lite object.</param>
		/// <param name="liteObject">Lite object with values, that should be copied to 
		/// parent object.</param>
		/// <param name="top">If TRUE, lite object type contains properties that belong
		/// exactly to type of parentObject. Otherwise, lite object contains all read-write
		/// properties of parent object.</param>
		/// <returns>Parent object with updated values.</returns>
		object CopyLiteObjectValues(object parentObject, object liteObject, bool top = true);
	}
}
