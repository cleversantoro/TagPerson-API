using System.ComponentModel.DataAnnotations.Schema;

namespace TagPerson.Domain.Entities;

[Table("personagem_combate")]
public class CharacterCombatSkill
{
    [Column("id_personagem")] public int CharacterId { get; set; }
    public Character Character { get; set; } = default!;

    [Column("id_combate")] public int CombatSkillId { get; set; }
    public CombatSkill CombatSkill { get; set; } = default!;

    [Column("grupo")] public int? Group { get; set; }

    [Column("nivel")] public int? Level { get; set; }
}
