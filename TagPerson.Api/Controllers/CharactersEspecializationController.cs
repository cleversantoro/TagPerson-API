using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TagPerson.Application.DTOs;
using TagPerson.Application.Interfaces;
using TagPerson.Domain.Entities;

namespace TagPerson.Api.Controllers;

/// <summary>Controlador de personagens.</summary>
[ApiController]
[Route("api/characters_especialization")]
[Authorize]
public class CharactersEspecializationController : ControllerBase
{
    /// <summary>Endpoints de personagens.</summary>
    private readonly ICharacterService _service;

    /// <summary>Cria o controller de personagens.</summary>
    public CharactersEspecializationController(ICharacterService service)
    {
        _service = service;
    }

    #region Especialização
    /// <summary>Adiciona especializacao da habilidade ao personagem.</summary>
    [HttpPost("{id:int}/skills/{skillId:int}/specializations")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddSkillSpecialization(int id, int skillId, [FromBody] CharacterSkillSpecializationRequestDto req)
    {
        var ok = await _service.AddSkillSpecializationAsync(id, skillId, req, HttpContext.RequestAborted);
        return ok ? NoContent() : NotFound();
    }

    /// <summary>Lista especializacoes da habilidade do personagem.</summary>
    [HttpGet("{id:int}/skills/{skillId:int}/specializations")]
    [ProducesResponseType(typeof(IReadOnlyList<CharacterSkillSpecializationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SkillSpecializations(int id, int skillId)
    {
        var list = await _service.GetSkillSpecializationsAsync(id, skillId, HttpContext.RequestAborted);
        return Ok(list);
    }
    #endregion
    
}
