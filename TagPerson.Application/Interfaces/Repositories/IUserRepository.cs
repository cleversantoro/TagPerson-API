using TagPerson.Domain.Entities;

namespace TagPerson.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<IReadOnlyList<AppUser>> ListAsync(CancellationToken ct);
    Task<AppUser?> GetByIdAsync(int id, CancellationToken ct);
    Task<AppUser?> GetByUsernameAsync(string username, CancellationToken ct);
    Task AddAsync(AppUser user, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}
