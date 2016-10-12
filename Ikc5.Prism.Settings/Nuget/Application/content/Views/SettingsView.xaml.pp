<UserControl x:Class="$rootnamespace$.Views.SettingsView"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:prism="http://prismlibrary.com/"
             xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
             xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
             mc:Ignorable="d"
             xmlns:xctk="http://schemas.xceed.com/wpf/xaml/toolkit"
             xmlns:viewModels="clr-namespace:$rootnamespace$.ViewModels"
             prism:ViewModelLocator.AutoWireViewModel="True">

    <UserControl.Resources>
    </UserControl.Resources>

    <Grid d:DataContext="{d:DesignInstance Type=viewModels:DesignSettingsViewModel}">

        <Grid.ColumnDefinitions>
            <ColumnDefinition Width="Auto"/>
            <ColumnDefinition Width="120"/>
        </Grid.ColumnDefinitions>

        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
        </Grid.RowDefinitions>

       </Grid>
</UserControl>
