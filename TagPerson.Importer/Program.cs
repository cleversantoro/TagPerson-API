using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using TagPerson.Infrastructure.Data;
using TagPerson.Domain.Entities;
using System.Text.RegularExpressions;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(cfg =>
    {
        cfg.AddJsonFile("appsettings.json", optional: false);
    })
    .Build();

var config = host.Services.GetService(typeof(IConfiguration)) as IConfiguration;
if (config is null) throw new Exception("Configuration not loaded.");

var mysqlCs = config.GetConnectionString("MySql")!;
var sqlitePath = config["Importer:SqlitePath"]!;

Console.WriteLine("SQLite:", sqlitePath);
Console.WriteLine("MySQL :", mysqlCs);

var dbOpt = new DbContextOptionsBuilder<AppDbContext>()
    .UseMySql(mysqlCs, ServerVersion.AutoDetect(mysqlCs))
    .Options;

await using var mysql = new AppDbContext(dbOpt);

// NOTE: schema deve existir (use docker-compose + init.sql)
await mysql.Database.OpenConnectionAsync();

await using var sqlite = new SqliteConnection($"Data Source={sqlitePath};Mode=ReadOnly");
await sqlite.OpenAsync();

static async Task<List<Dictionary<string, object?>>> ReadAll(SqliteConnection con, string sql)
{
    await using var cmd = con.CreateCommand();
    cmd.CommandText = sql;
    await using var rd = await cmd.ExecuteReaderAsync();
    var list = new List<Dictionary<string, object?>>();
    while (await rd.ReadAsync())
    {
        var d = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
        for (int i = 0; i < rd.FieldCount; i++)
            d[rd.GetName(i)] = await rd.IsDBNullAsync(i) ? null : rd.GetValue(i);
        list.Add(d);
    }
    return list;
}

// Helper conversions
static int? I(object? v) => v is null ? null : Convert.ToInt32(v);
static int I0(object? v) => v is null ? 0 : Convert.ToInt32(v);
static string? S(object? v) => v?.ToString();

Console.WriteLine("Importing rules...");

// races
var races = await ReadAll(sqlite, "SELECT * FROM raca");
foreach (var r in races)
{
    var id = I0(r["id"]);
    // SQLite raca possui: id,nome,descricao,base_speed,ef, attribute_bonus (7 cols?) 
    // No seu DB os bônus normalmente vêm em campos separados ou em texto; vamos tentar detectar.
    // Se não existirem, ficam 0 (você ainda pode ajustar após import).
    var race = new Race
    {
        Id = id,
        Name = S(r.TryGetValue("nome", out var n) ? n : r["name"]) ?? $"Raca {id}",
        Description = S(r.TryGetValue("descricao", out var d) ? d : null),
        ImageFile = S(r.TryGetValue("image_file", out var im) ? im : null),
        BaseSpeed = I0(r.TryGetValue("velocidade_inicio", out var bs) ? bs : 0),
        EfBonus = I0(r.TryGetValue("energia_fisica", out var ef) ? ef : 0),
        // bônus individuais: se existirem colunas, usamos; senão 0.
        BonusInt = I0(r.TryGetValue("attr_int", out var bi) ? bi : 0),
        BonusAur = I0(r.TryGetValue("attr_aur", out var ba) ? ba : 0),
        BonusCar = I0(r.TryGetValue("attr_car", out var bc) ? bc : 0),
        BonusFor = I0(r.TryGetValue("attr_for", out var bf) ? bf : 0),
        BonusFis = I0(r.TryGetValue("attr_fis", out var bfi) ? bfi : 0),
        BonusAgi = I0(r.TryGetValue("attr_agi", out var bag) ? bag : 0),
        BonusPer = I0(r.TryGetValue("attr_per", out var bp) ? bp : 0),
        BaseHeight = I(r.TryGetValue("altura_inicio", out var h) ? h : null),
        BaseWeight = I(r.TryGetValue("peso_inicio", out var w) ? w : null),
        AgeMin = I(r.TryGetValue("idade_minima", out var amin) ? amin : null),
        AgeMax = I(r.TryGetValue("idade_maxima", out var amax) ? amax : null),
    };

    // Alguns bancos guardam attribute_bonus como string tipo "[0,0,...]"
    if (race.BonusInt==0 && r.TryGetValue("attribute_bonus", out var ab) && ab is string s && s.Contains("["))
    {
        try
        {
            // extrai ints da string
            var nums = IntArrayRegex().Matches(s).Select(m => int.Parse(m.Value)).ToArray();
            if (nums.Length >= 7)
            {
                race.BonusInt = nums[0];
                race.BonusAur = nums[1];
                race.BonusCar = nums[2];
                race.BonusFor = nums[3];
                race.BonusFis = nums[4];
                race.BonusAgi = nums[5];
                race.BonusPer = nums[6];
            }
        } catch { }
    }

    mysql.Races.Update(race);
}
await mysql.SaveChangesAsync();

