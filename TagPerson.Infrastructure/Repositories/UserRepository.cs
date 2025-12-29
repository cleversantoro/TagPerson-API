using Microsoft.EntityFrameworkCore;
using TagPerson.Application.Interfaces.Repositories;
using TagPerson.Domain.Entities;
using TagPerson.Infrastructure.Data;

namespace TagPerson.Infrastructure.Repositories;

public sealed class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;

    public UserRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<AppUser>> ListAsync(CancellationToken ct)
    {
        return await _db.Users.AsNoTracking().OrderBy(x => x.Username).ToListAsync(ct);
    }

    public Task<AppUser?> GetByIdAsync(int id, CancellationToken ct)
    {
        return _db.Users.FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public Task<AppUser?> GetByUsernameAsync(string username, CancellationToken ct)
    {
        return _db.Users.FirstOrDefaultAsync(x => x.Username == username, ct);
    }

    public async Task AddAsync(AppUser user, CancellationToken ct)
    {
        await _db.Users.AddAsync(user, ct);
    }

    public Task SaveChangesAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);
}
