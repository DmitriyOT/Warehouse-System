using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Warehouse.Contracts.Api.Request.Dtos;
using Warehouse.Contracts.Api.Response;
using Warehouse.Contracts.Api.Response.Dtos;
using Warehouse.Contracts.Application;
using Warehouse.Contracts.Exceptions;

namespace Warehouse.Api.Controllers;

/// <summary>
/// Аутентификация: вход в систему по логину и паролю
/// </summary>
[ApiController]
[Route("[controller]")]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="authService"></param>
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Войти в систему и получить JWT-токен
    /// </summary>
    /// <param name="request">Логин и пароль</param>
    /// <returns>JWT-токен или 401 при неверной паре логин/пароль</returns>
    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginRequestDto request)
    {
        try
        {
            var token = await _authService.Login(request.Login, request.Password);
            return Ok(new ResponseDto<LoginResponseDto>(new LoginResponseDto(token)));
        }
        catch (UserException ex)
        {
            return Unauthorized(new ErrorResponseDto(ex.Message));
        }
    }
}
