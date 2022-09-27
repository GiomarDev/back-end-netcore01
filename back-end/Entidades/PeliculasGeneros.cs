namespace back_end.Entidades
{
    public class PeliculasGeneros
    {
        public int peliculaID { get; set; }
        public int generoID { get; set; }
        public Pelicula pelicula { get; set; }
        public Genero genero { get; set; }
    }
}
