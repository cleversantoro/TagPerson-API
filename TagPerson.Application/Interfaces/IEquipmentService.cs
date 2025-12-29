using TagPerson.Application.DTOs;

namespace TagPerson.Application.Interfaces;

public interface IEquipmentService
{
    Task<IReadOnlyList<EquipmentGroupDto>> GetGroupsAsync(CancellationToken ct);
    Task<IReadOnlyList<EquipmentFromGroupDto>> GetItemsByGroupAsync(int groupId, CancellationToken ct);
}
