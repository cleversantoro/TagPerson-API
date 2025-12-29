using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TagPerson.Domain.Entities;

[Table("equipamento_defesa")]
public class EquipmentDefenseStats
{
    [Column("id")] public int Id { get; set; }
    [Column("id_equipamento")] public int? EquipmentId { get; set; }
    public Equipment Equipment { get; set; } = default!;

    [Column("defesa_base")] [MaxLength(3)] public string? BaseDefense { get; set; }
    [Column("absorcao")] public int? Absorption { get; set; }

    [Column("fisico_minimo")] public int? MinPhysic { get; set; }
    [Column("forca_minima")] public int? MinStrength { get; set; }

    [Column("P")] public int? P { get; set; }
    [Column("A")] public int? A { get; set; }
    [Column("E")] public int? E { get; set; }
    [Column("M")] public int? M { get; set; }
    [Column("H")] public int? H { get; set; }
}
