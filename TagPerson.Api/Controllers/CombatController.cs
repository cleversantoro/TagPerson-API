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

    /// <summary>Lista grupos pais.</summary>
    [HttpGet("groups")]
    [ProducesResponseType(typeof(IReadOnlyList<CombatGroupDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GroupParents()
    {
        var list = await _service.GetGroupParentsAsync(HttpContext.RequestAborted);
        return Ok(list);
    }

    /// <summary>Lista grupos filhos de um pai.</summary>
    [HttpGet("groups/{parentId:int}/children")]
    [ProducesResponseType(typeof(IReadOnlyList<CombatGroupDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GroupChildren(int parentId)
    {
        var list = await _service.GetGroupChildrenAsync(parentId, HttpContext.RequestAborted);
        return Ok(list);
    }

    /// <summary>Lista tecnicas por grupo.</summary>
    [HttpGet("groups/{groupId:int}/items")]
    [ProducesResponseType(typeof(IReadOnlyList<CombatFromGroupDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ItemsFromGroup(int groupId)
    {
        var list = await _service.GetCombatFromGroupAsync(groupId, HttpContext.RequestAborted);
        return Ok(list);
    }
}
