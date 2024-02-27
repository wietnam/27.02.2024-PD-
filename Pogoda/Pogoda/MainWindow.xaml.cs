using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using Microsoft.Maps.MapControl.WPF;
using Pogoda;
using System.Windows.Input;

namespace Pogoda
{
    public partial class MainWindow : Window
    {
        private const string ApiKey = "8e574bad8dd20c31183f9537ef07c041";
        private const string ApiUrl = "http://api.openweathermap.org/data/2.5/weather";

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void BingMap_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var mousePosition = e.GetPosition(bingMap);
            var location = bingMap.ViewportPointToLocation(mousePosition);
            var weatherData = await GetWeatherDataAsync(location.Latitude, location.Longitude);
            LabelWeatherDescription.Content = $"Pogoda: {weatherData.Weather[0].Description}";
            LabelTemperature.Content = $"Temperatura: {weatherData.Main.Temp}°C";
            LabelFeelsLike.Content = $"Odczuwalna temperatura: {weatherData.Main.FeelsLike}°C";
            LabelHumidity.Content = $"Wilgotność: {weatherData.Main.Humidity}%";
        }

        private async Task<WeatherData> GetWeatherDataAsync(double latitude, double longitude)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetStringAsync($"{ApiUrl}?lat={latitude}&lon={longitude}&appid={ApiKey}&units=metric");
                return JsonConvert.DeserializeObject<WeatherData>(response);
            }
        }
    }
}

