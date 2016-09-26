using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace Ikc5.TypeLibrary
{
	/// <summary>
	/// Some used links - for references
	/// http://stackoverflow.com/questions/3740532/how-to-use-expression-to-build-an-anonymous-type
	/// https://msdn.microsoft.com/en-us/library/2sd82fz7(v=vs.110).aspx
	/// </summary>
	public class LiteObjectService : ILiteObjectService
	{
		private readonly AssemblyName _assemblyName = new AssemblyName() { Name = "LiteObjectTypes" };
		private readonly ModuleBuilder _moduleBuilder = null;
		private readonly IDictionary<Tuple<string, bool>, Type> _builtTypes = new Dictionary<Tuple<string, bool>, Type>(3);

		public LiteObjectService()
		{
			var assemblyBuilder = Thread.GetDomain()
				.DefineDynamicAssembly(_assemblyName, AssemblyBuilderAccess.Run);
			_moduleBuilder = assemblyBuilder.DefineDynamicModule(_assemblyName.Name);
		}

		/// <summary>
		/// Returns type object that describes lite object with public properties
		/// from parent object.
		/// </summary>
		/// <param name="parentType">Type of parent object.</param>
		/// <param name="top">If TRUE, lite object type contains properties that belong
		/// exactly to type of parentObject. Otherwise, lite object contains all read-write
		/// properties of parent object.</param>
		/// <returns>Type of lite object. Could be used for object creation.</returns>
		private Type GetLiteType(Type parentType, bool top = true)
		{
			if (parentType == null)
				return null;

			var typeKey = new Tuple<string, bool>(parentType.FullName, top);
			lock (_builtTypes)
			{
				// could be  different classes with the same name, so use full name of origin class
				if (_builtTypes.ContainsKey(typeKey))
					return _builtTypes[typeKey];

				var className = $"{parentType.Namespace}.Lite{parentType.Name}" + (top ? "Top" : "All");
				var typeBuilder = _moduleBuilder.DefineType(className,
					TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.Serializable,
					typeof(LiteObjectBase));

				// get property of top level hierarchy class 
				var propertyInfos =
					parentType
						.GetProperties(BindingFlags.Public | BindingFlags.Instance)
						.Where(property => ((top && property.DeclaringType == parentType) || !top) && (property.CanRead && property.CanWrite));

				// create the constructor
				var constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[0]);
				var constructorIL = constructorBuilder.GetILGenerator();
				constructorIL.Emit(OpCodes.Ldarg_0);
				var superConstructor = typeof(LiteObjectBase).GetConstructor(new Type[0]);
				if (superConstructor != null)
					constructorIL.Emit(OpCodes.Call, superConstructor);

				foreach (var propertyInfo in propertyInfos)
				{
					// create private field and init it by default value from DefaultValue attribute
					var fieldName = $"_{propertyInfo.Name.ToLower()[0]}";
					if (propertyInfo.Name.Length > 1)
						fieldName += propertyInfo.Name.Substring(1);
					var fieldBuilder = typeBuilder.DefineField(fieldName, propertyInfo.PropertyType, FieldAttributes.Private);

					var propertyBuilder = typeBuilder.DefineProperty(
											propertyInfo.Name, propertyInfo.Attributes | PropertyAttributes.HasDefault,
											propertyInfo.PropertyType, null);

					// the property set and property get methods require a special set of attributes.
					const MethodAttributes accessorAttributes =
						MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;

					// define the "get" accessor method for CustomerName.
					var getterMethod = typeBuilder.DefineMethod($"get_{propertyInfo.Name}",
											accessorAttributes,
											propertyInfo.PropertyType,
											Type.EmptyTypes);

					var getterMethodIL = getterMethod.GetILGenerator();

					getterMethodIL.Emit(OpCodes.Ldarg_0);
					getterMethodIL.Emit(OpCodes.Ldfld, fieldBuilder);
					getterMethodIL.Emit(OpCodes.Ret);

					// Define the "set" accessor method for CustomerName.
					var setterMethod = typeBuilder.DefineMethod($"set_{propertyInfo.Name}",
											accessorAttributes,
											null,
											new Type[] { propertyInfo.PropertyType });

					var setterMethodIL = setterMethod.GetILGenerator();

					setterMethodIL.Emit(OpCodes.Ldarg_0);
					setterMethodIL.Emit(OpCodes.Ldarg_1);
					setterMethodIL.Emit(OpCodes.Stfld, fieldBuilder);
					setterMethodIL.Emit(OpCodes.Ret);

					// Last, we must map the two methods created above to our PropertyBuilder to 
					// their corresponding behaviors, "get" and "set" respectively. 
					propertyBuilder.SetGetMethod(getterMethod);
					propertyBuilder.SetSetMethod(setterMethod);

					foreach (var customAttribute in propertyInfo.CustomAttributes)
					{
						var builder = new CustomAttributeBuilder(
											customAttribute.Constructor,
											customAttribute.ConstructorArguments.Select(arguments => arguments.Value).ToArray());
						propertyBuilder.SetCustomAttribute(builder);
					}
				}

				constructorIL.Emit(OpCodes.Ret);
				_builtTypes[typeKey] = typeBuilder.CreateType();

				return _builtTypes[typeKey];
			}
		}

		#region ILiteObjectService

		/// <summary>
		/// Returns type object that describes lite object with public properties
		/// from parent object.
		/// </summary>
		/// <param name="parentObject">Parent object for lite object.</param>
		/// <param name="top">If TRUE, lite object type contains properties that belong
		/// exactly to type of parentObject. Otherwise, lite object contains all read-write
		/// properties of parent object.</param>
		/// <returns>Type of lite object. Could be used for object creation.</returns>
		public Type GetLiteObjectType(object parentObject, bool top = true)
		{
			if (parentObject == null)
				return null;
			return GetLiteType(parentObject.GetType(), top);
		}

		/// <summary>
		/// Returns instance of lite object that contains public properties
		/// from parent object.
		/// </summary>
		/// <param name="parentObject">Parent object for lite object.</param>
		/// <param name="top">If TRUE, lite object type contains properties that belong
		/// exactly to type of parentObject. Otherwise, lite object contains all read-write
		/// properties of parent object.</param>
		/// <returns>Instance of the lite object. Could be used for serialization.</returns>
		public object GetLiteObject(object parentObject, bool top = true)
		{
			if (parentObject == null)
				return null;

			var liteObjectType = GetLiteType(parentObject.GetType(), top);
			if (liteObjectType == null)
				return null;

			var liteObject = Activator.CreateInstance(liteObjectType);
			if (liteObject == null)
				return null;

			parentObject.CopyValuesTo(liteObject, top);
			return liteObject;
		}

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
		public object CopyLiteObjectValues(object parentObject, object liteObject, bool top = true)
		{
			if (parentObject == null || liteObject == null)
				return null;

			var liteObjectType = GetLiteType(parentObject.GetType(), top);
			if (liteObjectType == null || liteObjectType != liteObject.GetType())
				return null;

			return parentObject.CopyValuesFrom(liteObject, top);
		}

		#endregion
	}
}
