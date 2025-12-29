using System.ComponentModel.DataAnnotations;

namespace TagPerson.Application.DTOs;

public sealed record TokenRequestDto(
    [Required, MinLength(3), MaxLength(80)] string Username,
    [Required, MinLength(8), MaxLength(200)] string Password
);

public sealed record TokenResponseDto(string AccessToken, DateTime ExpiresAtUtc);
