using System.ComponentModel.DataAnnotations.Schema;

namespace TagPerson.Domain.Entities;

[Table("personagem_magia")]
public class CharacterSpell
{
    [Column("id_personagem")] public int CharacterId { get; set; }
    public Character Character { get; set; } = default!;

    [Column("id_magia")] public int SpellId { get; set; }
    public Spell Spell { get; set; } = default!;

    [Column("id_magia_grupo")] public int SpellGroupId { get; set; }
    public SpellGroup SpellGroup { get; set; } = default!;

    [Column("nivel")] public int? Level { get; set; }

    [Column("tipo")] public int? Type { get; set; }
}
