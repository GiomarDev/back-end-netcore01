namespace back_end.DTOs
{
    public class PaginacionDTO
    {
        public int pagina { get; set; } = 1;
        
        public int recordsPagina = 10;
        public readonly int cantidadMaximaRecordsPagina = 50;

        public int RecordsPagina
        {
            get 
            {
                return recordsPagina;
            }
            set
            { 
                recordsPagina = (value > cantidadMaximaRecordsPagina) ? cantidadMaximaRecordsPagina : value;
            }
        }
    }
}
