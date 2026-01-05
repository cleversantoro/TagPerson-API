using System.ComponentModel.DataAnnotations.Schema;

namespace TagPerson.Domain.Entities;

[Table("personagem_caracterizacao")]
public class CharacterCharacterization
{
    [Column("id_personagem")] public int CharacterId { get; set; } 
    
    public Character Character { get; set; } = default!;
    
    [Column("id_caracterizacao")] public int CharacterizationId { get; set; }
    
    public Characterization Characterization { get; set; } = default!;
    
    [Column("nivel")] public int? Level { get; set; }
}
