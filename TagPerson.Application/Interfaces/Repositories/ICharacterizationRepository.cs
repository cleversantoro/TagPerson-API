using TagPerson.Application.DTOs;
using TagPerson.Domain.Entities;

namespace TagPerson.Application.Interfaces.Repositories;

public interface ICharacterizationRepository
{
    Task<IReadOnlyList<CharacterizationType>> GetTypesAsync(CancellationToken ct);
    Task<IReadOnlyList<CharacterizationGroup>> GetGroupAsync(int typeId, CancellationToken ct);
    Task<IReadOnlyList<CharacterizationDto>> GetCharacterizationAsync(int typeId, int groupId, CancellationToken ct);

}
