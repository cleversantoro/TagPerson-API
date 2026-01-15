using TagPerson.Application.DTOs;
using TagPerson.Domain.Entities;

namespace TagPerson.Application.Interfaces.Repositories;

public interface ISpellRepository
{
    Task<IReadOnlyList<SpellGroup>> GetGroupParentsAsync(CancellationToken ct);
    Task<IReadOnlyList<SpellGroup>> GetGroupChildrenAsync(int parentId, CancellationToken ct);
    Task<IReadOnlyList<SpellFromGroupDto>> GetSpellsFromGroupAsync(int groupId, CancellationToken ct);
    Task<Spell?> GetByIdAsync(int spellId, CancellationToken ct);
}
