using System.Xml.Linq;
using TagPerson.Application.DTOs;
using TagPerson.Application.Interfaces;
using TagPerson.Application.Interfaces.Repositories;
using TagPerson.Domain.Entities;

namespace TagPerson.Application.Services;

public sealed class CombatService : ICombatService
{
    private readonly ICombatRepository _repo;

    public CombatService(ICombatRepository repo)
    {
        _repo = repo;
    }

    public async Task<IReadOnlyList<CombatGroupDto>> GetGroupParentsAsync(CancellationToken ct)
    {
        var items = await _repo.GetGroupParentsAsync(ct);
        return items.Select(x => new CombatGroupDto(x.Id, x.Name, x.ParentId)).ToList();
    }

    public async Task<IReadOnlyList<CombatGroupDto>> GetGroupChildrenAsync(int parentId, CancellationToken ct)
    {
        var items = await _repo.GetGroupChildrenAsync(parentId, ct);
        return items.Select(x => new CombatGroupDto(x.Id, x.Name, x.ParentId)).ToList();
    }

    public Task<IReadOnlyList<CombatFromGroupDto>> GetCombatFromGroupAsync(int groupId, CancellationToken ct)
    {
        return _repo.GetCombatFromGroupAsync(groupId, ct);
    }

    public async Task<IReadOnlyList<CombatTechniquesDto>> GetBasicsAsync(CancellationToken ct)
    {
        var items = await _repo.GetBasicsAsync(ct);
        return items.Select(x => new CombatTechniquesDto(
            x.CombatId,
            x.ProfEspId,
            x.CombatName,
            x.AttributeCode,
            x.Effect,
            x.Notes,
            x.Requisite,
            x.RollTable,
            x.Improvement,
            x.CombatGroupId,
            x.GroupName,
            x.CategoryId,
            x.CategoryName,
            x.Cost,
            x.Bonus,
            x.Reduction
          )
        ).ToList();
    }

    public async Task<IReadOnlyList<CombatTechniquesDto>> GetProfessionsAsync(int professionalId, CancellationToken ct)
    {
        var items = await _repo.GetProfessionsAsync(professionalId, ct);
        return items.Select(x => new CombatTechniquesDto(
            x.CombatId,
            x.ProfEspId,
            x.CombatName,
            x.AttributeCode,
            x.Effect,
            x.Notes,
            x.Requisite,
            x.RollTable,
            x.Improvement,
            x.CombatGroupId,
            x.GroupName,
            x.CategoryId,
            x.CategoryName,
            x.Cost,
            x.Bonus,
            x.Reduction
          )
        ).ToList();
    }

    public async Task<IReadOnlyList<CombatTechniquesDto>> GetEspecializationsAsync(int especializationId, CancellationToken ct)
    {
        var items = await _repo.GetEspecializationsAsync(especializationId, ct);
        return items.Select(x => new CombatTechniquesDto(
            x.CombatId,
            x.ProfEspId,
            x.CombatName,
            x.AttributeCode,
            x.Effect,
            x.Notes,
            x.Requisite,
            x.RollTable,
            x.Improvement,
            x.CombatGroupId,
            x.GroupName,
            x.CategoryId,
            x.CategoryName,
            x.Cost,
            x.Bonus,
            x.Reduction
          )
        ).ToList();
    }
}
