using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TagPerson.Application.DTOs;
using TagPerson.Application.Interfaces;
using TagPerson.Domain.Entities;

namespace TagPerson.Api.Controllers;

/// <summary>Controlador de personagens.</summary>
[ApiController]
[Route("api/characters_equipment")]
[Authorize]
public class CharactersEquipmentController : ControllerBase
{
    /// <summary>Endpoints de personagens.</summary>
    private readonly ICharacterService _service;

    /// <summary>Cria o controller de personagens.</summary>
    public CharactersEquipmentController(ICharacterService service)
    {
        _service = service;
    }

    /// <summary>Adiciona equipamento ao personagem.</summary>
    [HttpPost("{id:int}/equipments")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddEquipment(int id, [FromBody] CharacterEquipmentRequestDto req)
    {
        var ok = await _service.AddEquipmentAsync(id, req, HttpContext.RequestAborted);
        if (!ok)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>Seleciona os equipamentos do personagem.</summary>
    [HttpGet("{id:int}/equipments")]
    [ProducesResponseType(typeof(IReadOnlyList<CharacterEquipmentDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCharacterEquipments(int id)
    {
        var list = await _service.GetCharacterEquipmentsAsync(id, HttpContext.RequestAborted);
        return Ok(list);
    }

    /// <summary>Exclui um equipamento do personagem.</summary>
    [HttpDelete("{id:int}/equipments")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCharacterEquipments(int id, int equipmentId)
    {
        var ok = await _service.DeleteCharacterEquipmentsAsync(id, equipmentId, HttpContext.RequestAborted);
        return ok ? NoContent() : NotFound();
    }

}
