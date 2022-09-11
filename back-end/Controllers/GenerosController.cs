using back_end.Repositorios;

namespace back_end.Controllers
{
    public class GenerosController
    {
        private readonly IRepositorio repositorio;

        public GenerosController(IRepositorio repositorio)
        {
            this.repositorio = repositorio;
        }
    }
}
