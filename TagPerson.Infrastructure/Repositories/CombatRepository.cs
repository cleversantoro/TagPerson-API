using Microsoft.EntityFrameworkCore;
using TagPerson.Application.DTOs;
using TagPerson.Application.Interfaces.Repositories;
using TagPerson.Domain.Entities;
using TagPerson.Infrastructure.Data;

namespace TagPerson.Infrastructure.Repositories;

public sealed class CombatRepository : ICombatRepository
{
    private readonly AppDbContext _db;

    public CombatRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<CombatGroup>> GetGroupParentsAsync(CancellationToken ct)
    {
        return await _db.CombatGroups
            .AsNoTracking()
            //.Where(x => x.ParentId == null || x.ParentId == -1)
            .OrderBy(x => x.Name)
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<CombatGroup>> GetGroupChildrenAsync(int parentId, CancellationToken ct)
    {
        return await _db.CombatGroups
            .AsNoTracking()
            .Where(x => x.ParentId == parentId)
            .OrderBy(x => x.Name)
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<CombatFromGroupDto>> GetCombatFromGroupAsync(int groupId, CancellationToken ct)
    {
        return await _db.CombatGroupCosts
            .AsNoTracking()
            .Where(x => x.CombatGroupId == groupId)
            .Join(_db.CombatSkills.AsNoTracking(),
                combatgroupcost => combatgroupcost.CombatSkillId,
                combat => combat.Id,
                (combatgroupcost, combat) => new { combatgroupcost, combat })
            .OrderBy(x => x.combat.Name)
            .Select(x => new CombatFromGroupDto(
                x.combat.Id,
                x.combat.Name,
                x.combatgroupcost.Cost,
                x.combat.Bonus,
                x.combatgroupcost.Reduction,
                x.combat.HasSpecialization,
                x.combat.AttributeCode,
                x.combat.CategoryId,
                x.combat.Effect,
                x.combat.Requisite,
                x.combat.Notes,
                x.combat.RollTable,
                x.combat.Improvement
            ))
            .ToListAsync(ct);
    }
}
