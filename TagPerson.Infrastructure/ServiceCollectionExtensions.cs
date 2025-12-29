using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TagPerson.Application.Interfaces;
using TagPerson.Application.Interfaces.Repositories;
using TagPerson.Infrastructure.Data;
using TagPerson.Infrastructure.Options;
using TagPerson.Infrastructure.Repositories;
using TagPerson.Infrastructure.Services;

namespace TagPerson.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var cs = configuration.GetConnectionString("MySql");
        services.AddDbContext<AppDbContext>(opt =>
        {
            opt.UseMySql(cs, ServerVersion.AutoDetect(cs));
        });

        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
        services.Configure<AuthOptions>(configuration.GetSection(AuthOptions.SectionName));

        services.AddScoped<ICharacterRepository, CharacterRepository>();
        services.AddScoped<ILookupRepository, LookupRepository>();
        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ISkillRepository, SkillRepository>();
        services.AddScoped<ICombatRepository, CombatRepository>();
        services.AddScoped<ISpellRepository, SpellRepository>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddSingleton<JwtTokenService>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddScoped<AuthSeeder>();

        return services;
    }
}
