using System.ComponentModel.DataAnnotations;

namespace back_end.DTOs
{
    public class CineCreacionDTO
    {
        [Required]
        [StringLength(maximumLength: 75)]
        public string name { get; set; }
        [Range(-600, 600)]
        public double latitud { get; set; }
        [Range(-600, 600)]
        public double longitud { get; set; }
    }
}
