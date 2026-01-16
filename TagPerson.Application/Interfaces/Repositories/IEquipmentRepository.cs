using TagPerson.Application.DTOs;
using TagPerson.Domain.Entities;

namespace TagPerson.Application.Interfaces.Repositories;

public interface IEquipmentRepository
{
    Task<IReadOnlyList<EquipmentDefenseDto>> GetArmorAsync(CancellationToken ct);
    Task<IReadOnlyList<EquipmentBelongingsDto>> GetBelongingsAsync(CancellationToken ct);
    Task<IReadOnlyList<EquipmentDefenseDto>> GetDefenseAsync(CancellationToken ct);
    Task<IReadOnlyList<EquipmentDefenseDto>> GetHelmetAsync(CancellationToken ct);
    Task<IReadOnlyList<EquipmentDefenseDto>> GetShieldsAsync(CancellationToken ct);
    Task<IReadOnlyList<EquipmentWeaponsDto>> GetWeaponsAsync(CancellationToken ct);
    
    Task<IReadOnlyList<EquipmentGroup>> GetGroupsAsync(CancellationToken ct);
    Task<IReadOnlyList<EquipmentFromGroupDto>> GetItemsByGroupAsync(int groupId, CancellationToken ct);
}
