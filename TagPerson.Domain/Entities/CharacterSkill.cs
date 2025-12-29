using System.ComponentModel.DataAnnotations.Schema;

namespace TagPerson.Domain.Entities;

[Table("personagem_habilidade")]
public class CharacterSkill
{
    [Column("id_personagem")] public int CharacterId { get; set; }
    public Character Character { get; set; } = default!;

    [Column("id_habilidade")] public int SkillId { get; set; }
    public Skill Skill { get; set; } = default!;

    [Column("nivel")] public int? Level { get; set; }
}
