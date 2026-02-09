using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TagPerson.Application.DTOs;
using TagPerson.Application.Interfaces;
using TagPerson.Domain.Entities;

namespace TagPerson.Api.Controllers;

/// <summary>Controlador de personagens.</summary>
[ApiController]
[Route("api/characters_characterization")]
[Authorize]
public class CharactersCharacterizationController : ControllerBase
{
    /// <summary>Endpoints de personagens.</summary>
    private readonly ICharacterService _service;

    /// <summary>Cria o controller de personagens.</summary>
    public CharactersCharacterizationController(ICharacterService service)
    {
        _service = service;
    }

    /// <summary>Adiciona caracterização ao personagem.</summary>
    [HttpPost("{id:int}/characterizations")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddCharacterization(int id, [FromBody] CharacterCharacterizationRequestDto req)
    {
        var ok = await _service.AddCharacterizationAsync(id, req, HttpContext.RequestAborted);
        if (!ok)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>Seleciona a caracterização do personagem.</summary>
    [HttpGet("{id:int}/characterizations")]
    [ProducesResponseType(typeof(IReadOnlyList<CharacterCharacterizationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCharacterCharacterizations(int id)
    {
        var list = await _service.GetCharacterCharacterizationsAsync(id, HttpContext.RequestAborted);
        return Ok(list);
    }

    /// <summary>Exclui uma caracterização do personagem.</summary>
    [HttpDelete("{id:int}/characterizations")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCharacterCharacterizations(int id, int characterizationId)
    {
        var ok = await _service.DeleteCharacterCharacterizationsAsync(id, characterizationId, HttpContext.RequestAborted);
        return ok ? NoContent() : NotFound();
    }


}
