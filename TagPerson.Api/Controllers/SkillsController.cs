using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TagPerson.Application.DTOs;
using TagPerson.Application.Interfaces;

namespace TagPerson.Api.Controllers;

/// <summary>Controlador de habilidades e grupos.</summary>
[ApiController]
[Route("api/skills")]
[Authorize]
public class SkillsController : ControllerBase
{
    private readonly ISkillService _service;

    /// <summary>Cria o controller de habilidades.</summary>
    public SkillsController(ISkillService service)
    {
        _service = service;
    }

    /// <summary>Lista os grupos.</summary>
    [HttpGet("groups")]
    [ProducesResponseType(typeof(IReadOnlyList<SkillGroupDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GroupParents()
    {
        var list = await _service.GetGroupParentsAsync(HttpContext.RequestAborted);
        return Ok(list);
    }

    /// <summary>Lista grupos pelo seu pai.</summary>
    /// <param name="parentId">ID do grupo pai.</param>
    [HttpGet("groups/{parentId:int}/children")]
    [ProducesResponseType(typeof(IReadOnlyList<SkillGroupDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GroupChildren(int parentId)
    {
        var list = await _service.GetGroupChildrenAsync(parentId, HttpContext.RequestAborted);
        return Ok(list);
    }

    /// <summary>Lista habilidades por grupo.</summary>
    [HttpGet("groups/{groupId:int}/skills")]
    [ProducesResponseType(typeof(IReadOnlyList<SkillFromGroupDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SkillsFromGroup(int groupId)
    {
        var list = await _service.GetSkillsFromGroupAsync(groupId, HttpContext.RequestAborted);
        return Ok(list);
    }

    /// <summary>Lista sugestoes de especializacao da habilidade.</summary>
    [HttpGet("{skillId:int}/specializations")]
    [ProducesResponseType(typeof(IReadOnlyList<SkillSpecializationSuggestionDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Specializations(int skillId)
    {
        var list = await _service.GetSpecializationSuggestionsAsync(skillId, HttpContext.RequestAborted);
        return Ok(list);
    }
}
