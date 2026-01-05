using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TagPerson.Domain.Entities;

[Table("caracterizacao_grupo")]
public class CharacterizationGroup
{
    [Key] [Column("id")] public int Id { get; set; }
    [Column("id_caracterizacao_tipo")] public int CharacterizationTypeId { get; set; }
    [Column("nome")] [MaxLength(100)] public string Name { get; set; } = "";
    [Column("ordem_exibicao")] public int DisplayOrder { get; set; }
}
