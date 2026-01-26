using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TagPerson.Domain.Entities;

[Table("magia_grupo")]
public class SpellGroup
{
    [Key] [Column("id")] public int Id { get; set; }
    [Column("id_pai")] public int? ParentId { get; set; }
    [Column("nome")] [MaxLength(100)] public string Name { get; set; } = "";
    [Column("profissao")] public int? IsProfession { get; set; }
    [Column("especializacao")] public int? IsEspecialization { get; set; }
}
