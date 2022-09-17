using back_end.Entidades;
using back_end.Filtros;
using back_end.Repositorios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace back_end.Controllers
{
    [Route("api/generos")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GenerosController: ControllerBase
    {
        private readonly IRepositorio repositorio;
        private readonly WeatherForecastController weatherForecastController;
        private readonly ILogger<GenerosController> logger;

        public GenerosController(IRepositorio repositorio, 
                                WeatherForecastController weatherForecastController,
                                ILogger<GenerosController> logger)
        {
            this.repositorio = repositorio;
            this.weatherForecastController = weatherForecastController;
            this.logger = logger;
        }

        [HttpGet] // api/generos
        [HttpGet("listado")] // api/genero/listado
        [HttpGet("/listadogeneros")] // listado/generos,
        //[ResponseCache(Duration = 60)] //stop al cache
        [ServiceFilter(typeof(MiFiltroDeAccion))]
        public ActionResult<List<Genero>> Get() 
        {
            logger.LogInformation("Vamos a mostrar los generos");
            return repositorio.obtenerTodosLosGeneros(); ;
        }

        [HttpGet("guid")]
        public ActionResult<Guid> GetGUID()
        {
            return Ok(new
            {
                GUID_GenerosController = repositorio.obtenerGuid(),
                GUID_WeatherForCastController = weatherForecastController.obtenerGuidWeatherForCastController()
            });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Genero>> Get(int id,[FromHeader] string nombre)
        {
            logger.LogDebug($"Obteniendo un genero por el id: {id}");
            var genero = await repositorio.obetenerPorID(id);

            if (genero == null) {
                throw new ApplicationException($"El genero de ID {id} no fue encontrado");
                logger.LogWarning($"No pudimos encontrar el genero de id: {id}");
                return NotFound();
            }

            return genero;
        }

        [HttpPost]
        public ActionResult Post([FromBody] Genero genero) 
        {
            repositorio.crearGenero(genero);
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
