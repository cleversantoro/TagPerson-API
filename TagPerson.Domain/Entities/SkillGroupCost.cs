using System.ComponentModel.DataAnnotations.Schema;

namespace TagPerson.Domain.Entities;

[Table("habilidade_grupo_custo")]
public class SkillGroupCost
{
    [Column("id_habilidade")] public int SkillId { get; set; }
    [Column("id_habilidade_grupo")] public int SkillGroupId { get; set; }
    [Column("custo")] public int? Cost { get; set; }
}
