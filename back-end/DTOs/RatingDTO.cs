using System.ComponentModel.DataAnnotations;

namespace back_end.DTOs
{
    public class RatingDTO
    {
        public int peliculaID { get; set; }
        [Range(1, 5)]
        public int puntuacion { get; set; }
    }
}
