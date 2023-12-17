using System;
using Xamarin.Forms;
using System.ComponentModel;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Dynamic;
using System.Text.Json.Nodes;

namespace API_NASA
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        static private Random gen = new Random();

        private string _imageUrl = string.Empty;
        public string ImageUrl
        {
            get => _imageUrl;
            set
            {
                _imageUrl = value;
                OnPropertyChanged(nameof(ImageUrl));
            }
        }

        DateTime RandomDay()
        {
            DateTime start = new DateTime(1995, 6, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(gen.Next(range));
        }

        private Button[] buttons;

        private string[] titles = new string[]
        {
            "Geminids over China's Nianhu Lake",
            "Kemble's Cascade",
            "Annular Eclipse: The Ring of Fire",
            "Saturn's Hyperion: A Moon with Odd Craters",
            "Jupiter Eclipsing Ganymede",
            "A Partial Eclipse Over Manila Bay",
            "Grand Spiral Galaxy NGC 1232",
            "Halloween and the Ghost Head Nebula",
            "Plane, Clouds, Moon, Spots, Sun",
            "Halloween and the Wizard Nebula",
            "PDS 70: Disk, Planets, and Moons",
            "Messier Craters in Stereo",
            "M16: Pillars of Star Creation",
            "NGC 1365: Majestic Island Universe",
            "Recycling Cassiopeia A",
            "Asteroids in the Distance",
            "Blue Straggler Stars in Globular Cluster M53",
            "Pillars of the Eagle Nebula in Infrared",
            "The Medusa Nebula",
            "The Pencil Nebula Supernova Shock Wave",
            "Veil Nebula: Wisps of an Exploded Star"
        };

        public MainPage()
        {
            InitializeComponent();

            BindingContext = this;

            // set buttons
            buttons = new Button[]
            {
                butt1, butt2, butt3, butt4
            };

        }

        public async void UpdateImage(object sender, EventArgs args)
        {
            var buttonIndex = gen.Next(buttons.Length);
            var btn = buttons[buttonIndex];

            for (int i = 0; i < buttons.Length; i++)
            {
                var button = buttons[i];
                var TitleIndex = gen.Next(titles.Length);

                button.Text = titles[TitleIndex];
                button.BackgroundColor = default;
                button.Clicked += (s, a) =>
                {
                    for (int j = 0; j < buttons.Length; j++)
                    {
                        if (j == buttonIndex)
                            buttons[j].BackgroundColor = Color.Green;
                        else
                            buttons[j].BackgroundColor = Color.Red;
                    }
                };
            }

            string ApiKey = "DEMO_KEY";
            DateTime date = RandomDay();
            string ApiUrl = $"https://api.nasa.gov/planetary/apod?api_key={ApiKey}&date={date:yyyy-MM-dd}";

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(ApiUrl);
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<JsonNode>(json);

                ImageUrl = data["url"].GetValue<string>();
                var title = data["title"].GetValue<string>();

                btn.Text = title;
            }
        }
    }
}
