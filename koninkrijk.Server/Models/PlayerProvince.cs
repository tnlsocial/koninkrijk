using System.Text.Json.Serialization;

namespace koninkrijk.Server.Models
{
    public class PlayerProvince
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public int ProvinceId { get; set; }
        public Player Player { get; set; }
        public Province Province { get; set; }

        public DateTime LastCaptureTry { get; set; }
        public string? CurrentFeedback { get; set; }

        [JsonIgnore]
        public string? CurrentWord { get; set; }
        public int Tries { get; set; }

        public bool CanGuess()
        {
            if (LastCaptureTry == null)
            {
                LastCaptureTry = DateTime.Now;
            }

            if (LastCaptureTry.DayOfYear < DateTime.Now.DayOfYear)
            {
                LastCaptureTry = DateTime.Now;
                Tries = 0;
                return true;
            }
            else
            {
                if (Tries >= Config.MaxTries)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

    }
}
