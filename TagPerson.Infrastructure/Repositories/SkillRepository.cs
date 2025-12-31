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

    public async Task<IReadOnlyList<SkillSpecializationSuggestionDto>> GetSpecializationSuggestionsAsync(int skillId, CancellationToken ct)
    {
        return await _db.SkillSpecializationSuggestions
            .AsNoTracking()
            .Where(x => x.SkillId == skillId)
            .OrderBy(x => x.Suggestion)
            .Select(x => new SkillSpecializationSuggestionDto(x.Id, x.SkillId ?? skillId, x.Suggestion))
            .ToListAsync(ct);
    }
}
