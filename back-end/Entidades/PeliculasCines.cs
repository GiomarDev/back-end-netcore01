namespace back_end.Entidades
{
    public class PeliculasCines
    {
        public int peliculaID { get; set; }
        public int cineID { get; set; }
        public Pelicula pelicula { get; set; }
        public Cine cine { get; set; }
    }
}
