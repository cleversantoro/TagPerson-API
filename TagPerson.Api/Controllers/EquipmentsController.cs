using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TagPerson.Application.DTOs;
using TagPerson.Application.Interfaces;

namespace TagPerson.Api.Controllers;

/// <summary>Controlador de equipamentos.</summary>
[ApiController]
[Route("api/equipments")]
[Authorize]
public class EquipmentsController : ControllerBase
{
    private readonly IEquipmentService _service;

    /// <summary>Cria o controller de equipamentos.</summary>
    public EquipmentsController(IEquipmentService service)
    {
        _service = service;
    }

    /// <summary>Lista grupos de equipamentos.</summary>
    [HttpGet("groups")]
    [ProducesResponseType(typeof(IReadOnlyList<EquipmentGroupDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Groups()
    {
        var list = await _service.GetGroupsAsync(HttpContext.RequestAborted);
        return Ok(list);
    }

    /// <summary>Lista equipamentos por grupo.</summary>
    [HttpGet("groups/{groupId:int}/items")]
    [ProducesResponseType(typeof(IReadOnlyList<EquipmentFromGroupDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ItemsByGroup(int groupId)
    {
        var list = await _service.GetItemsByGroupAsync(groupId, HttpContext.RequestAborted);
        return Ok(list);
    }
}
