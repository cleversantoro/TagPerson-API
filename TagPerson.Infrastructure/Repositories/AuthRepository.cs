using Microsoft.EntityFrameworkCore;
using TagPerson.Application.Interfaces.Repositories;
using TagPerson.Domain.Entities;
using TagPerson.Infrastructure.Data;

namespace TagPerson.Infrastructure.Repositories;

public sealed class AuthRepository : IAuthRepository
{
    private readonly AppDbContext _db;

    public AuthRepository(AppDbContext db)
    {
        _db = db;
    }

    public Task<AppUser?> GetByUsernameAsync(string username, CancellationToken ct)
    {
        return _db.Users.FirstOrDefaultAsync(x => x.Username == username, ct);
    }

    public Task<bool> AnyAsync(CancellationToken ct) => _db.Users.AnyAsync(ct);

    public async Task AddAsync(AppUser user, CancellationToken ct)
    {
        await _db.Users.AddAsync(user, ct);
    }

    public Task SaveChangesAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);
}
