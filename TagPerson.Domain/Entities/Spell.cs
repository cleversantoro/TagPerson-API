using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TagPerson.Domain.Entities;

[Table("magia")]
public class Spell
{
    [Key] [Column("id")] public int Id { get; set; }
    [Column("nome")] [MaxLength(60)] public string Name { get; set; } = "";
    [Column("evocacao")] [MaxLength(40)] public string? Evocation { get; set; }
    [Column("alcance")] [MaxLength(40)] public string? Range { get; set; }
    [Column("duracao")] [MaxLength(60)] public string? Duration { get; set; }
    [Column("descricao")] public string? Description { get; set; }
    [Column("niveis")] public string? LevelsJson { get; set; }
}
