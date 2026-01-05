using Microsoft.EntityFrameworkCore;
using TagPerson.Application.DTOs;
using TagPerson.Application.Interfaces.Repositories;
using TagPerson.Domain.Entities;
using TagPerson.Infrastructure.Data;

namespace TagPerson.Infrastructure.Repositories;

public sealed class CharacterizationRepository : ICharacterizationRepository
{
    private readonly AppDbContext _db;

    public CharacterizationRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<CharacterizationType>> GetTypesAsync(CancellationToken ct)
    {
        return await _db.CharacterizationTypes
            .AsNoTracking()
            .OrderBy(x => x.DisplayOrder)
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<CharacterizationGroup>> GetGroupAsync(int typeId, CancellationToken ct)
    {
        return await _db.CharacterizationGroups
            .AsNoTracking()
            .Where(x => x.CharacterizationTypeId == typeId)
            .OrderBy(x => x.DisplayOrder)
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<CharacterizationDto>> GetCharacterizationAsync(int typeId, int groupId, CancellationToken ct)
    {
        return await _db.Characterizations
                   .AsNoTracking()
                   .Where(x => x.CharacterizationTypeId == typeId && x.CharacterizationGroupId == groupId)
                   .Join(_db.CharacterizationGroupCosts.AsNoTracking(),
                       characterization => characterization.Id,
                       characterizationgroupcost => characterizationgroupcost.CharacterizationId,
                       (characterization, characterizationgroupcost) => new { characterization, characterizationgroupcost })
                   .OrderBy(x => x.characterization.Id)
                   .Select(x => new CharacterizationDto(
                       x.characterization.Id,
                       x.characterization.CharacterizationTypeId,
                       x.characterization.CharacterizationGroupId,
                       x.characterizationgroupcost.PlaceId,
                       x.characterization.Name,
                       x.characterization.Description,
                       x.characterization.Notes,
                       x.characterizationgroupcost.Cost,
                       x.characterizationgroupcost.IsInitial,
                       x.characterizationgroupcost.IsRare,
                       x.characterizationgroupcost.IsAllowGame
                   ))
                   .ToListAsync(ct);
    }
}
