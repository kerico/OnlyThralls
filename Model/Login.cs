using System.ComponentModel.DataAnnotations;

namespace OnlyThrals.Model
{
    public class Login
    {
        [Required]
        public string? Username { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
