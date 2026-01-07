using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TagPerson.Application.DTOs;

public sealed record RaceDto(
    int Id,
    string? Name,
    string? Description,
    string? ImageFile,
    int? BaseSpeed,
    int? EfBonus,
    int? BonusAgi,
    int? BonusPer,
    int? BonusInt,
    int? BonusAur,
    int? BonusCar,
    int? BonusFor,
    int? BonusFis,
    int? BaseHeight,
    int? BaseWeight,
    int? AgeMin,
    int? AgeMax
);


public sealed record ProfessionDto(
    int Id,
    string? Name,
    string? ImageFile,
    string? Description,
    string? StartingEquipment,
    int? CoinsCopper,
    int? HeroicEnergy,
    int? SkillPoints,
    int? WeaponPoints,
    int? CombatPoints,
    int? PenalizedSkillGroup,
    int? SpecializationSkill,
    int? AttributeForMagic,
    int? SpellGroup,
    string? BasicDefense,
    string? Absorption
);

