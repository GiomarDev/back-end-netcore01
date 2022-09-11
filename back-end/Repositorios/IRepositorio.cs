﻿using back_end.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace back_end.Repositorios
{
    public interface IRepositorio
    {
        List<Genero> obtenerTodosLosGeneros();
        Task<Genero> obetenerPorID(int id);
    }
}
