using TagPerson.Application.Interfaces;
using TagPerson.Application.Interfaces.Repositories;
using TagPerson.Domain.Entities;

namespace TagPerson.Application.Services;

/// <summary>
/// Serviço responsável pela inicialização de personagens com dados baseados em raça e profissão.
/// </summary>
public sealed class CharacterInitializationService : ICharacterInitializationService
{
    private readonly IRaceProfessionRepository _raceProfessionRepository;
    private readonly ICharacterRepository _characterRepository;
    private readonly ISkillRepository _skillRepository;
    private readonly ICombatSkillRepository _combatSkillRepository;
    private readonly ISpellRepository _spellRepository;

    public CharacterInitializationService(
        IRaceProfessionRepository raceProfessionRepository,
        ICharacterRepository characterRepository,
        ISkillRepository skillRepository,
        ICombatSkillRepository combatSkillRepository,
        ISpellRepository spellRepository)
    {
        _raceProfessionRepository = raceProfessionRepository;
        _characterRepository = characterRepository;
        _skillRepository = skillRepository;
        _combatSkillRepository = combatSkillRepository;
        _spellRepository = spellRepository;
    }

    /// <summary>
    /// Inicializa um personagem com atributos, habilidades e magias baseado na raça e profissão.
    /// </summary>
    public async Task InitializeCharacterAsync(Character character, CancellationToken ct)
    {
        // Validar que raça e profissão foram definidas
        if (!character.RaceId.HasValue || !character.ProfessionId.HasValue)
        {
            throw new InvalidOperationException("Raça e profissão são obrigatórias para inicializar um personagem.");
        }

        // Buscar raça e profissão
        var race = await _characterRepository.GetRaceByIdAsync(character.RaceId.Value, ct);
        var profession = await _characterRepository.GetProfessionByIdAsync(character.ProfessionId.Value, ct);

        if (race is null || profession is null)
        {
            throw new InvalidOperationException("Raça ou profissão inválida.");
        }

        // Validar que a combinação raça/profissão é válida
        var isValidCombination = await _raceProfessionRepository.ExistsAsync(character.RaceId.Value, character.ProfessionId.Value, ct);
        if (!isValidCombination)
        {
            throw new InvalidOperationException("A combinação de raça e profissão não é permitida.");
        }

        // Inicializar atributos baseado na raça
        InitializeAttributesByRace(character, race);

        // Inicializar pontos baseado na profissão
        InitializePointsByProfession(character, profession);

        // Inicializar defesas
        InitializeDefenses(character, profession);

        // Inicializar moedas
        character.CoinsCopper = profession.CoinsCopper ?? 0;
        character.CoinsSilver = profession.CoinsSilver ?? 0;
        character.CoinsGold = profession.CoinsGold ?? 0;

        character.ClassSocialId = 7;

        // Adicionar habilidades da profissão
        //await AddProfessionSkillsAsync(character, profession, ct);

        // Adicionar técnicas de combate da profissão
        //await AddProfessionCombatSkillsAsync(character, profession, ct);

        // Adicionar magias da profissão
        //await AddProfessionSpellsAsync(character, profession, ct);
    }

    /// <summary>
    /// Inicializa os atributos do personagem baseado na raça.
    /// </summary>
    private static void InitializeAttributesByRace(Character character, Race race)
    {
        // Valores base de atributo (você pode ajustar esses valores conforme suas regras)
        //const int baseAttributeValue = 3;

        character.AttAgi = race.BonusAgi;
        character.AttPer = race.BonusPer;
        character.AttInt = race.BonusInt;
        character.AttAur = race.BonusAur;
        character.AttCar = race.BonusCar;
        character.AttFor = race.BonusFor;
        character.AttFis = race.BonusFis;
    }

    /// <summary>
    /// Inicializa os pontos do personagem baseado na profissão.
    /// </summary>
    private static void InitializePointsByProfession(Character character, Profession profession)
    {
        character.PointsSkill = profession.SkillPoints ?? 10;
        character.PointsWeapon = profession.WeaponPoints ?? 5;
        character.PointsCombat = profession.CombatPoints ?? 10;
        character.PointsMagic = profession.SpellGroup is not null ? 10 : 0;
        character.HeroicEnergyMax = profession.HeroicEnergy ?? 10;
        character.HeroicEnergy = character.HeroicEnergyMax;
    }

    /// <summary>
    /// Inicializa as defesas do personagem baseado na profissão.
    /// </summary>
    private static void InitializeDefenses(Character character, Profession profession)
    {
        // Defesa ativa e passiva base
        var basicDefense = profession.BasicDefense switch
        {
            "Alto" => 4,
            "Médio" => 2,
            "Baixo" => 0,
            _ => 2
        };

        character.ActiveDefense = basicDefense;
        character.PassiveDefense = basicDefense - 1;

        // Absorção
        var absorption = profession.Absorption switch
        {
            "Alto" => 4,
            "Médio" => 2,
            "Baixo" => 0,
            _ => 0
        };

        character.Absorption = absorption;
    }

