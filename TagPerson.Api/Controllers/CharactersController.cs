using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TagPerson.Application.DTOs;
using TagPerson.Application.Interfaces;

namespace TagPerson.Api.Controllers;

/// <summary>Controlador de personagens.</summary>
[ApiController]
[Route("api/characters")]
[Authorize]
public class CharactersController : ControllerBase
{
    /// <summary>Endpoints de personagens.</summary>
    private readonly ICharacterService _service;

    /// <summary>Cria o controller de personagens.</summary>
    public CharactersController(ICharacterService service)
    {
        _service = service;
    }

    /// <summary>Lista personagens com dados resumidos.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<CharacterListItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List()
    {
        var list = await _service.ListAsync(HttpContext.RequestAborted);
        return Ok(list);
    }

    /// <summary>Obtem a ficha completa do personagem.</summary>
    [HttpGet("{id:int}/sheet")]
    [ProducesResponseType(typeof(CharacterSheetDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Sheet(int id)
    {
        var dto = await _service.GetSheetAsync(id, HttpContext.RequestAborted);
        return dto is null ? NotFound() : Ok(dto);
    }

    /// <summary>Cria um personagem.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(CharacterSheetDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateCharacterRequestDto req)
    {
        var created = await _service.CreateAsync(req, HttpContext.RequestAborted);
        return CreatedAtAction(nameof(Sheet), new { id = created.Id }, created);
    }

    /// <summary>Atualiza dados basicos do personagem.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCharacterRequestDto req)
    {
        var updated = await _service.UpdateAsync(id, req, HttpContext.RequestAborted);
        return updated ? NoContent() : NotFound();
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

    /// <summary>Exclui um personagem.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _service.DeleteAsync(id, HttpContext.RequestAborted);
        return ok ? NoContent() : NotFound();
    }
}
