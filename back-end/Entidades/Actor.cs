﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace back_end.Entidades
{
    public class Actor
    {
        public int id { get; set; }
        [Required]
        [StringLength(maximumLength: 200)]
        public string nombre { get; set; }
        public string biografia { get; set; }
        public DateTime fechaNacimiento { get; set; }
        public string foto { get; set; }
        public List<PeliculasActores> peliculasActores { get; set; }
    }
}
