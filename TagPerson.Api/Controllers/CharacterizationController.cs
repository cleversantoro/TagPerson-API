using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TagPerson.Application.DTOs;
using TagPerson.Application.Interfaces;

namespace TagPerson.Api.Controllers;

/// <summary>Controlador de caracterização.</summary>
[ApiController]
[Route("api/characterization")]
[Authorize]
public class CharacterizationController : ControllerBase
{
    private readonly ICharacterizationService _service;

    /// <summary>Cria o controller de caracterização.</summary>
    public CharacterizationController(ICharacterizationService service)
    {
        _service = service;
    }

    /// <summary>Lista os tipos de Caracterização.</summary>
    [HttpGet("types")]
    [ProducesResponseType(typeof(IReadOnlyList<CharacterizationTypeDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Types()
    {
        var list = await _service.GetTypesAsync(HttpContext.RequestAborted);
        return Ok(list);
    }

    /// <summary>Lista os grupos de Caracterização pelo tipo.</summary>
    [HttpGet("groups{typeId:int}")]
    [ProducesResponseType(typeof(IReadOnlyList<CharacterizationGroupDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GroupParents(int typeId)
    {
        var list = await _service.GetGroupAsync(typeId, HttpContext.RequestAborted);
        return Ok(list);
    }

    /// <summary>Lista caracterização pelo tipo e grupo.</summary>
    [HttpGet("{typeId:int}/type/{groupId:int}/group")]
    [ProducesResponseType(typeof(IReadOnlyList<CharacterizationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CharacterizationByTypeAndGroup(int typeId, int groupId)
    {
        var list = await _service.GetCharacterizationAsync(typeId, groupId, HttpContext.RequestAborted);
        return Ok(list);
    }

}
