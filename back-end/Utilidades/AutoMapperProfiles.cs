using AutoMapper;
using back_end.DTOs;
using back_end.Entidades;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;

namespace back_end.Utilidades
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles(GeometryFactory geometryFactory)
        {
            CreateMap<Genero, GeneroDTO>().ReverseMap();
            CreateMap<GeneroCreacionDTO, Genero>();
            CreateMap<Actor, ActorDTO>().ReverseMap();
            CreateMap<ActorCreacionDTO, Actor>().ForMember(x => x.foto, options => options.Ignore());
            CreateMap<CineCreacionDTO, Cine>().ForMember(x => x.ubicacion,
                                                         x => x.MapFrom(dto => geometryFactory.CreatePoint
                                                         (new Coordinate(dto.longitud, dto.latitud))));
            CreateMap<Cine, CineDTO>().ForMember(x => x.latitud, dto => dto.MapFrom(campo => campo.ubicacion.Y))
                                      .ForMember(x => x.longitud, dto => dto.MapFrom(campo => campo.ubicacion.X));
            CreateMap<PeliculaCreacionDTO, Pelicula>().
                                                       ForMember(x => x.poster, opciones => opciones.Ignore()).
                                                       ForMember(x => x.peliculasGeneros, opciones => opciones.MapFrom(MapearPeliculasGeneros))
                                                       .ForMember(x => x.peliculasCines, opciones => opciones.MapFrom(MapearPeliculasCines))
                                                       .ForMember(x => x.peliculasActores, opciones => opciones.MapFrom(MapearPeliculasActores));
            CreateMap<Pelicula, PeliculaDTO>()
                                                .ForMember(x => x.generos, options => options.MapFrom(MapearPeliculasGeneros))
                                                .ForMember(x => x.actores, options => options.MapFrom(MapearPeliculasActores))
                                                .ForMember(x => x.cines, options => options.MapFrom(MapearPeliculasCines));
        }

        private List<CineDTO> MapearPeliculasCines(Pelicula pelicula, PeliculaDTO peliculaDTO)
        {
            var resultado = new List<CineDTO>();

            if (pelicula.peliculasCines != null)
            {
                foreach (var peliculasCines in pelicula.peliculasCines)
                {
                    resultado.Add(new CineDTO()
                    {
                        id = peliculasCines.cineID,
                        name = peliculasCines.cine.name,
                        latitud = peliculasCines.cine.ubicacion.Y,
                        longitud = peliculasCines.cine.ubicacion.X
                    });
                }
            }

            return resultado;
        }

        private List<PeliculaActorDTO> MapearPeliculasActores(Pelicula pelicula, PeliculaDTO peliculaDTO)
        {
            var resultado = new List<PeliculaActorDTO>();

            if (pelicula.peliculasActores != null)
            {
                foreach (var actorPeliculas in pelicula.peliculasActores)
                {
                    resultado.Add(new PeliculaActorDTO() 
                    { 
                        id = actorPeliculas.actorID,
                        nombre = actorPeliculas.actor.nombre,
                        foto = actorPeliculas.actor.foto,
                        orden = actorPeliculas.orden,
                        personaje = actorPeliculas.personaje
                    });
                }
            }

            return resultado;
        }

        private List<GeneroDTO> MapearPeliculasGeneros(Pelicula pelicula, PeliculaDTO peliculaDTO)
        { 
            var resultado = new List<GeneroDTO>();

            if (pelicula.peliculasGeneros != null) {
                foreach (var genero in pelicula.peliculasGeneros)
                {
                    resultado.Add(new GeneroDTO() { id = genero.generoID, nombre = genero.genero.nombre });
                }
            }

            return resultado;
        }

        private List<PeliculasActores> MapearPeliculasActores(PeliculaCreacionDTO peliculaCreacionDTO,
                                                              Pelicula pelicula)
        {
            var resultado = new List<PeliculasActores>();

            if (peliculaCreacionDTO.actores == null)
            {
                return resultado;
            }

            foreach (var actor in peliculaCreacionDTO.actores)
            {
                resultado.Add(new PeliculasActores() { actorID = actor.id, personaje = actor.personaje });
            }

            return resultado;
        }

        private List<PeliculasGeneros> MapearPeliculasGeneros(PeliculaCreacionDTO peliculaCreacionDTO,
                                                              Pelicula pelicula)
        {
            var resultado = new List<PeliculasGeneros>();

            if (peliculaCreacionDTO.generosIds == null)
            {
                return resultado;
            }

            foreach (var id in peliculaCreacionDTO.generosIds)
            {
                resultado.Add(new PeliculasGeneros() { generoID = id });
            }

            return resultado;
        }

        private List<PeliculasCines> MapearPeliculasCines(PeliculaCreacionDTO peliculaCreacionDTO,
                                                      Pelicula pelicula)
        {
            var resultado = new List<PeliculasCines>();

            if (peliculaCreacionDTO.cinesIds == null)
            {
                return resultado;
            }

            foreach (var id in peliculaCreacionDTO.cinesIds)
            {
                resultado.Add(new PeliculasCines() { cineID = id });
            }

            return resultado;
        }
    }
}
