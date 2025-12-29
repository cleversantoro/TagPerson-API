using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TagPerson.Domain.Entities;

[Table("habilidade")]
public class Skill
{
    [Key] [Column("id")] public int Id { get; set; }
    [Column("id_habilidade_grupo")] public int? SkillGroupId { get; set; }
    [Column("nome")] [MaxLength(50)] public string Name { get; set; } = "";
    [Column("atributo")] [MaxLength(3)] public string? AttributeCode { get; set; }
    [Column("teste_nivel")] public int? LevelTest { get; set; }
    [Column("restrita")] public int? Restricted { get; set; }
    [Column("penalidades")] [MaxLength(50)] public string? Penalties { get; set; }
    [Column("tarefas_aperfeicoadas")] public string? ImprovedTasks { get; set; }
    [Column("descricao")] public string? Description { get; set; }
    [Column("niveis")] public string? LevelsJson { get; set; } // campo texto no SQL
    [Column("bonus")] public int? Bonus { get; set; }
    [Column("possui_especializacao")] public int? HasSpecialization { get; set; }
}
