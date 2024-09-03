using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using WinPrayersApp.Contracts.Services;
using WinPrayersApp.Models;

namespace WinPrayersApp.ViewModels;

public class MainViewModel : ObservableObject
{
    private readonly ISharedDataService _sharedDataService;
    private Location _location;
    private PrayerTimes _prayerTimes;

    public MainViewModel(ISharedDataService sharedDataService)
    {
        _sharedDataService = sharedDataService;
        _sharedDataService.PropertyChanged += OnSharedDataChanged;

        // Initialize properties based on current shared data
        _location = _sharedDataService.CurrentLocation;
        _prayerTimes = _sharedDataService.CurrentPrayerTimes;

        UpdatePropertiesFromSharedData();
    }

    public string Location
    {
        get => _location != null ? $"Latitude: {_location.Latitude}, Longitude: {_location.Longitude}" : "No Location Data";
        // Assuming Location is a string, don't set it directly; it's read-only
    }

    public string Fajr
    {
        get => _prayerTimes?.Fajr ?? "Not Available";
        set
        {
            if (_prayerTimes != null)
            {
                _prayerTimes.Fajr = value;
                OnPropertyChanged();
            }
        }
    }

    public string Dhuhr
    {
        get => _prayerTimes?.Dhuhr ?? "Not Available";
        set
        {
            if (_prayerTimes != null)
            {
                _prayerTimes.Dhuhr = value;
                OnPropertyChanged();
            }
        }
    }

    public string Asr
    {
        get => _prayerTimes?.Asr ?? "Not Available";
        set
        {
            if (_prayerTimes != null)
            {
                _prayerTimes.Asr = value;
                OnPropertyChanged();
            }
        }
    }

    public string Maghrib
    {
        get => _prayerTimes?.Maghrib ?? "Not Available";
        set
        {
            if (_prayerTimes != null)
            {
                _prayerTimes.Maghrib = value;
                OnPropertyChanged();
            }
        }
    }

    public string Isha
    {
        get => _prayerTimes?.Isha ?? "Not Available";
        set
        {
            if (_prayerTimes != null)
            {
                _prayerTimes.Isha = value;
                OnPropertyChanged();
            }
        }
    }

    private void OnSharedDataChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ISharedDataService.CurrentLocation))
        {
            _location = _sharedDataService.CurrentLocation;
            OnPropertyChanged(nameof(Location));
        }
        else if (e.PropertyName == nameof(ISharedDataService.CurrentPrayerTimes))
        {
            _prayerTimes = _sharedDataService.CurrentPrayerTimes;
            UpdatePropertiesFromSharedData();
        }
    }

    private void UpdatePropertiesFromSharedData()
    {
        if (_prayerTimes != null)
        {
            Fajr = _prayerTimes.Fajr;
            Dhuhr = _prayerTimes.Dhuhr;
            Asr = _prayerTimes.Asr;
            Maghrib = _prayerTimes.Maghrib;
            Isha = _prayerTimes.Isha;
        }
    }
}
