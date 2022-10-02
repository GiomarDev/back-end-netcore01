using AutoMapper;
using back_end.DTOs;
using back_end.Entidades;
using back_end.Utilidades;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Controllers
{
    [ApiController]
    [Route("api/peliculas")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PeliculasController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IAlamacenadorArchivos alamacenadorArchivos;
        private readonly UserManager<IdentityUser> userManager;
        private readonly string contenedor = "peliculas";

        public PeliculasController(ApplicationDbContext context,
                                   IMapper mapper,
                                   IAlamacenadorArchivos alamacenadorArchivos,
                                   UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.alamacenadorArchivos = alamacenadorArchivos;
            this.userManager = userManager;
        }

        #region EndPoints

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var pelicula = await context.Peliculas.FirstOrDefaultAsync(x => x.id == id);

            if (pelicula == null)
            {
                return NotFound();
            }

            context.Remove(pelicula);
            await context.SaveChangesAsync();
            await alamacenadorArchivos.BorrarArchivo(pelicula.poster, contenedor);
            return NoContent();
        }

        [HttpGet("PutGet/{id:int}")]
        public async Task<ActionResult<PeliculasPutGetDTO>> PutGet(int id)
        {
            var peliculaActionResult = await Get(id);
            if (peliculaActionResult.Result is NotFoundResult) { return NotFound(); }
            
            var pelicula = peliculaActionResult.Value;
            
            //Pimero arma el query
            var generosSeleccionadosIds = pelicula.generos.Select(x => x.id).ToList();
            //Ejecuta la accion del query
            var generosNoSeleccionados = await context.Generos.
                                         Where(x => !generosSeleccionadosIds.Contains(x.id)).
                                         ToListAsync();

            var cinesSeleccionadosIds = pelicula.cines.Select(x => x.id).ToList();
            var cinesNoSeleccionados = await context.Cines.
                                       Where(x => !cinesSeleccionadosIds.Contains(x.id)).
                                       ToListAsync();

            var generosNoSeleccionadosDTO = mapper.Map<List<GeneroDTO>>(generosNoSeleccionados);
            var cinesNoSeleccionadosDTO = mapper.Map<List<CineDTO>>(cinesNoSeleccionados);

            var respuesta = new PeliculasPutGetDTO();
            respuesta.peliculaDTO = pelicula;
            respuesta.generosSeleccionados = pelicula.generos;
            respuesta.generosNoSeleccionados = generosNoSeleccionadosDTO;
            respuesta.cineSeleccionados = pelicula.cines;
            respuesta.cineNoSeleccionado = cinesNoSeleccionadosDTO;
            respuesta.actores = pelicula.actores;

            return respuesta;
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromForm] PeliculaCreacionDTO peliculaCreacionDTO)
        {
            var pelicula = await context.Peliculas.
                           Include(x => x.peliculasActores).
                           Include(x => x.peliculasGeneros).
                           Include(x => x.peliculasCines).
                           FirstOrDefaultAsync(x => x.id == id);
            
            if(pelicula == null) { return NotFound(); }

            pelicula = mapper.Map(peliculaCreacionDTO, pelicula);

            if (peliculaCreacionDTO.poster != null) {
                pelicula.poster = await alamacenadorArchivos.EditarArchivo(contenedor, peliculaCreacionDTO.poster, pelicula.poster);
            }

            EscribirOrdenActores(pelicula);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<LandingPageDTO>> Get() 
        {
            var top = 6;
            var hoy = DateTime.Today;

            var proximosEstrenos = await context.Peliculas.Where(x => x.fechaLanzamiento > hoy)
                                   .OrderBy(x => x.fechaLanzamiento).
                                   Take(top).
                                   ToListAsync();
            var enCines = await context.Peliculas.Where(x => x.enCines).
                          OrderBy(x => x.fechaLanzamiento).
                          Take(top).
                          ToListAsync();

            var resultado = new LandingPageDTO();
            resultado.proximosEstrenos = mapper.Map<List<PeliculaDTO>>(proximosEstrenos);
            resultado.enCines = mapper.Map<List<PeliculaDTO>>(enCines);

            return resultado;
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<PeliculaDTO>> Get(int id)
        {
            var pelicula = await context.Peliculas.
                           Include(x => x.peliculasGeneros).ThenInclude(x => x.genero).
                           Include(x => x.peliculasActores).ThenInclude(x => x.actor).
                           Include(x => x.peliculasCines).ThenInclude(x => x.cine).
                           FirstOrDefaultAsync(x => x.id == id);
            if (pelicula == null) { return NotFound(); }

            var promedioVoto = 0.00;
            var usuarioVoto = 0;
            if (await context.Ratings.AnyAsync(x => x.peliculaID == id)) {

                promedioVoto = await context.Ratings.Where(x => x.peliculaID == id).AverageAsync(x => x.puntuacion);

                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    var email = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "email").Value;
                    var usuario = await userManager.FindByEmailAsync(email);
                    var userID = usuario.Id;
                    var ratingBD = await context.Ratings.FirstOrDefaultAsync(x => x.userID == userID && x.peliculaID == id);

                    if (ratingBD != null) {
                        usuarioVoto = ratingBD.puntuacion;
                    }
                }
            }
            
            var dto = mapper.Map<PeliculaDTO>(pelicula);
            dto.votoUser = usuarioVoto;
            dto.promedioVoto = promedioVoto;
            dto.actores = dto.actores.OrderBy(x => x.orden).ToList();

            return dto;
        }

        [HttpGet("filtrar")]
        [AllowAnonymous]
        public async Task<ActionResult<List<PeliculaDTO>>> Filtrar([FromQuery] PeliculasFiltrarDTO peliculasFiltrarDTO)
        {
            var peliculasQueryable = context.Peliculas.AsQueryable();

            if (!string.IsNullOrEmpty(peliculasFiltrarDTO.titulo))
            {
                peliculasQueryable = peliculasQueryable.Where(x => x.titulo.Contains(peliculasFiltrarDTO.titulo));
            }

            if (peliculasFiltrarDTO.enCines)
            {
                peliculasQueryable = peliculasQueryable.Where(x => x.enCines);
            }

            if (peliculasFiltrarDTO.proximosEstrenos)
            {
                var hoy = DateTime.Today;
                peliculasQueryable = peliculasQueryable.Where(x => x.fechaLanzamiento > hoy);
            }

            if (peliculasFiltrarDTO.generoId != 0)
            {
                peliculasQueryable = peliculasQueryable
                    .Where(x => x.peliculasGeneros.Select(y => y.generoID)
                    .Contains(peliculasFiltrarDTO.generoId));
            }

            await HttpContext.InsertarParametrosPaginacionCabecera(peliculasQueryable);

            var peliculas = await peliculasQueryable.Paginar(peliculasFiltrarDTO.paginacionDTO).ToListAsync();
            return mapper.Map<List<PeliculaDTO>>(peliculas);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post([FromForm] PeliculaCreacionDTO peliculaCreacionDTO)
        {
            var pelicula = mapper.Map<Pelicula>(peliculaCreacionDTO);
            if (peliculaCreacionDTO.poster != null)
            {
                pelicula.poster = await alamacenadorArchivos.GuardarArchivo(contenedor, peliculaCreacionDTO.poster);
            }
            EscribirOrdenActores(pelicula);
            context.Add(pelicula);
            await context.SaveChangesAsync();
            return pelicula.id;
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
