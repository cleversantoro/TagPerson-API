using System.ComponentModel.DataAnnotations;

namespace TagPerson.Application.DTOs;

public sealed record UserDto(
    int Id,
    string Username,
    string Role,
    int IsActive,
    DateTime CreatedAt
);

public sealed record CreateUserRequestDto(
    [Required, MinLength(3), MaxLength(80)] string Username,
    [Required, MinLength(8), MaxLength(200)] string Password,
    [Required, MinLength(3), MaxLength(40)] string Role,
    [Range(0, 1)] int IsActive
);

public sealed record UpdateUserRequestDto(
    [Required, MinLength(3), MaxLength(40)] string Role,
    [Range(0, 1)] int IsActive,
    [MinLength(8), MaxLength(200)] string? Password
);

public sealed record ChangePasswordRequestDto(
    [Required, MinLength(8), MaxLength(200)] string CurrentPassword,
    [Required, MinLength(8), MaxLength(200)] string NewPassword
);
