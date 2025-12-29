using TagPerson.Application.DTOs;
using TagPerson.Domain.Entities;

namespace TagPerson.Application.Interfaces.Repositories;

public interface IEquipmentRepository
{
    Task<IReadOnlyList<EquipmentGroup>> GetGroupsAsync(CancellationToken ct);
    Task<IReadOnlyList<EquipmentFromGroupDto>> GetItemsByGroupAsync(int groupId, CancellationToken ct);
}
