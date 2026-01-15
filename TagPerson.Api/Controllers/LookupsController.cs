using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TagPerson.Application.DTOs;
using TagPerson.Application.Interfaces;

namespace TagPerson.Api.Controllers;

/// <summary>Controlador de listas de apoio.</summary>
[ApiController]
[Route("api/lookups")]
[Authorize]
public class LookupsController : ControllerBase
{
    /// <summary>Endpoints de listas de apoio.</summary>
    private readonly ILookupService _service;

    /// <summary>Cria o controller de listas de apoio.</summary>
    public LookupsController(ILookupService service)
    {
        _service = service;
    }

    /// <summary>Lista racas.</summary>
    [HttpGet("races")]
    [ProducesResponseType(typeof(IReadOnlyList<SimpleLookupDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Races() =>
        Ok(await _service.RacesAsync(HttpContext.RequestAborted));

    /// <summary>Lista profissoes.</summary>
    [HttpGet("professions")]
    [ProducesResponseType(typeof(IReadOnlyList<SimpleLookupDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Professions() =>
        Ok(await _service.ProfessionsAsync(HttpContext.RequestAborted));

    /// <summary>Lista habilidades.</summary>
    [HttpGet("skills")]
    [ProducesResponseType(typeof(IReadOnlyList<SkillLookupDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Skills() =>
        Ok(await _service.SkillsAsync(HttpContext.RequestAborted));

    /// <summary>Lista magias.</summary>
    [HttpGet("spells")]
    [ProducesResponseType(typeof(IReadOnlyList<SimpleLookupDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Spells() =>
        Ok(await _service.SpellsAsync(HttpContext.RequestAborted));

    /// <summary>Lista tecnicas de combate.</summary>
    [HttpGet("combat")]
    [ProducesResponseType(typeof(IReadOnlyList<SimpleLookupDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Combat() =>
        Ok(await _service.CombatAsync(HttpContext.RequestAborted));

    /// <summary>Lista equipamentos.</summary>
    [HttpGet("equipments")]
    [ProducesResponseType(typeof(IReadOnlyList<EquipmentLookupDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Equipments() =>
        Ok(await _service.EquipmentsAsync(HttpContext.RequestAborted));

    /// <summary>Lista categorias de combate.</summary>
    [HttpGet("categories")]
    [ProducesResponseType(typeof(IReadOnlyList<CategoryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Categories() =>
        Ok(await _service.CategoriesAsync(HttpContext.RequestAborted));

    /// <summary>Lista de caracterizações para personagem.</summary>
    [HttpGet("characterization")]
    [ProducesResponseType(typeof(IReadOnlyList<CharacterizationLookupDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Characterization() => 
        Ok(await _service.CharacterizationAsync(HttpContext.RequestAborted));

    /// <summary>Lista de Localidade.</summary>
    [HttpGet("place")]
    [ProducesResponseType(typeof(IReadOnlyList<PlaceLookupDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Place() =>
        Ok(await _service.PlaceAsync(HttpContext.RequestAborted));

    /// <summary>Lista de Classe Social.</summary>
    [HttpGet("socialclass")]
    [ProducesResponseType(typeof(IReadOnlyList<ClassSocialLookupDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ClassSocial() =>
        Ok(await _service.ClassSocialAsync(HttpContext.RequestAborted));

    /// <summary>Lista de Divindades.</summary>
    [HttpGet("deity")]
    [ProducesResponseType(typeof(IReadOnlyList<DeityLookupDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Deity() =>
        Ok(await _service.DeityAsync(HttpContext.RequestAborted));

    /// <summary>Lista de Linha do Tempo.</summary>
    [HttpGet("timeline")]
    [ProducesResponseType(typeof(IReadOnlyList<TimeLineLookupDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> TimeLine() =>
        Ok(await _service.TimeLineAsync(HttpContext.RequestAborted));

    /// <summary>Lista de especializações.</summary>
    [HttpGet("specialization")]
    [ProducesResponseType(typeof(IReadOnlyList<SimpleLookupDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Specialization() =>
        Ok(await _service.SpecializationAsync(HttpContext.RequestAborted));


}
