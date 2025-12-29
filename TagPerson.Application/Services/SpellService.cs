using TagPerson.Application.DTOs;
using TagPerson.Application.Interfaces;
using TagPerson.Application.Interfaces.Repositories;

namespace TagPerson.Application.Services;

public sealed class SpellService : ISpellService
{
    private readonly ISpellRepository _repo;

    public SpellService(ISpellRepository repo)
    {
        _repo = repo;
    }

    public async Task<IReadOnlyList<SpellGroupDto>> GetGroupParentsAsync(CancellationToken ct)
    {
        var items = await _repo.GetGroupParentsAsync(ct);
        return items.Select(x => new SpellGroupDto(x.Id, x.Name, x.ParentId)).ToList();
    }

    public async Task<IReadOnlyList<SpellGroupDto>> GetGroupChildrenAsync(int parentId, CancellationToken ct)
    {
        var items = await _repo.GetGroupChildrenAsync(parentId, ct);
        return items.Select(x => new SpellGroupDto(x.Id, x.Name, x.ParentId)).ToList();
    }

    public Task<IReadOnlyList<SpellFromGroupDto>> GetSpellsFromGroupAsync(int groupId, CancellationToken ct)
    {
        return _repo.GetSpellsFromGroupAsync(groupId, ct);
    }
}
