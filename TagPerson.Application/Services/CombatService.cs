using TagPerson.Application.DTOs;
using TagPerson.Application.Interfaces;
using TagPerson.Application.Interfaces.Repositories;

namespace TagPerson.Application.Services;

public sealed class CombatService : ICombatService
{
    private readonly ICombatRepository _repo;

    public CombatService(ICombatRepository repo)
    {
        _repo = repo;
    }

    public async Task<IReadOnlyList<CombatGroupDto>> GetGroupParentsAsync(CancellationToken ct)
    {
        var items = await _repo.GetGroupParentsAsync(ct);
        return items.Select(x => new CombatGroupDto(x.Id, x.Name, x.ParentId)).ToList();
    }

    public async Task<IReadOnlyList<CombatGroupDto>> GetGroupChildrenAsync(int parentId, CancellationToken ct)
    {
        var items = await _repo.GetGroupChildrenAsync(parentId, ct);
        return items.Select(x => new CombatGroupDto(x.Id, x.Name, x.ParentId)).ToList();
    }

    public Task<IReadOnlyList<CombatFromGroupDto>> GetCombatFromGroupAsync(int groupId, CancellationToken ct)
    {
        return _repo.GetCombatFromGroupAsync(groupId, ct);
    }
}