    /// <summary>
    /// Adiciona as habilidades padrão da profissão ao personagem.
    /// </summary>
    private async Task AddProfessionSkillsAsync(Character character, Profession profession, CancellationToken ct)
    {
        // Você pode adicionar lógica para buscar habilidades padrão da profissão
        // Por exemplo, através de uma tabela ProfessionSkill ou similar
        // Exemplo simples: adicionar habilidades específicas por profissão
        
        // Esta é uma implementação básica - você pode expandir conforme necessário
        var defaultSkills = GetDefaultSkillsByProfession(profession.Id);
        
        foreach (var skillId in defaultSkills)
        {
            var skill = await _skillRepository.GetByIdAsync(skillId, ct);
            if (skill is not null)
            {
                character.Skills.Add(new CharacterSkill
                {
                    CharacterId = character.Id,
                    SkillId = skillId,
                    Level = 1
                });
            }
        }
    }

    /// <summary>
    /// Adiciona as técnicas de combate padrão da profissão ao personagem.
    /// </summary>
    private async Task AddProfessionCombatSkillsAsync(Character character, Profession profession, CancellationToken ct)
    {
        // Buscar técnicas de combate padrão da profissão
        var defaultCombatSkills = GetDefaultCombatSkillsByProfession(profession.Id);
        
        foreach (var combatSkillId in defaultCombatSkills)
        {
            var combatSkill = await _combatSkillRepository.GetByIdAsync(combatSkillId, ct);
            if (combatSkill is not null)
            {
                character.CombatSkills.Add(new CharacterCombatSkill
                {
                    CharacterId = character.Id,
                    CombatSkillId = combatSkillId,
                    Level = 1
                });
            }
        }
    }

    /// <summary>
    /// Adiciona as magias padrão da profissão ao personagem.
    /// </summary>
    private async Task AddProfessionSpellsAsync(Character character, Profession profession, CancellationToken ct)
    {
        // Se a profissão não tem grupo de magia, não adiciona magias
        if (profession.SpellGroup is null || profession.SpellGroup == 0)
        {
            return;
        }

        // Buscar magias do grupo da profissão
        var defaultSpells = GetDefaultSpellsByProfession(profession.Id);
        
        foreach (var spellId in defaultSpells)
        {
            var spell = await _spellRepository.GetByIdAsync(spellId, ct);
            if (spell is not null)
            {
                character.Spells.Add(new CharacterSpell
                {
                    CharacterId = character.Id,
                    SpellId = spellId,
                    Level = 1
                });
            }
        }
    }

    /// <summary>
    /// Retorna os IDs das habilidades padrão para uma profissão.
    /// NOTA: Esta implementação é básica e deve ser expandida com dados reais do seu banco.
    /// </summary>
    private static List<int> GetDefaultSkillsByProfession(int professionId)
    {
        // Esta é uma implementação placeholder
        // Você deve consultar o banco de dados ou ter uma tabela que defina
        // quais habilidades cada profissão começa com
        return professionId switch
        {
            1 => new List<int> { 1, 2, 3 }, // Exemplo: Guerreiro
            2 => new List<int> { 4, 5, 6 }, // Exemplo: Ladino
            3 => new List<int> { 7, 8, 9 }, // Exemplo: Mago
            _ => new List<int>()
        };
    }

    /// <summary>
    /// Retorna os IDs das técnicas de combate padrão para uma profissão.
    /// NOTA: Esta implementação é básica e deve ser expandida com dados reais do seu banco.
    /// </summary>
    private static List<int> GetDefaultCombatSkillsByProfession(int professionId)
    {
        // Esta é uma implementação placeholder
        // Você deve consultar o banco de dados ou ter uma tabela que defina
        // quais técnicas de combate cada profissão começa com
        return professionId switch
        {
            1 => new List<int> { 1, 2 }, // Exemplo: Guerreiro
            2 => new List<int> { 3, 4 }, // Exemplo: Ladino
            3 => new List<int> { 5 },    // Exemplo: Mago
            _ => new List<int>()
        };
    }

    /// <summary>
    /// Retorna os IDs das magias padrão para uma profissão.
    /// NOTA: Esta implementação é básica e deve ser expandida com dados reais do seu banco.
    /// </summary>
    private static List<int> GetDefaultSpellsByProfession(int professionId)
    {
        // Esta é uma implementação placeholder
        // Você deve consultar o banco de dados ou ter uma tabela que defina
        // quais magias cada profissão começa com
        return professionId switch
        {
            3 => new List<int> { 1, 2, 3 }, // Exemplo: Mago
            4 => new List<int> { 4, 5 },    // Exemplo: Sacerdote
            _ => new List<int>()
        };
    }
}
