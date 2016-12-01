using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using Ikc5.Prism.Common.Logging;
using Ikc5.TypeLibrary;
using Prism.Logging;

namespace Ikc5.Prism.Settings.Providers
{
	/// <summary>
	/// Serialize and deserialize user settings from file in Personal application folder.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class PersonalXmlUserSettingsProvider<T> : LocalXmlUserSettingsProvider<T> where T : class, IUserSettings
	{
		public PersonalXmlUserSettingsProvider(ILiteObjectService liteObjectService, ILoggerFacade logger)
			: base(liteObjectService, logger)
		{
			var assembly = Assembly.GetEntryAssembly();
			var assemblyCompany = Attribute.GetCustomAttribute(assembly, typeof(AssemblyCompanyAttribute)) as AssemblyCompanyAttribute;
			var company = assemblyCompany?.Company ?? "IReznykov";
			var assemblyProduct = Attribute.GetCustomAttribute(assembly, typeof(AssemblyProductAttribute)) as AssemblyProductAttribute;
			var product = assemblyProduct?.Product ?? "Ikc5.Prism.Settings";

			FolderName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), company, product, "Settings");
		}

	}
}
