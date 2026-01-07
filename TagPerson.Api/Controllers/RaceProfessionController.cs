using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TagPerson.Application.DTOs;
using TagPerson.Application.Interfaces;

namespace TagPerson.Api.Controllers;

/// <summary>Controlador de relacionamentos entre Raça e Profissão.</summary>
[ApiController]
[Route("api/race-professions")]
[Authorize]
public class RaceProfessionController : ControllerBase
{
    /// <summary>Serviço de relacionamento entre raça e profissão.</summary>
    private readonly IRaceProfessionService _service;

    /// <summary>Cria o controller de relacionamento entre raça e profissão.</summary>
    public RaceProfessionController(IRaceProfessionService service)
    {
        _service = service;
    }

    /// <summary>Retorna as profissões de uma raça específica.</summary>
    /// <param name="raceId">ID da raça</param>
    [HttpGet("races/{raceId}/professions")]
    [ProducesResponseType(typeof(IReadOnlyList<RaceDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProfessionsByRace([FromRoute] int raceId) =>
        Ok(await _service.GetProfessionsByRaceAsync(raceId, HttpContext.RequestAborted));

    /// <summary>Retorna as raças de uma profissão específica.</summary>
    /// <param name="professionId">ID da profissão</param>
    [HttpGet("professions/{professionId}/races")]
    [ProducesResponseType(typeof(IReadOnlyList<ProfessionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRacesByProfession([FromRoute] int professionId) =>
        Ok(await _service.GetRacesByProfessionAsync(professionId, HttpContext.RequestAborted));
}
