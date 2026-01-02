using Microsoft.EntityFrameworkCore;
using TagPerson.Domain.Entities;

namespace TagPerson.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Character> Characters => Set<Character>();
    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<Race> Races => Set<Race>();
    public DbSet<Profession> Professions => Set<Profession>();
    public DbSet<Specialization> Specializations => Set<Specialization>();
    public DbSet<Place> Places => Set<Place>();

    public DbSet<Skill> Skills => Set<Skill>();
    public DbSet<SkillGroup> SkillGroups => Set<SkillGroup>();
    public DbSet<SkillGroupCost> SkillGroupCosts => Set<SkillGroupCost>();
    public DbSet<SkillSpecialization> SkillSpecialization => Set<SkillSpecialization>();
    public DbSet<SkillImproved> SkillImproved => Set<SkillImproved>();
    public DbSet<CombatGroup> CombatGroups => Set<CombatGroup>();
    public DbSet<CombatGroupCost> CombatGroupCosts => Set<CombatGroupCost>();
    public DbSet<CombatSkill> CombatSkills => Set<CombatSkill>();
    public DbSet<Spell> Spells => Set<Spell>();
    public DbSet<SpellGroup> SpellGroups => Set<SpellGroup>();
    public DbSet<SpellGroupCost> SpellGroupCosts => Set<SpellGroupCost>();
    public DbSet<Category> Categories => Set<Category>();

    public DbSet<Equipment> Equipments => Set<Equipment>();
    public DbSet<EquipmentGroup> EquipmentGroups => Set<EquipmentGroup>();
    public DbSet<EquipmentWeaponStats> EquipmentWeaponStats => Set<EquipmentWeaponStats>();
    public DbSet<EquipmentDefenseStats> EquipmentDefenseStats => Set<EquipmentDefenseStats>();

    public DbSet<CharacterSkill> CharacterSkills => Set<CharacterSkill>();
    public DbSet<CharacterSpell> CharacterSpells => Set<CharacterSpell>();
    public DbSet<CharacterCombatSkill> CharacterCombatSkills => Set<CharacterCombatSkill>();
    public DbSet<CharacterEquipment> CharacterEquipments => Set<CharacterEquipment>();
    public DbSet<CharacterSkillSpecialization> CharacterSkillSpecializations => Set<CharacterSkillSpecialization>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CharacterSkill>().HasKey(x => new { x.CharacterId, x.SkillId });
        modelBuilder.Entity<CharacterSpell>().HasKey(x => new { x.CharacterId, x.SpellId });
        modelBuilder.Entity<CharacterCombatSkill>().HasKey(x => new { x.CharacterId, x.CombatSkillId });
        modelBuilder.Entity<CharacterEquipment>().HasKey(x => new { x.CharacterId, x.EquipmentId });
        modelBuilder.Entity<CharacterSkillSpecialization>().HasKey(x => x.Id);

        modelBuilder.Entity<EquipmentWeaponStats>().HasKey(x => x.Id);
        modelBuilder.Entity<EquipmentDefenseStats>().HasKey(x => x.Id);
        modelBuilder.Entity<SkillGroupCost>().HasKey(x => new { x.SkillId, x.SkillGroupId });
        modelBuilder.Entity<CombatGroupCost>().HasKey(x => new { x.CombatSkillId, x.CombatGroupId });
        modelBuilder.Entity<SpellGroupCost>().HasKey(x => new { x.SpellId, x.SpellGroupId });

        modelBuilder.Entity<EquipmentWeaponStats>()
            .HasOne(x => x.Equipment).WithOne(x => x.WeaponStats).HasForeignKey<EquipmentWeaponStats>(x => x.EquipmentId);

        modelBuilder.Entity<EquipmentDefenseStats>()
            .HasOne(x => x.Equipment).WithOne(x => x.DefenseStats).HasForeignKey<EquipmentDefenseStats>(x => x.EquipmentId);

        modelBuilder.Entity<Place>()
            .HasOne(x => x.Parent).WithMany(x => x.Children).HasForeignKey(x => x.ParentId);

        modelBuilder.Entity<Specialization>()
            .HasOne(x => x.Profession).WithMany().HasForeignKey(x => x.ProfessionId);

        modelBuilder.Entity<Character>()
            .HasOne(x => x.Race).WithMany().HasForeignKey(x => x.RaceId);

        modelBuilder.Entity<Character>()
            .HasOne(x => x.Profession).WithMany().HasForeignKey(x => x.ProfessionId);

        modelBuilder.Entity<Character>()
            .HasOne(x => x.Specialization).WithMany().HasForeignKey(x => x.SpecializationId);

        modelBuilder.Entity<Character>()
            .HasOne(x => x.BirthPlace).WithMany().HasForeignKey(x => x.BirthPlaceId);

        modelBuilder.Entity<AppUser>()
            .HasIndex(x => x.Username)
            .IsUnique();
    }
}
