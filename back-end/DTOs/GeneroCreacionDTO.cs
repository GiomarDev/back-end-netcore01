using back_end.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace back_end.DTOs
{
    public class GeneroCreacionDTO
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 50, ErrorMessage = "Tiene una longitud de 50 caracteres")]
        [PrimeraLetraMayuscula]
        public string nombre { get; set; }
    }
}
