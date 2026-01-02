using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TagPerson.Domain.Entities;

[Table("habilidade_aperfeicoadas")]
public class SkillImproved
{
    [Key] [Column("id")] public int Id { get; set; }
    [Column("id_habilidade")] public int? SkillId { get; set; }
    [Column("id_habilidade_grupo")] public int? SkillGroupId { get; set; }
    [Column("descricao")] [MaxLength(255)] public string? Description { get; set; }
}
