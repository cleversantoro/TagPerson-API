using TagPerson.Application.DTOs;
using TagPerson.Application.Interfaces;
using TagPerson.Application.Interfaces.Repositories;
using TagPerson.Domain.Entities;

namespace TagPerson.Application.Services;

public sealed class UserService : IUserService
{
    private readonly IUserRepository _repo;
    private readonly IPasswordHasher _hasher;

    public UserService(IUserRepository repo, IPasswordHasher hasher)
    {
        _repo = repo;
        _hasher = hasher;
    }

    public async Task<IReadOnlyList<UserDto>> ListAsync(CancellationToken ct)
    {
        var items = await _repo.ListAsync(ct);
        return items.Select(Map).ToList();
    }

    public async Task<UserDto?> GetAsync(int id, CancellationToken ct)
    {
        var user = await _repo.GetByIdAsync(id, ct);
        return user is null ? null : Map(user);
    }

    public async Task<UserDto?> GetByUsernameAsync(string username, CancellationToken ct)
    {
        var user = await _repo.GetByUsernameAsync(username, ct);
        return user is null ? null : Map(user);
    }

    public async Task<UserDto?> CreateAsync(CreateUserRequestDto request, CancellationToken ct)
    {
        var existing = await _repo.GetByUsernameAsync(request.Username, ct);
        if (existing is not null) return null;

        var user = new AppUser
        {
            Username = request.Username,
            PasswordHash = _hasher.Hash(request.Password),
            Role = request.Role,
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow
        };

        await _repo.AddAsync(user, ct);
        await _repo.SaveChangesAsync(ct);
        return Map(user);
    }

    public async Task<bool> UpdateAsync(int id, UpdateUserRequestDto request, CancellationToken ct)
    {
        var user = await _repo.GetByIdAsync(id, ct);
        if (user is null) return false;

        user.Role = request.Role;
        user.IsActive = request.IsActive;
        if (!string.IsNullOrWhiteSpace(request.Password))
        {
            user.PasswordHash = _hasher.Hash(request.Password);
        }

        await _repo.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeactivateAsync(int id, CancellationToken ct)
    {
        var user = await _repo.GetByIdAsync(id, ct);
        if (user is null) return false;

        user.IsActive = 0;
        await _repo.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> ChangePasswordAsync(string username, ChangePasswordRequestDto request, CancellationToken ct)
    {
        var user = await _repo.GetByUsernameAsync(username, ct);
        if (user is null || user.IsActive != 1) return false;
        if (!_hasher.Verify(request.CurrentPassword, user.PasswordHash)) return false;

        user.PasswordHash = _hasher.Hash(request.NewPassword);
        await _repo.SaveChangesAsync(ct);
        return true;
    }

    private static UserDto Map(AppUser user) =>
        new(user.Id, user.Username, user.Role, user.IsActive, user.CreatedAt);
}
