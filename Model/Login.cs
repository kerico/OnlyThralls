using System.ComponentModel.DataAnnotations;

namespace OnlyThrals.Model
{
    public class Login
    {
        [Required]
        [Display(Name ="Login")]
        public string? Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
