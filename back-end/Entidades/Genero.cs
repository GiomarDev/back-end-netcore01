using back_end.Validaciones;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace back_end.Entidades
{
    public class Genero: IValidatableObject
    {
        public int id { get; set; }
        
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 10)]
        //[PrimeraLetraMayuscula]
        public string nombre { get; set; }
        
        [Range(18, 20)]
        public int edad { get; set; }
        
        [CreditCard]
        public string tarjetaCredito { get; set; }

        [Url]
        public string url { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(nombre)) {
                var primeraLetra = nombre[0].ToString();
                if (primeraLetra != primeraLetra.ToUpper()) {
                    yield return new ValidationResult("La primera letra debe ser mayuscula", new string[] { nameof(nombre)});
                }
            }
        }
    }
}
