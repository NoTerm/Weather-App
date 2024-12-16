using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace SimpleWindowApp
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Luo uusi ikkuna (Form)
            Form mainWindow = new Form
            {
                Text = "Sääikkuna",
                Width = 500,
                Height = 400,
                BackColor = Color.LightBlue // Taustaväri
            };

            // Otsikko
            Label titleLabel = new Label
            {
                Text = "Sää Oulussa",
                Font = new Font("Arial", 20, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                AutoSize = true,
                Location = new Point(180, 20)
            };


            // Säätieto-teksti
            Label weatherLabel = new Label
            {
                Text = "Ladataan säätietoja...",
                Font = new Font("Arial", 14),
                ForeColor = Color.Black,
                AutoSize = true,
                Location = new Point(50, 100)
            };

            // Päivitä-painike
            Button refreshButton = new Button
            {
                Text = "Päivitä sää",
                Font = new Font("Arial", 12),
                Location = new Point(200, 300),
                AutoSize = true
            };

            refreshButton.Click += (sender, e) => FetchWeatherAsync(weatherLabel);

            // Lisää komponentit ikkunaan
            mainWindow.Controls.Add(titleLabel);
            mainWindow.Controls.Add(weatherLabel);
            mainWindow.Controls.Add(refreshButton);

            // Hae säätiedot aluksi
            FetchWeatherAsync(weatherLabel);

            // Näytä ikkuna
            Application.Run(mainWindow);
        }

        static async void FetchWeatherAsync(Label label)
        {
            string apiKey = "3565e04d4cb0319af55cee18152d27df"; 
            string city = "Oulu";
            string apiUrl = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric";



            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string response = await client.GetStringAsync(apiUrl);
                    dynamic weatherData = Newtonsoft.Json.JsonConvert.DeserializeObject(response);

                    string description = weatherData.weather[0].description;
                    double temp = weatherData.main.temp;

                    label.Text = $"Sää: {description}, Lämpötila: {temp} °C";
                }
            }
            catch (Exception ex)
            {
                label.Text = $"Virhe haettaessa säätietoja: {ex.Message}";
            }
        }
    }
}
