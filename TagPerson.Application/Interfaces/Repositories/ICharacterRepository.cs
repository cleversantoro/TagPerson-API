using TagPerson.Domain.Entities;

namespace TagPerson.Application.Interfaces.Repositories;

public interface ICharacterRepository
{
    Task<IReadOnlyList<Character>> ListAsync(CancellationToken ct);
    Task<Character?> GetAsync(int id, CancellationToken ct);
    Task<Character?> GetSheetAsync(int id, CancellationToken ct);
    Task AddAsync(Character character, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    Task<bool> SkillExistsAsync(int skillId, CancellationToken ct);
    Task<CharacterSkill?> GetSkillAsync(int characterId, int skillId, CancellationToken ct);
    Task AddSkillAsync(CharacterSkill skill, CancellationToken ct);
    Task<IReadOnlyList<CharacterSkillSpecialization>> ListSkillSpecializationsAsync(int characterId, int skillId, CancellationToken ct);
    Task<bool> EquipmentExistsAsync(int equipmentId, CancellationToken ct);
    Task<CharacterEquipment?> GetEquipmentAsync(int characterId, int equipmentId, CancellationToken ct);
    Task AddEquipmentAsync(CharacterEquipment equipment, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}
