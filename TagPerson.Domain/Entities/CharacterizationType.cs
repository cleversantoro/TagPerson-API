using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TagPerson.Domain.Entities;

[Table("caracterizacao_tipo")]
public class CharacterizationType
{
    [Key][Column("id")] public int Id { get; set; }
    [Column("nome")][MaxLength(80)] public string Name { get; set; } = "";
    [Column("descricao")] public string? Description { get; set; }
    [Column("ordem_exibicao")] public int DisplayOrder { get; set; }
}
