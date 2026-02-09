using Microsoft.EntityFrameworkCore;
using System;
using System.Text.RegularExpressions;
using TagPerson.Application.DTOs;
using TagPerson.Application.Interfaces.Repositories;
using TagPerson.Domain.Entities;
using TagPerson.Infrastructure.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TagPerson.Infrastructure.Repositories;

public sealed class CharacterRepository : ICharacterRepository
{
    private readonly AppDbContext _db;

    public CharacterRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<Character>> ListAsync(CancellationToken ct)
    {
        return await _db.Characters
            .AsNoTracking()
            .Include(x => x.Race)
            .Include(x => x.Profession)
            .OrderBy(x => x.Name)
            .ToListAsync(ct);
    }

    public async Task<Character?> GetAsync(int id, CancellationToken ct)
    {
        return await _db.Characters.FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<Character?> GetSheetAsync(int id, CancellationToken ct)
    {
        return await _db.Characters
            .AsNoTracking()
            .Include(x => x.Race)
            .Include(x => x.Profession)
            .Include(x => x.Specialization)
            .Include(x => x.Deity)
            .Include(x => x.BirthPlace)
            .Include(x => x.ClassSocial)
            .Include(x => x.Skills).ThenInclude(x => x.Skill)
            .Include(x => x.Spells).ThenInclude(x => x.Spell)
            .Include(x => x.CombatSkills).ThenInclude(x => x.CombatSkill)
            .Include(x => x.Equipments).ThenInclude(x => x.Equipment).ThenInclude(e => e.DefenseStats)
            .Include(x => x.Characterizations).ThenInclude(x => x.Characterization)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task AddAsync(Character character, CancellationToken ct)
    {
        await _db.Characters.AddAsync(character, ct);
    }

    public async Task<Race?> GetRaceByIdAsync(int raceId, CancellationToken ct)
    {
        return await _db.Races.FirstOrDefaultAsync(x => x.Id == raceId, ct);
    }

    public async Task<Profession?> GetProfessionByIdAsync(int professionId, CancellationToken ct)
    {
        return await _db.Professions.FirstOrDefaultAsync(x => x.Id == professionId, ct);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var entity = await _db.Characters.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (entity is null) return false;
        _db.Characters.Remove(entity);
        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> SkillExistsAsync(int skillId, CancellationToken ct)
    {
        return await _db.Skills.AnyAsync(x => x.Id == skillId, ct);
    }

    public async Task<bool> CharacterizationExistsAsync(int characterizationId, CancellationToken ct)
    {
        return await _db.Characterizations.AnyAsync(x => x.Id == characterizationId, ct);
    }

    public async Task AddCharacterizationAsync(CharacterCharacterization characterization, CancellationToken ct)
    {
        await _db.CharacterCharacterizations.AddAsync(characterization, ct);
    }

    public async Task<CharacterCharacterization?> GetCharacterizationAsync(int characterId, int characterizationId, CancellationToken ct)
    {
        return await _db.CharacterCharacterizations
            .FirstOrDefaultAsync(x => x.CharacterId == characterId && x.CharacterizationId == characterizationId, ct);
    }

    public async Task<CharacterSkill?> GetSkillAsync(int characterId, int skillId, CancellationToken ct)
    {
        return await _db.CharacterSkills
            .FirstOrDefaultAsync(x => x.CharacterId == characterId && x.SkillId == skillId, ct);
    }

    public async Task AddSkillAsync(CharacterSkill skill, CancellationToken ct)
    {
        await _db.CharacterSkills.AddAsync(skill, ct);
    }

    public async Task AddSkillSpecializationAsync(CharacterSkillSpecialization specialization, CancellationToken ct)
    {
        await _db.CharacterSkillSpecializations.AddAsync(specialization, ct);
    }

    public async Task<IReadOnlyList<CharacterSkillSpecialization>> ListSkillSpecializationsAsync(int characterId, int skillId, CancellationToken ct)
    {
        return await _db.CharacterSkillSpecializations
            .AsNoTracking()
            .Where(x => x.CharacterId == characterId && x.SkillId == skillId)
            .ToListAsync(ct);
    }

    public async Task<bool> CombatSkillExistsAsync(int combatSkillId, CancellationToken ct)
    {
        return await _db.CombatSkills.AnyAsync(x => x.Id == combatSkillId, ct);
    }

    public async Task<CharacterCombatSkill?> GetCombatSkillAsync(int characterId, int combatSkillId, int combatGroupId, CancellationToken ct)
    {
        return await _db.CharacterCombatSkills
            .FirstOrDefaultAsync(x => x.CharacterId == characterId && x.CombatSkillId == combatSkillId, ct);
    }

    public async Task AddCombatSkillAsync(CharacterCombatSkill skill, CancellationToken ct)
    {
        await _db.CharacterCombatSkills.AddAsync(skill, ct);
    }

    public async Task<bool> EquipmentExistsAsync(int equipmentId, CancellationToken ct)
    {
        return await _db.Equipments.AnyAsync(x => x.Id == equipmentId, ct);
    }

    public async Task<CharacterEquipment?> GetEquipmentAsync(int characterId, int equipmentId, CancellationToken ct)
    {
        return await _db.CharacterEquipments
            .FirstOrDefaultAsync(x => x.CharacterId == characterId && x.EquipmentId == equipmentId, ct);
    }

    public async Task AddEquipmentAsync(CharacterEquipment equipment, CancellationToken ct)
    {
        await _db.CharacterEquipments.AddAsync(equipment, ct);
    }

    public Task SaveChangesAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);

    public async Task<bool> SpellExistsAsync(int spellId, CancellationToken ct)
    {
        return await _db.Spells.AnyAsync(x => x.Id == spellId, ct);
    }

    public async Task<CharacterSpell?> GetSpellAsync(int characterId, int spellId, CancellationToken ct)
    {
        return await _db.CharacterSpells
           .FirstOrDefaultAsync(x => x.CharacterId == characterId && x.SpellId == spellId, ct);
    }

    public async Task AddSPellAsync(CharacterSpell characterSpell, CancellationToken ct)
    {
        await _db.CharacterSpells.AddAsync(characterSpell, ct);
    }

    public async Task<IReadOnlyList<SpellFromCharacterDto>> GetCharacterSpellAsync(int id, CancellationToken ct)
    {
        return await _db.CharacterSpells
             .Where(pm => pm.CharacterId == id)
             .Join(_db.Spells, pm => pm.SpellId, m => m.Id, (pm, m) => new { pm, m })
             .Join(_db.SpellGroupCosts, x => x.m.Id, mgc => mgc.SpellId, (x, mgc) => new { x.pm, x.m, mgc })
             .Join(_db.SpellGroups, x => x.mgc.SpellGroupId, mg => mg.Id, (x, mg) => new { x.pm, x.m, x.mgc, mg })
             .Where(x => x.pm.SpellGroupId == x.mg.Id)
             .Select(x => new SpellFromCharacterDto(
                 x.m.Id,
                 x.m.Name,
                 x.m.Description,
                 x.m.Evocation,
                 x.m.Range,
                 x.m.Duration,
                 x.m.LevelsJson,
                 x.mgc.Cost,
                 x.pm.Level,
                 x.pm.Type
             ))
             .ToListAsync(ct);

    }

    public async Task<bool> DeleteCharacterSpellAsync(int id, int spellId, int spellGroupId, CancellationToken ct)
    {
        var entity = await _db.CharacterSpells.FirstOrDefaultAsync(x => x.CharacterId == id && x.SpellId == spellId && x.SpellGroupId == spellGroupId, ct);
        if (entity is null) return false;
        _db.CharacterSpells.Remove(entity);
        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<IReadOnlyList<CombatFromCharacterDto>> GetCharacterCombatAsync(int id, CancellationToken ct)
    {
        return await _db.CharacterCombatSkills
            .Where(pc => pc.CharacterId == id)
             .Join(_db.CombatSkills, pc => pc.CombatSkillId, c => c.Id, (pc, c) => new { pc, c })
             .Join(_db.CombatGroupCosts, x => x.c.Id, cgc => cgc.CombatSkillId, (x, cgc) => new { x.pc, x.c, cgc })
             .Join(_db.CombatGroups, x => x.cgc.CombatGroupId, cg => cg.Id, (x, cg) => new { x.pc, x.c, x.cgc, cg })
             .Join(_db.Categories, x => x.c.CategoryId, ctg => ctg.Id, (x, ctg) => new { x.pc, x.c, x.cgc, x.cg, ctg })
             .Where(x => x.pc.CombatGroupId == x.cg.Id)
             .Select(x => new CombatFromCharacterDto(
                    x.c.Id,
                    x.c.Name,
                    x.c.AttributeCode,
                    x.c.Effect,
                    x.c.Notes,
                    x.c.Requisite,
                    x.c.RollTable,
                    x.c.Improvement,
                    x.cg.ParentId,
                    x.cg.Id,
                    x.cg.Name,
                    x.ctg.Id,
                    x.ctg.Name,
                    x.cgc.Cost,
                    x.c.Bonus,
                    x.cgc.Reduction,
                    x.pc.Type,
                    x.pc.Level
             )).ToListAsync(ct);
    }

    public async Task<bool> DeleteCharacterCombatAsync(int id, int combatId, int combatGroupId, CancellationToken ct)
    {
        var entity = await _db.CharacterCombatSkills.FirstOrDefaultAsync(x => x.CharacterId == id && x.CombatSkillId == combatId && x.CombatGroupId == combatGroupId, ct);
        if (entity is null) return false;
        _db.CharacterCombatSkills.Remove(entity);
        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<IReadOnlyList<CharacterCharacterizationDto>> GetCharacterCharacterizationAsync(int id, CancellationToken ct)
    {
        return await _db.CharacterCharacterizations
            .AsNoTracking()
            .Where(pc => pc.CharacterId == id)
            .Join(_db.Characterizations.AsNoTracking(),
                pc => pc.CharacterizationId,
                c => c.Id,
                (pc, c) => new { pc, c })
            .Join(_db.CharacterizationTypes.AsNoTracking(),
                x => x.c.CharacterizationTypeId,
                cType => cType.Id,
                (x, cType) => new { x.pc, x.c, cType })
            .Join(_db.CharacterizationGroups.AsNoTracking(),
                x => x.cType.Id,
                cg => cg.CharacterizationTypeId,
                (x, cg) => new { x.pc, x.c, x.cType, cg })
            .Join(_db.CharacterizationGroupCosts.AsNoTracking(),
                x => x.c.Id,
                cgc => cgc.CharacterizationId,
                (x, cgc) => new { x.pc, x.c, x.cType, x.cg, cgc })
            .Where(x => x.cgc.CharacterizationGroupId == x.cg.Id)
            .OrderBy(x => x.c.Name)
            .Select(x => new CharacterCharacterizationDto(
                x.c.Id,
                x.c.Name,
                x.c.CharacterizationTypeId,
                x.cType.Name,
                x.c.CharacterizationGroupId,
                x.cg.Name,
                x.c.Description,
                x.c.Notes,
                x.cgc.PlaceId,
                x.cgc.Cost,
                x.cgc.IsInitial,
                x.cgc.IsRare,
                x.cgc.IsAllowGame,
                x.pc.Level
            ))
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<SkillFromCharacterDto>> GetCharacterSkillAsync(int id, CancellationToken ct)
    {
        return await _db.CharacterSkills
            .AsNoTracking()
            .Where(ph => ph.CharacterId == id)
            .Join(_db.Skills.AsNoTracking(),
                ph => ph.SkillId,
                h => h.Id,
                (ph, h) => new { ph, h })
            .Join(_db.SkillGroups.AsNoTracking(),
                x => x.h.SkillGroupId,
                hg => hg.Id,
                (x, hg) => new { x.ph, x.h, hg })
            .Join(_db.SkillGroupCosts.AsNoTracking(),
                x => x.h.Id,
                hgc => hgc.SkillId,
                (x, hgc) => new { x.ph, x.h, x.hg, hgc })
            .OrderBy(x => x.h.Name)
            .Select(x => new SkillFromCharacterDto(
                x.h.Id,
                x.h.Name,
                x.h.SkillGroupId,
                x.hg.Name,
                x.h.Description,
                x.h.AttributeCode,
                x.h.LevelTest,
                x.h.Restricted,
                x.h.Penalties,
                x.h.ImprovedTasks,
                x.h.LevelsJson,
                x.h.Bonus,
                x.h.HasSpecialization,
                x.hgc.Cost,
                x.ph.Level
            ))
            .ToListAsync(ct);
    }

    public async Task<bool> DeleteCharacterSkillAsync(int id, int skillId, CancellationToken ct)
    {
        var entity = await _db.CharacterSkills.FirstOrDefaultAsync(x => x.CharacterId == id && x.SkillId == skillId, ct);
        if (entity is null) return false;
        _db.CharacterSkills.Remove(entity);
        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteCharacterEquipmentsAsync(int id, int equipmentId, CancellationToken ct)
    {
        var entity = await _db.CharacterEquipments.FirstOrDefaultAsync(x => x.CharacterId == id && x.EquipmentId == equipmentId, ct);
        if (entity is null) return false;
        _db.CharacterEquipments.Remove(entity);
        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteCharacterCharacterizationsAsync(int id, int characterizationId, CancellationToken ct)
    {
        var entity = await _db.CharacterCharacterizations.FirstOrDefaultAsync(x => x.CharacterId == id && x.CharacterizationId == characterizationId, ct);
        if (entity is null) return false;
        _db.CharacterCharacterizations.Remove(entity);
        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<IReadOnlyList<CharacterEquipmentDetailDto>> GetCharacterEquipmentsAsync(int id, CancellationToken ct)
    {
        return await _db.CharacterEquipments
            .AsNoTracking()
            .Where(pe => pe.CharacterId == id)
            .Join(_db.Equipments.AsNoTracking(),
                pe => pe.EquipmentId,
                e => e.Id,
                (pe, e) => new { pe, e })
            .Join(_db.EquipmentGroups.AsNoTracking(),
                x => x.e.GroupId,
                eg => eg.Id,
                (x, eg) => new { x.pe, x.e, eg })
            .OrderBy(x => x.e.Name)
            .Select(x => new CharacterEquipmentDetailDto(
                x.e.Id,
                x.e.Name,
                x.e.GroupId,
                x.eg.Name,
                x.e.Description,
                x.e.ImageFile,
                x.e.Price,
                x.e.IsWeapon,
                x.e.IsDefense,
                x.e.IsArmor,
                x.e.IsShield,
                x.e.IsHelmet,
                x.pe.Qty
            ))
            .ToListAsync(ct);
    }

}
