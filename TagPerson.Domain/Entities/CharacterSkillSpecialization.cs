using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TagPerson.Domain.Entities;

[Table("personagem_habilidade_especializacao")]
public class CharacterSkillSpecialization
{
    [Column("id")] public int Id { get; set; }
    [Column("id_personagem")] public int? CharacterId { get; set; }
    [Column("id_habilidade")] public int? SkillId { get; set; }
    [Column("id_habilidade_especializacao")] public int? SkillSpecializationId { get; set; }
    [Column("especializacao")] [MaxLength(40)] public string? Specialization { get; set; }
    [Column("nivel")] public int? Level { get; set; }
}
