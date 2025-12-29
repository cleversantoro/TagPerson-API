using Microsoft.EntityFrameworkCore;
using TagPerson.Application.Interfaces.Repositories;
using TagPerson.Domain.Entities;
using TagPerson.Infrastructure.Data;

namespace TagPerson.Infrastructure.Repositories;

public sealed class CharacterRepository : ICharacterRepository
{
    private readonly AppDbContext _db;

    public CharacterRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<Character>> ListAsync(CancellationToken ct)
    {
        return await _db.Characters
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync(ct);
    }

    public async Task<Character?> GetAsync(int id, CancellationToken ct)
    {
        return await _db.Characters.FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<Character?> GetSheetAsync(int id, CancellationToken ct)
    {
        return await _db.Characters
            .AsNoTracking()
            .Include(x => x.Race)
            .Include(x => x.Profession)
            .Include(x => x.Skills).ThenInclude(x => x.Skill)
            .Include(x => x.Spells).ThenInclude(x => x.Spell)
            .Include(x => x.CombatSkills).ThenInclude(x => x.CombatSkill)
            .Include(x => x.Equipments).ThenInclude(x => x.Equipment).ThenInclude(e => e.DefenseStats)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task AddAsync(Character character, CancellationToken ct)
    {
        await _db.Characters.AddAsync(character, ct);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var entity = await _db.Characters.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (entity is null) return false;
        _db.Characters.Remove(entity);
        await _db.SaveChangesAsync(ct);
        return true;
    }

    public Task SaveChangesAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);
}
