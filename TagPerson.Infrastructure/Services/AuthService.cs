using Microsoft.Extensions.Options;
using TagPerson.Application.DTOs;
using TagPerson.Application.Interfaces;
using TagPerson.Application.Interfaces.Repositories;
using TagPerson.Domain.Entities;
using TagPerson.Infrastructure.Options;

namespace TagPerson.Infrastructure.Services;

public sealed class AuthService : IAuthService
{
    private readonly IAuthRepository _repo;
    private readonly IPasswordHasher _hasher;
    private readonly JwtTokenService _tokenService;

    public AuthService(IAuthRepository repo, IPasswordHasher hasher, JwtTokenService tokenService)
    {
        _repo = repo;
        _hasher = hasher;
        _tokenService = tokenService;
    }

    public async Task<TokenResponseDto?> AuthenticateAsync(TokenRequestDto request, CancellationToken ct)
    {
        var user = await _repo.GetByUsernameAsync(request.Username, ct);
        if (user is null || user.IsActive != 1) return null;
        if (!_hasher.Verify(request.Password, user.PasswordHash)) return null;

        return _tokenService.CreateToken(user.Username, user.Role);
    }
}
