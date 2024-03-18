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
            LabelSnow.Content = weatherData.Snow != null && weatherData.Snow._3h > 0 ? "Czy pada śnieg: Tak" : "Czy pada śnieg: Nie";
            LabelRain.Content = weatherData.Rain != null && weatherData.Rain._1h > 0 ? "Czy pada deszcz: Tak" : "Czy pada deszcz: Nie";
            LabelCity.Content = $"Miasto: {weatherData.Name}"; // Update the city name label
        }


        private void BingMap_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            // Ogranicz poziom zoomu do 2.5, aby umożliwić zoomowanie do tego poziomu
            if (bingMap.ZoomLevel >= 2.5 && e.Delta < 0)
            {
                // Pozwól na zoomowanie w głąb, jeśli poziom zoomu jest większy lub równy 2.5
            }
            else if (bingMap.ZoomLevel <= 1 && e.Delta > 0)
            {
                e.Handled = true; // Ignoruj zoomowanie na zewnątrz, jeśli poziom zoomu jest mniejszy lub równy 1
            }
        }




        private async Task<WeatherData> GetWeatherDataAsync(double latitude, double longitude)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var response = await httpClient.GetAsync($"{ApiUrl}?lat={latitude}&lon={longitude}&appid={ApiKey}&units=metric");
                    response.EnsureSuccessStatusCode();
                    var responseString = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<WeatherData>(responseString);
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    return null;
                }
            }
        }

    }
}

