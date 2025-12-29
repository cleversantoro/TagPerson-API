namespace TagPerson.Application.DTOs;

public sealed record CharacterListItemDto(int Id, string Name, int? Level);

public sealed record CharacterAttributesDto(
    int? Agi,
    int? Per,
    int? Int,
    int? Aur,
    int? Car,
    int? For,
    int? Fis
);

public sealed record CharacterPointsDto(
    int? PointsSkill,
    int? PointsWeapon,
    int? PointsCombat,
    int? PointsMagic
);

public sealed record CharacterCoinsDto(int? Copper, int? Silver, int? Gold);

public sealed record CharacterFeaturesDto(
    int? Age,
    int? Height,
    int? Weight,
    string? Eyes,
    string? Hair,
    string? Skin,
    string? Appearance,
    string? History,
    CharacterCoinsDto Coins
);

public sealed record CharacterSkillDto(
    int SkillId,
    string Name,
    int? Level,
    string? AttributeCode,
    int? Restricted,
    int? HasSpecialization
);

public sealed record CharacterSpellDto(
    int SpellId,
    string Name,
    int? Level,
    string? Evocation,
    string? Range,
    string? Duration
);

public sealed record CharacterCombatSkillDto(
    int CombatSkillId,
    string Name,
    int? Level,
    string? AttributeCode
);

public sealed record CharacterEquipmentDto(int EquipmentId, string Name, int? Qty);

public sealed record SimpleLookupDto(int Id, string Name);

public sealed record CategoryDto(int Id, string Name, string? Icon);

public sealed record SkillGroupDto(int Id, string Name, int? ParentId);

public sealed record SkillFromGroupDto(
    int Id,
    string Name,
    int? Cost,
    int? Bonus,
    int? HasSpecialization,
    int? Restricted,
    string? AttributeCode
);

public sealed record SkillSpecializationSuggestionDto(int SkillId, string? Suggestion);

public sealed record CombatGroupDto(int Id, string Name, int? ParentId);

public sealed record CombatFromGroupDto(
    int Id,
    string Name,
    int? Cost,
    int? Bonus,
    int? HasSpecialization,
    string? AttributeCode,
    int? CategoryId
);

public sealed record SpellGroupDto(int Id, string Name, int? ParentId);

public sealed record SpellFromGroupDto(
    int Id,
    string Name,
    int? Cost,
    string? Evocation,
    string? Range,
    string? Duration,
    string? Description,
    string? Effects
);
public sealed record CharacterSheetDto(
    int Id,
    string Name,
    int? Level,
    string? Player,
    SimpleLookupDto? Race,
    SimpleLookupDto? Profession,
    CharacterAttributesDto Attributes,
    CharacterPointsDto Points,
    CharacterFeaturesDto Features,
    DerivedStatsDto Derived,
    IReadOnlyList<CharacterSkillDto> Skills,
    IReadOnlyList<CharacterSpellDto> Spells,
    IReadOnlyList<CharacterCombatSkillDto> Combat,
    IReadOnlyList<CharacterEquipmentDto> Equipments
);

public sealed record CreateCharacterRequestDto(
    [System.ComponentModel.DataAnnotations.Required]
    [System.ComponentModel.DataAnnotations.MaxLength(100)]
    string Name,
    string? Player,
    int? Level,
    int? RaceId,
    int? ProfessionId
);

public sealed record UpdateCharacterRequestDto(
    string Name,
    string? Player,
    int? Level,
    int? RaceId,
    int? ProfessionId,
    int? AttAgi,
    int? AttPer,
    int? AttInt,
    int? AttAur,
    int? AttCar,
    int? AttFor,
    int? AttFis,
    int? CoinsCopper,
    int? CoinsSilver,
    int? CoinsGold,
    int? PointsSkill,
    int? PointsWeapon,
    int? PointsCombat,
    int? PointsMagic
);
