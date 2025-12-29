using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TagPerson.Domain.Entities;

[Table("raca")]
public class Race
{
    [Key] [Column("id")] public int Id { get; set; }
    [Column("nome")] [MaxLength(60)] public string Name { get; set; } = "";
    [Column("descricao")] public string? Description { get; set; }
    [Column("image_file")] [MaxLength(60)] public string? ImageFile { get; set; }

    [Column("velocidade_inicio")] public int BaseSpeed { get; set; }
    [Column("energia_fisica")] public int EfBonus { get; set; }

    [Column("attr_agi")] public int BonusAgi { get; set; }
    [Column("attr_per")] public int BonusPer { get; set; }
    [Column("attr_int")] public int BonusInt { get; set; }
    [Column("attr_aur")] public int BonusAur { get; set; }
    [Column("attr_car")] public int BonusCar { get; set; }
    [Column("attr_for")] public int BonusFor { get; set; }
    [Column("attr_fis")] public int BonusFis { get; set; }

    [Column("altura_inicio")] public int? BaseHeight { get; set; }
    [Column("peso_inicio")] public int? BaseWeight { get; set; }
    [Column("idade_minima")] public int? AgeMin { get; set; }
    [Column("idade_maxima")] public int? AgeMax { get; set; }
}
