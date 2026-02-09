using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TagPerson.Application.DTOs;
using TagPerson.Application.Interfaces;
using TagPerson.Domain.Entities;

namespace TagPerson.Api.Controllers;

/// <summary>Controlador de personagens.</summary>
[ApiController]
[Route("api/characters_spell")]
[Authorize]
public class CharactersSpellController : ControllerBase
{
    /// <summary>Endpoints de personagens.</summary>
    private readonly ICharacterService _service;

    /// <summary>Cria o controller de personagens.</summary>
    public CharactersSpellController(ICharacterService service)
    {
        _service = service;
    }

    /// <summary>Adiciona magia ao personagem.</summary>
    [HttpPost("{id:int}/spells")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddSpell(int id, [FromBody] CharacterSpellRequestDto req)
    {
        var ok = await _service.AddSpellAsync(id, req, HttpContext.RequestAborted);
        return ok ? NoContent() : NotFound();
    }

    /// <summary>Seleciona as magias do personagem.</summary>
    [HttpGet("{id:int}/spells")]
    [ProducesResponseType(typeof(IReadOnlyList<SpellFromCharacterDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCharacterSpell(int id)
    {
        var list = await _service.GetCharacterSpellAsync(id, HttpContext.RequestAborted);
        return Ok(list);
    }

    /// <summary>Exclui uma magia do personagem.</summary>
    [HttpDelete("{id:int}/spells")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCharacterSpell(int id, int spellId, int spellGroupId)
    {
        var ok = await _service.DeleteCharacterSpellAsync(id, spellId, spellGroupId, HttpContext.RequestAborted);
        return ok ? NoContent() : NotFound();
    }
}
