using System.ComponentModel.DataAnnotations.Schema;

namespace TagPerson.Domain.Entities;

[Table("raca_profissao")]
public class RaceProfession
{
    [Column("id_raca")] public int RaceId { get; set; }
    public Race Race { get; set; } = default!;

    [Column("id_profissao")] public int ProfessionId { get; set; }
    public Profession Profession { get; set; } = default!;
}
