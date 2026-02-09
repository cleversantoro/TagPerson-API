using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TagPerson.Application.DTOs;
using TagPerson.Application.Interfaces;
using TagPerson.Domain.Entities;

namespace TagPerson.Api.Controllers;

/// <summary>Controlador de personagens.</summary>
[ApiController]
[Route("api/characters_skill")]
[Authorize]
public class CharactersSkillController : ControllerBase
{
    /// <summary>Endpoints de personagens.</summary>
    private readonly ICharacterService _service;

    /// <summary>Cria o controller de personagens.</summary>
    public CharactersSkillController(ICharacterService service)
    {
        _service = service;
    }

    /// <summary>Adiciona habilidade ao personagem.</summary>
    [HttpPost("{id:int}/skills")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddSkill(int id, [FromBody] CharacterSkillRequestDto req)
    {
        var ok = await _service.AddSkillAsync(id, req, HttpContext.RequestAborted);
        return ok ? NoContent() : NotFound();
    }

    /// <summary>Seleciona as habilidades do personagem.</summary>
    [HttpGet("{id:int}/skills")]
    [ProducesResponseType(typeof(IReadOnlyList<SkillFromCharacterDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCharacterSkill(int id)
    {
        var list = await _service.GetCharacterSkillAsync(id, HttpContext.RequestAborted);
        return Ok(list);
    }

    /// <summary>Exclui uma habilidade do personagem.</summary>
    [HttpDelete("{id:int}/skills")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCharacterSkill(int id, int skillId)
    {
        var ok = await _service.DeleteCharacterSkillAsync(id, skillId, HttpContext.RequestAborted);
        return ok ? NoContent() : NotFound();
    }


}
