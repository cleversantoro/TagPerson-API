using TagPerson.Application.DTOs;
using TagPerson.Domain.Entities;

namespace TagPerson.Application.Interfaces.Repositories;

public interface ICharacterRepository
{
    Task<IReadOnlyList<Character>> ListAsync(CancellationToken ct);
    Task<Character?> GetAsync(int id, CancellationToken ct);
    Task<Character?> GetSheetAsync(int id, CancellationToken ct);
    Task AddAsync(Character character, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    
    Task<Race?> GetRaceByIdAsync(int raceId, CancellationToken ct);
    Task<Profession?> GetProfessionByIdAsync(int professionId, CancellationToken ct);
    
    Task<bool> CharacterizationExistsAsync(int characterizationId, CancellationToken ct);
    Task AddCharacterizationAsync(CharacterCharacterization characterization, CancellationToken ct);
    Task<CharacterCharacterization?> GetCharacterizationAsync(int characterId, int characterizationId, CancellationToken ct);
    
    Task<bool> SkillExistsAsync(int skillId, CancellationToken ct);
    Task AddSkillAsync(CharacterSkill skill, CancellationToken ct);
    Task<CharacterSkill?> GetSkillAsync(int characterId, int skillId, CancellationToken ct);

    Task AddSkillSpecializationAsync(CharacterSkillSpecialization specialization, CancellationToken ct);
    Task<IReadOnlyList<CharacterSkillSpecialization>> ListSkillSpecializationsAsync(int characterId, int skillId, CancellationToken ct);
    Task<bool> CombatSkillExistsAsync(int combatSkillId, CancellationToken ct);
    Task<CharacterCombatSkill?> GetCombatSkillAsync(int characterId, int combatSkillId, CancellationToken ct);
    Task AddCombatSkillAsync(CharacterCombatSkill skill, CancellationToken ct);
    Task<bool> EquipmentExistsAsync(int equipmentId, CancellationToken ct);
    Task<CharacterEquipment?> GetEquipmentAsync(int characterId, int equipmentId, CancellationToken ct);
    Task AddEquipmentAsync(CharacterEquipment equipment, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
    
    Task<bool> SpellExistsAsync(int spellId, CancellationToken ct);
    Task<CharacterSpell?> GetSpellAsync(int characterId, int spellId, CancellationToken ct);
    Task AddSPellAsync(CharacterSpell characterSpell, CancellationToken ct);
    Task<IReadOnlyList<SpellFromCharacterDto>> GetCharacterSpellAsync(int id, CancellationToken ct);
    Task<bool> DeleteCharacterSpellAsync(int id, int spellId, int spellGroupId, CancellationToken ct);
}
