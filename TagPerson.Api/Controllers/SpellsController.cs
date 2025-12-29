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

    /// <summary>Lista grupos pais.</summary>
    [HttpGet("groups")]
    [ProducesResponseType(typeof(IReadOnlyList<SpellGroupDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GroupParents()
    {
        var list = await _service.GetGroupParentsAsync(HttpContext.RequestAborted);
        return Ok(list);
    }

    /// <summary>Lista grupos filhos de um pai.</summary>
    [HttpGet("groups/{parentId:int}/children")]
    [ProducesResponseType(typeof(IReadOnlyList<SpellGroupDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GroupChildren(int parentId)
    {
        var list = await _service.GetGroupChildrenAsync(parentId, HttpContext.RequestAborted);
        return Ok(list);
    }

    /// <summary>Lista magias por grupo.</summary>
    [HttpGet("groups/{groupId:int}/spells")]
    [ProducesResponseType(typeof(IReadOnlyList<SpellFromGroupDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SpellsFromGroup(int groupId)
    {
        var list = await _service.GetSpellsFromGroupAsync(groupId, HttpContext.RequestAborted);
        return Ok(list);
    }
}
