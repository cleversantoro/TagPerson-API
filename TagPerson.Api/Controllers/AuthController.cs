using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TagPerson.Application.DTOs;
using TagPerson.Application.Interfaces;

namespace TagPerson.Api.Controllers;

/// <summary>Controlador de autenticacao.</summary>
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    /// <summary>Endpoints de autenticacao.</summary>
    private readonly IAuthService _auth;

    /// <summary>Cria o controller de autenticacao.</summary>
    public AuthController(IAuthService auth)
    {
        _auth = auth;
    }

    /// <summary>Gera um token JWT valido.</summary>
    [HttpPost("token")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(TokenResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Token([FromBody] TokenRequestDto request)
    {
        var token = await _auth.AuthenticateAsync(request, HttpContext.RequestAborted);
        return token is null ? Unauthorized() : Ok(token);
    }
}
