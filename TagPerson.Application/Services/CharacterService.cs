using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml.Linq;
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

        var armour = c.Equipments
            .FirstOrDefault(item => item.Equipped && item.Slot == "armadura")
            ?.Equipment?.DefenseStats;
        var helmet = c.Equipments
            .FirstOrDefault(item => item.Equipped && item.Slot == "elmo")
            ?.Equipment?.DefenseStats;
        var shield = c.Equipments
            .FirstOrDefault(item => item.Equipped && item.Slot == "escudo")
            ?.Equipment?.DefenseStats;
        var derived = _calculator.Calculate(c, c.Race, c.Profession, armour, helmet, shield);

        var startingEquipments = ParseStartingEquipments(c.Profession?.StartingEquipment);

        var spellCharacter = await _repo.GetCharacterSpellAsync(id, ct);

        var combatCharacter = await _repo.GetCharacterCombatAsync(id, ct);
        
        var characterizationCharacter = await _repo.GetCharacterCharacterizationAsync(id, ct);

        var skillCharacter = await _repo.GetCharacterSkillAsync(id, ct);
        
        var equipmentCharacter = await _repo.GetCharacterEquipmentsAsync(id, ct);

        var budget = CalculateBudget(c, skillCharacter, spellCharacter, combatCharacter);

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
                c.Specialization.Name,
                c.Specialization.Description,
                c.Specialization.ProfessionId,
                c.Specialization.SpellGroupId,
                c.Specialization.CombatGroupId
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
            skillCharacter,
            spellCharacter,
            combatCharacter,
            equipmentCharacter,
            characterizationCharacter,
            startingEquipments,
            budget
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
        c.Experience = request.Experience;
        c.SpecializationId = request.SpecializationId;
        c.DeityId = request.DeityId;
        c.ClassSocialId = request.ClassSocialId;
        c.BirthPlaceId = request.BirthPlaceId;

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
        var level = request.Level ?? current?.Level ?? 0;
        var cost = await _repo.GetSkillCostAsync(request.SkillId, ct);
        
        if (cost is null)
        {
            throw new InvalidOperationException("A habilidade não possui custo configurado.");
        }

        EnsureLevel(level, c.Level);
        var currentSkills = await _repo.GetCharacterSkillAsync(id, ct);
        
        EnsureBudget(
            c.PointsSkill ?? 0, 
            currentSkills.Sum(item => (item.Cost ?? 0) * (item.Level ?? 0)),
            (current?.Level ?? 0) * cost.Value, level * cost.Value, "habilidade"
        );

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
        var level = req.Level ?? current?.Level ?? 0;
        var cost = await _repo.GetSpellCostAsync(req.SpellId, req.SpellGroupId, ct);
        if (cost is null)
        {
            throw new InvalidOperationException("A magia não pertence ao grupo selecionado.");
        }

        EnsureLevel(level, c.Level);
        var currentSpells = await _repo.GetCharacterSpellAsync(id, ct);
        EnsureBudget(c.PointsMagic ?? 0, currentSpells.Sum(item => (item.Cost ?? 0) * (item.Level ?? 0)),
            (current?.Level ?? 0) * cost.Value, level * cost.Value, "magia");

        if (current is null)
        {
            await _repo.AddSPellAsync(new CharacterSpell
            {
                CharacterId = id,
                SpellId = req.SpellId,
                SpellGroupId = req.SpellGroupId,
                Level = req.Level ?? 0,
                Type = req.type
            }, ct);
        }
        else if (req.Level.HasValue)
        {
            current.Level = req.Level;
        }

        await _repo.SaveChangesAsync(ct);
        return true;
    }

    public async Task<IReadOnlyList<SpellFromCharacterDto>> GetCharacterSpellAsync(int id, CancellationToken ct)
    {
        var c = await _repo.GetCharacterSpellAsync(id, ct);

        return c.Select(c => new SpellFromCharacterDto(
            c.Id,
            c.Name,
            c.Description,
            c.Evocation,
            c.Range,
            c.Duration,
            c.Effects,
            c.Cost,
            c.Level,
            c.Type
        )).ToList();
    }

    public async Task<bool> AddCombatSkillAsync(int id, CharacterCombatSkillRequestDto request, CancellationToken ct)
    {
        var c = await _repo.GetAsync(id, ct);
        if (c is null) return false;

        var skillExists = await _repo.CombatSkillExistsAsync(request.CombatSkillId, ct);
        if (!skillExists) return false;

        var current = await _repo.GetCombatSkillAsync(id, request.CombatSkillId, request.CombatGroupId, ct);
        var level = request.Level ?? current?.Level ?? 0;
        var cost = await _repo.GetCombatSkillCostAsync(request.CombatSkillId, request.CombatGroupId, ct);
        if (cost is null)
        {
            throw new InvalidOperationException("A técnica não pertence ao grupo selecionado.");
        }

        EnsureLevel(level, c.Level);
        var currentCombat = await _repo.GetCharacterCombatAsync(id, ct);
        EnsureBudget(c.PointsCombat ?? 0, currentCombat.Sum(item => (item.Cost ?? 0) * (item.Level ?? 0)),
            (current?.Level ?? 0) * cost.Value, level * cost.Value, "combate");

        if (current is null)
        {
            await _repo.AddCombatSkillAsync(new CharacterCombatSkill
            {
                CharacterId = id,
                CombatSkillId = request.CombatSkillId,
                CombatGroupId = request.CombatGroupId,
                Level = request.Level ?? 0,
                Type = request.Type
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

        var slot = request.Slot ?? "nenhum";
        if (request.Equipped == true && slot is "armadura" or "escudo" or "elmo")
        {
            await _repo.UnequipSlotAsync(id, slot, ct);
        }

        var current = await _repo.GetEquipmentAsync(id, request.EquipmentId, ct);
        if (current is null)
        {
            await _repo.AddEquipmentAsync(new CharacterEquipment
            {
                CharacterId = id,
                EquipmentId = request.EquipmentId,
                Qty = request.Qty ?? 1,
                Equipped = request.Equipped ?? false,
                Slot = slot
            }, ct);
        }
        else
        {
            var qty = request.Qty ?? (current.Qty ?? 0) + 1;
            current.Qty = qty;
            current.Equipped = request.Equipped ?? current.Equipped;
            current.Slot = request.Slot ?? current.Slot;
        }

        await _repo.SaveChangesAsync(ct);
        return true;
    }

    public Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        return _repo.DeleteAsync(id, ct);
    }

    public async Task<AttributeDistributionResponseDto?> ValidateAttributeDistributionAsync(int id, AttributeDistributionRequestDto request, CancellationToken ct)
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

    public async Task<(bool success, string message)> ApplyAttributeDistributionAsync(int id, AttributeDistributionRequestDto request, CancellationToken ct)
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

    private CharacterPointBudgetDto CalculateBudget(
        Character character,
        IReadOnlyList<SkillFromCharacterDto> skills,
        IReadOnlyList<SpellFromCharacterDto> spells,
        IReadOnlyList<CombatFromCharacterDto> combat)
    {
        var attributes = new Dictionary<AttributeType, int>
        {
            [AttributeType.Agilidade] = character.AttAgi ?? 0,
            [AttributeType.Percepcao] = character.AttPer ?? 0,
            [AttributeType.Intelecto] = character.AttInt ?? 0,
            [AttributeType.Aura] = character.AttAur ?? 0,
            [AttributeType.Carisma] = character.AttCar ?? 0,
            [AttributeType.Forca] = character.AttFor ?? 0,
            [AttributeType.Fisico] = character.AttFis ?? 0
        };
        var attributeGranted = character.Race is null ? 0 : _attributeCalculation.CalculateInitialPoints(character.Race);
        var attributeUsed = character.Race is null
            ? 0
            : Math.Max(0, _attributeCalculation.CalculateDistributionCost(character.Race, attributes).NetCost);

        return new CharacterPointBudgetDto(
            Allocation(attributeGranted, attributeUsed),
            Allocation(character.PointsSkill ?? 0, skills.Sum(item => (item.Cost ?? 0) * (item.Level ?? 0))),
            Allocation(character.PointsWeapon ?? 0, 0),
            Allocation(character.PointsCombat ?? 0, combat.Sum(item => (item.Cost ?? 0) * (item.Level ?? 0))),
            Allocation(character.PointsMagic ?? 0, spells.Sum(item => (item.Cost ?? 0) * (item.Level ?? 0)))
        );
    }

    private static PointAllocationDto Allocation(int granted, int used) => new(granted, used, granted - used);

    private static void EnsureLevel(int level, int? characterLevel)
    {
        if (level < 0 || level > (characterLevel ?? 0))
        {
            throw new InvalidOperationException("O nível informado deve estar entre zero e o nível do personagem.");
        }
    }

    private static void EnsureBudget(int granted, int used, int replacedCost, int requestedCost, string category)
    {
        if (used - replacedCost + requestedCost > granted)
        {
            throw new InvalidOperationException($"Pontos de {category} insuficientes para esta alteração.");
        }
    }

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

    public Task<bool> DeleteCharacterSpellAsync(int id, int spellId, int spellGroupId, CancellationToken ct)
    {
        return _repo.DeleteCharacterSpellAsync(id, spellId, spellGroupId, ct);
    }

    public async Task<IReadOnlyList<CombatFromCharacterDto>> GetCharacterCombatAsync(int id, CancellationToken ct)
    {
        var c = await _repo.GetCharacterCombatAsync(id, ct);

        return c.Select(c => new CombatFromCharacterDto(
            c.CombatId,
            c.CombatName,
            c.AttributeCode,
            c.Effect,
            c.Notes,
            c.Requisite,
            c.RollTable,
            c.Improvement,
            c.ProfEspId,
            c.CombatGroupId,
            c.GroupName,
            c.CategoryId,
            c.CategoryName,
            c.Cost,
            c.Bonus,
            c.Reduction,
            c.Type,
            c.Level
        )).ToList();

    }

    public Task<bool> DeleteCharacterCombatAsync(int id, int combatId, int combatGroupId, CancellationToken ct)
    {
        return _repo.DeleteCharacterCombatAsync(id, combatId, combatGroupId, ct);
    }

    public async Task<IReadOnlyList<SkillFromCharacterDto>> GetCharacterSkillAsync(int id, CancellationToken ct)
    {
        var c = await _repo.GetCharacterSkillAsync(id, ct);

        return c.Select(c => new SkillFromCharacterDto(
            c.Id,
            c.Name,
            c.SkillGroupId,
            c.GroupName,
            c.Description,
            c.AttributeCode,
            c.LevelTest,
            c.Restricted,
            c.Penalties,
            c.ImprovedTasks,
            c.Levelsjson,
            c.Bonus,
            c.HasSpecialization,
            c.Cost,
            c.Level
        )).ToList();
    }

    public Task<bool> DeleteCharacterSkillAsync(int id, int skillId, CancellationToken ct)
    {
        return _repo.DeleteCharacterSkillAsync(id, skillId,  ct);
    }

    public async Task<IReadOnlyList<CharacterEquipmentDetailDto>> GetCharacterEquipmentsAsync(int id, CancellationToken ct)
    {
        var c = await _repo.GetCharacterEquipmentsAsync(id, ct);

        return c.Select(c => new CharacterEquipmentDetailDto(
            c.Id,
            c.Name,
            c.GroupId,
            c.GroupName,
            c.Description,
            c.ImageFile,
            c.Price,
            c.IsWeapon,
            c.IsDefense,
            c.IsArmor,
            c.IsShield,
            c.IsHelmet,
            c.Qty,
            c.Equipped,
            c.Slot
        )).ToList();
    }

    public Task<bool> DeleteCharacterEquipmentsAsync(int id, int equipmentId, CancellationToken ct)
    {
        return _repo.DeleteCharacterEquipmentsAsync(id, equipmentId, ct);
    }

    public async Task<IReadOnlyList<CharacterCharacterizationDto>> GetCharacterCharacterizationsAsync(int id, CancellationToken ct)
    {
        var c = await _repo.GetCharacterCharacterizationAsync(id, ct);

        return c.Select(c => new CharacterCharacterizationDto(
            c.Id,
            c.Name,
            c.CharacterizationTypeId,
            c.NameType,
            c.CharacterizationGroupId,
            c.NameGroup,
            c.Description,
            c.Notes,
            c.PlaceId,
            c.Cost,
            c.IsInitial,
            c.IsRare,
            c.IsAllowGame,
            c.Level
        )).ToList();
    }

    public Task<bool> DeleteCharacterCharacterizationsAsync(int id, int characterizationId, CancellationToken ct)
    {
        return _repo.DeleteCharacterCharacterizationsAsync(id, characterizationId, ct);
    }

    private sealed record EquipmentItem(int EquipmentId, string Name);

}





