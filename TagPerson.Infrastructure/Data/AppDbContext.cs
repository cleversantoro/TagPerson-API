using Microsoft.EntityFrameworkCore;
using TagPerson.Domain.Entities;

namespace TagPerson.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    #region Auxiliares
    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<Race> Races => Set<Race>();
    public DbSet<Profession> Professions => Set<Profession>();
    public DbSet<RaceProfession> RaceProfessions => Set<RaceProfession>();
    public DbSet<Specialization> Specializations => Set<Specialization>();
    public DbSet<Place> Places => Set<Place>();
    public DbSet<Deity> Deitys => Set<Deity>();
    public DbSet<ClassSocial> ClassSocials => Set<ClassSocial>();
    public DbSet<TimeLine> Timelines => Set<TimeLine>();
    public DbSet<Category> Categories => Set<Category>();
    #endregion

    #region Habilidades
    public DbSet<Skill> Skills => Set<Skill>();
    public DbSet<SkillGroup> SkillGroups => Set<SkillGroup>();
    public DbSet<SkillGroupCost> SkillGroupCosts => Set<SkillGroupCost>();
    public DbSet<SkillSpecialization> SkillSpecialization => Set<SkillSpecialization>();
    public DbSet<SkillImproved> SkillImproved => Set<SkillImproved>();
    #endregion

    #region Caracterizações
    public DbSet<Characterization> Characterizations => Set<Characterization>();
    public DbSet<CharacterizationGroup> CharacterizationGroups => Set<CharacterizationGroup>();
    public DbSet<CharacterizationGroupCost> CharacterizationGroupCosts => Set<CharacterizationGroupCost>();
    public DbSet<CharacterizationType> CharacterizationTypes => Set<CharacterizationType>();
    #endregion

    #region Combate
    public DbSet<CombatGroup> CombatGroups => Set<CombatGroup>();
    public DbSet<CombatGroupCost> CombatGroupCosts => Set<CombatGroupCost>();
    public DbSet<CombatSkill> CombatSkills => Set<CombatSkill>();
    public DbSet<CombatTechniquesBasics> CombatTechniquesBasics => Set<CombatTechniquesBasics>();
    public DbSet<CombatTechniquesEspecialization> CombatTechniquesEspecializations => Set<CombatTechniquesEspecialization>();
    public DbSet<CombatTechniquesProfession> CombatTechniquesProfessions => Set<CombatTechniquesProfession>();
    #endregion

    #region Magia
    public DbSet<Spell> Spells => Set<Spell>();
    public DbSet<SpellGroup> SpellGroups => Set<SpellGroup>();
    public DbSet<SpellGroupCost> SpellGroupCosts => Set<SpellGroupCost>();
    public DbSet<SpellEspecialization> SpellEspecializations => Set<SpellEspecialization>();
    public DbSet<SpellProfession> SpellProfessions => Set<SpellProfession>();
    #endregion

    #region Equipamentos
    public DbSet<Equipment> Equipments => Set<Equipment>();
    public DbSet<EquipmentGroup> EquipmentGroups => Set<EquipmentGroup>();
    public DbSet<EquipmentWeaponStats> EquipmentWeaponStats => Set<EquipmentWeaponStats>();
    public DbSet<EquipmentDefenseStats> EquipmentDefenseStats => Set<EquipmentDefenseStats>();
    #endregion

    #region Personagem
    public DbSet<Character> Characters => Set<Character>();
    public DbSet<CharacterSkill> CharacterSkills => Set<CharacterSkill>();
    public DbSet<CharacterSpell> CharacterSpells => Set<CharacterSpell>();
    public DbSet<CharacterCombatSkill> CharacterCombatSkills => Set<CharacterCombatSkill>();
    public DbSet<CharacterEquipment> CharacterEquipments => Set<CharacterEquipment>();
    public DbSet<CharacterSkillSpecialization> CharacterSkillSpecializations => Set<CharacterSkillSpecialization>();
    public DbSet<CharacterCharacterization> CharacterCharacterizations => Set<CharacterCharacterization>();
    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CharacterSkill>().HasKey(x => new { x.CharacterId, x.SkillId });
        modelBuilder.Entity<CharacterSpell>().HasKey(x => new { x.CharacterId, x.SpellId });
        modelBuilder.Entity<CharacterCombatSkill>().HasKey(x => new { x.CharacterId, x.CombatSkillId });
        modelBuilder.Entity<CharacterEquipment>().HasKey(x => new { x.CharacterId, x.EquipmentId });
        modelBuilder.Entity<CharacterSkillSpecialization>().HasKey(x => x.Id);
        modelBuilder.Entity<CharacterCharacterization>().HasKey(x => new { x.CharacterId, x.CharacterizationId });
        modelBuilder.Entity<RaceProfession>().HasKey(x => new { x.RaceId, x.ProfessionId });

        modelBuilder.Entity<EquipmentWeaponStats>().HasKey(x => x.Id);
        modelBuilder.Entity<EquipmentDefenseStats>().HasKey(x => x.Id);
        modelBuilder.Entity<SkillGroupCost>().HasKey(x => new { x.SkillId, x.SkillGroupId });
        modelBuilder.Entity<CombatGroupCost>().HasKey(x => new { x.CombatSkillId, x.CombatGroupId });
        modelBuilder.Entity<SpellGroupCost>().HasKey(x => new { x.SpellId, x.SpellGroupId });

        modelBuilder.Entity<CombatTechniquesBasics>(x =>{x.ToView("vw_tecnicas_basicas");x.HasNoKey();});
        modelBuilder.Entity<CombatTechniquesEspecialization>(x => { x.ToView("vw_tecnicas_especializacao"); x.HasNoKey(); });
        modelBuilder.Entity<CombatTechniquesProfession>(x => { x.ToView("vw_tecnicas_profissao"); x.HasNoKey(); });

        modelBuilder.Entity<SpellEspecialization>(x => { x.ToView("vw_magia_especializacao"); x.HasNoKey(); });
        modelBuilder.Entity<SpellProfession>(x => { x.ToView("vw_magia_profissao"); x.HasNoKey(); });

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


