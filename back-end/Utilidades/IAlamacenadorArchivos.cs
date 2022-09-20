using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace back_end.Utilidades
{
    public interface IAlamacenadorArchivos
    {
        Task BorrarArchivo(string ruta, string contenedor);
        Task<string> EditarArchivo(string contenedor, IFormFile archivo, string ruta);
        Task<string> GuardarArchivo(string contenedor, IFormFile archivo);
    }
}