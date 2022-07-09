using System.ComponentModel.DataAnnotations;

namespace OnlyThrals.Model
{
    public class Thrall
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public string? Icon { get; set; }
        public string? TwitchUserName { get; set; }
        public string? TwitterUserName { get; set; }
        public string? Description { get; set; }
        public bool? IsLive { get; set; }
        public DateTime? LastUpdated { get; set; }
    }
}
