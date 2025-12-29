using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TagPerson.Domain.Entities;

[Table("localidade")]
public class Place
{
    [Key] [Column("id")] public int Id { get; set; }
    [Column("nome")] [MaxLength(60)] public string Name { get; set; } = "";
    [Column("brasao")] [MaxLength(60)] public string? CoatOfArms { get; set; }
    [Column("nota")] public string? Note { get; set; }
    [Column("autor")] [MaxLength(120)] public string? Author { get; set; }

    [Column("id_pai")] public int? ParentId { get; set; }
    public Place? Parent { get; set; }
    public ICollection<Place> Children { get; set; } = new List<Place>();

    [Column("x")] public int? X { get; set; }
    [Column("y")] public int? Y { get; set; }
}
