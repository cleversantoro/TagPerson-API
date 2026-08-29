using Microsoft.EntityFrameworkCore;
using TagPerson.Application.DTOs;
using TagPerson.Application.Interfaces.Repositories;
using TagPerson.Domain.Entities;
using TagPerson.Infrastructure.Data;

namespace TagPerson.Infrastructure.Repositories;

public sealed class SkillRepository : ISkillRepository
{
    private readonly AppDbContext _db;

    public SkillRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<SkillGroup>> GetGroupParentsAsync(CancellationToken ct)
    {
        return await _db.SkillGroups
            .AsNoTracking()
            .Where(x => x.ParentId == null || x.ParentId == -1)
            .OrderBy(x => x.Name)
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<SkillGroup>> GetGroupChildrenAsync(int parentId, CancellationToken ct)
    {
        return await _db.SkillGroups
            .AsNoTracking()
            .Where(x => x.ParentId == parentId)
            .OrderBy(x => x.Name)
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<SkillFromGroupDto>> GetSkillsFromGroupAsync(int groupId, CancellationToken ct)
    {
        return await _db.Skills
            .AsNoTracking()
            .Where(x => x.SkillGroupId == groupId)
            .Join(_db.SkillGroupCosts.AsNoTracking(),
                skill => skill.Id,
                cost => cost.SkillId,
                (skill, cost) => new { skill, cost })
            .OrderBy(x => x.skill.Name)
            .Select(x => new SkillFromGroupDto(
                x.skill.Id,
                x.skill.Name,
                x.cost.Cost,
                x.skill.Bonus,
                x.skill.HasSpecialization,
                x.skill.Restricted,
                x.skill.AttributeCode
            ))
            .ToListAsync(ct);
    }

    public async Task<SkillDto?> GetSkillByIdAsync(int id, CancellationToken ct)
    {
        return await _db.Skills
            .AsNoTracking()
            .Where(h => h.Id == id)
            .Join(_db.SkillGroups.AsNoTracking(),
                h => h.SkillGroupId,
                hg => hg.Id,
                (h, hg) => new { h, hg })
            .Join(_db.SkillGroupCosts.AsNoTracking(),
                x => x.h.Id,
                hgc => hgc.SkillId,
                (x, hgc) => new { x.h, x.hg, hgc })
            .OrderBy(x => x.h.Name)
            .Select(x => new SkillDto(
                x.h.Id,
                x.h.Name,
                x.h.SkillGroupId,
                x.hg.Name,
                x.h.Description,
                x.h.AttributeCode,
                x.h.LevelTest,
                x.h.Restricted,
                x.h.Penalties,
                x.h.ImprovedTasks,
                x.h.LevelsJson,
                x.h.Bonus,
                x.h.HasSpecialization,
                x.hgc.Cost
            ))
            .FirstOrDefaultAsync(ct);
    }

    public async Task<IReadOnlyList<SkillSpecializationSuggestionDto>> GetSpecializationSuggestionsAsync(int skillId, CancellationToken ct)
    {
        return await _db.SkillSpecialization
            .AsNoTracking()
            .Where(x => x.SkillId == skillId)
            .OrderBy(x => x.Suggestion)
            .Select(x => new SkillSpecializationSuggestionDto(x.Id, x.SkillId ?? skillId, x.Suggestion))
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<SkillImprovedDto>> GetImprovedAsync(int skillId, CancellationToken ct)
    {
        return await _db.SkillImproved
            .AsNoTracking()
            .Where(x => x.SkillId == skillId)
            .OrderBy(x => x.Description)
            .Select(x => new SkillImprovedDto(x.Id, x.SkillId ?? skillId, x.SkillGroupId, x.Description))
            .ToListAsync(ct);
    }

    public async Task<Skill?> GetByIdAsync(int skillId, CancellationToken ct)
    {
        return await _db.Skills.FirstOrDefaultAsync(x => x.Id == skillId, ct);
    }

}