// professions
var profs = await ReadAll(sqlite, "SELECT * FROM profissao");
foreach (var p in profs)
{
    var id = I0(p["id"]);
    var prof = new Profession
    {
        Id = id,
        Name = S(p.TryGetValue("nome", out var n) ? n : p["name"]) ?? $"Profissao {id}",
        ImageFile = S(p.TryGetValue("image_file", out var im) ? im : null),
        Description = S(p.TryGetValue("descricao", out var d) ? d : null),
        StartingEquipment = S(p.TryGetValue("equipamentos_iniciais", out var ei) ? ei : null),
        CoinsCopper = I(p.TryGetValue("moedas_cobre", out var mc) ? mc : null),
        HeroicEnergy = I(p.TryGetValue("energia_heroica", out var eh) ? eh : null),
        SkillPoints = I0(p.TryGetValue("pontos_habilidade", out var sp) ? sp : 0),
        WeaponPoints = I0(p.TryGetValue("pontos_arma", out var wp) ? wp : 0),
        CombatPoints = I0(p.TryGetValue("pontos_combate", out var cp) ? cp : 0),
        PenalizedSkillGroup = I(p.TryGetValue("penalidade_habilidade_grupo", out var psg) ? psg : null),
        SpecializationSkill = I(p.TryGetValue("especialidade_habilidade", out var ss) ? ss : null),
        AttributeForMagic = I0(p.TryGetValue("atributo_magia", out var afm) ? afm : -1),
        SpellGroup = I(p.TryGetValue("id_magia_grupo", out var sg) ? sg : null),
    };
    mysql.Professions.Update(prof);
}
await mysql.SaveChangesAsync();

Console.WriteLine("Importing skills...");
var skills = await ReadAll(sqlite, "SELECT * FROM habilidade");
foreach (var s in skills)
{
    var sk = new Skill
    {
        Id = I0(s["id"]),
        Name = S(s.TryGetValue("nome", out var n) ? n : s["name"]) ?? "",
        Description = S(s.TryGetValue("descricao", out var d) ? d : null),
        AttributeCode = S(s.TryGetValue("atributo", out var a) ? a : null),
        SkillGroupId = I(s.TryGetValue("id_habilidade_grupo", out var gid) ? gid : null),
        LevelTest = I(s.TryGetValue("teste_nivel", out var tn) ? tn : null),
        Restricted = I(s.TryGetValue("restrita", out var r) ? r : null),
        Penalties = S(s.TryGetValue("penalidades", out var pn) ? pn : null),
        ImprovedTasks = S(s.TryGetValue("tarefas_aperfeicoadas", out var ta) ? ta : null),
        HasSpecialization = I(s.TryGetValue("possui_especializacao", out var hs) ? hs : null),
        Bonus = I(s.TryGetValue("bonus", out var b) ? b : null),
        LevelsJson = S(s.TryGetValue("niveis", out var lv) ? lv : null)
    };
    mysql.Skills.Update(sk);
}
await mysql.SaveChangesAsync();

Console.WriteLine("Importing spells...");
var spells = await ReadAll(sqlite, "SELECT * FROM magia");
foreach (var s in spells)
{
    var sp = new Spell
    {
        Id = I0(s["id"]),
        Name = S(s.TryGetValue("nome", out var n) ? n : s["name"]) ?? "",
        Description = S(s.TryGetValue("descricao", out var d) ? d : null),
        Evocation = S(s.TryGetValue("evocacao", out var e) ? e : null),
        Range = S(s.TryGetValue("alcance", out var r) ? r : null),
        Duration = S(s.TryGetValue("duracao", out var du) ? du : null),
        LevelsJson = S(s.TryGetValue("niveis", out var lv) ? lv : null)
    };
    mysql.Spells.Update(sp);
}
await mysql.SaveChangesAsync();

