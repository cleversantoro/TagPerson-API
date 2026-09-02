using System.ComponentModel.DataAnnotations.Schema;

namespace TagPerson.Domain.Entities;

[Table("personagem_equipamento")]
public class CharacterEquipment
{
    [Column("id_personagem")] public int CharacterId { get; set; }
    public Character Character { get; set; } = default!;

    [Column("id_equipamento")] public int EquipmentId { get; set; }
    public Equipment Equipment { get; set; } = default!;

    [Column("quantidade")] public int? Qty { get; set; }
    [Column("equipado")] public bool Equipped { get; set; }
    [Column("slot")] public string Slot { get; set; } = "nenhum";
}
