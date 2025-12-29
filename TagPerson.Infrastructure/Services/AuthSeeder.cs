using Microsoft.Extensions.Options;
using TagPerson.Application.Interfaces;
using TagPerson.Application.Interfaces.Repositories;
using TagPerson.Domain.Entities;
using TagPerson.Infrastructure.Options;

namespace TagPerson.Infrastructure.Services;

public sealed class AuthSeeder
{
    private readonly IAuthRepository _repo;
    private readonly IPasswordHasher _hasher;
    private readonly AuthOptions _options;

    public AuthSeeder(IAuthRepository repo, IPasswordHasher hasher, IOptions<AuthOptions> options)
    {
        _repo = repo;
        _hasher = hasher;
        _options = options.Value;
    }

    public async Task EnsureSeededAsync(CancellationToken ct)
    {
        if (_options.Seed != 1) return;
        if (await _repo.AnyAsync(ct)) return;

        var user = new AppUser
        {
            Username = _options.Username,
            PasswordHash = _hasher.Hash(_options.Password),
            Role = _options.Role,
            IsActive = 1,
            CreatedAt = DateTime.UtcNow
        };

        await _repo.AddAsync(user, ct);
        await _repo.SaveChangesAsync(ct);
    }
}
