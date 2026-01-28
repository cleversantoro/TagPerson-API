using System;
using System.ComponentModel.DataAnnotations.Schema;
using TagPerson.Domain.Entities;

namespace TagPerson.Application.DTOs;


#region Personagem

public sealed record CharacterListItemDto(int Id, string Name, int? Level, SimpleLookupDto? Race, SimpleLookupDto? Profession);

public sealed record CharacterSheetDto(
    int Id,
    string Name,
    int? Level,
    int? Experience,
    string? Player,
    string? ImageFile,
    SimpleLookupDto? ClassSocial,
    SimpleLookupDto? BirthPlace,
    SimpleLookupDto? Race,
    SimpleLookupDto? Profession,
    SpecializationDto? Specialization,
    SimpleLookupDto? Deity,
    CharacterAttributesDto Attributes,
    CharacterPointsDto Points,
    CharacterFeaturesDto Features,
    CharacterCoinsDto Coins,
    DerivedStatsDto Derived,
    IReadOnlyList<CharacterSkillDto> Skills,
    //IReadOnlyList<CharacterSpellDto> Spells,
    IReadOnlyList<SpellFromCharacterDto> Spells,
    IReadOnlyList<CharacterCombatSkillDto> Combat,
    IReadOnlyList<CharacterEquipmentDto> Equipments,
    IReadOnlyList<CharacterCharacterizationDto> Characterizations,
    IReadOnlyList<StartingEquipmentDto> StartingEquipments
);

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
    string? History
);

public sealed record CharacterSkillDto(
    int SkillId,
    string Name,
    int? Level,
    string? AttributeCode,
    int? Restricted,
    int? HasSpecialization
);

public sealed record CharacterSkillSpecializationDto(
    int Id,
    int SkillId,
    int? SkillSpecializationId,
    string? Specialization,
    int? Level
);

public sealed record CharacterSkillSpecializationRequestDto(
    string? Specialization,
    int? Level,
    int? SkillSpecializationId
);

public sealed record CharacterSpellDto(
    int SpellId,
    string Name,
    int? Level,
    string? Evocation,
    string? Range,
    string? Duration,
    int? type
);

public sealed record CharacterCombatSkillDto(
    int CombatSkillId,
    string Name,
    int? Level,
    int? Group,
    string? AttributeCode
);

public sealed record CharacterEquipmentDto(
    int EquipmentId,
    int? Groupid,
    string Name,
    string? Description,
    int? Price,
    int? Qty,
    int? IsWeapon,
    int? IsDefense,
    int? IsArmor,
    int? IsShield,
    int? IsHelmet
);

public sealed record CharacterCharacterizationDto(int CharacterizationId, string Name, int? Level);

public sealed record CharacterCharacterizationRequestDto(
    int CharacterizationId,
    int? Level
);

public sealed record CharacterEquipmentRequestDto(
    int EquipmentId,
    int? Qty
);

public sealed record CharacterSpellRequestDto(
    int SpellId,
    int SpellGroupId,
    int? Level,
    int? type 
);

public sealed record CharacterSkillRequestDto(
    int SkillId,
    int? Level
);

public sealed record CharacterCombatSkillRequestDto(
    int CombatSkillId,
    int? Group,
    int? Level
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

    string? Eyes,
    string? Hair,
    string? Skin,
    int? Age,
    int? Weight,
    int? Height,
    string? Appearance,
    string? History,

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

#endregion

#region Habilidades
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

public sealed record SkillSpecializationSuggestionDto(int Id, int SkillId, string? Suggestion);

public sealed record SkillImprovedDto(int Id, int SkillId, int? SkillGroupId, string? Description);

#endregion

#region Magia
public sealed record SpellGroupDto(int Id, string Name, int? ParentId);

public sealed record SpellFromGroupDto(
    int Id,
    string Name,
    int? Cost,
    string? Evocation,
    string? Range,
    string? Duration,
    string? Description,
    string? Effects,
    int? isProfession,
    int? isEspecialization 
);

public sealed record SpellFromCharacterDto(
    int Id,
    string Name,
    string? Description,
    string? Evocation,
    string? Range,
    string? Duration,
    string? Effects,
    int? Cost,
    int? Level,
    int? Type
);

public sealed record SpellTechniquesDto(
    int? Id,
    int? ProfEspId,
    string? Name,
    int? SpellGroupId,
    string? GroupName,
    string? Description,
    string? Evocation,
    string? Range,
    string? Duration,
    string? Effects,
    int? Cost
);


#endregion

#region Combate
public sealed record CombatGroupDto(int Id, string Name, int? ParentId);

public sealed record CombatTechniquesDto(
    int? CombatId,
    int? ProfEspId,
    string? CombatName,
    string? AttributeCode,
    string? Effect,
    string? Notes,
    string? Requisite,
    string? RollTable,
    string? Improvement,
    int? CombatGroupId,
    string? GroupName,
    int? CategoryId,
    string? CategoryName,
    int? Cost,
    int? Bonus,
    int? Reduction
);

public sealed record CombatFromGroupDto(
    int Id,
    string Name,
    int? Cost,
    int? Reduction,
    int? Bonus,
    int? HasSpecialization,
    string? AttributeCode,
    int? CategoryId,
    string? Effect,
    string? Requisite,
    string? Notes,
    string? RollTable,
    string? Improvement
);

#endregion

#region Categoria
public sealed record CategoryDto(int Id, string Name, string? Icon);

#endregion

public sealed record SpecializationDto(int Id, int? ProfessionId, int? SpellGroupId, int? CombatGroupI, string Name);

public sealed record StartingEquipmentDto(int EquipmentId, string Name);

public sealed record SimpleLookupDto(int Id, string Name);

public sealed record AttributeDistributionRequestDto(
    int? AttAgi,
    int? AttPer,
    int? AttInt,
    int? AttAur,
    int? AttCar,
    int? AttFor,
    int? AttFis
);

public sealed record AttributeDistributionResponseDto(
    Dictionary<string, int> FinalValues,
    List<(string Type, int PointsUsed)> PointsUsed,
    List<(string Type, decimal PointsGained)> PointsGained,
    int TotalPointsNeeded,
    decimal TotalPointsGained,
    int NetCost,
    bool IsValid,
    List<string>? Errors
);