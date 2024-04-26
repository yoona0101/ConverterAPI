using KonverterAPI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KonverterAPI
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        static async Task Main(string I, string O, decimal res)
        {
            string baseCurrency = I; // Базовая валюта
            string targetCurrency = O; // Целевая валюта
            decimal amount = res; // Сумма для конвертации
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync($"https://api.exchangerate-api.com/v4/latest/{baseCurrency}");
                    response.EnsureSuccessStatusCode(); // Гарантирует, что ответ успешный
                    string responseBody = await response.Content.ReadAsStringAsync();
                    // Преобразуем ответ в объект
                    ExchangeRates exchangeRates = Newtonsoft.Json.JsonConvert.DeserializeObject<ExchangeRates>(responseBody);
                    // Получаем курс для целевой валюты
                    decimal exchangeRate = exchangeRates.Rates.First(r => r.Key == targetCurrency).Value;
                    // Вычисляем конвертированную сумму
                    decimal convertedAmount = amount * exchangeRate;
                    MessageBox.Show($"{amount} {baseCurrency} = {convertedAmount} {targetCurrency}");
                }
                catch (HttpRequestException e)
                {
                    MessageBox.Show($"Ошибка: {e.Message}");
                }
            }
        }
        // Класс для десериализации JSON-ответа
        public class ExchangeRates
        {
            public string Base { get; set; }
            public string Date { get; set; }
            public Dictionary<string, decimal> Rates { get; set; }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem typeItem = (ComboBoxItem)cbInput.SelectedItem;
            ComboBoxItem typeItem2 = (ComboBoxItem)cbOutput.SelectedItem;
            string str1 = typeItem.Content.ToString();
            string str2 = typeItem2.Content.ToString();
           
            await Main(str1, str2, decimal.Parse(res.Text));
           
        }
    }
}