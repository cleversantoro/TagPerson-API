using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TagPerson.Domain.Entities;

[Table("divindade")]
public class Deity
{
    [Key][Column("id")] public int Id { get; set; }
    [Column("nome")][MaxLength(60)] public string Name { get; set; } = "";
    [Column("dominio")][MaxLength(100)] public string? Domain { get; set; }
}
