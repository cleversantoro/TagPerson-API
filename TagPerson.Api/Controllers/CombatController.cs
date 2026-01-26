using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TagPerson.Application.DTOs;
using TagPerson.Application.Interfaces;

namespace TagPerson.Api.Controllers;

/// <summary>Controlador de tecnicas de combate.</summary>
[ApiController]
[Route("api/combat")]
[Authorize]
public class CombatController : ControllerBase
{
    private readonly ICombatService _service;

    /// <summary>Cria o controller de combate.</summary>
    public CombatController(ICombatService service)
    {
        _service = service;
    }

    /// <summary>Lista as técnicas de Combate.</summary>
    [HttpGet("groups")]
    [ProducesResponseType(typeof(IReadOnlyList<CombatGroupDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GroupParents()
    {
        var list = await _service.GetGroupParentsAsync(HttpContext.RequestAborted);
        return Ok(list);
    }

    /// <summary>Lista as tecnicas pelo grupo.</summary>
    [HttpGet("groups/{groupId:int}/combat_items")]
    [ProducesResponseType(typeof(IReadOnlyList<CombatFromGroupDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ItemsFromGroup(int groupId)
    {
        var list = await _service.GetCombatFromGroupAsync(groupId, HttpContext.RequestAborted);
        return Ok(list);
    }

    /// <summary>Lista tecnicas Basicas.</summary>
    [HttpGet("views/combat_basic")]
    [ProducesResponseType(typeof(IReadOnlyList<CombatTechniquesDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBasics()
    {
        var list = await _service.GetBasicsAsync(HttpContext.RequestAborted);
        return Ok(list);
    }

    /// <summary>Lista tecnicas da Profissão.</summary>
    [HttpGet("views/{professionalId:int}/combat_profession")]
    [ProducesResponseType(typeof(IReadOnlyList<CombatTechniquesDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProfessions(int professionalId)
    {
        var list = await _service.GetProfessionsAsync(professionalId, HttpContext.RequestAborted);
        return Ok(list);
    }

    /// <summary>Lista tecnicas da especialização.</summary>
    [HttpGet("views/{especializationId:int}/combat_especialization")]
    [ProducesResponseType(typeof(IReadOnlyList<CombatTechniquesDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEspecializations(int especializationId)
    {
        var list = await _service.GetEspecializationsAsync(especializationId, HttpContext.RequestAborted);
        return Ok(list);
    }

}
