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

    public async Task<IReadOnlyList<CharacterizationLookupDto>> CharacterizationAsync(CancellationToken ct)
    {
        var items = await _repo.CharacterizationAsync(ct);
        return items.Select(x => new CharacterizationLookupDto(
                x.Id, 
                x.Name, 
                x.CharacterizationType is null ? null : new SimpleLookupDto(x.CharacterizationType.Id, x.CharacterizationType.Name), 
                x.CharacterizationGroup is null ? null : new SimpleLookupDto(x.CharacterizationGroup.Id, x.CharacterizationGroup.Name)
            )
        ).ToList();
    }

    public async Task<IReadOnlyList<PlaceLookupDto>> PlaceAsync(CancellationToken ct)
    {
        var items = await _repo.PlaceAsync(ct);
        return items.Select(x => new PlaceLookupDto(x.Id, x.Name)).ToList();
    }

    public async Task<IReadOnlyList<ClassSocialLookupDto>> ClassSocialAsync(CancellationToken ct)
    {
        var items = await _repo.ClassSocialAsync(ct);
        return items.Select(x => new ClassSocialLookupDto(x.Id, x.Name)).ToList();
    }

    public async Task<IReadOnlyList<DeityLookupDto>> DeityAsync(CancellationToken ct)
    {
        var items = await _repo.DeityAsync(ct);
        return items.Select(x => new DeityLookupDto(x.Id, x.Name)).ToList();
    }

    public async Task<IReadOnlyList<TimeLineLookupDto>> TimeLineAsync(CancellationToken ct)
    {
        var items = await _repo.TimeLineAsync(ct);
        return items.Select(x => new TimeLineLookupDto(
            x.Id, 
            x.Place is null ? null : new SimpleLookupDto(x.Place.Id, x.Place.Name), 
            x.Year, 
            x.Occurrence
          )
        ).ToList();
    }
}
