using Microsoft.UI.Xaml.Controls;
using WinPrayersApp.ViewModels;

namespace WinPrayersApp.Views;

public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel
    {
        get;
    }

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        InitializeComponent();

        DataContext = ViewModel;
    }
}
