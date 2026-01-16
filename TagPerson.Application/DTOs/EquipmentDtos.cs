using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TagPerson.Domain.Entities;

namespace TagPerson.Application.DTOs;


public sealed record EquipmentLookupDto(
    int Id,
    string Name,
    int? IsWeapon,
    int? IsDefense,
    int? IsArmor,
    int? IsShield,
    int? IsHelmet
);

public sealed record EquipmentGroupDto(int Id, string Name);

public sealed record EquipmentFromGroupDto(
    int Id,
    string Name,
    string? Description,
    int? Value,
    int? GroupId
);

public sealed record EquipmentWeaponsDto(
    int Id,
    string Name,
    string? Description,
    int? Value,
    string? WeaponType,
    int? Cost,
    string? Range,
    int? MinStrength,
    string? Bonus,
    int? L,
    int? M,
    int? P,
    int? V25,
    int? V50,
    int? V75,
    int? V100,
    string? Pq,
    string? An,
    string? El,
    string? Me,
    string? Hu
);

public sealed record EquipmentDefenseDto(
    int Id,
    string Name,
    string? Description,
    int? Value,
    string? BaseDefense,
    int? Absorption,
    int? MinPhysic,
    int? MinStrength,
    int? P,
    int? A,
    int? E,
    int? M,
    int? H
);

public sealed record EquipmentBelongingsDto(
    int Id,
    string Name,
    string? Description,
    int? Value
);
