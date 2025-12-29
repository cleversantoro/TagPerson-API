using TagPerson.Application.DTOs;

namespace TagPerson.Application.Interfaces;

public interface ICharacterService
{
    Task<IReadOnlyList<CharacterListItemDto>> ListAsync(CancellationToken ct);
    Task<CharacterSheetDto?> GetSheetAsync(int id, CancellationToken ct);
    Task<CharacterSheetDto> CreateAsync(CreateCharacterRequestDto request, CancellationToken ct);
    Task<bool> UpdateAsync(int id, UpdateCharacterRequestDto request, CancellationToken ct);
    Task<bool> AddEquipmentAsync(int id, CharacterEquipmentRequestDto request, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
}
