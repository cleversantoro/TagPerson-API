using TagPerson.Application.DTOs;
using TagPerson.Domain.Entities;

namespace TagPerson.Application.Interfaces.Repositories;

public interface ICombatRepository
{
    Task<IReadOnlyList<CombatGroup>> GetGroupParentsAsync(CancellationToken ct);
    Task<IReadOnlyList<CombatGroup>> GetGroupChildrenAsync(int parentId, CancellationToken ct);
    Task<IReadOnlyList<CombatFromGroupDto>> GetCombatFromGroupAsync(int groupId, CancellationToken ct);
    Task<CombatSkill?> GetByIdAsync(int combatSkillId, CancellationToken ct);
}

public interface ICombatSkillRepository : ICombatRepository
{
}
