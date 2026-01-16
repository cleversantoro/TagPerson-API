using System.Xml.Linq;
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

    public async Task<IReadOnlyList<EquipmentDefenseDto>> GetArmorsAsync(CancellationToken ct)
    {
        var armors = await _repo.GetArmorAsync(ct);
        return armors.Select(x => new EquipmentDefenseDto(
                x.Id,
                x.Name,
                x.Description,
                x.Value,
                x.BaseDefense,
                x.Absorption,
                x.MinPhysic,
                x.MinStrength,
                x.P,
                x.A,
                x.E,
                x.M,
                x.H
            )
        )
        .ToList();
    }

    public async Task<IReadOnlyList<EquipmentBelongingsDto>> GetBelongingsAsync(CancellationToken ct)
    {
        var s = await _repo.GetBelongingsAsync(ct);
        return s.Select(x => new EquipmentBelongingsDto(
                x.Id,
                x.Name,
                x.Description,
                x.Value
            )
        )
        .ToList();
    }

    public async Task<IReadOnlyList<EquipmentDefenseDto>> GetDefenseAsync(CancellationToken ct)
    {
        var s = await _repo.GetDefenseAsync(ct);
        return s.Select(x => new EquipmentDefenseDto(
                x.Id,
                x.Name,
                x.Description,
                x.Value,
                x.BaseDefense,
                x.Absorption,
                x.MinPhysic,
                x.MinStrength,
                x.P,
                x.A,
                x.E,
                x.M,
                x.H
            )
        )
        .ToList();
    }

    public async Task<IReadOnlyList<EquipmentDefenseDto>> GetHelmetsAsync(CancellationToken ct)
    {
        var s = await _repo.GetHelmetAsync(ct);
        return s.Select(x => new EquipmentDefenseDto(
                x.Id,
                x.Name,
                x.Description,
                x.Value,
                x.BaseDefense,
                x.Absorption,
                x.MinPhysic,
                x.MinStrength,
                x.P,
                x.A,
                x.E,
                x.M,
                x.H
            )
        )
        .ToList();
    }

    public async Task<IReadOnlyList<EquipmentDefenseDto>> GetShieldsAsync(CancellationToken ct)
    {
        var s = await _repo.GetShieldsAsync(ct);
        return s.Select(x => new EquipmentDefenseDto(
                x.Id,
                x.Name,
                x.Description,
                x.Value,
                x.BaseDefense,
                x.Absorption,
                x.MinPhysic,
                x.MinStrength,
                x.P,
                x.A,
                x.E,
                x.M,
                x.H
            )
        )
        .ToList();
    }

    public async Task<IReadOnlyList<EquipmentWeaponsDto>> GetWeaponsAsync(CancellationToken ct)
    {
        var s = await _repo.GetWeaponsAsync(ct);
        return s.Select(x => new EquipmentWeaponsDto(
                x.Id,
                x.Name,
                x.Description,
                x.Value,
                x.WeaponType,
                x.Cost,
                x.Range,
                x.MinStrength,
                x.Bonus,
                x.L,
                x.M,
                x.P,
                x.V25,
                x.V50,
                x.V75,
                x.V100,
                x.Pq,
                x.An,
                x.El,
                x.Me,
                x.Hu
            )
        )
        .ToList();
    }
}
