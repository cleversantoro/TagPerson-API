using TagPerson.Domain.Entities;
using TagPerson.Domain.ValueObjects;

namespace TagPerson.Domain.Services;

/// <summary>
/// Replica as regras de cálculo do projeto Python.
/// - Atributos efetivos = base + bônus da raça
/// - max EF = FOR + FIS + race.ef
/// - RF = level + FIS ; RM = level + AUR
/// - Velocidade = base_speed + FIS
/// - Karma = AUR * (level + 1)
/// - Defesa passiva = armour.base_defense + shield.base_defense
/// - Defesa ativa = passiva + AGI (nunca menor que passiva)
/// - Absorção = 0 se não tiver armadura, senão maxEf + armour.abs + helmet.abs
/// - Pontos magia = 0 se profession.attribute_for_magic == -1, senão atributo + MAGIC_PTS (const)
/// </summary>
public sealed class TagmarCalculator
{
    // equivalente ao const.MAGIC_PTS do python (ajuste se quiser bater 100% com seu Constants.py)
    public const int MAGIC_PTS = 5;

    public DerivedStats Calculate(Character c, Race? race, Profession? prof, EquipmentDefenseStats? armour, EquipmentDefenseStats? helmet, EquipmentDefenseStats? shield)
    {
        static int AsInt(string? value)
        {
            return int.TryParse(value, out var parsed) ? parsed : 0;
        }

        var level = c.Level ?? 0;
        // bônus de raça
        var bAgi = race?.BonusAgi ?? 0;
        var bPer = race?.BonusPer ?? 0;
        var bInt = race?.BonusInt ?? 0;
        var bAur = race?.BonusAur ?? 0;
        var bCar = race?.BonusCar ?? 0;
        var bFor = race?.BonusFor ?? 0;
        var bFis = race?.BonusFis ?? 0;

        var agi = (c.AttAgi ?? 0) + bAgi;
        var aur = (c.AttAur ?? 0) + bAur;
        var fis = (c.AttFis ?? 0) + bFis;
        var forca = (c.AttFor ?? 0) + bFor;

        var maxEf = forca + fis + (race?.EfBonus ?? 0);

        var rf = level + fis;
        var rm = level + aur;

        var vel = (race?.BaseSpeed ?? 0) + fis;
        var karma = aur * (level + 1);

        var defPassiva = AsInt(armour?.BaseDefense) + AsInt(shield?.BaseDefense);
        var defAtiva = defPassiva + agi;
        if (defAtiva < defPassiva) defAtiva = defPassiva;

        var absorcao = 0;
        if (armour != null)
        {
            absorcao = maxEf + (armour.Absorption ?? 0) + (helmet?.Absorption ?? 0);
        }

        int pontosMagia = 0;
        var attributeForMagic = prof?.AttributeForMagic ?? -1;
        if (prof is not null && attributeForMagic != -1)
        {
            // atributos no python são indexados 0..6; aqui já temos campos separados
            // map:
            // 0 INT, 1 AUR, 2 CAR, 3 FOR, 4 FIS, 5 AGI, 6 PER  (igual Persona.get_att() no python)
            var att = attributeForMagic switch
            {
                0 => ((c.AttInt ?? 0) + bInt),
                1 => ((c.AttAur ?? 0) + bAur),
                2 => ((c.AttCar ?? 0) + bCar),
                3 => ((c.AttFor ?? 0) + bFor),
                4 => ((c.AttFis ?? 0) + bFis),
                5 => ((c.AttAgi ?? 0) + bAgi),
                6 => ((c.AttPer ?? 0) + bPer),
                _ => 0
            };
            pontosMagia = att + MAGIC_PTS;
        }

        return new DerivedStats
        {
            MaxEf = maxEf,
            ResistenciaFisica = rf,
            ResistenciaMagica = rm,
            Velocidade = vel,
            Karma = karma,
            DefesaAtiva = defAtiva,
            DefesaPassiva = defPassiva,
            Absorcao = absorcao,
            PontosMagia = pontosMagia
        };
    }
}
