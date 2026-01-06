using Microsoft.EntityFrameworkCore;
using TagPerson.Application.Interfaces.Repositories;
using TagPerson.Domain.Entities;
using TagPerson.Infrastructure.Data;

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

    public async Task<CharacterCombatSkill?> GetCombatSkillAsync(int characterId, int combatSkillId, CancellationToken ct)
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

}
