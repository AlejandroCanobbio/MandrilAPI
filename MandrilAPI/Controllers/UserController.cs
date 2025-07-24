using System.Threading.Tasks;
using AutoMapper;
using MandrilAPI.DTOs;
using MandrilAPI.Helpers;
using MandrilAPI.Models;
using MandrilAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace MandrilAPI.Controllers;

[ApiController]
[Route("api/[controller]")]

public class UserController : ControllerBase
{
    private readonly UserService _userService;
    private readonly IMapper _mapper;

    public UserController(UserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }
    [HttpGet("{sCorreo}/{sClave}")]
    public async Task<ActionResult<UserResponseDto>> GetUser(string sCorreo, string sClave)
    {
        var user = await _userService.ObtenerPorId(sCorreo, sClave);

        if (user == null)
        {
            return NotFound(Mensajes.User.NotFound);
        }

        var userDto = _mapper.Map<UserResponseDto>(user);
        return Ok(userDto);
    }

    [HttpPost]
    public async Task<ActionResult<UserResponseDto>> PostUser(UserCreateDto userCreateDto)
    {
        var user = _mapper.Map<User>(userCreateDto);
        var userCreado = await _userService.InsertarUser(user);
        

        var mandrilResponse = _mapper.Map<UserResponseDto>(userCreado);
        return CreatedAtAction(nameof(GetUser),
            new { sCorreo = mandrilResponse?.Correo, sClave = mandrilResponse?.Clave },
            mandrilResponse
        );
    }
}
