using System.Text.Json;
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
    private readonly ICharacterInitializationService _characterInitialization;
    private readonly AttributeCalculationService _attributeCalculation;

    public CharacterService(
        ICharacterRepository repo,
        TagmarCalculator calculator,
        ICharacterInitializationService characterInitialization,
        AttributeCalculationService attributeCalculation)
    {
        _repo = repo;
        _calculator = calculator;
        _characterInitialization = characterInitialization;
        _attributeCalculation = attributeCalculation;
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

        var startingEquipments = ParseStartingEquipments(c.Profession?.StartingEquipment);

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
            c.Specialization is null ? null : new SpecializationDto(
                c.Specialization.Id,
                c.Specialization.ProfessionId,
                c.Specialization.SpellGroupId,
                c.Specialization.CombatGroupId,
                c.Specialization.Name
            ),
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
                s.Group,
                s.CombatSkill.AttributeCode
            )).ToList(),
            c.Equipments.Select(e => new CharacterEquipmentDto(
                e.EquipmentId,
                e.Equipment.GroupId,
                e.Equipment.Name,
                e.Equipment.Description,
                e.Equipment.Price,
                e.Qty,
                e.Equipment.IsWeapon,
                e.Equipment.IsDefense,
                e.Equipment.IsArmor,    
                e.Equipment.IsShield,
                e.Equipment.IsHelmet
            )).ToList(),
            c.Characterizations.Select(ch => new CharacterCharacterizationDto(
                ch.CharacterizationId,
                ch.Characterization.Name,
                ch.Level
            )).ToList(),
            startingEquipments
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

        // Inicializar personagem com atributos, habilidades e magias baseado em raça/profissão
        try
        {
            await _characterInitialization.InitializeCharacterAsync(character, ct);
            await _repo.SaveChangesAsync(ct);
        }
        catch (InvalidOperationException)
        {
            // Se houver erro na inicialização, continua mesmo assim
            // Você pode registrar o erro ou tratá-lo diferentemente conforme necessário
        }

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

        c.Age = request.Age;
        c.Height = request.Height;
        c.Weight = request.Weight;
        c.Eyes = request.Eyes;
        c.Hair = request.Hair;
        c.Skin = request.Skin;
        c.Appearance = request.Appearance;
        c.History = request.History;

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

    public async Task<bool> AddSpellAsync(int id, CharacterSpellRequestDto req, CancellationToken ct)
    {
        var c = await _repo.GetAsync(id, ct);
        if (c is null) return false;

        var spellExists = await _repo.SpellExistsAsync(req.SpellId, ct);
        if (!spellExists) return false;

        var current = await _repo.GetSpellAsync(id, req.SpellId, ct);
        if (current is null)
        {
            await _repo.AddSPellAsync(new CharacterSpell
            {
                CharacterId = id,
                SpellId = req.SpellId,
                Level = req.Level ?? 0
            }, ct);
        }
        else if (req.Level.HasValue)
        {
            current.Level = req.Level;
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
                Group = request.Group ?? 0,
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

    public async Task<AttributeDistributionResponseDto?> ValidateAttributeDistributionAsync(
        int id,
        AttributeDistributionRequestDto request,
        CancellationToken ct)
    {
        var character = await _repo.GetAsync(id, ct);
        if (character?.RaceId is null)
            return null;

        var race = await _repo.GetRaceByIdAsync(character.RaceId.Value, ct);
        if (race is null)
            return null;

        var desiredValues = new Dictionary<AttributeType, int>();

        if (request.AttAgi.HasValue) desiredValues[AttributeType.Agilidade] = request.AttAgi.Value;
        if (request.AttAur.HasValue) desiredValues[AttributeType.Aura] = request.AttAur.Value;
        if (request.AttCar.HasValue) desiredValues[AttributeType.Carisma] = request.AttCar.Value;
        if (request.AttFis.HasValue) desiredValues[AttributeType.Fisico] = request.AttFis.Value;
        if (request.AttFor.HasValue) desiredValues[AttributeType.Forca] = request.AttFor.Value;
        if (request.AttInt.HasValue) desiredValues[AttributeType.Intelecto] = request.AttInt.Value;
        if (request.AttPer.HasValue) desiredValues[AttributeType.Percepcao] = request.AttPer.Value;

        var result = _attributeCalculation.CalculateDistributionCost(race, desiredValues);

        return new AttributeDistributionResponseDto(
            result.FinalValues.ToDictionary(x => x.Key.ToString(), x => x.Value),
            result.PointsUsed.Select(x => (x.Type.ToString(), x.PointsUsed)).ToList(),
            result.PointsGained.Select(x => (x.Type.ToString(), x.PointsGained)).ToList(),
            result.TotalPointsNeeded,
            result.TotalPointsGained,
            result.NetCost,
            result.IsValid,
            result.Errors.Any() ? result.Errors : null
        );
    }

    public async Task<(bool success, string message)> ApplyAttributeDistributionAsync(
        int id,
        AttributeDistributionRequestDto request,
        CancellationToken ct)
    {
        var character = await _repo.GetAsync(id, ct);
        if (character?.RaceId is null)
            return (false, "Personagem não encontrado ou sem raça definida");

        var race = await _repo.GetRaceByIdAsync(character.RaceId.Value, ct);
        if (race is null)
            return (false, "Raça não encontrada");

        var desiredValues = new Dictionary<AttributeType, int>();

        if (request.AttAgi.HasValue) desiredValues[AttributeType.Agilidade] = request.AttAgi.Value;
        if (request.AttAur.HasValue) desiredValues[AttributeType.Aura] = request.AttAur.Value;
        if (request.AttCar.HasValue) desiredValues[AttributeType.Carisma] = request.AttCar.Value;
        if (request.AttFis.HasValue) desiredValues[AttributeType.Fisico] = request.AttFis.Value;
        if (request.AttFor.HasValue) desiredValues[AttributeType.Forca] = request.AttFor.Value;
        if (request.AttInt.HasValue) desiredValues[AttributeType.Intelecto] = request.AttInt.Value;
        if (request.AttPer.HasValue) desiredValues[AttributeType.Percepcao] = request.AttPer.Value;

        var (isValid, message) = _attributeCalculation.ValidateAttributeDistribution(race, desiredValues);
        if (!isValid)
            return (false, message);

        // Aplicar os atributos
        character.AttAgi = desiredValues.TryGetValue(AttributeType.Agilidade, out var agi) ? agi : character.AttAgi;
        character.AttAur = desiredValues.TryGetValue(AttributeType.Aura, out var aur) ? aur : character.AttAur;
        character.AttCar = desiredValues.TryGetValue(AttributeType.Carisma, out var car) ? car : character.AttCar;
        character.AttFis = desiredValues.TryGetValue(AttributeType.Fisico, out var fis) ? fis : character.AttFis;
        character.AttFor = desiredValues.TryGetValue(AttributeType.Forca, out var for_) ? for_ : character.AttFor;
        character.AttInt = desiredValues.TryGetValue(AttributeType.Intelecto, out var int_) ? int_ : character.AttInt;
        character.AttPer = desiredValues.TryGetValue(AttributeType.Percepcao, out var per) ? per : character.AttPer;

        await _repo.SaveChangesAsync(ct);
        return (true, "Atributos aplicados com sucesso");
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

    private static IReadOnlyList<StartingEquipmentDto> ParseStartingEquipments(string? startingEquipment)
    {
        if (string.IsNullOrWhiteSpace(startingEquipment))
            return Array.Empty<StartingEquipmentDto>();

        try
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var equipments = JsonSerializer.Deserialize<List<EquipmentItem>>(startingEquipment, options);

            if (equipments is null || equipments.Count == 0)
                return Array.Empty<StartingEquipmentDto>();

            return equipments
                .Select(e => new StartingEquipmentDto(e.EquipmentId, e.Name))
                .ToList();
        }
        catch
        {
            return Array.Empty<StartingEquipmentDto>();
        }
    }

    private sealed record EquipmentItem(int EquipmentId, string Name);

}





