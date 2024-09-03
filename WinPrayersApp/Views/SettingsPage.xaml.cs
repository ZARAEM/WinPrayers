using Microsoft.UI.Xaml.Controls;
using WinPrayersApp.ViewModels;

namespace WinPrayersApp.Views;

public sealed partial class SettingsPage : Page
{
    public SettingsViewModel ViewModel
    {
        get;
    }

    public SettingsPage()
    {
        ViewModel = App.GetService<SettingsViewModel>();
        InitializeComponent();
        DataContext = ViewModel;
    }
}
