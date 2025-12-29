using TagPerson.Application.DTOs;

namespace TagPerson.Application.Interfaces;

public interface IAuthService
{
    Task<TokenResponseDto?> AuthenticateAsync(TokenRequestDto request, CancellationToken ct);
}
