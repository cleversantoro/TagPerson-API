using TagPerson.Application.DTOs;

namespace TagPerson.Application.Interfaces;

public interface ICharacterizationService
{
    Task<IReadOnlyList<CharacterizationTypeDto>> GetTypesAsync(CancellationToken ct);
    Task<IReadOnlyList<CharacterizationGroupDto>> GetGroupAsync(int typeId, CancellationToken ct);
    Task<IReadOnlyList<CharacterizationDto>> GetCharacterizationAsync(int typeId, int groupId, CancellationToken ct);
}
