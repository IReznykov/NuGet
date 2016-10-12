---------------------------------------------------
------------ Ikc5.Prism.Settings.Module -----------
---------------------------------------------------

This package is intended to be used in WPF Prism module.
It requires the following changes in $ProjectName$Module.cs:

public class $ProjectName$Module : IModule
{
	private readonly IRegionManager _regionManager;

	private readonly IUnityContainer _container;

	public $ProjectName$Module(IRegionManager regionManager, IUnityContainer container)
	{
		_regionManager = regionManager;
		_container = container;
		ConfigureContainer();
	}

	private void ConfigureContainer()
	{
		_container.RegisterType<ISettings, Models.Settings>(new ContainerControlledLifetimeManager());
	}

	public void Initialize()
	{
		_regionManager.RegisterViewWithRegion($"{GetType().Name}{RegionNames.ModuleSettingsRegion}", typeof(SettingsView));
	}
}

In addition, the following files are added to the project:

+Model
	- ISettings.cs
		// empty interface for application-specific
		// settings
	- Settings.cs
		// empty class bases on library's classes, could
		// contains properties in the form:
		//private object _example;
		// /// <summary>
		// /// Example.
		// /// </summary>
		//[DefaultValue("Value")]
		//public object Example
		//{
		//	get { return _example; }
		//	set { SetProperty(ref _example, value); }
		//}
+ViewModels
	- DesignSettingsViewModel.cs
		// class for view model that is used in SettingView at design time.
		// could contins auto-properties with default implementation.
	- ISettingsViewModel.cs
		// interface is ready for use, as contains the same properties as ISettings
	- SettingsViewModel.cs
		// class bases on library's classes, could contains properties in the form:
		//private object _example;
		// /// <summary>
		// /// Example.
		// /// </summary>
		//public object Example
		//{
		//	get { return _example; }
		//	set { SetProperty(ref _example, value); }
		//}
+Views
	- SettingsView.xaml
		// usercontrol that should provide some GUI elements for settings
	- SettingsView.xaml.cs
		// default implementation, is ready for use