Console.WriteLine("Importing combat skills...");
var combats = await ReadAll(sqlite, "SELECT * FROM combate");
foreach (var c in combats)
{
    var cb = new CombatSkill
    {
        Id = I0(c["id"]),
        Name = S(c.TryGetValue("nome", out var n) ? n : c["name"]) ?? "",
        AttributeCode = S(c.TryGetValue("atributo", out var a) ? a : null),
        CombatGroupId = I(c.TryGetValue("id_combate_grupo", out var hgid) ? hgid : null),
        CategoryId = I(c.TryGetValue("id_categoria", out var cid) ? cid : null),
        Bonus = I(c.TryGetValue("bonus", out var b) ? b : null),
        Effect = S(c.TryGetValue("efeito", out var ef) ? ef : null),
        Requisite = S(c.TryGetValue("requisitos", out var rq) ? rq : null),
        Notes = S(c.TryGetValue("obs", out var ob) ? ob : null),
        RollTable = S(c.TryGetValue("quadro_rolagem", out var qr) ? qr : null),
        Improvement = S(c.TryGetValue("aprimoramento", out var ap) ? ap : null),
        HasSpecialization = I(c.TryGetValue("possui_especializacao", out var hs) ? hs : null)
    };
    mysql.CombatSkills.Update(cb);
}
await mysql.SaveChangesAsync();

Console.WriteLine("Importing equipments...");
var eqs = await ReadAll(sqlite, "SELECT * FROM equipamento");
foreach (var e in eqs)
{
    var eq = new Equipment
    {
        Id = I0(e["id"]),
        Name = S(e.TryGetValue("nome", out var n) ? n : e["name"]) ?? "",
        Description = S(e.TryGetValue("descricao", out var d) ? d : null),
        GroupId = I(e.TryGetValue("id_grupo", out var g) ? g : null),
        ImageFile = S(e.TryGetValue("image_file", out var im) ? im : null),
        Price = I(e.TryGetValue("valor", out var v) ? v : null),
        IsWeapon = I(e.TryGetValue("arma", out var w) ? w : null),
        IsDefense = I(e.TryGetValue("defesa", out var df) ? df : null),
        IsArmor = I(e.TryGetValue("armadura", out var ar) ? ar : null),
        IsShield = I(e.TryGetValue("escudo", out var sc) ? sc : null),
        IsHelmet = I(e.TryGetValue("capacete", out var cp) ? cp : null)
    };
    mysql.Equipments.Update(eq);
}
await mysql.SaveChangesAsync();

