using System.ComponentModel;
using WinPrayersApp.Services;
using WinPrayersApp.Models;

namespace WinPrayersApp.Contracts.Services;

public interface ISharedDataService : INotifyPropertyChanged
{
    PrayerTimes CurrentPrayerTimes
    {
        get; set;
    }
    Location CurrentLocation
    {
        get; set;
    }
}
