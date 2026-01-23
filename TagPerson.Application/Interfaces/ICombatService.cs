using TagPerson.Application.DTOs;

namespace TagPerson.Application.Interfaces;

public interface ICombatService
{
    Task<IReadOnlyList<CombatGroupDto>> GetGroupParentsAsync(CancellationToken ct);
    Task<IReadOnlyList<CombatGroupDto>> GetGroupChildrenAsync(int parentId, CancellationToken ct);
    Task<IReadOnlyList<CombatFromGroupDto>> GetCombatFromGroupAsync(int groupId, CancellationToken ct);
    Task<IReadOnlyList<CombatTechniquesDto>> GetBasicsAsync(CancellationToken ct);
    Task<IReadOnlyList<CombatTechniquesDto>> GetProfessionsAsync(int professionalId ,CancellationToken ct);
    Task<IReadOnlyList<CombatTechniquesDto>> GetEspecializationsAsync(int especializationId, CancellationToken ct);
}
