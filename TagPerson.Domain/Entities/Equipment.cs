using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TagPerson.Domain.Entities;

[Table("equipamento")]
public class Equipment
{
    [Key] [Column("id")] public int Id { get; set; }
    [Column("id_grupo")] public int? GroupId { get; set; }
    [Column("nome")] [MaxLength(60)] public string Name { get; set; } = "";
    [Column("descricao")] [MaxLength(160)] public string? Description { get; set; }
    [Column("image_file")] [MaxLength(50)] public string? ImageFile { get; set; }
    [Column("valor")] public int? Price { get; set; }
    [Column("arma")] public int? IsWeapon { get; set; }
    [Column("defesa")] public int? IsDefense { get; set; }
    [Column("armadura")] public int? IsArmor { get; set; }
    [Column("escudo")] public int? IsShield { get; set; }
    [Column("capacete")] public int? IsHelmet { get; set; }

    public EquipmentWeaponStats? WeaponStats { get; set; }
    public EquipmentDefenseStats? DefenseStats { get; set; }
}
