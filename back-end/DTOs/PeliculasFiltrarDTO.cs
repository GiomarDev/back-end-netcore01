﻿namespace back_end.DTOs
{
    public class PeliculasFiltrarDTO
    {
        public int pagina { get; set; }
        public int recordsPorPagina { get; set; }
        public PaginacionDTO paginacionDTO 
        { 
            get { return new PaginacionDTO() 
            { 
                pagina = pagina, recordsPorPorPagina = recordsPorPagina 
            }; } 
        }
        public string titulo { get; set; }
        public int generoId { get; set; }
        public bool enCines { get; set; }
        public bool proximosEstrenos { get; set; }
    }
}
