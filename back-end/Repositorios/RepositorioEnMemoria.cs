using back_end.Entidades;
using System.Collections.Generic;

namespace back_end.Repositorios
{
    public class RepositorioEnMemoria: IRepositorio
    {
        private List<Genero> _generos;
        
        public RepositorioEnMemoria()
        {
            _generos = new List<Genero>() {
                new Genero(){ id = 1, nombre = "Comedia"},
                new Genero(){ id = 2, nombre = "Acción"}
            };
        }

        public List<Genero> obtenerTodosLosGeneros() {
            return _generos;
        }
    }
}
