using System.ComponentModel.DataAnnotations.Schema;

namespace TagPerson.Domain.Entities;

[Table("combate_grupo_custo")]
public class CombatGroupCost
{
    [Column("id_combate")] public int CombatSkillId { get; set; }
    [Column("id_combate_grupo")] public int CombatGroupId { get; set; }
    [Column("custo")] public int? Cost { get; set; }
    [Column("reducao")] public int? Reduction { get; set; }
}
