using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TagPerson.Domain.Entities;

[Table("caracterizacao_grupo_custo")]
public class CharacterizationGroupCost
{
    [Key][Column("id")] public int Id { get; set; }
    [Column("id_caracterizacao")] public int? CharacterizationId { get; set; }
    [Column("id_caracterizacao_tipo")] public int? CharacterizationTypeId { get; set; }
    [Column("id_caracterizacao_grupo")] public int? CharacterizationGroupId { get; set; }
    [Column("id_localidade")] public int? PlaceId { get; set; }
    [Column("custo")] public int? Cost { get; set; }    
    [Column("is_inicial")] public int IsInitial { get; set; } 
    [Column("is_muito_rara")] public int IsRare { get; set; }
    [Column("permite_durante_jogo")] public int IsAllowGame { get; set; }
}
