using System;

namespace back_end.DTOs
{
    public class RespuestaAutenticacion
    {
        public string token { get; set; }
        public DateTime expiracion { get; set; }
    }
}
