using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TagPerson.Application.DTOs;
using TagPerson.Application.Interfaces;

namespace TagPerson.Api.Controllers;

/// <summary>Controlador de magias e grupos.</summary>
[ApiController]
[Route("api/spells")]
[Authorize]
public class SpellsController : ControllerBase
{
    private readonly ISpellService _service;

    /// <summary>Cria o controller de magias.</summary>
    public SpellsController(ISpellService service)
    {
        _service = service;
    }

    /// <summary>Lista os grupos de magia.</summary>
    [HttpGet("groups")]
    [ProducesResponseType(typeof(IReadOnlyList<SpellGroupDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GroupParents()
    {
        var list = await _service.GetGroupParentsAsync(HttpContext.RequestAborted);
        return Ok(list);
    }

    /// <summary>Lista magias por grupo.</summary>
    [HttpGet("groups/{groupId:int}/spell_items")]
    [ProducesResponseType(typeof(IReadOnlyList<SpellFromGroupDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SpellsFromGroup(int groupId)
    {
        var list = await _service.GetSpellsFromGroupAsync(groupId, HttpContext.RequestAborted);
        return Ok(list);
    }

    /// <summary>Lista magias por profissão.</summary>
    [HttpGet("views/{professionalId:int}/spell_profession")]
    [ProducesResponseType(typeof(IReadOnlyList<SpellTechniquesDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProfessions(int professionalId)
    {
        var list = await _service.GetProfessionsAsync(professionalId, HttpContext.RequestAborted);
        return Ok(list);
    }

    /// <summary>Lista magias por especialização.</summary>
    [HttpGet("views/{especializationId:int}/spell_especialization")]
    [ProducesResponseType(typeof(IReadOnlyList<SpellTechniquesDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEspecializations(int especializationId)
    {
        var list = await _service.GetEspecializationsAsync(especializationId, HttpContext.RequestAborted);
        return Ok(list);
    }
}
