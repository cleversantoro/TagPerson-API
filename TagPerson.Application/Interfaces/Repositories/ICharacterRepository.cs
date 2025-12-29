using TagPerson.Domain.Entities;

namespace TagPerson.Application.Interfaces.Repositories;

public interface ICharacterRepository
{
    Task<IReadOnlyList<Character>> ListAsync(CancellationToken ct);
    Task<Character?> GetAsync(int id, CancellationToken ct);
    Task<Character?> GetSheetAsync(int id, CancellationToken ct);
    Task AddAsync(Character character, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}
