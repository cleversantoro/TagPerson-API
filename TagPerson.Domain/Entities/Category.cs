using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TagPerson.Domain.Entities;

[Table("categoria")]
public class Category
{
    [Key] [Column("id")] public int Id { get; set; }
    [Column("nome")] [MaxLength(60)] public string Name { get; set; } = "";
    [Column("icon")] [MaxLength(50)] public string? Icon { get; set; }
}
