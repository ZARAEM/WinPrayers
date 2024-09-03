using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using WinPrayersApp.Models;
namespace WinPrayersApp.Services;

public class PrayerTimesService
{
    private readonly HttpClient _httpClient;

    public PrayerTimesService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PrayerTimes> GetPrayerTimesAsync(double latitude, double longitude, int method)
    {
        try
        {
            var baseUrl = "http://api.aladhan.com/v1/timings";
            var date = DateTime.UtcNow.ToString("yyyy-MM-dd");
            var url = $"{baseUrl}/{date}?latitude={latitude}&longitude={longitude}&method={method}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            JObject jsonResponse = JObject.Parse(responseBody);

            var prayerTimes = jsonResponse["data"]["timings"];
            return new PrayerTimes
            {
                Fajr = prayerTimes["Fajr"]?.ToString(),
                Dhuhr = prayerTimes["Dhuhr"]?.ToString(),
                Asr = prayerTimes["Asr"]?.ToString(),
                Maghrib = prayerTimes["Maghrib"]?.ToString(),
                Isha = prayerTimes["Isha"]?.ToString()
            };
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error fetching prayer times: {ex.Message}");
            return null;
        }
    }
}