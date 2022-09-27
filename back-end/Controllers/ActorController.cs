using AutoMapper;
using back_end.DTOs;
using back_end.Entidades;
using back_end.Utilidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Controllers
{
    [Route("api/actores")]
    [ApiController]
    public class ActorController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IAlamacenadorArchivos alamacenadorArchivos;
        private readonly string contenedor = "actores";

        public ActorController(ApplicationDbContext context,
                               IMapper mapper,
                               IAlamacenadorArchivos alamacenadorArchivos)
        {
            this.context = context;
            this.mapper = mapper;
            this.alamacenadorArchivos = alamacenadorArchivos;
        }

        #region EndPoints

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] ActorCreacionDTO actorCreacionDTO)
        {
            var actor = mapper.Map<Actor>(actorCreacionDTO);
            if (actorCreacionDTO.foto != null) {
                actor.foto = await alamacenadorArchivos.GuardarArchivo(contenedor, actorCreacionDTO.foto);
            }
            context.Add(actor);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<List<ActorDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryble = context.Actores.AsQueryable();
            await HttpContext.InsertarParametrosPaginacionCabecera(queryble);
            var actores = await queryble.OrderBy(x => x.nombre).Paginar(paginacionDTO).ToListAsync();
            return mapper.Map<List<ActorDTO>>(actores);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var actor = await context.Actores.FirstOrDefaultAsync(x => x.id == id);

            if (actor == null)
            {
                return NotFound();
            }

            context.Remove(actor);
            await context.SaveChangesAsync();
            await alamacenadorArchivos.BorrarArchivo(actor.foto, contenedor);
            return NoContent();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ActorDTO>> Get(int id)
        {
            var actor = await context.Actores.FirstOrDefaultAsync(x => x.id == id);
            
            if (actor == null) {
                return NotFound();
            }

            return mapper.Map<ActorDTO>(actor);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromForm] ActorCreacionDTO actorCreacionDTO)
        {
            var actor = await context.Actores.FirstOrDefaultAsync(x => x.id == id);

            if (actor == null) {
                return NotFound();
            }

            actor = mapper.Map(actorCreacionDTO, actor);
            
            if (actorCreacionDTO.foto != null)
            {
                actor.foto = await alamacenadorArchivos.EditarArchivo(contenedor, actorCreacionDTO.foto, actor.foto);
            }

            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("buscarPorNombre")]
        public async Task<ActionResult<List<PeliculaActorDTO>>> BuscarPorNombre([FromBody] string nombre) 
        {
            if (string.IsNullOrEmpty(nombre)) { return new List<PeliculaActorDTO>(); }
            return await context.Actores.
                                         Where(x => x.nombre.Contains(nombre)).
                                         Select(x => new PeliculaActorDTO { id = x.id, nombre = x.nombre, foto = x.foto}).
                                         Take(5).
                                         ToListAsync();
        }

        #endregion
    }
}
