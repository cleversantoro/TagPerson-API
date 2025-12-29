using System.ComponentModel.DataAnnotations.Schema;

namespace TagPerson.Domain.Entities;

[Table("magia_grupo_custo")]
public class SpellGroupCost
{
    [Column("id_magia")] public int SpellId { get; set; }
    [Column("id_magia_grupo")] public int SpellGroupId { get; set; }
    [Column("custo")] public int? Cost { get; set; }
}
