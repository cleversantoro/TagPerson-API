using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;


namespace TagPerson.Domain.Entities;

[Table("vw_tecnicas_basicas")]
[Keyless] 
public class CombatTechniquesBasics
{
    [Column("id_combate")] public int? CombatId { get; set; }
    [Column("id_prof_esp")] public int? ProfEspId { get; set; } 
    [Column("nome_combate")] public string? CombatName { get; set; } = string.Empty;
    [Column("atributo")] public string? AttributeCode { get; set; }
    [Column("efeito")] public string? Effect { get; set; }
    [Column("obs")] public string? Notes { get; set; }
    [Column("requisitos")] public string? Requisite { get; set; }
    [Column("quadro_rolagem")] public string? RollTable { get; set; }
    [Column("aprimoramento")] public string? Improvement { get; set; }
    [Column("id_combate_grupo")] public int? CombatGroupId { get; set; }
    [Column("nome_grupo")] public string? GroupName { get; set; } = string.Empty;
    [Column("id_categoria")] public int? CategoryId { get; set; }
    [Column("nome_categoria")] public string? CategoryName { get; set; } = string.Empty;
    [Column("custo")] public int? Cost { get; set; }
    [Column("bonus")] public int? Bonus { get; set; } 
    [Column("reducao")] public int? Reduction { get; set; }

}

