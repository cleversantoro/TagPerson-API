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

public sealed record PlaceDto(int? Id,string? Name);

public sealed record PlaceLookupDto(int Id,string Name);

public sealed record ClassSocialDto(int? Id, string? Name);

public sealed record ClassSocialLookupDto(int Id, string Name);

public sealed record DeityDto(int? Id, string? Name);

public sealed record DeityLookupDto(int Id, string Name);

public sealed record TimeLineDto(int Id, string? Name, int? Year, string? Occurrence);

public sealed record TimeLineLookupDto(
    int Id, 
    SimpleLookupDto? Place,
    int? Year, 
    string? Occurrence
);
