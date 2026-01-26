using Microsoft.EntityFrameworkCore;
using TagPerson.Application.DTOs;
using TagPerson.Application.Interfaces.Repositories;
using TagPerson.Domain.Entities;
using TagPerson.Infrastructure.Data;

namespace TagPerson.Infrastructure.Repositories;

public sealed class SpellRepository : ISpellRepository
{
    private readonly AppDbContext _db;

    public SpellRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<SpellGroup>> GetGroupParentsAsync(CancellationToken ct)
    {
        return await _db.SpellGroups
            .AsNoTracking()
            //.Where(x => x.ParentId == null || x.ParentId == -1)
            .OrderBy(x => x.Id)
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<SpellFromGroupDto>> GetSpellsFromGroupAsync(int groupId, CancellationToken ct)
    {
        return await _db.SpellGroupCosts
            .AsNoTracking()
            .Where(x => x.SpellGroupId == groupId)
            .Join(_db.Spells.AsNoTracking(),
                cost => cost.SpellId,
                spell => spell.Id,
                (cost, spell) => new { cost, spell })
            .Join(_db.SpellGroups.AsNoTracking(),
                prev => prev.cost.SpellGroupId,
                group => group.Id,
                (prev, group) => new { prev.cost, prev.spell, group })
            .OrderBy(x => x.spell.Name)
            .Select(x => new SpellFromGroupDto(
                x.spell.Id,
                x.spell.Name,
                x.cost.Cost,
                x.spell.Evocation,
                x.spell.Range,
                x.spell.Duration,
                x.spell.Description,
                x.spell.LevelsJson,
                x.group.IsProfession,
                x.group.IsEspecialization                
            ))
            .ToListAsync(ct);
    }

    public async Task<Spell?> GetByIdAsync(int spellId, CancellationToken ct)
    {
        return await _db.Spells.FirstOrDefaultAsync(x => x.Id == spellId, ct);
    }

    public async Task<IReadOnlyList<SpellProfession>> GetProfessionsAsync(int professionalId, CancellationToken ct)
    {
        return await _db.SpellProfessions
            .AsNoTracking()
            .Where(x => x.ProfEspId == professionalId)
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<SpellEspecialization>> GetEspecializationsAsync(int especializationId, CancellationToken ct)
    {
        return await _db.SpellEspecializations
            .AsNoTracking()
            .Where(x => x.ProfEspId == especializationId)
            .ToListAsync(ct);
    }
}
