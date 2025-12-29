using TagPerson.Application.DTOs;
using TagPerson.Application.Interfaces;
using TagPerson.Application.Interfaces.Repositories;
using TagPerson.Domain.Entities;
using TagPerson.Domain.Services;
using TagPerson.Domain.ValueObjects;

namespace TagPerson.Application.Services;

public sealed class CharacterService : ICharacterService
{
    private readonly ICharacterRepository _repo;
    private readonly TagmarCalculator _calculator;

    public CharacterService(ICharacterRepository repo, TagmarCalculator calculator)
    {
        _repo = repo;
        _calculator = calculator;
    }

    public async Task<IReadOnlyList<CharacterListItemDto>> ListAsync(CancellationToken ct)
    {
        var items = await _repo.ListAsync(ct);
        return items.Select(c => new CharacterListItemDto(c.Id, c.Name, c.Level)).ToList();
    }

    public async Task<CharacterSheetDto?> GetSheetAsync(int id, CancellationToken ct)
    {
        var c = await _repo.GetSheetAsync(id, ct);
        if (c is null) return null;

        var derived = _calculator.Calculate(c, c.Race, c.Profession, null, null, null);

        return new CharacterSheetDto(
            c.Id,
            c.Name,
            c.Level,
            c.Player,
            c.Race is null ? null : new SimpleLookupDto(c.Race.Id, c.Race.Name),
            c.Profession is null ? null : new SimpleLookupDto(c.Profession.Id, c.Profession.Name),
            new CharacterAttributesDto(
                c.AttAgi,
                c.AttPer,
                c.AttInt,
                c.AttAur,
                c.AttCar,
                c.AttFor,
                c.AttFis
            ),
            new CharacterPointsDto(
                c.PointsSkill,
                c.PointsWeapon,
                c.PointsCombat,
                c.PointsMagic
            ),
            new CharacterFeaturesDto(
                c.Age,
                c.Height,
                c.Weight,
                c.Eyes,
                c.Hair,
                c.Skin,
                c.Appearance,
                c.History,
                new CharacterCoinsDto(c.CoinsCopper, c.CoinsSilver, c.CoinsGold)
            ),
            Map(derived),
            c.Skills.Select(s => new CharacterSkillDto(
                s.SkillId,
                s.Skill.Name,
                s.Level,
                s.Skill.AttributeCode,
                s.Skill.Restricted,
                s.Skill.HasSpecialization
            )).ToList(),
            c.Spells.Select(s => new CharacterSpellDto(
                s.SpellId,
                s.Spell.Name,
                s.Level,
                s.Spell.Evocation,
                s.Spell.Range,
                s.Spell.Duration
            )).ToList(),
            c.CombatSkills.Select(s => new CharacterCombatSkillDto(
                s.CombatSkillId,
                s.CombatSkill.Name,
                s.Level,
                s.CombatSkill.AttributeCode
            )).ToList(),
            c.Equipments.Select(e => new CharacterEquipmentDto(
                e.EquipmentId,
                e.Equipment.Name,
                e.Qty
            )).ToList()
        );
    }

    public async Task<CharacterSheetDto> CreateAsync(CreateCharacterRequestDto request, CancellationToken ct)
    {
        var character = new Character
        {
            Name = request.Name,
            Player = request.Player,
            Level = request.Level ?? 1,
            RaceId = request.RaceId,
            ProfessionId = request.ProfessionId
        };

        await _repo.AddAsync(character, ct);
        await _repo.SaveChangesAsync(ct);

        var sheet = await _repo.GetSheetAsync(character.Id, ct);
        if (sheet is null)
        {
            throw new InvalidOperationException("Character create failed.");
        }

        var mapped = await GetSheetAsync(sheet.Id, ct);
        if (mapped is null)
        {
            throw new InvalidOperationException("Character create failed.");
        }

        return mapped;
    }

    public async Task<bool> UpdateAsync(int id, UpdateCharacterRequestDto request, CancellationToken ct)
    {
        var c = await _repo.GetAsync(id, ct);
        if (c is null) return false;

        c.Name = request.Name;
        c.Player = request.Player;
        c.Level = request.Level;
        c.RaceId = request.RaceId;
        c.ProfessionId = request.ProfessionId;

        c.AttAgi = request.AttAgi;
        c.AttPer = request.AttPer;
        c.AttInt = request.AttInt;
        c.AttAur = request.AttAur;
        c.AttCar = request.AttCar;
        c.AttFor = request.AttFor;
        c.AttFis = request.AttFis;

        c.CoinsCopper = request.CoinsCopper;
        c.CoinsSilver = request.CoinsSilver;
        c.CoinsGold = request.CoinsGold;

        c.PointsSkill = request.PointsSkill;
        c.PointsWeapon = request.PointsWeapon;
        c.PointsCombat = request.PointsCombat;
        c.PointsMagic = request.PointsMagic;

        await _repo.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> AddEquipmentAsync(int id, CharacterEquipmentRequestDto request, CancellationToken ct)
    {
        var c = await _repo.GetAsync(id, ct);
        if (c is null) return false;

        var exists = await _repo.EquipmentExistsAsync(request.EquipmentId, ct);
        if (!exists) return false;

        var current = await _repo.GetEquipmentAsync(id, request.EquipmentId, ct);
        if (current is null)
        {
            await _repo.AddEquipmentAsync(new CharacterEquipment
            {
                CharacterId = id,
                EquipmentId = request.EquipmentId,
                Qty = request.Qty ?? 1
            }, ct);
        }
        else
        {
            var qty = request.Qty ?? (current.Qty ?? 0) + 1;
            current.Qty = qty;
        }

        await _repo.SaveChangesAsync(ct);
        return true;
    }

    public Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        return _repo.DeleteAsync(id, ct);
    }

    private static DerivedStatsDto Map(DerivedStats stats) => new()
    {
        MaxEf = stats.MaxEf,
        ResistenciaFisica = stats.ResistenciaFisica,
        ResistenciaMagica = stats.ResistenciaMagica,
        Velocidade = stats.Velocidade,
        Karma = stats.Karma,
        DefesaAtiva = stats.DefesaAtiva,
        DefesaPassiva = stats.DefesaPassiva,
        Absorcao = stats.Absorcao,
        PontosMagia = stats.PontosMagia
    };
}


