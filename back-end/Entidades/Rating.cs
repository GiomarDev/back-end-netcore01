using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace back_end.Entidades
{
    public class Rating
    {
        public int id { get; set; }
        [Range(1, 5)]
        public int puntuacion { get; set; }
        public int peliculaID { get; set; }
        public Pelicula pelicula { get; set; }
        public string userID { get; set; }
        public IdentityUser usuario { get; set; }
    }
}
