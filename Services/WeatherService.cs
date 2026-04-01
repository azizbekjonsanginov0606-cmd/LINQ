using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace YourNamespace.Services
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;

        public WeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetWeatherAsync(string city)
        {
            var response = await _httpClient.GetAsync($"https://api.weatherapi.com/v1/current.json?key=YOUR_API_KEY&q={city}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}