using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TagPerson.Domain.Entities;

[Table("personagem")]
public class Character
{
    [Key] [Column("id")] public int Id { get; set; }

    [Column("nome")] [MaxLength(100)] public string Name { get; set; } = "";
    [Column("jogador")] [MaxLength(100)] public string? Player { get; set; }
    [Column("image_file")] [MaxLength(60)] public string? ImageFile { get; set; }

    [Column("nivel")] public int? Level { get; set; }

    [Column("raca")] public int? RaceId { get; set; }
    public Race? Race { get; set; }

    [Column("profissao")] public int? ProfessionId { get; set; }
    public Profession? Profession { get; set; }

    [Column("especializacao")] public int? SpecializationId { get; set; }
    public Specialization? Specialization { get; set; }

    [Column("divindade")] public int? DeityId { get; set; }
    [Column("classe_social")] [MaxLength(30)] public string? ClassSocial { get; set; }

    [Column("local_nascimento")] public int? BirthPlaceId { get; set; }
    public Place? BirthPlace { get; set; }

    [Column("att_agilidade")] public int? AttAgi { get; set; }
    [Column("att_percepcao")] public int? AttPer { get; set; }
    [Column("att_intelecto")] public int? AttInt { get; set; }
    [Column("att_aura")] public int? AttAur { get; set; }
    [Column("att_carisma")] public int? AttCar { get; set; }
    [Column("att_forca")] public int? AttFor { get; set; }
    [Column("att_fisico")] public int? AttFis { get; set; }

    [Column("defesa_ativa")] public int? ActiveDefense { get; set; }
    [Column("defesa_passiva")] public int? PassiveDefense { get; set; }
    [Column("energia_heroica_maxima")] public int? HeroicEnergyMax { get; set; }
    [Column("energia_heroica")] public int? HeroicEnergy { get; set; }
    [Column("energia_fisica")] public int? PhysicalEnergy { get; set; }
    [Column("absorcao")] public int? Absorption { get; set; }

    [Column("pontos_habilidade")] public int? PointsSkill { get; set; }
    [Column("pontos_arma")] public int? PointsWeapon { get; set; }
    [Column("pontos_combate")] public int? PointsCombat { get; set; }
    [Column("pontos_magia")] public int? PointsMagic { get; set; }

    [Column("altura")] public int? Height { get; set; }
    [Column("peso")] public int? Weight { get; set; }
    [Column("idade")] public int? Age { get; set; }

    [Column("olhos")] [MaxLength(60)] public string? Eyes { get; set; }
    [Column("cabelo")] [MaxLength(60)] public string? Hair { get; set; }
    [Column("pele")] [MaxLength(60)] public string? Skin { get; set; }
    [Column("aparencia")] public string? Appearance { get; set; }
    [Column("historia")] public string? History { get; set; }

    [Column("moedas_cobre")] public int? CoinsCopper { get; set; }
    [Column("moedas_prata")] public int? CoinsSilver { get; set; }
    [Column("moedas_ouro")] public int? CoinsGold { get; set; }

    public ICollection<CharacterSkill> Skills { get; set; } = new List<CharacterSkill>();
    public ICollection<CharacterSpell> Spells { get; set; } = new List<CharacterSpell>();
    public ICollection<CharacterCombatSkill> CombatSkills { get; set; } = new List<CharacterCombatSkill>();
    public ICollection<CharacterEquipment> Equipments { get; set; } = new List<CharacterEquipment>();
    public ICollection<CharacterCharacterization> Characterizations { get; set; } = new List<CharacterCharacterization>();
}
