
namespace koninkrijk.Server.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
        public DateTime LastCaptureTry { get; set; }
        
        public string ApiKey { get; set; } = Guid.NewGuid().ToString("N");

        // Foreign keys
        public int? ProvinceId { get; set; }
        public Province? Province { get; set; }

        public Player(string name) { Name = name; }

    }
}
