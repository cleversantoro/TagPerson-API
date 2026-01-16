using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TagPerson.Domain.Entities;

[Table("especializacao")]
public class Specialization
{
    [Key] [Column("id")] public int Id { get; set; }
    [Column("nome")] [MaxLength(60)] public string Name { get; set; } = "";
    [Column("descricao")] public string? Description { get; set; }

    [Column("id_profissao")] public int? ProfessionId { get; set; }
    public Profession? Profession { get; set; }

    [Column("id_magia_grupo")] public int? SpellGroupId { get; set; }
    [Column("id_combate_grupo")] public int? CombatGroupId { get; set; }
}
