using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TagPerson.Domain.Entities;

[Table("linha_tempo")]
public class TimeLine
{
    [Key][Column("id")] public int Id { get; set; }

    [Column("id_localidade")] public int PlaceId { get; set; }
    public Place? Place { get; set; }

    [Column("ano")] public int Year { get; set; }
    [Column("evento")][MaxLength(300)] public string? Occurrence { get; set; }
}
