using System.ComponentModel.DataAnnotations;

namespace back_end.DTOs
{
    public class CredencialesUsuario
    {
        [EmailAddress]
        [Required]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
    }
}
