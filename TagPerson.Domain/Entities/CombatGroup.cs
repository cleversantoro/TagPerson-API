using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TagPerson.Domain.Entities;

[Table("combate_grupo")]
public class CombatGroup
{
    [Key] [Column("id")] public int Id { get; set; }
    [Column("id_pai")] public int? ParentId { get; set; }
    [Column("nome")] [MaxLength(100)] public string Name { get; set; } = "";
}
