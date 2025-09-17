namespace koninkrijk.Server.Models
{
    public class Log
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public string Message { get; set; }

        public Log() { }

        public Log(string message) { Message = message; }
    }
}
