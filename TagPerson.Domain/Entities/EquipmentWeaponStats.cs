using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TagPerson.Domain.Entities;

[Table("equipamento_armas")]
public class EquipmentWeaponStats
{
    [Column("id")] public int Id { get; set; }
    [Column("id_equipamento")] public int? EquipmentId { get; set; }
    public Equipment Equipment { get; set; } = default!;

    [Column("tipo")] [MaxLength(50)] public string? WeaponType { get; set; }
    [Column("custo")] public int? Cost { get; set; }
    [Column("alcance")] [MaxLength(50)] public string? Range { get; set; }
    [Column("forca_minima")] public int? MinStrength { get; set; }
    [Column("bonus")] public string? Bonus { get; set; }
    [Column("l")] public int? L { get; set; }
    [Column("m")] public int? M { get; set; }
    [Column("p")] public int? P { get; set; }
    [Column("25")] public int? V25 { get; set; }
    [Column("50")] public int? V50 { get; set; }
    [Column("75")] public int? V75 { get; set; }
    [Column("100")] public int? V100 { get; set; }
    [Column("Pq")] [MaxLength(50)] public string? Pq { get; set; }
    [Column("An")] [MaxLength(50)] public string? An { get; set; }
    [Column("El")] [MaxLength(50)] public string? El { get; set; }
    [Column("ME")] [MaxLength(50)] public string? Me { get; set; }
    [Column("Hu")] [MaxLength(50)] public string? Hu { get; set; }
}
