using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
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
using static System.Net.Mime.MediaTypeNames;

namespace _2023_01_30_DownloadWebPage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            // Регистрацию провайдера перенесли в App.xaml.cs
            // Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            InitializeComponent();
        }

        private async void buttonDownload_Click(object sender, RoutedEventArgs e)
        {
            // очистка используемых полей
            textBoxCode.Text = string.Empty;
            textBoxPage.Text = string.Empty;

            // проверка на пустую строку
            if (textBoxURL.Text.Length == 0 )
            {
                MessageBox.Show("Пустая строка адреса!");
                return;
            }

            // проверка на пустую строку
            if (!Uri.IsWellFormedUriString(textBoxURL.Text, UriKind.Absolute))
            {
                MessageBox.Show("Строка запроса не соответствует адресу!");
                return;
            }

            // Запрос получения данных страницы
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(textBoxURL.Text);
                    response.EnsureSuccessStatusCode();
                    textBoxPage.Text = await response.Content.ReadAsStringAsync();
                    // Above three lines can be replaced with new helper method below
                    // string response = await client.GetStringAsync(textBoxURL.Text);
                    textBoxCode.Text = ((int)response.StatusCode).ToString();
                }
            }
            // Обработка исключений от HttpClient
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"\nСловили ошибку!\nОшибка: {ex.Message} ");
            }
            // Обработка общих исключений
            catch (Exception ex)
            {
                MessageBox.Show($"\nСловили ошибку!\nОшибка: {ex.Message} ");
            }
        }

        private async void textBoxFileName_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxFileName.Text == string.Empty)
            {
                MessageBox.Show("Пустое имя файла!");
                return;
            }

            using (StreamWriter writer = File.CreateText(textBoxFileName.Text))
            {
                await writer.WriteAsync(textBoxPage.Text);
            }
        }
    }
}
