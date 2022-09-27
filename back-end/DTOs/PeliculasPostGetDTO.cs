using System.Collections.Generic;

namespace back_end.DTOs
{
    public class PeliculasPostGetDTO
    {
        public List<GeneroDTO> generos { get; set; }
        public List<CineDTO> cines { get; set; }
    }
}
