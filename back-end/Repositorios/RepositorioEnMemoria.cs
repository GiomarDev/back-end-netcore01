using back_end.Entidades;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Repositorios
{
    public class RepositorioEnMemoria: IRepositorio
    {
        private List<Genero> _generos;
        
        public RepositorioEnMemoria(ILogger<RepositorioEnMemoria> logger)
        {
            _generos = new List<Genero>() {
                new Genero(){ id = 1, nombre = "Comedia"},
                new Genero(){ id = 2, nombre = "Acción"}
            };

            _guid = Guid.NewGuid();
        }

        public Guid _guid { get; set; }

        public List<Genero> obtenerTodosLosGeneros() 
        {
            return _generos;
        }

        public async Task<Genero> obetenerPorID(int id)
        {
            await Task.Delay(1);

            return  _generos.FirstOrDefault(x => x.id == id);
        }

        public Guid obtenerGuid()
        {
            return _guid;
        }

        public void crearGenero(Genero genero)
        {
            genero.id = _generos.Count() + 1;
            _generos.Add(genero);
        }
    }
}
