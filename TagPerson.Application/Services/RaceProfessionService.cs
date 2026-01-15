using System.Xml.Linq;
using TagPerson.Application.DTOs;
using TagPerson.Application.Interfaces;
using TagPerson.Application.Interfaces.Repositories;
using TagPerson.Domain.Entities;

namespace TagPerson.Application.Services;

public sealed class RaceProfessionService : IRaceProfessionService
{
    private readonly IRaceProfessionRepository _repository;

    public RaceProfessionService(IRaceProfessionRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<ProfessionDto>> GetProfessionsByRaceAsync(int raceId, CancellationToken ct)
    {
        var professions = await _repository.GetProfessionsByRaceAsync(raceId, ct);
        return professions
            .Select(p => new ProfessionDto(
                    p.Id, 
                    p.Name,
                    p.ImageFile,
                    p.Description,
                    p.StartingEquipment,
                    p.CoinsCopper,
                    p.CoinsSilver,
                    p.CoinsGold,
                    p.HeroicEnergy,
                    p.SkillPoints,
                    p.WeaponPoints,
                    p.CombatPoints,
                    p.PenalizedSkillGroup,
                    p.SpecializationSkill,
                    p.AttributeForMagic,
                    p.SpellGroup,
                    p.BasicDefense,
                    p.WeaponDamageMaximun,
                    p.Absorption
                )
            )
            .ToList();
    }

    public async Task<IReadOnlyList<RaceDto>> GetRacesByProfessionAsync(int professionId, CancellationToken ct)
    {
        var races = await _repository.GetRacesByProfessionAsync(professionId, ct);
        return races
            .Select(r => new RaceDto(
                r.Id,
                r.Name,
                r.Description,
                r.ImageFile,
                r.BaseSpeed,
                r.EfBonus,
                r.BonusAgi,
                r.BonusPer,
                r.BonusInt,
                r.BonusAur,
                r.BonusCar,
                r.BonusFor,
                r.BonusFis,
                r.BaseHeight,
                r.BaseWeight,
                r.AgeMin,
                r.AgeMax                )
            )
            .ToList();
    }
}
