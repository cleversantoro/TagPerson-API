using TagPerson.Application.DTOs;

namespace TagPerson.Application.Interfaces;

public interface IRaceProfessionService
{
    Task<IReadOnlyList<ProfessionDto>> GetProfessionsByRaceAsync(int raceId, CancellationToken ct);
    Task<IReadOnlyList<RaceDto>> GetRacesByProfessionAsync(int professionId, CancellationToken ct);
}
