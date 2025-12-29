using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TagPerson.Domain.Entities;

[Table("usuarios")]
public class AppUser
{
    [Key] [Column("id")] public int Id { get; set; }
    [Column("username")] [MaxLength(80)] public string Username { get; set; } = "";
    [Column("password_hash")] [MaxLength(200)] public string PasswordHash { get; set; } = "";
    [Column("role")] [MaxLength(40)] public string Role { get; set; } = "user";
    [Column("is_active")] public int IsActive { get; set; } = 1;
    [Column("created_at")] public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
