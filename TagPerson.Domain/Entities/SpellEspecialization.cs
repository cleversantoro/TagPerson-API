using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TagPerson.Domain.Entities;

[Table("vw_magias_especializacao")]
[Keyless]
public class SpellEspecialization
{
    [Column("id_magia")] public int? SpellId { get; set; }
    [Column("id_prof_esp")] public int? ProfEspId { get; set; }
    [Column("nome_magia")] public string? SpellName { get; set; } = string.Empty;
    [Column("id_magia_grupo")] public int? SpellGroupId { get; set; }
    [Column("nome_grupo")] public string? GroupName { get; set; } = string.Empty;
    [Column("descricao")] public string? Description { get; set; }
    [Column("evocacao")] public string? Evocation { get; set; }
    [Column("alcance")] public string? Range { get; set; }
    [Column("duracao")] public string? Duration { get; set; }
    [Column("niveis")] public string? Levels { get; set; }
    [Column("custo")] public int? Cost { get; set; }

}
