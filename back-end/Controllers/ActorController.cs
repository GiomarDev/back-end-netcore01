using AutoMapper;
using back_end.DTOs;
using back_end.Entidades;
using back_end.Utilidades;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
        public async Task<ActionResult> Post([FromForm]ActorCreacionDTO actorCreacionDTO) 
        {
            var actor = mapper.Map<Actor>(actorCreacionDTO);
            if (actorCreacionDTO.foto != null) {
                actor.foto = await alamacenadorArchivos.GuardarArchivo(contenedor, actorCreacionDTO.foto);
            }
            context.Add(actor);
            await context.SaveChangesAsync();
            return NoContent();
        }

    }
}
