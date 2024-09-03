using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WinPrayersApp.Contracts.Services;
using Windows.ApplicationModel;
using WinPrayersApp.Helpers;
using WinPrayersApp.Models;
using WinPrayersApp.Services;

namespace WinPrayersApp.ViewModels;

public partial class SettingsViewModel : ObservableRecipient
{
    private readonly IThemeSelectorService _themeSelectorService;
    private readonly INavigationService _navigationService;
    private readonly PrayerTimesService _prayerTimesService;
    private readonly LocationService _locationService;
    private readonly ISharedDataService _sharedDataService;

    private string _selectedBackdrop;
    private bool _isAcrylicChecked;
    private bool _isMicaChecked;
    private bool _isDefaultChecked;

    private Window _window;

    [ObservableProperty]
    private ElementTheme _elementTheme;

    [ObservableProperty]
    private string _versionDescription;

    [ObservableProperty]
    private string _locationMessage;

    public ICommand SwitchThemeCommand
    {
        get;
    }

    public string SelectedBackdrop
    {
        get => _selectedBackdrop;
        set
        {
            SetProperty(ref _selectedBackdrop, value);
            SetBackdrop(value);
        }
    }

    public ICommand SetBackdropCommand
    {
        get;
    }
    public ICommand FetchLocationCommand
    {
        get;
    }

    public SettingsViewModel(IThemeSelectorService themeSelectorService, INavigationService navigationService, LocationService locationService, PrayerTimesService prayerTimesService, ISharedDataService sharedDataService)
    {
        _themeSelectorService = themeSelectorService;
        _navigationService = navigationService;
        _locationService = locationService;
        _prayerTimesService = prayerTimesService;
        _sharedDataService = sharedDataService;
        _window = App.MainWindow;

        _elementTheme = _themeSelectorService.Theme;
        _versionDescription = GetVersionDescription();

        SwitchThemeCommand = new RelayCommand<ElementTheme>(async param =>
        {
            if (ElementTheme != param)
            {
                ElementTheme = param;
                await _themeSelectorService.SetThemeAsync(param);
            }
        });

        SetBackdropCommand = new RelayCommand<string>(SetBackdrop);

        _selectedBackdrop = "Default";

        UpdateBackdropSelection(SelectedBackdrop);

        FetchLocationCommand = new RelayCommand(async () =>
        {
            await FetchLocationAndUpdateMainViewModelAsync();
        });
    }

    private void UpdateBackdropSelection(string backdrop)
    {
        IsAcrylicChecked = backdrop == "Acrylic";
        IsMicaChecked = backdrop == "Mica";
        IsDefaultChecked = backdrop == "Default";
    }

    private static string GetVersionDescription()
    {
        Version version;

        if (RuntimeHelper.IsMSIX)
        {
            var packageVersion = Package.Current.Id.Version;
            version = new Version(packageVersion.Major, packageVersion.Minor, packageVersion.Build, packageVersion.Revision);
        }
        else
        {
            version = Assembly.GetExecutingAssembly().GetName().Version!;
        }

        return $"{"AppDisplayName".GetLocalized()} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
    }

    private async Task FetchLocationAndUpdateMainViewModelAsync()
    {
        try
        {
            var location = await _locationService.GetLocationAsync();
            var latitude = location.Latitude;
            var longitude = location.Longitude;

            var prayerTimes = await _prayerTimesService.GetPrayerTimesAsync(latitude, longitude, 2);

            // Create a Location object to pass to the shared data service
            var locationModel = new Location
            {
                Latitude = latitude,
                Longitude = longitude
            };

            // Update the shared data service with the new location and prayer times
            _sharedDataService.CurrentLocation = locationModel;
            _sharedDataService.CurrentPrayerTimes = prayerTimes;

            LocationMessage = $"Latitude: {latitude}, Longitude: {longitude}";

            await Task.Delay(1000);
            LocationMessage = string.Empty;

            // Navigate to the main page
            _navigationService.NavigateTo("WinPrayersApp.ViewModels.MainViewModel");
        }
        catch (Exception ex)
        {
            LocationMessage = "Error retrieving location.";
            Debug.WriteLine($"Error fetching location or prayer times: {ex.Message}");
        }
    }

    public void SetBackdrop(string backdropType)
    {
        switch (backdropType)
        {
            case "Acrylic":
                trySetAcrylicBackdrop();
                break;
            case "Mica":
                trySetMicaBackdrop();
                break;
            case "Default":
                resetBackdrop();
                break;
            default:
                break;
        }
    }

    private void trySetAcrylicBackdrop()
    {
        Microsoft.UI.Xaml.Media.DesktopAcrylicBackdrop desktopAcrylicBackdrop = new Microsoft.UI.Xaml.Media.DesktopAcrylicBackdrop();

        _window.SystemBackdrop = desktopAcrylicBackdrop;
    }

    private void trySetMicaBackdrop()
    {
        Microsoft.UI.Xaml.Media.MicaBackdrop micaBackdrop = new Microsoft.UI.Xaml.Media.MicaBackdrop();
        micaBackdrop.Kind = Microsoft.UI.Composition.SystemBackdrops.MicaKind.Base;

        _window.SystemBackdrop = micaBackdrop;
    }

    private void resetBackdrop()
    {
        _window.SystemBackdrop = null;
    }

    public bool IsAcrylicChecked
    {
        get => _isAcrylicChecked;
        set
        {
            SetProperty(ref _isAcrylicChecked, value);
            if (value) SetBackdrop("Acrylic");
        }
    }

    public bool IsMicaChecked
    {
        get => _isMicaChecked;
        set
        {
            SetProperty(ref _isMicaChecked, value);
            if (value) SetBackdrop("Mica");
        }
    }

    public bool IsDefaultChecked
    {
        get => _isDefaultChecked;
        set
        {
            SetProperty(ref _isDefaultChecked, value);
            if (value) SetBackdrop("Default");
        }
    }
}
