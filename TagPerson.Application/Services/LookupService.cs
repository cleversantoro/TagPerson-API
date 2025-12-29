using TagPerson.Application.DTOs;
using TagPerson.Application.Interfaces;
using TagPerson.Application.Interfaces.Repositories;

namespace TagPerson.Application.Services;

public sealed class LookupService : ILookupService
{
    private readonly ILookupRepository _repo;

    public LookupService(ILookupRepository repo)
    {
        _repo = repo;
    }

    public async Task<IReadOnlyList<SimpleLookupDto>> RacesAsync(CancellationToken ct)
    {
        var items = await _repo.RacesAsync(ct);
        return items.Select(x => new SimpleLookupDto(x.Id, x.Name)).ToList();
    }

    public async Task<IReadOnlyList<SimpleLookupDto>> ProfessionsAsync(CancellationToken ct)
    {
        var items = await _repo.ProfessionsAsync(ct);
        return items.Select(x => new SimpleLookupDto(x.Id, x.Name)).ToList();
    }

    public async Task<IReadOnlyList<SkillLookupDto>> SkillsAsync(CancellationToken ct)
    {
        var items = await _repo.SkillsAsync(ct);
        return items.Select(x => new SkillLookupDto(x.Id, x.Name, x.AttributeCode, x.Restricted)).ToList();
    }

    public async Task<IReadOnlyList<SimpleLookupDto>> SpellsAsync(CancellationToken ct)
    {
        var items = await _repo.SpellsAsync(ct);
        return items.Select(x => new SimpleLookupDto(x.Id, x.Name)).ToList();
    }

    public async Task<IReadOnlyList<SimpleLookupDto>> CombatAsync(CancellationToken ct)
    {
        var items = await _repo.CombatAsync(ct);
        return items.Select(x => new SimpleLookupDto(x.Id, x.Name)).ToList();
    }

    public async Task<IReadOnlyList<EquipmentLookupDto>> EquipmentsAsync(CancellationToken ct)
    {
        var items = await _repo.EquipmentsAsync(ct);
        return items.Select(x => new EquipmentLookupDto(
            x.Id,
            x.Name,
            x.IsWeapon,
            x.IsDefense,
            x.IsArmor,
            x.IsShield,
            x.IsHelmet
        )).ToList();
    }

    public async Task<IReadOnlyList<CategoryDto>> CategoriesAsync(CancellationToken ct)
    {
        var items = await _repo.CategoriesAsync(ct);
        return items.Select(x => new CategoryDto(x.Id, x.Name, x.Icon)).ToList();
    }
}
