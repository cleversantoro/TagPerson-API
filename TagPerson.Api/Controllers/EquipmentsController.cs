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

    /// <summary>Lista equipamentos Diversos.</summary>
    [HttpGet("belongings")]
    [ProducesResponseType(typeof(IReadOnlyList<EquipmentBelongingsDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBelongings()
    {
        var list = await _service.GetBelongingsAsync(HttpContext.RequestAborted);
        return Ok(list);
    }

    /// <summary>Lista equipamentos armas.</summary>
    [HttpGet("weapons")]
    [ProducesResponseType(typeof(IReadOnlyList<EquipmentWeaponsDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetWeapons()
    {
        var list = await _service.GetWeaponsAsync(HttpContext.RequestAborted);
        return Ok(list);
    }

    /// <summary>Lista equipamentos defesa.</summary>
    [HttpGet("defense")]
    [ProducesResponseType(typeof(IReadOnlyList<EquipmentDefenseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDefense()
    {
        var list = await _service.GetDefenseAsync(HttpContext.RequestAborted);
        return Ok(list);
    }

    /// <summary>Lista equipamentos armaduras.</summary>
    [HttpGet("armors")]
    [ProducesResponseType(typeof(IReadOnlyList<EquipmentDefenseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetArmors()
    {
        var list = await _service.GetArmorsAsync(HttpContext.RequestAborted);
        return Ok(list);
    }

    /// <summary>Lista equipamentos escudos.</summary>
    [HttpGet("shields")]
    [ProducesResponseType(typeof(IReadOnlyList<EquipmentDefenseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetShields()
    {
        var list = await _service.GetShieldsAsync(HttpContext.RequestAborted);
        return Ok(list);
    }

    /// <summary>Lista equipamentos elmos.</summary>
    [HttpGet("helmets")]
    [ProducesResponseType(typeof(IReadOnlyList<EquipmentDefenseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHelmets()
    {
        var list = await _service.GetHelmetsAsync(HttpContext.RequestAborted);
        return Ok(list);
    }

}
