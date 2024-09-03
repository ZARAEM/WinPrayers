using System.ComponentModel;
using WinPrayersApp.Contracts.Services;
using WinPrayersApp.Models;

namespace WinPrayersApp.Services;

public class SharedDataService : ISharedDataService
{
    private PrayerTimes _currentPrayerTimes;
    private Location _currentLocation;

    public event PropertyChangedEventHandler PropertyChanged;

    public PrayerTimes CurrentPrayerTimes
    {
        get => _currentPrayerTimes;
        set
        {
            if (_currentPrayerTimes != value)
            {
                _currentPrayerTimes = value;
                OnPropertyChanged(nameof(CurrentPrayerTimes));
            }
        }
    }

    public Location CurrentLocation
    {
        get => _currentLocation;
        set
        {
            if (_currentLocation != value)
            {
                _currentLocation = value;
                OnPropertyChanged(nameof(CurrentLocation));
            }
        }
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
