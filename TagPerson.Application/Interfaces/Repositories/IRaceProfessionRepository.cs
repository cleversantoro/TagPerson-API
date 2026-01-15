using TagPerson.Domain.Entities;

namespace TagPerson.Application.Interfaces.Repositories;

public interface IRaceProfessionRepository
{
    Task<IReadOnlyList<Profession>> GetProfessionsByRaceAsync(int raceId, CancellationToken ct);
    Task<IReadOnlyList<Race>> GetRacesByProfessionAsync(int professionId, CancellationToken ct);
    Task<bool> ExistsAsync(int raceId, int professionId, CancellationToken ct);
}
