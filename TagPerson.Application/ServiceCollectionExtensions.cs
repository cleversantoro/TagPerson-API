using Microsoft.Extensions.DependencyInjection;
using TagPerson.Application.Interfaces;
using TagPerson.Application.Services;

namespace TagPerson.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICharacterService, CharacterService>();
        services.AddScoped<ILookupService, LookupService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ISkillService, SkillService>();
        services.AddScoped<ICombatService, CombatService>();
        services.AddScoped<ISpellService, SpellService>();
        return services;
    }
}
