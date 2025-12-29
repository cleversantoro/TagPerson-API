using TagPerson.Application.DTOs;

namespace TagPerson.Application.Interfaces;

public interface IUserService
{
    Task<IReadOnlyList<UserDto>> ListAsync(CancellationToken ct);
    Task<UserDto?> GetAsync(int id, CancellationToken ct);
    Task<UserDto?> GetByUsernameAsync(string username, CancellationToken ct);
    Task<UserDto?> CreateAsync(CreateUserRequestDto request, CancellationToken ct);
    Task<bool> UpdateAsync(int id, UpdateUserRequestDto request, CancellationToken ct);
    Task<bool> DeactivateAsync(int id, CancellationToken ct);
    Task<bool> ChangePasswordAsync(string username, ChangePasswordRequestDto request, CancellationToken ct);
}
