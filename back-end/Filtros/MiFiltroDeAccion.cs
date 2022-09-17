using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace back_end.Filtros
{
    public class MiFiltroDeAccion : IActionFilter
    {
        private readonly ILogger<MiFiltroDeAccion> logger;

        public MiFiltroDeAccion(ILogger<MiFiltroDeAccion> logger)
        {
            this.logger = logger;
        }

        //Antes de Ejecutar la accion
        public void OnActionExecuting(ActionExecutingContext context)
        {
            logger.LogInformation("Antes de Ejecutar la accion");
        }

        //Despues de ejecutar la accion
        public void OnActionExecuted(ActionExecutedContext context)
        {
            logger.LogInformation("Despues de Ejecutar la accion");
        }
    }
}
