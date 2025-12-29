namespace TagPerson.Application.DTOs;

public sealed record SkillLookupDto(int Id, string Name, string? AttributeCode, int? Restricted);

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
