using TagPerson.Application.DTOs;

namespace TagPerson.Application.Interfaces;

public interface ILookupService
{
    Task<IReadOnlyList<SimpleLookupDto>> RacesAsync(CancellationToken ct);
    Task<IReadOnlyList<SimpleLookupDto>> ProfessionsAsync(CancellationToken ct);
    Task<IReadOnlyList<SkillLookupDto>> SkillsAsync(CancellationToken ct);
    Task<IReadOnlyList<SimpleLookupDto>> SpellsAsync(CancellationToken ct);
    Task<IReadOnlyList<SimpleLookupDto>> CombatAsync(CancellationToken ct);
    Task<IReadOnlyList<EquipmentLookupDto>> EquipmentsAsync(CancellationToken ct);
    Task<IReadOnlyList<CategoryDto>> CategoriesAsync(CancellationToken ct);
    Task<IReadOnlyList<CharacterizationLookupDto>> CharacterizationAsync(CancellationToken ct);
    Task<IReadOnlyList<PlaceLookupDto>> PlaceAsync(CancellationToken requestAborted);
    Task<IReadOnlyList<ClassSocialLookupDto>> ClassSocialAsync(CancellationToken requestAborted);
    Task<IReadOnlyList<DeityLookupDto>> DeityAsync(CancellationToken requestAborted);
    Task<IReadOnlyList<TimeLineLookupDto>> TimeLineAsync(CancellationToken requestAborted);
}
