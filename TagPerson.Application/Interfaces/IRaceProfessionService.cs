using TagPerson.Application.DTOs;

namespace TagPerson.Application.Interfaces;

public interface IRaceProfessionService
{
    Task<IReadOnlyList<SimpleLookupDto>> GetProfessionsByRaceAsync(int raceId, CancellationToken ct);
    Task<IReadOnlyList<SimpleLookupDto>> GetRacesByProfessionAsync(int professionId, CancellationToken ct);
}
