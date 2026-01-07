using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TagPerson.Domain.Entities;

[Table("caracterizacao")]
public class Characterization
{
    [Key] [Column("id")] public int Id { get; set; }
    
    [Column("id_caracterizacao_tipo")] public int? CharacterizationTypeId { get; set; }
    public CharacterizationType? CharacterizationType { get; set; }

    [Column("id_caracterizacao_grupo")] public int? CharacterizationGroupId { get; set; }
    public CharacterizationGroup? CharacterizationGroup { get; set; }

    [Column("nome")] [MaxLength(140)] public string Name { get; set; } = "";
    [Column("descricao")] public string? Description { get; set; }
    [Column("observacoes")] public string? Notes { get; set; }    
}
