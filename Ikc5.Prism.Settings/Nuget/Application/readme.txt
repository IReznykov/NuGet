---------------------------------------------------
--------- Ikc5.Prism.Settings.Application ---------
---------------------------------------------------

This package is intended to be used in WPF Prism application.
It requires the following changes in Bootstrapper.cs:

Method ConfigureContainer():

	Container
		.RegisterType<IUserSettingsService, UserSettingsService>(new ContainerControlledLifetimeManager())
		.RegisterType<ILiteObjectService, LiteObjectService>(new ContainerControlledLifetimeManager())
		.RegisterType<ISettings, Settings>(new ContainerControlledLifetimeManager());

	Container.RegisterType(
		typeof(IUserSettingsProvider<>),
		typeof(LocalXmlUserSettingsProvider<>),
		new ContainerControlledLifetimeManager());

Method InitializeShell():

	// add some views to region adapter
	var regionManager = Container.Resolve<IRegionManager>();
	regionManager.RegisterViewWithRegion(RegionNames.AppSettingsRegion, typeof(SettingsView));

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
	- SettingsWindow.xaml
		// window that shows settings views from application and all modules,
		// is ready for use
	- SettingsWindow.xaml.cs
		// implementation that allows serialize settings, is ready for use

