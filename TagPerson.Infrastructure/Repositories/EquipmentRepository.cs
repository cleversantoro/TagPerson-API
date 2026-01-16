using Microsoft.EntityFrameworkCore;
using TagPerson.Application.DTOs;
using TagPerson.Application.Interfaces.Repositories;
using TagPerson.Domain.Entities;
using TagPerson.Infrastructure.Data;

namespace TagPerson.Infrastructure.Repositories;

public sealed class EquipmentRepository : IEquipmentRepository
{
    private readonly AppDbContext _db;

    public EquipmentRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<EquipmentFromGroupDto>> GetItemsByGroupAsync(int groupId, CancellationToken ct)
    {
        return await _db.Equipments
            .AsNoTracking()
            .Where(x => x.GroupId == groupId)
            .OrderBy(x => x.Name)
            .Select(x => new EquipmentFromGroupDto(
                x.Id,
                x.Name,
                x.Description,
                x.Price,
                x.GroupId
            ))
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<EquipmentGroup>> GetGroupsAsync(CancellationToken ct)
    {
        return await _db.EquipmentGroups
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<EquipmentWeaponsDto>> GetWeaponsAsync(CancellationToken ct)
    {
        return await _db.Equipments
            .AsNoTracking()
            .Where(e => e.IsWeapon == 1)
            .Include(e => e.WeaponStats)
            .OrderBy(e => e.Name)
            .Select(e => new EquipmentWeaponsDto(
                e.Id,
                e.Name,
                e.Description,
                e.Price,
                e.WeaponStats!.WeaponType,
                e.WeaponStats!.Cost,
                e.WeaponStats!.Range,
                e.WeaponStats!.MinStrength,
                e.WeaponStats!.Bonus,
                e.WeaponStats!.L,
                e.WeaponStats!.M,
                e.WeaponStats!.P,
                e.WeaponStats!.V25,
                e.WeaponStats!.V50,
                e.WeaponStats!.V75,
                e.WeaponStats!.V100,
                e.WeaponStats!.Pq,
                e.WeaponStats!.An,
                e.WeaponStats!.El,
                e.WeaponStats!.Me,
                e.WeaponStats!.Hu
            ))
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<EquipmentDefenseDto>> GetDefenseAsync(CancellationToken ct)
    {
        return await _db.Equipments
            .AsNoTracking()
            .Where(e => e.IsDefense == 1)
            .Include(e => e.DefenseStats)
            .OrderBy(e => e.Name)
            .Select(e => new EquipmentDefenseDto(
                e.Id,
                e.Name,
                e.Description,
                e.Price,
                e.DefenseStats!.BaseDefense,
                e.DefenseStats!.Absorption,
                e.DefenseStats!.MinPhysic,
                e.DefenseStats!.MinStrength,
                e.DefenseStats!.P,
                e.DefenseStats!.A,
                e.DefenseStats!.E,
                e.DefenseStats!.M,
                e.DefenseStats!.H
            ))
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<EquipmentDefenseDto>> GetArmorAsync(CancellationToken ct)
    {
        return await _db.Equipments
            .AsNoTracking()
            .Where(e => e.IsArmor == 1)
            .Include(e => e.DefenseStats)
            .OrderBy(e => e.Name)
            .Select(e => new EquipmentDefenseDto(
                e.Id,
                e.Name,
                e.Description,
                e.Price,
                e.DefenseStats!.BaseDefense,
                e.DefenseStats!.Absorption,
                e.DefenseStats!.MinPhysic,
                e.DefenseStats!.MinStrength,
                e.DefenseStats!.P,
                e.DefenseStats!.A,
                e.DefenseStats!.E,
                e.DefenseStats!.M,
                e.DefenseStats!.H
            ))
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<EquipmentDefenseDto>> GetHelmetAsync(CancellationToken ct)
    {
        return await _db.Equipments
            .AsNoTracking()
            .Where(e => e.IsHelmet == 1)
            .Include(e => e.DefenseStats)
            .OrderBy(e => e.Name)
            .Select(e => new EquipmentDefenseDto(
                e.Id,
                e.Name,
                e.Description,
                e.Price,
                e.DefenseStats!.BaseDefense,
                e.DefenseStats!.Absorption,
                e.DefenseStats!.MinPhysic,
                e.DefenseStats!.MinStrength,
                e.DefenseStats!.P,
                e.DefenseStats!.A,
                e.DefenseStats!.E,
                e.DefenseStats!.M,
                e.DefenseStats!.H
            ))
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<EquipmentDefenseDto>> GetShieldsAsync(CancellationToken ct)
    {
        return await _db.Equipments
            .AsNoTracking()
            .Where(e => e.IsShield == 1)
            .Include(e => e.DefenseStats)
            .OrderBy(e => e.Name)
            .Select(e => new EquipmentDefenseDto(
                e.Id,
                e.Name,
                e.Description,
                e.Price,
                e.DefenseStats!.BaseDefense,
                e.DefenseStats!.Absorption,
                e.DefenseStats!.MinPhysic,
                e.DefenseStats!.MinStrength,
                e.DefenseStats!.P,
                e.DefenseStats!.A,
                e.DefenseStats!.E,
                e.DefenseStats!.M,
                e.DefenseStats!.H
            ))
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<EquipmentBelongingsDto>> GetBelongingsAsync(CancellationToken ct)
    {
        return await _db.Equipments
            .AsNoTracking()
            .Where(e => e.IsWeapon == 0 && e.IsDefense == 0 && e.IsArmor ==0 && e.IsShield == 0 && e.IsHelmet == 0)
            .OrderBy(e => e.Name)
            .Select(e => new EquipmentBelongingsDto(e.Id, e.Name, e.Description, e.Price))
            .ToListAsync(ct);
    }

}
