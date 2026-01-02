using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TagPerson.Domain.Entities;

[Table("habilidade_especializacao")]
public class SkillSpecialization
{
    [Key] [Column("id")] public int Id { get; set; }
    [Column("id_habilidade")] public int? SkillId { get; set; }
    [Column("sugestao")] [MaxLength(40)] public string? Suggestion { get; set; }
}
