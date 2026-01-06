using Microsoft.EntityFrameworkCore;
using TagPerson.Application.Interfaces.Repositories;
using TagPerson.Domain.Entities;
using TagPerson.Infrastructure.Data;

namespace TagPerson.Infrastructure.Repositories;

public sealed class RaceProfessionRepository : IRaceProfessionRepository
{
    private readonly AppDbContext _db;

    public RaceProfessionRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<Profession>> GetProfessionsByRaceAsync(int raceId, CancellationToken ct)
    {
        return await _db.RaceProfessions
            .AsNoTracking()
            .Where(rp => rp.RaceId == raceId)
            .Select(rp => rp.Profession)
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<Race>> GetRacesByProfessionAsync(int professionId, CancellationToken ct)
    {
        return await _db.RaceProfessions
            .AsNoTracking()
            .Where(rp => rp.ProfessionId == professionId)
            .Select(rp => rp.Race)
            .ToListAsync(ct);
    }
}
