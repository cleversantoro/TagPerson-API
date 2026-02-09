using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TagPerson.Application.DTOs;
using TagPerson.Application.Interfaces;
using TagPerson.Domain.Entities;

namespace TagPerson.Api.Controllers;

/// <summary>Controlador de personagens.</summary>
[ApiController]
[Route("api/characters_combat")]
[Authorize]
public class CharactersCombatController : ControllerBase
{
    /// <summary>Endpoints de personagens.</summary>
    private readonly ICharacterService _service;

    /// <summary>Cria o controller de personagens.</summary>
    public CharactersCombatController(ICharacterService service)
    {
        _service = service;
    }

    /// <summary>Adiciona tecnica de combate ao personagem.</summary>
    [HttpPost("{id:int}/combat")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddCombat(int id, [FromBody] CharacterCombatSkillRequestDto req)
    {
        var ok = await _service.AddCombatSkillAsync(id, req, HttpContext.RequestAborted);
        return ok ? NoContent() : NotFound();
    }

    /// <summary>Seleciona as Tecnicas de Combate do personagem.</summary>
    [HttpGet("{id:int}/combat")]
    [ProducesResponseType(typeof(IReadOnlyList<CombatFromCharacterDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCharacterCombat(int id)
    {
        var list = await _service.GetCharacterCombatAsync(id, HttpContext.RequestAborted);
        return Ok(list);
    }

    /// <summary>Exclui uma Tecnica de Combate do personagem.</summary>
    [HttpDelete("{id:int}/combat")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCharacterCombat(int id, int combatId, int combatGroupId)
    {
        var ok = await _service.DeleteCharacterCombatAsync(id, combatId, combatGroupId, HttpContext.RequestAborted);
        return ok ? NoContent() : NotFound();
    }
    
}
