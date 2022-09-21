using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;

namespace back_end.Entidades
{
    public class Cine
    {
        public int id { get; set; }
        [Required]
        [StringLength(maximumLength:75)]
        public string name { get; set; }
        public Point ubicacion { get; set; }
    }
}
