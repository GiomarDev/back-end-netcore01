using System.ComponentModel.DataAnnotations;

namespace back_end.Entidades
{
    public class PeliculasActores
    {
        public int peliculaID { get; set; }
        public int actorID { get; set; }
        public Pelicula pelicula { get; set; }
        public Actor actor { get; set; }
        [StringLength(maximumLength: 100)]
        public string personaje { get; set; }
        public int orden { get; set; }
    }
}
