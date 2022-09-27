using System.Collections.Generic;

namespace back_end.DTOs
{
    public class PeliculasPutGetDTO
    {
        public PeliculaDTO peliculaDTO { get; set; }
        public List<GeneroDTO> generosSeleccionados { get; set; }
        public List<GeneroDTO> generosNoSeleccionados { get; set; }
        public List<CineDTO> cineSeleccionados { get; set; }
        public List<CineDTO> cineNoSeleccionado { get; set; }
        public List<PeliculaActorDTO> actores { get; set; }
    }
}
