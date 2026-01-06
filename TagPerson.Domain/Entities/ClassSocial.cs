using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TagPerson.Domain.Entities;

[Table("classe_social")]
public class ClassSocial
{
    [Key] [Column("id")] public int Id { get; set; }
    [Column("nome")] [MaxLength(80)] public string Name { get; set; } = "";
    [Column("is_default")] public int? IsDefault { get; set; }
}
