using System.ComponentModel.DataAnnotations.Schema;

namespace TagPerson.Domain.Entities;

[Table("personagem_combate")]
public class CharacterCombatSkill
{
    [Column("id_personagem")] public int CharacterId { get; set; }
    public Character Character { get; set; } = default!;

    [Column("id_combate")] public int CombatSkillId { get; set; }
    public CombatSkill CombatSkill { get; set; } = default!;

    [Column("id_combate_grupo")] public int CombatGroupId { get; set; }
    public CombatGroup CombatGroup { get; set; } = default!;

    [Column("nivel")] public int? Level { get; set; }

    [Column("tipo")] public int? Type { get; set; }
}
