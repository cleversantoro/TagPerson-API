using TagPerson.Application.DTOs;

namespace TagPerson.Application.Interfaces;

public interface ISpellService
{
    Task<IReadOnlyList<SpellGroupDto>> GetGroupParentsAsync(CancellationToken ct);
    Task<IReadOnlyList<SpellFromGroupDto>> GetSpellsFromGroupAsync(int groupId, CancellationToken ct);
    Task<IReadOnlyList<SpellTechniquesDto>> GetProfessionsAsync(int professionalId, CancellationToken ct);
    Task<IReadOnlyList<SpellTechniquesDto>> GetEspecializationsAsync(int especializationId, CancellationToken ct);
}
