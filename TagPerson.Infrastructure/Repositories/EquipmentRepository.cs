using Microsoft.EntityFrameworkCore;
using TagPerson.Application.DTOs;
using TagPerson.Application.Interfaces.Repositories;
using TagPerson.Domain.Entities;
using TagPerson.Infrastructure.Data;

namespace TagPerson.Infrastructure.Repositories;

public sealed class EquipmentRepository : IEquipmentRepository
{
    private readonly AppDbContext _db;

    public EquipmentRepository(AppDbContext db)
    {
        _db = db;
    }

    {
        return await _db.EquipmentGroups
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<EquipmentFromGroupDto>> GetItemsByGroupAsync(int groupId, CancellationToken ct)
    {
        return await _db.Equipments
            .AsNoTracking()
            .Where(x => x.GroupId == groupId)
            .OrderBy(x => x.Name)
            .Select(x => new EquipmentFromGroupDto(
                x.Id,
                x.Name,
                x.Description,
                x.Price,
                x.GroupId
            ))
            .ToListAsync(ct);
    }
}
