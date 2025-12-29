using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TagPerson.Domain.Entities;

[Table("combate")]
public class CombatSkill
{
    [Key] [Column("id")] public int Id { get; set; }
    [Column("id_habilidade_grupo")] public int? SkillGroupId { get; set; }
    [Column("id_categoria")] public int? CategoryId { get; set; }
    [Column("nome")] [MaxLength(50)] public string Name { get; set; } = "";
    [Column("atributo")] [MaxLength(3)] public string? AttributeCode { get; set; }
    [Column("efeito")] public string? Effect { get; set; }
    [Column("requisitos")] public string? Requisite { get; set; }
    [Column("obs")] public string? Notes { get; set; }
    [Column("quadro_rolagem")] public string? RollTable { get; set; }
    [Column("aprimoramento")] public string? Improvement { get; set; }
    [Column("bonus")] public int? Bonus { get; set; }
    [Column("possui_especializacao")] public int? HasSpecialization { get; set; }
}
