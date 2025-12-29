using TagPerson.Domain.Entities;

namespace TagPerson.Application.Interfaces.Repositories;

public interface IAuthRepository
{
    Task<AppUser?> GetByUsernameAsync(string username, CancellationToken ct);
    Task<bool> AnyAsync(CancellationToken ct);
    Task AddAsync(AppUser user, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}