Console.WriteLine("Importing characters...");
var chars = await ReadAll(sqlite, "SELECT * FROM personagem");
foreach (var p in chars)
{
    var ch = new Character
    {
        Id = I0(p["id"]),
        Name = S(p.TryGetValue("nome", out var n) ? n : p["name"]) ?? "",
        Player = S(p.TryGetValue("jogador", out var j) ? j : null),
        ImageFile = S(p.TryGetValue("image_file", out var im) ? im : null),
        Level = I(p.TryGetValue("nivel", out var lv) ? lv : null),

        RaceId = I(p.TryGetValue("raca", out var rid) ? rid : null),
        ProfessionId = I(p.TryGetValue("profissao", out var pid) ? pid : null),
        SpecializationId = I(p.TryGetValue("especializacao", out var sid) ? sid : null),
        DeityId = I(p.TryGetValue("divindade", out var did) ? did : null),
        ClassSocial = S(p.TryGetValue("classe_social", out var cs) ? cs : null),
        BirthPlaceId = I(p.TryGetValue("local_nascimento", out var ln) ? ln : null),

        AttInt = I(p.TryGetValue("att_intelecto", out var ai) ? ai : null),
        AttAur = I(p.TryGetValue("att_aura", out var aa) ? aa : null),
        AttCar = I(p.TryGetValue("att_carisma", out var ac) ? ac : null),
        AttFor = I(p.TryGetValue("att_forca", out var af) ? af : null),
        AttFis = I(p.TryGetValue("att_fisico", out var aF) ? aF : null),
        AttAgi = I(p.TryGetValue("att_agilidade", out var ag) ? ag : null),
        AttPer = I(p.TryGetValue("att_percepcao", out var ap) ? ap : null),

        ActiveDefense = I(p.TryGetValue("defesa_ativa", out var da) ? da : null),
        PassiveDefense = I(p.TryGetValue("defesa_passiva", out var dp) ? dp : null),
        HeroicEnergyMax = I(p.TryGetValue("energia_heroica_maxima", out var ehm) ? ehm : null),
        HeroicEnergy = I(p.TryGetValue("energia_heroica", out var eh) ? eh : null),
        PhysicalEnergy = I(p.TryGetValue("energia_fisica", out var ef) ? ef : null),
        Absorption = I(p.TryGetValue("absorcao", out var ab) ? ab : null),

        PointsSkill = I(p.TryGetValue("pontos_habilidade", out var ph) ? ph : null),
        PointsCombat = I(p.TryGetValue("pontos_combate", out var pc) ? pc : null),
        PointsWeapon = I(p.TryGetValue("pontos_arma", out var pa) ? pa : null),
        PointsMagic = I(p.TryGetValue("pontos_magia", out var pm) ? pm : null),

        Age = I(p.TryGetValue("idade", out var idade) ? idade : null),
        Height = I(p.TryGetValue("altura", out var alt) ? alt : null),
        Weight = I(p.TryGetValue("peso", out var peso) ? peso : null),
        Eyes = S(p.TryGetValue("olhos", out var ol) ? ol : null),
        Hair = S(p.TryGetValue("cabelo", out var cab) ? cab : null),
        Skin = S(p.TryGetValue("pele", out var pe) ? pe : null),
        Appearance = S(p.TryGetValue("aparencia", out var apar) ? apar : null),
        History = S(p.TryGetValue("historia", out var hist) ? hist : null),

        CoinsCopper = I(p.TryGetValue("moedas_cobre", out var mc) ? mc : null),
        CoinsSilver = I(p.TryGetValue("moedas_prata", out var mp) ? mp : null),
        CoinsGold = I(p.TryGetValue("moedas_ouro", out var mo) ? mo : null),
    };

    mysql.Characters.Update(ch);
}
await mysql.SaveChangesAsync();

// joins
Console.WriteLine("Importing character joins...");

async Task ImportJoin(string table, string charCol, string idCol, string levelCol, Action<int,int,int> add)
{
    var rows = await ReadAll(sqlite, $"SELECT * FROM {table}");
    foreach (var r in rows)
    {
        var cid = I0(r[charCol]);
        var iid = I0(r[idCol]);
        var lvl = I0(r[levelCol]);
        add(cid, iid, lvl);
    }
}

// Skills
await ImportJoin("personagem_habilidade", "id_personagem", "id_habilidade", "nivel",
    (cid, sid, lvl) => mysql.CharacterSkills.Update(new CharacterSkill { CharacterId = cid, SkillId = sid, Level = lvl }));
await mysql.SaveChangesAsync();

// Spells
await ImportJoin("personagem_magia", "id_personagem", "id_magia", "nivel",
    (cid, sid, lvl) => mysql.CharacterSpells.Update(new CharacterSpell { CharacterId = cid, SpellId = sid, Level = lvl }));
await mysql.SaveChangesAsync();

// Combat
await ImportJoin("personagem_combate", "id_personagem", "id_combate", "nivel",
    (cid, sid, lvl) => mysql.CharacterCombatSkills.Update(new CharacterCombatSkill { CharacterId = cid, CombatSkillId = sid, Level = lvl }));
await mysql.SaveChangesAsync();

// Equipments
var eqRows = await ReadAll(sqlite, "SELECT * FROM personagem_equipamento");
foreach (var r in eqRows)
{
    var cid = I0(r["id_personagem"]);
    var eid = I0(r["id_equipamento"]);
    var qty = I0(r.TryGetValue("quantidade", out var q) ? q : 1);
    mysql.CharacterEquipments.Update(new CharacterEquipment { CharacterId = cid, EquipmentId = eid, Qty = qty });
}
await mysql.SaveChangesAsync();

Console.WriteLine("DONE ✅");

// Adicione esta função utilitária antes do primeiro uso de IntArrayRegex()
static Regex IntArrayRegex()
{
    // Regex para encontrar inteiros (positivos/negativos) em uma string tipo "[0,1,-2,3]"
    return new Regex(@"-?\d+", RegexOptions.Compiled);
}
