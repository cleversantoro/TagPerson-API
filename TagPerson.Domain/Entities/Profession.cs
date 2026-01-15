using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TagPerson.Domain.Entities;

[Table("profissao")]
public class Profession
{
    [Key] [Column("id")] public int Id { get; set; }
    [Column("nome")] [MaxLength(60)] public string Name { get; set; } = "";
    [Column("image_file")] [MaxLength(60)] public string? ImageFile { get; set; }
    [Column("descricao")] public string? Description { get; set; }
    [Column("equipamentos_iniciais")] public string? StartingEquipment { get; set; }

    [Column("moedas_cobre")] public int? CoinsCopper { get; set; }
    [Column("moedas_prata")] public int? CoinsSilver { get; set; }
    [Column("moedas_ouro")] public int? CoinsGold { get; set; }

    [Column("energia_heroica")] public int? HeroicEnergy { get; set; }
    [Column("pontos_habilidade")] public int? SkillPoints { get; set; }
    [Column("pontos_arma")] public int? WeaponPoints { get; set; }
    [Column("pontos_combate")] public int? CombatPoints { get; set; }

    [Column("penalidade_habilidade_grupo")] public int? PenalizedSkillGroup { get; set; }
    [Column("especialidade_habilidade")] public int? SpecializationSkill { get; set; }

    [Column("atributo_magia")] public int? AttributeForMagic { get; set; }
    [Column("id_magia_grupo")] public int? SpellGroup { get; set; }

    [Column("defesa_base")][MaxLength(2)] public string? BasicDefense { get; set; }
    [Column("arma_dano_maximo")] public int? WeaponDamageMaximun { get; set; }
    [Column("absorcao")][MaxLength(4)] public string? Absorption { get; set; }

    public ICollection<RaceProfession> Races { get; set; } = new List<RaceProfession>();
}
