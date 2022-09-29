using back_end.DTOs;
using System.Linq;

namespace back_end.Utilidades
{
    public static class IQuerybleExtensions
    {
        public static IQueryable<T> Paginar<T>(this IQueryable<T> queryble, PaginacionDTO paginacionDTO)
        {
            return queryble.Skip((paginacionDTO.pagina - 1) * paginacionDTO.recordsPorPorPagina).
                   Take(paginacionDTO.recordsPorPorPagina);
        }
    }
}
