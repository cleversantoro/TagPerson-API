using TagPerson.Application.DTOs;

namespace TagPerson.Application.Interfaces;

public interface IEquipmentService
{
    Task<IReadOnlyList<EquipmentWeaponsDto>> GetWeaponsAsync(CancellationToken ct);
    Task<IReadOnlyList<EquipmentDefenseDto>> GetDefenseAsync(CancellationToken ct);
    Task<IReadOnlyList<EquipmentDefenseDto>> GetArmorsAsync(CancellationToken ct);
    Task<IReadOnlyList<EquipmentDefenseDto>> GetShieldsAsync(CancellationToken ct);
    Task<IReadOnlyList<EquipmentDefenseDto>> GetHelmetsAsync(CancellationToken ct);
    Task<IReadOnlyList<EquipmentBelongingsDto>> GetBelongingsAsync(CancellationToken ct);

    Task<IReadOnlyList<EquipmentGroupDto>> GetGroupsAsync(CancellationToken ct);
    Task<IReadOnlyList<EquipmentFromGroupDto>> GetItemsByGroupAsync(int groupId, CancellationToken ct);
}
