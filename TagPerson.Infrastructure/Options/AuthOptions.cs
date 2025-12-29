namespace TagPerson.Infrastructure.Options;

public sealed class AuthOptions
{
    public const string SectionName = "Auth";

    public string Username { get; init; } = "";
    public string Password { get; init; } = "";
    public string Role { get; init; } = "admin";
    public int Seed { get; init; } = 1;
}
