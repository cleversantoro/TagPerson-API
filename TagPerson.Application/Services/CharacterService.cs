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
        return items
            .Select(c => new CharacterListItemDto(
                c.Id, 
                c.Name, 
                c.Level, 
                c.Race is null ? null : new SimpleLookupDto(c.Race.Id, c.Race.Name),
                c.Profession is null ? null : new SimpleLookupDto(c.Profession.Id, c.Profession.Name)
            )
        )
        .ToList();
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
            c.Experience,
            c.Player,
            c.ImageFile,
            c.ClassSocial is null ? null : new SimpleLookupDto(c.ClassSocial.Id, c.ClassSocial.Name),
            c.BirthPlace is null ? null : new SimpleLookupDto(c.BirthPlace.Id, c.BirthPlace.Name),
            c.Race is null ? null : new SimpleLookupDto(c.Race.Id, c.Race.Name),
            c.Profession is null ? null : new SimpleLookupDto(c.Profession.Id, c.Profession.Name),
            c.Specialization is null ? null : new SimpleLookupDto(c.Specialization.Id, c.Specialization.Name),
            c.Deity is null ? null : new SimpleLookupDto(c.Deity.Id, c.Deity.Name),
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
                c.History
            ),
            new CharacterCoinsDto(c.CoinsCopper, c.CoinsSilver, c.CoinsGold),
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
            )).ToList(),
            c.Characterizations.Select(ch => new CharacterCharacterizationDto(
                ch.CharacterizationId,
                ch.Characterization.Name,
                ch.Level
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

    public async Task<bool> AddCharacterizationAsync(int id, CharacterCharacterizationRequestDto request, CancellationToken ct)
    {
        var c = await _repo.GetAsync(id, ct);
        if (c is null) return false;

        var characterizatonExists = await _repo.CharacterizationExistsAsync(request.CharacterizationId, ct);
        if (!characterizatonExists) return false;

        var current = await _repo.GetCharacterizationAsync(id, request.CharacterizationId, ct);
        if (current is null)
        {
            await _repo.AddCharacterizationAsync(new CharacterCharacterization
            {
                CharacterId = id,
                CharacterizationId = request.CharacterizationId,
                Level = request.Level ?? 0
            }, ct);
        }
        else if (request.Level.HasValue)
        {
            current.Level = request.Level;
        }

        await _repo.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> AddSkillAsync(int id, CharacterSkillRequestDto request, CancellationToken ct)
    {
        var c = await _repo.GetAsync(id, ct);
        if (c is null) return false;

        var skillExists = await _repo.SkillExistsAsync(request.SkillId, ct);
        if (!skillExists) return false;

        var current = await _repo.GetSkillAsync(id, request.SkillId, ct);
        if (current is null)
        {
            await _repo.AddSkillAsync(new CharacterSkill
            {
                CharacterId = id,
                SkillId = request.SkillId,
                Level = request.Level ?? 0
            }, ct);
        }
        else if (request.Level.HasValue)
        {
            current.Level = request.Level;
        }

        await _repo.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> AddCombatSkillAsync(int id, CharacterCombatSkillRequestDto request, CancellationToken ct)
    {
        var c = await _repo.GetAsync(id, ct);
        if (c is null) return false;

        var skillExists = await _repo.CombatSkillExistsAsync(request.CombatSkillId, ct);
        if (!skillExists) return false;

        var current = await _repo.GetCombatSkillAsync(id, request.CombatSkillId, ct);
        if (current is null)
        {
            await _repo.AddCombatSkillAsync(new CharacterCombatSkill
            {
                CharacterId = id,
                CombatSkillId = request.CombatSkillId,
                Level = request.Level ?? 0
            }, ct);
        }
        else if (request.Level.HasValue)
        {
            current.Level = request.Level;
        }

        await _repo.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> AddSkillSpecializationAsync(int id, int skillId, CharacterSkillSpecializationRequestDto request, CancellationToken ct)
    {
        var c = await _repo.GetAsync(id, ct);
        if (c is null) return false;

        var skillExists = await _repo.SkillExistsAsync(skillId, ct);
        if (!skillExists) return false;

        var current = await _repo.GetSkillAsync(id, skillId, ct);
        if (current is null) return false;

        await _repo.AddSkillSpecializationAsync(new CharacterSkillSpecialization
        {
            CharacterId = id,
            SkillId = skillId,
            SkillSpecializationId = request.SkillSpecializationId,
            Specialization = request.Specialization,
            Level = request.Level
        }, ct);

        await _repo.SaveChangesAsync(ct);
        return true;
    }

    public async Task<IReadOnlyList<CharacterSkillSpecializationDto>> GetSkillSpecializationsAsync(int id, int skillId, CancellationToken ct)
    {
        var list = await _repo.ListSkillSpecializationsAsync(id, skillId, ct);
        return list.Select(s => new CharacterSkillSpecializationDto(
            s.Id,
            s.SkillId ?? 0,
            s.SkillSpecializationId,
            s.Specialization,
            s.Level
        )).ToList();
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


