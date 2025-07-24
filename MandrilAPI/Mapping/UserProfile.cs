using AutoMapper;
using MandrilAPI.DTOs;
using MandrilAPI.Models;

namespace MandrilAPI.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserCreateDto, User>();

        CreateMap<User, UserResponseDto>(); // Ignorar la clave en la respuesta
    }
}
