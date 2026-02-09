using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TagPerson.Application.DTOs;
using TagPerson.Application.Interfaces;
using TagPerson.Domain.Entities;

namespace TagPerson.Api.Controllers;

/// <summary>Controlador de personagens.</summary>
[ApiController]
[Route("api/characters_atributes")]
[Authorize]
public class CharactersAtributesController : ControllerBase
{
    /// <summary>Endpoints de personagens.</summary>
    private readonly ICharacterService _service;

    /// <summary>Cria o controller de personagens.</summary>
    public CharactersAtributesController(ICharacterService service)
    {
        _service = service;
    }

    /// <summary>Valida a distribuição de atributos do personagem.</summary>
    [HttpPost("{id:int}/validate-attributes")]
    [ProducesResponseType(typeof(AttributeDistributionResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ValidateAttributes(int id, [FromBody] AttributeDistributionRequestDto req)
    {
        var result = await _service.ValidateAttributeDistributionAsync(id, req, HttpContext.RequestAborted);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Aplica a distribuição de atributos do personagem.</summary>
    [HttpPost("{id:int}/apply-attributes")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ApplyAttributes(int id, [FromBody] AttributeDistributionRequestDto req)
    {
        var result = await _service.ApplyAttributeDistributionAsync(id, req, HttpContext.RequestAborted);
        return result.success ? NoContent() : BadRequest(result.message);
    }
}
