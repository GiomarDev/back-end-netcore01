using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Http;

namespace back_end.DTOs
{
    public class ActorCreacionDTO
    {
        [Required]
        [StringLength(maximumLength: 200)]
        public string nombre { get; set; }
        public string biografia { get; set; }
        public DateTime fechaNacimiento { get; set; }
        public IFormFile foto { get; set; }
    }
}
