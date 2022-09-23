using AutoMapper;
using back_end.DTOs;
using back_end.Entidades;
using NetTopologySuite.Geometries;

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
        }
    }
}
