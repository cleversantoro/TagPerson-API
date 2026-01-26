using TagPerson.Application.DTOs;
using TagPerson.Application.Interfaces;
using TagPerson.Application.Interfaces.Repositories;
using TagPerson.Domain.Entities;

namespace TagPerson.Application.Services;

public sealed class SpellService : ISpellService
{
    private readonly ISpellRepository _repo;

    public SpellService(ISpellRepository repo)
    {
        _repo = repo;
    }

    public async Task<IReadOnlyList<SpellGroupDto>> GetGroupParentsAsync(CancellationToken ct)
    {
        var items = await _repo.GetGroupParentsAsync(ct);
        return items.Select(x => new SpellGroupDto(x.Id, x.Name, x.ParentId)).ToList();
    }

    public Task<IReadOnlyList<SpellFromGroupDto>> GetSpellsFromGroupAsync(int groupId, CancellationToken ct)
    {
        return _repo.GetSpellsFromGroupAsync(groupId, ct);
    }

    public async Task<IReadOnlyList<SpellTechniquesDto>> GetProfessionsAsync(int professionalId, CancellationToken ct)
    {
        var items = await _repo.GetProfessionsAsync(professionalId, ct);
        return items.Select(x => new SpellTechniquesDto(
                x.SpellId,
                x.ProfEspId,
                x.SpellName,
                x.SpellGroupId,
                x.GroupName,
                x.Description,
                x.Evocation,
                x.Range,
                x.Duration,
                x.Levels,
                x.Cost
            )
        ).ToList();
    }

    public async Task<IReadOnlyList<SpellTechniquesDto>> GetEspecializationsAsync(int especializationId, CancellationToken ct)
    {
        var items = await _repo.GetEspecializationsAsync(especializationId, ct);
        return items.Select(x => new SpellTechniquesDto(
                x.SpellId,
                x.ProfEspId,
                x.SpellName,
                x.SpellGroupId,
                x.GroupName,
                x.Description,
                x.Evocation,
                x.Range,
                x.Duration,
                x.Levels,
                x.Cost
            )
        ).ToList();
    }
}
