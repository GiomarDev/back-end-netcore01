using back_end.Entidades;
using back_end.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace back_end.Controllers
{
    [Route("api/generos")]
    [ApiController]
    public class GenerosController: ControllerBase
    {
        private readonly IRepositorio repositorio;

        public GenerosController(IRepositorio repositorio)
        {
            this.repositorio = repositorio;
        }

        [HttpGet] // api/generos
        [HttpGet("listado")] // api/genero/listado
        [HttpGet("/listadogeneros")] // listado/generos
        public ActionResult<List<Genero>> Get() 
        {
            return repositorio.obtenerTodosLosGeneros(); ;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Genero>> Get(int id,[FromHeader] string nombre)
        {
            var genero = await repositorio.obetenerPorID(id);

            if (genero == null) {
                return NotFound();
            }

            return genero;
        }

        [HttpPost]
        public ActionResult Post([FromBody] Genero genero) 
        {
            return NoContent();
        }

        [HttpPut]
        public ActionResult Put([FromBody] Genero genero)
        {
            return NoContent();
        }

        [HttpDelete]
        public ActionResult Delete()
        {
            return NoContent();
        }

    }
}
