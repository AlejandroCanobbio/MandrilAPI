using AutoMapper;
using MandrilAPI.Models;
using MandrilAPI.DTOs;

namespace MandrilAPI.Mapping;

public class MandrilProfile : Profile
{
    public MandrilProfile()
    {
        CreateMap<MandrilCreateDto, Mandril>()
            .ForMember(dest => dest.Habilidades, opt => opt.MapFrom(src => new List<Habilidad>()));

        // Mapeo Mandril a MandrilResponseDto
        CreateMap<Mandril, MandrilResponseDto>();

        // Mapeo Habilidad a HabilidadDto, convertimos enum Potencia a string
        CreateMap<Habilidad, HabilidadDto>()
            .ForMember(dest => dest.Potencia, opt => opt.MapFrom(src => src.Potencia.ToString()));

        // Mapeo HabilidadCreateDto a Habilidad para crear habilidades nuevas
        CreateMap<HabilidadCreateDto, Habilidad>();
    }
}
