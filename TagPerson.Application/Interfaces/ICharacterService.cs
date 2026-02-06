using TagPerson.Application.DTOs;
using TagPerson.Domain.Entities;

namespace TagPerson.Application.Interfaces;

public interface ICharacterService
{
    Task<IReadOnlyList<CharacterListItemDto>> ListAsync(CancellationToken ct);
    Task<CharacterSheetDto?> GetSheetAsync(int id, CancellationToken ct);
    Task<CharacterSheetDto> CreateAsync(CreateCharacterRequestDto request, CancellationToken ct);
    Task<bool> UpdateAsync(int id, UpdateCharacterRequestDto request, CancellationToken ct);
    Task<bool> AddSkillAsync(int id, CharacterSkillRequestDto request, CancellationToken ct);
    Task<bool> AddCombatSkillAsync(int id, CharacterCombatSkillRequestDto request, CancellationToken ct);
    Task<bool> AddSkillSpecializationAsync(int id, int skillId, CharacterSkillSpecializationRequestDto request, CancellationToken ct);
    Task<bool> AddEquipmentAsync(int id, CharacterEquipmentRequestDto request, CancellationToken ct);
    Task<bool> AddCharacterizationAsync(int id, CharacterCharacterizationRequestDto request, CancellationToken ct);
    Task<IReadOnlyList<CharacterSkillSpecializationDto>> GetSkillSpecializationsAsync(int id, int skillId, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    Task<AttributeDistributionResponseDto?> ValidateAttributeDistributionAsync(int id, AttributeDistributionRequestDto request, CancellationToken ct);
    Task<(bool success, string message)> ApplyAttributeDistributionAsync(int id, AttributeDistributionRequestDto request, CancellationToken ct);
    Task<bool> AddSpellAsync(int id, CharacterSpellRequestDto req, CancellationToken requestAborted);
    Task<IReadOnlyList<SpellFromCharacterDto>> GetCharacterSpellAsync(int id, CancellationToken ct);
    Task<bool> DeleteCharacterSpellAsync(int id, int spellId, int spellGroupId, CancellationToken ct);
    Task<IReadOnlyList<CombatFromCharacterDto>> GetCharacterCombatAsync(int id, CancellationToken requestAborted);
    Task<bool> DeleteCharacterCombatAsync(int id, int combatId, int combatGroupId, CancellationToken requestAborted);
}
