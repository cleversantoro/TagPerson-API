using TagPerson.Application.DTOs;
using TagPerson.Domain.Entities;

namespace TagPerson.Application.Interfaces.Repositories;

public interface ILookupRepository
{
    Task<IReadOnlyList<Race>> RacesAsync(CancellationToken ct);
    Task<IReadOnlyList<Profession>> ProfessionsAsync(CancellationToken ct);
    Task<IReadOnlyList<Skill>> SkillsAsync(CancellationToken ct);
    Task<IReadOnlyList<Spell>> SpellsAsync(CancellationToken ct);
    Task<IReadOnlyList<CombatSkill>> CombatAsync(CancellationToken ct);
    Task<IReadOnlyList<Equipment>> EquipmentsAsync(CancellationToken ct);
    Task<IReadOnlyList<Category>> CategoriesAsync(CancellationToken ct);
    Task<IReadOnlyList<Characterization>> CharacterizationAsync(CancellationToken ct);
    Task<IReadOnlyList<Place>> PlaceAsync(CancellationToken ct);
    Task<IReadOnlyList<ClassSocial>> ClassSocialAsync(CancellationToken ct);
    Task<IReadOnlyList<Deity>> DeityAsync(CancellationToken ct);
    Task<IReadOnlyList<TimeLine>> TimeLineAsync(CancellationToken ct);
    Task<IReadOnlyList<Specialization>> SpecializationAsync(CancellationToken ct);
}
