using back_end.Validaciones;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace back_end.Entidades
{
    public class Genero
    {
        public int id { get; set; }
        
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 50)]
        [PrimeraLetraMayuscula]
        public string nombre { get; set; }
    }
}
