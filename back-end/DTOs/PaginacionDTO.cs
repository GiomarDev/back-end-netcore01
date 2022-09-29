namespace back_end.DTOs
{
    public class PaginacionDTO
    {
        public int pagina { get; set; } = 1;

        private int recordsPorPagina = 10;
        public readonly int cantidadMaximaRecordsPagina = 50;

        public int recordsPorPorPagina
        {
            get 
            {
                return recordsPorPagina;
            }
            set
            {
                recordsPorPagina = (value > cantidadMaximaRecordsPagina) ? cantidadMaximaRecordsPagina : value;
            }
        }
    }
}
