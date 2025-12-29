using TagPerson.Application.DTOs;
using TagPerson.Application.Interfaces;
using TagPerson.Application.Interfaces.Repositories;

namespace TagPerson.Application.Services;

public sealed class EquipmentService : IEquipmentService
{
    private readonly IEquipmentRepository _repo;

    public EquipmentService(IEquipmentRepository repo)
    {
        _repo = repo;
    }

    public async Task<IReadOnlyList<EquipmentGroupDto>> GetGroupsAsync(CancellationToken ct)
    {
        var groups = await _repo.GetGroupsAsync(ct);
        return groups.Select(x => new EquipmentGroupDto(x.Id, x.Name)).ToList();
    }

    public Task<IReadOnlyList<EquipmentFromGroupDto>> GetItemsByGroupAsync(int groupId, CancellationToken ct)
    {
        return _repo.GetItemsByGroupAsync(groupId, ct);
    }
}
