using System.Text.Json.Serialization;

namespace koninkrijk.Server.Models
{
    public class Province
    {
        private static readonly Random _random = new Random();

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ProvinceSize { get; set; }
        public DateTime LastCapture { get; set; }
        public string? CurrentPlayer
        {
            get
            {
                if (Player != null)
                {
                    return Player.Name;
                }
                else
                {
                    return null;
                }
            }
            set { }
        }

        // Foreign keys
        [JsonIgnore]
        public Player? Player { get; set; }

        public void Capture(Player player)
        {
            LastCapture = DateTime.Now;
            Player = player;
        }

        public int Score(DateTime dateTime)
        {
            int diff = (int)(dateTime - LastCapture).TotalMinutes;
            return diff * ProvinceSize;
        }

        public bool TryCapture()
        {
            int difficulty = ProvinceSize;

            double probability = 1.0 / difficulty;
            double rand = _random.NextDouble();
            bool success = rand < probability;

            System.Diagnostics.Trace.WriteLine("--------------------");
            System.Diagnostics.Trace.WriteLine($"TryCapture: ProvinceSize={ProvinceSize}, Difficulty={difficulty}, Probability={probability}, Rand={rand}, Success={success}");
            System.Diagnostics.Trace.WriteLine("--------------------");

            return success;
        }
    }
}
