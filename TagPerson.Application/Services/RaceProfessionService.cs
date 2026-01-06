using TagPerson.Application.DTOs;
using TagPerson.Application.Interfaces;
using TagPerson.Application.Interfaces.Repositories;

namespace TagPerson.Application.Services;

public sealed class RaceProfessionService : IRaceProfessionService
{
    private readonly IRaceProfessionRepository _repository;

    public RaceProfessionService(IRaceProfessionRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<SimpleLookupDto>> GetProfessionsByRaceAsync(int raceId, CancellationToken ct)
    {
        var professions = await _repository.GetProfessionsByRaceAsync(raceId, ct);
        return professions
            .Select(p => new SimpleLookupDto(p.Id, p.Name))
            .ToList();
    }

    public async Task<IReadOnlyList<SimpleLookupDto>> GetRacesByProfessionAsync(int professionId, CancellationToken ct)
    {
        var races = await _repository.GetRacesByProfessionAsync(professionId, ct);
        return races
            .Select(r => new SimpleLookupDto(r.Id, r.Name))
            .ToList();
    }
}
