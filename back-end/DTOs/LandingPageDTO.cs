using System.Collections.Generic;

namespace back_end.DTOs
{
    public class LandingPageDTO
    {
        public List<PeliculaDTO> enCines { get; set; }
        public List<PeliculaDTO> proximosEstrenos { get; set; }
    }
}
