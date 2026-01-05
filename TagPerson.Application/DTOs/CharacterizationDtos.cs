using TagPerson.Domain.Entities;

namespace TagPerson.Application.DTOs;


public sealed record CharacterizationTypeDto(int Id, string Name, string? Description, int? DisplayOrder);

public sealed record CharacterizationGroupDto(int Id, string Name, int CharacterizationTypeId, int? DisplayOrder);

public sealed record CharacterizationDto(
    int? Id,
    int? CharacterizationTypeId,
    int? CharacterizationGroupId,
    int? PlaceId,
    string? Name,
    string? Description,
    string? Notes,
    int? Cost,
    int? IsInitial,
    int? IsRare,
    int? IsAllowGame
);


