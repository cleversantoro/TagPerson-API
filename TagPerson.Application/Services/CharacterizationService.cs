using TagPerson.Application.DTOs;
using TagPerson.Application.Interfaces;
using TagPerson.Application.Interfaces.Repositories;

namespace TagPerson.Application.Services;

public sealed class CharacterizationService : ICharacterizationService
{
    private readonly ICharacterizationRepository _repo;

    public CharacterizationService(ICharacterizationRepository repo)
    {
        _repo = repo;
    }

    public async Task<IReadOnlyList<CharacterizationTypeDto>> GetTypesAsync(CancellationToken ct)
    {
        var items = await _repo.GetTypesAsync(ct);
        return items.Select(x => new CharacterizationTypeDto(x.Id, x.Name, x.Description, x.DisplayOrder)).ToList();
    }
    public async Task<IReadOnlyList<CharacterizationGroupDto>> GetGroupAsync(int typeId, CancellationToken ct)
    {
        var items = await _repo.GetGroupAsync(typeId, ct);
        return items.Select(x => new CharacterizationGroupDto(x.Id, x.Name, x.CharacterizationTypeId, x.DisplayOrder)).ToList();
    }

    public async Task<IReadOnlyList<CharacterizationDto>> GetCharacterizationAsync(int typeId, int groupId, CancellationToken ct)
    {
        var items = await _repo.GetCharacterizationAsync(typeId, groupId, ct);
        return items.Select(x => new CharacterizationDto(
            x.Id,
            x.CharacterizationTypeId, 
            x.CharacterizationGroupId, 
            x.PlaceId,
            x.Name,
            x.Description,
            x.Notes,
            x.Cost,
            x.IsInitial,
            x.IsRare,
            x.IsAllowGame
          )
        ).ToList();
    }

}
