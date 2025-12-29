using Microsoft.EntityFrameworkCore;
using TagPerson.Application.Interfaces.Repositories;
using TagPerson.Domain.Entities;
using TagPerson.Infrastructure.Data;

namespace TagPerson.Infrastructure.Repositories;

public sealed class LookupRepository : ILookupRepository
{
    private readonly AppDbContext _db;

    public LookupRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<Race>> RacesAsync(CancellationToken ct) =>
        await _db.Races.AsNoTracking().OrderBy(x => x.Name).ToListAsync(ct);

    public async Task<IReadOnlyList<Profession>> ProfessionsAsync(CancellationToken ct) =>
        await _db.Professions.AsNoTracking().OrderBy(x => x.Name).ToListAsync(ct);

    public async Task<IReadOnlyList<Skill>> SkillsAsync(CancellationToken ct) =>
        await _db.Skills.AsNoTracking().OrderBy(x => x.Name).ToListAsync(ct);

    public async Task<IReadOnlyList<Spell>> SpellsAsync(CancellationToken ct) =>
        await _db.Spells.AsNoTracking().OrderBy(x => x.Name).ToListAsync(ct);

    public async Task<IReadOnlyList<CombatSkill>> CombatAsync(CancellationToken ct) =>
        await _db.CombatSkills.AsNoTracking().OrderBy(x => x.Name).ToListAsync(ct);

    public async Task<IReadOnlyList<Equipment>> EquipmentsAsync(CancellationToken ct) =>
        await _db.Equipments.AsNoTracking().OrderBy(x => x.Name).ToListAsync(ct);

    public async Task<IReadOnlyList<Category>> CategoriesAsync(CancellationToken ct) =>
        await _db.Categories.AsNoTracking().OrderBy(x => x.Name).ToListAsync(ct);
}
