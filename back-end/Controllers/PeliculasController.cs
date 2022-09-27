using AutoMapper;
using back_end.DTOs;
using back_end.Entidades;
using back_end.Utilidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace back_end.Controllers
{
    [ApiController]
    [Route("api/peliculas")]
    public class PeliculasController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IAlamacenadorArchivos alamacenadorArchivos;
        private readonly string contenedor = "peliculas";

        public PeliculasController(ApplicationDbContext context,
                                   IMapper mapper,
                                   IAlamacenadorArchivos alamacenadorArchivos)
        {
            this.context = context;
            this.mapper = mapper;
            this.alamacenadorArchivos = alamacenadorArchivos;
        }

        #region EndPoints
        
        [HttpPost]
        public async Task<ActionResult> Post([FromForm] PeliculaCreacionDTO peliculaCreacionDTO)
        {
            var pelicula = mapper.Map<Pelicula>(peliculaCreacionDTO);
            if (peliculaCreacionDTO.poster != null)
            {
                pelicula.poster = await alamacenadorArchivos.GuardarArchivo(contenedor, peliculaCreacionDTO.poster);
            }
            EscribirOrdenActores(pelicula);
            context.Add(pelicula);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("PostGet")]
        public async Task<ActionResult<PeliculasPostGetDTO>> PostGet()
        {
            var cines = await context.Cines.ToListAsync();
            var generos = await context.Generos.ToListAsync();

            var cinesDTO = mapper.Map<List<CineDTO>>(cines);
            var generoDTO = mapper.Map<List<GeneroDTO>>(generos);

            return new PeliculasPostGetDTO { cines = cinesDTO, generos = generoDTO };
        }

        #endregion

        #region Methods

        private void EscribirOrdenActores(Pelicula pelicula)
        {
            if (pelicula.peliculasActores != null)
            {
                for (int i = 0; i < pelicula.peliculasActores.Count; i++)
                {
                    pelicula.peliculasActores[i].orden = i;
                }
            }
        }

        #endregion

    }
}
