using TagPerson.Application.DTOs;

namespace TagPerson.Application.Interfaces;

public interface ICombatService
{
    Task<IReadOnlyList<CombatGroupDto>> GetGroupParentsAsync(CancellationToken ct);
    Task<IReadOnlyList<CombatGroupDto>> GetGroupChildrenAsync(int parentId, CancellationToken ct);
    Task<IReadOnlyList<CombatFromGroupDto>> GetCombatFromGroupAsync(int groupId, CancellationToken ct);
}
