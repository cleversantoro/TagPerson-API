using TagPerson.Application.DTOs;

namespace TagPerson.Application.Interfaces;

public interface ISpellService
{
    Task<IReadOnlyList<SpellGroupDto>> GetGroupParentsAsync(CancellationToken ct);
    Task<IReadOnlyList<SpellGroupDto>> GetGroupChildrenAsync(int parentId, CancellationToken ct);
    Task<IReadOnlyList<SpellFromGroupDto>> GetSpellsFromGroupAsync(int groupId, CancellationToken ct);
}
