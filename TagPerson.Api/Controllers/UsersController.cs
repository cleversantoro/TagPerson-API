using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TagPerson.Application.DTOs;
using TagPerson.Application.Interfaces;

namespace TagPerson.Api.Controllers;

/// <summary>Controlador de administracao de usuarios.</summary>
[ApiController]
[Route("api/users")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _service;

    /// <summary>Cria o controller de usuarios.</summary>
    public UsersController(IUserService service)
    {
        _service = service;
    }

    /// <summary>Lista usuarios.</summary>
    [HttpGet]
    [Authorize(Roles = "admin")]
    [ProducesResponseType(typeof(IReadOnlyList<UserDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List()
    {
        var users = await _service.ListAsync(HttpContext.RequestAborted);
        return Ok(users);
    }

    /// <summary>Obtem um usuario por id.</summary>
    [HttpGet("{id:int}")]
    [Authorize(Roles = "admin")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(int id)
    {
        var user = await _service.GetAsync(id, HttpContext.RequestAborted);
        return user is null ? NotFound() : Ok(user);
    }

    /// <summary>Obtem o usuario autenticado.</summary>
    [HttpGet("me")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Me()
    {
        var username = User.Identity?.Name;
        if (string.IsNullOrWhiteSpace(username)) return Unauthorized();

        var user = await _service.GetByUsernameAsync(username, HttpContext.RequestAborted);
        return user is null ? Unauthorized() : Ok(user);
    }

    /// <summary>Cria um usuario.</summary>
    [HttpPost]
    [Authorize(Roles = "admin")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateUserRequestDto request)
    {
        var user = await _service.CreateAsync(request, HttpContext.RequestAborted);
        if (user is null) return Conflict("Username ja existe.");
        return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
    }

    /// <summary>Atualiza role/estado e opcionalmente senha.</summary>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUserRequestDto request)
    {
        var updated = await _service.UpdateAsync(id, request, HttpContext.RequestAborted);
        return updated ? NoContent() : NotFound();
    }

    /// <summary>Desativa um usuario.</summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deactivate(int id)
    {
        var updated = await _service.DeactivateAsync(id, HttpContext.RequestAborted);
        return updated ? NoContent() : NotFound();
    }

    /// <summary>Altera a senha do usuario logado.</summary>
    [HttpPut("me/password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDto request)
    {
        var username = User.Identity?.Name;
        if (string.IsNullOrWhiteSpace(username)) return Unauthorized();

        var ok = await _service.ChangePasswordAsync(username, request, HttpContext.RequestAborted);
        return ok ? NoContent() : Unauthorized();
    }
}
